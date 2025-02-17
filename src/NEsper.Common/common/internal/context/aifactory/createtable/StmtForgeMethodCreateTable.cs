///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2019 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;

using com.espertech.esper.common.client;
using com.espertech.esper.common.client.meta;
using com.espertech.esper.common.client.util;
using com.espertech.esper.common.@internal.bytecodemodel.@base;
using com.espertech.esper.common.@internal.bytecodemodel.core;
using com.espertech.esper.common.@internal.compile.multikey;
using com.espertech.esper.common.@internal.compile.stage1.spec;
using com.espertech.esper.common.@internal.compile.stage2;
using com.espertech.esper.common.@internal.compile.stage3;
using com.espertech.esper.common.@internal.context.module;
using com.espertech.esper.common.@internal.epl.agg.core;
using com.espertech.esper.common.@internal.epl.annotation;
using com.espertech.esper.common.@internal.epl.expression.agg.@base;
using com.espertech.esper.common.@internal.epl.expression.core;
using com.espertech.esper.common.@internal.epl.resultset.select.core;
using com.espertech.esper.common.@internal.epl.streamtype;
using com.espertech.esper.common.@internal.epl.table.compiletime;
using com.espertech.esper.common.@internal.epl.util;
using com.espertech.esper.common.@internal.@event.core;
using com.espertech.esper.common.@internal.filterspec;
using com.espertech.esper.common.@internal.rettype;
using com.espertech.esper.common.@internal.schedule;
using com.espertech.esper.common.@internal.serde.compiletime.eventtype;
using com.espertech.esper.common.@internal.serde.compiletime.resolve;
using com.espertech.esper.common.@internal.settings;
using com.espertech.esper.common.@internal.util;
using com.espertech.esper.compat.collections;

namespace com.espertech.esper.common.@internal.context.aifactory.createtable
{
    public class StmtForgeMethodCreateTable : StmtForgeMethod
    {
        public const string INTERNAL_RESERVED_PROPERTY = "internal-reserved";

        private readonly StatementBaseInfo @base;

        public StmtForgeMethodCreateTable(StatementBaseInfo @base)
        {
            this.@base = @base;
        }

        public StmtForgeMethodResult Make(
            string @namespace,
            string classPostfix,
            StatementCompileTimeServices services)
        {
            try {
                return Build(@namespace, classPostfix, services);
            }
            catch (ExprValidationException) {
                throw;
            }
            catch (Exception t) {
                throw new ExprValidationException(
                    "Unexpected exception creating table '" +
                    @base.StatementSpec.Raw.CreateTableDesc.TableName +
                    "': " +
                    t.Message,
                    t);
            }
        }

        private StmtForgeMethodResult Build(
            string @namespace,
            string classPostfix,
            StatementCompileTimeServices services)
        {
            var createDesc = @base.StatementSpec.Raw.CreateTableDesc;
            var tableName = createDesc.TableName;
            var additionalForgeables = new List<StmtClassForgeableFactory>();

            // determine whether already declared as table or variable
            EPLValidationUtil.ValidateAlreadyExistsTableOrVariable(
                tableName,
                services.VariableCompileTimeResolver,
                services.TableCompileTimeResolver,
                services.EventTypeCompileTimeResolver);

            // determine key types
            ValidateKeyTypes(createDesc.Columns, services.ImportServiceCompileTime);

            // check column naming, interpret annotations
            var columnsValidated = ValidateExpressions(createDesc.Columns, services);
            var columnDescs = columnsValidated.First;
            additionalForgeables.AddRange(columnsValidated.Second);

            // analyze and plan the state holders
            var plan = AnalyzePlanAggregations(createDesc.TableName, columnDescs, @base.StatementRawInfo, services);
            additionalForgeables.AddAll(plan.AdditionalForgeables);
            var visibility = plan.PublicEventType.Metadata.AccessModifier;

            // determine context information
            var contextName = @base.StatementRawInfo.ContextName;
            NameAccessModifier? contextVisibility = null;
            string contextModuleName = null;
            if (contextName != null) {
                var contextDetail = services.ContextCompileTimeResolver.GetContextInfo(contextName);
                if (contextDetail == null) {
                    throw new ExprValidationException("Failed to find context '" + contextName + "'");
                }

                contextVisibility = contextDetail.ContextVisibility;
                contextModuleName = contextDetail.ContextModuleName;
            }

            // add table
            var tableMetaData = new TableMetaData(
                tableName,
                @base.ModuleName,
                visibility,
                contextName,
                contextVisibility,
                contextModuleName,
                plan.InternalEventType,
                plan.PublicEventType,
                plan.PrimaryKeyColumns,
                plan.PrimaryKeyTypes,
                plan.PrimaryKeyColNums,
                plan.TableColumns,
                plan.ColsAggMethod.Length);
            services.TableCompileTimeRegistry.NewTable(tableMetaData);

            var aiFactoryProviderClassName = CodeGenerationIDGenerator.GenerateClassNameSimple(
                typeof(StatementAIFactoryProvider),
                classPostfix);
            var statementFieldsClassName =
                CodeGenerationIDGenerator.GenerateClassNameSimple(typeof(StatementFields), classPostfix);
            var statementProviderClassName =
                CodeGenerationIDGenerator.GenerateClassNameSimple(typeof(StatementProvider), classPostfix);

            var forge = new StatementAgentInstanceFactoryCreateTableForge(
                aiFactoryProviderClassName,
                tableMetaData.TableName,
                plan);

            // build forge list
            var namespaceScope = new CodegenNamespaceScope(
                @namespace,
                statementFieldsClassName,
                services.IsInstrumented);
            var forgeables = additionalForgeables
                .Select(additional => additional.Make(namespaceScope, classPostfix))
                .ToList();

            var aiFactoryForgeable = new StmtClassForgeableAIFactoryProviderCreateTable(
                aiFactoryProviderClassName,
                namespaceScope,
                forge,
                tableName);
            forgeables.Add(aiFactoryForgeable);

            var selectSubscriberDescriptor = new SelectSubscriberDescriptor();
            var informationals = StatementInformationalsUtil.GetInformationals(
                @base,
                EmptyList<FilterSpecCompiled>.Instance,
                EmptyList<ScheduleHandleCallbackProvider>.Instance,
                EmptyList<NamedWindowConsumerStreamSpec>.Instance,
                true,
                selectSubscriberDescriptor,
                namespaceScope,
                services);
            informationals.Properties.Put(StatementProperty.CREATEOBJECTNAME, createDesc.TableName);

            forgeables.Add(
                new StmtClassForgeableStmtProvider(
                    aiFactoryProviderClassName,
                    statementProviderClassName,
                    informationals,
                    namespaceScope));
            forgeables.Add(
                new StmtClassForgeableStmtFields(
                    statementFieldsClassName,
                    namespaceScope,
                    1));

            return new StmtForgeMethodResult(
                forgeables,
                EmptyList<FilterSpecCompiled>.Instance,
                EmptyList<ScheduleHandleCallbackProvider>.Instance,
                EmptyList<NamedWindowConsumerStreamSpec>.Instance,
                EmptyList<FilterSpecParamExprNodeForge>.Instance);
        }

        private void ValidateKeyTypes(
            IList<CreateTableColumn> columns,
            ImportServiceCompileTime importService)
        {
            foreach (var col in columns) {
                if (col.PrimaryKey == null || !col.PrimaryKey.Value) {
                    continue;
                }

                var msg = "Column '" + col.ColumnName + "' may not be tagged as primary key";
                if (col.OptExpression != null) {
                    throw new ExprValidationException(msg + ", an expression cannot become a primary key column");
                }

                var type = EventTypeUtility.BuildType(
                    new ColumnDesc(col.ColumnName, col.OptType.ToEPL()),
                    importService);
                if (!(type is Type)) {
                    throw new ExprValidationException(msg + ", received unexpected event type '" + type + "'");
                }
            }
        }

        private compat.collections.Pair<IList<TableColumnDesc>, IList<StmtClassForgeableFactory>> ValidateExpressions(
            IList<CreateTableColumn> columns, 
            StatementCompileTimeServices services)
        {
            ISet<string> columnNames = new HashSet<string>();
            IList<TableColumnDesc> descriptors = new List<TableColumnDesc>();

            var positionInDeclaration = 0;
            var additionalForgeables = new List<StmtClassForgeableFactory>();

            foreach (var column in columns) {
                var msgprefix = "For column '" + column.ColumnName + "'";

                // check duplicate name
                if (columnNames.Contains(column.ColumnName)) {
                    throw new ExprValidationException("Column '" + column.ColumnName + "' is listed more than once");
                }

                columnNames.Add(column.ColumnName);

                // determine presence of type annotation
                var optionalEventType = ValidateExpressionGetEventType(msgprefix, column.Annotations, services);

                // aggregation node
                TableColumnDesc descriptor;
                if (column.OptExpression != null) {
                    var pair = ValidateAggregationExpr(column.OptExpression, optionalEventType, services);
                    descriptor = new TableColumnDescAgg(positionInDeclaration, column.ColumnName, pair.First, optionalEventType);
                    additionalForgeables.AddRange(pair.Second);
                }
                else {
                    var unresolvedType = EventTypeUtility.BuildType(
                        new ColumnDesc(column.ColumnName, column.OptType.ToEPL()),
                        services.ImportServiceCompileTime);
                    descriptor = new TableColumnDescTyped(
                        positionInDeclaration,
                        column.ColumnName,
                        unresolvedType,
                        column.PrimaryKey.GetValueOrDefault(false));
                }

                descriptors.Add(descriptor);
                positionInDeclaration++;
            }

            return new compat.collections.Pair<IList<TableColumnDesc>, IList<StmtClassForgeableFactory>>(descriptors, additionalForgeables);
        }

        private static EventType ValidateExpressionGetEventType(
            string msgprefix,
            IList<AnnotationDesc> annotations,
            StatementCompileTimeServices services)
        {
            var annos = AnnotationUtil.MapByNameLowerCase(annotations);

            // check annotations used
            var typeAnnos = annos.Delete("type");
            if (!annos.IsEmpty()) {
                throw new ExprValidationException(msgprefix + " unrecognized annotation '" + annos.Keys.First() + "'");
            }

            // type determination
            EventType optionalType = null;
            if (typeAnnos != null) {
                var typeName = AnnotationUtil.GetExpectSingleStringValue(msgprefix, typeAnnos);
                optionalType = services.EventTypeCompileTimeResolver.GetTypeByName(typeName);
                if (optionalType == null) {
                    throw new ExprValidationException(msgprefix + " failed to find event type '" + typeName + "'");
                }
            }

            return optionalType;
        }

        private compat.collections.Pair<ExprAggregateNode, IList<StmtClassForgeableFactory>> ValidateAggregationExpr(
            ExprNode columnExpressionType,
            EventType optionalProvidedType,
            StatementCompileTimeServices services)
        {
            var classpathImportService = services.ImportServiceCompileTime;

            // determine validation context types and istream/irstream
            EventType[] types;
            string[] streamNames;
            bool[] istreamOnly;
            if (optionalProvidedType != null) {
                types = new[] {optionalProvidedType};
                streamNames = new[] {types[0].Name};
                istreamOnly = new[] {
                    false
                }; // always false (expected to be bound by data window), use "ever"-aggregation functions otherwise
            }
            else {
                types = new EventType[0];
                streamNames = new string[0];
                istreamOnly = new bool[0];
            }

            var streamTypeService = new StreamTypeServiceImpl(types, streamNames, istreamOnly, false, false);
            var validationContext =
                new ExprValidationContextBuilder(streamTypeService, @base.StatementRawInfo, services).Build();

            // substitute parameter nodes
            foreach (var childNode in columnExpressionType.ChildNodes) {
                if (childNode is ExprIdentNode) {
                    var identNode = (ExprIdentNode) childNode;
                    var propname = identNode.FullUnresolvedName.Trim();
                    var clazz = TypeHelper.GetTypeForSimpleName(
                        propname,
                        classpathImportService.TypeResolver);
                    if (propname.ToLowerInvariant().Trim().Equals("@object")) {
                        clazz = typeof(object);
                    }

                    ImportException ex = null;
                    if (clazz == null) {
                        try {
                            clazz = classpathImportService.ResolveType(propname, false, services.ClassProvidedExtension);
                        }
                        catch (ImportException e) {
                            ex = e;
                        }
                    }

                    if (clazz != null) {
                        var typeNode = new ExprTypedNoEvalNode(propname, clazz);
                        ExprNodeUtilityModify.ReplaceChildNode(columnExpressionType, identNode, typeNode);
                    }
                    else {
                        if (optionalProvidedType == null) {
                            if (ex != null) {
                                throw new ExprValidationException(
                                    "Failed to resolve type '" + propname + "': " + ex.Message,
                                    ex);
                            }

                            throw new ExprValidationException("Failed to resolve type '" + propname + "'");
                        }
                    }
                }
            }

            // validate
            var validated = ExprNodeUtilityValidate.GetValidatedSubtree(
                ExprNodeOrigin.CREATETABLECOLUMN,
                columnExpressionType,
                validationContext);
            if (!(validated is ExprAggregateNode)) {
                throw new ExprValidationException(
                    "Expression '" +
                    ExprNodeUtilityPrint.ToExpressionStringMinPrecedenceSafe(validated) +
                    "' is not an aggregation");
            }

            return new compat.collections.Pair<ExprAggregateNode, IList<StmtClassForgeableFactory>>(
                (ExprAggregateNode) validated,
                validationContext.AdditionalForgeables);
        }

        private TableAccessAnalysisResult AnalyzePlanAggregations(
            string tableName,
            IList<TableColumnDesc> columns,
            StatementRawInfo statementRawInfo,
            StatementCompileTimeServices services)
        {
            // once upfront: obtains aggregation factories for each aggregation
            // we do this once as a factory may be a heavier object
            IDictionary<TableColumnDesc, AggregationForgeFactory> aggregationFactories =
                new Dictionary<TableColumnDesc, AggregationForgeFactory>();
            foreach (var column in columns) {
                if (column is TableColumnDescAgg) {
                    var agg = (TableColumnDescAgg) column;
                    var factory = agg.Aggregation.Factory;
                    aggregationFactories.Put(column, factory);
                }
            }

            // sort into these categories:
            // plain / method-agg / access-agg
            // compile all-column public types
            IList<TableColumnDescTyped> plainColumns = new List<TableColumnDescTyped>();
            IList<TableColumnDescAgg> methodAggColumns = new List<TableColumnDescAgg>();
            IList<TableColumnDescAgg> accessAggColumns = new List<TableColumnDescAgg>();
            var allColumnsPublicTypes = new LinkedHashMap<string, object>();
            foreach (var column in columns) {
                // handle plain types
                if (column is TableColumnDescTyped) {
                    var typed = (TableColumnDescTyped) column;
                    plainColumns.Add(typed);
                    allColumnsPublicTypes.Put(column.ColumnName, typed.UnresolvedType);
                    continue;
                }

                // handle aggs
                var agg = (TableColumnDescAgg) column;
                var aggFactory = aggregationFactories.Get(agg);
                if (aggFactory.IsAccessAggregation) {
                    accessAggColumns.Add(agg);
                }
                else {
                    methodAggColumns.Add(agg);
                }

                allColumnsPublicTypes.Put(column.ColumnName, agg.Aggregation.EvaluationType);
            }

            // determine column metadata
            //
            var columnMetadata = new LinkedHashMap<string, TableMetadataColumn>();

            // handle typed columns
            var allColumnsInternalTypes = new LinkedHashMap<string, object>();
            allColumnsInternalTypes.Put(INTERNAL_RESERVED_PROPERTY, typeof(object));
            var indexPlain = 1;
            var assignPairsPlain = new TableMetadataColumnPairPlainCol[plainColumns.Count];
            foreach (var typedColumn in plainColumns) {
                allColumnsInternalTypes.Put(typedColumn.ColumnName, typedColumn.UnresolvedType);
                columnMetadata.Put(
                    typedColumn.ColumnName,
                    new TableMetadataColumnPlain(typedColumn.ColumnName, typedColumn.IsKey, indexPlain));
                assignPairsPlain[indexPlain - 1] = new TableMetadataColumnPairPlainCol(
                    typedColumn.PositionInDeclaration,
                    indexPlain);
                indexPlain++;
            }

            // determine internally-used event type
            var visibility = services.ModuleVisibilityRules.GetAccessModifierTable(@base, tableName);
            var internalName = EventTypeNameUtil.GetTableInternalTypeName(tableName);
            var internalMetadata = new EventTypeMetadata(
                internalName,
                @base.ModuleName,
                EventTypeTypeClass.TABLE_INTERNAL,
                EventTypeApplicationType.OBJECTARR,
                visibility,
                EventTypeBusModifier.NONBUS,
                false,
                EventTypeIdPair.Unassigned());
            var internalEventType = BaseNestableEventUtil.MakeOATypeCompileTime(
                internalMetadata,
                allColumnsInternalTypes,
                null,
                null,
                null,
                null,
                services.BeanEventTypeFactoryPrivate,
                services.EventTypeCompileTimeResolver);
            services.EventTypeCompileTimeRegistry.NewType(internalEventType);

            // for use by indexes and lookups
            var publicName = EventTypeNameUtil.GetTablePublicTypeName(tableName);
            var publicMetadata = new EventTypeMetadata(
                publicName,
                @base.ModuleName,
                EventTypeTypeClass.TABLE_PUBLIC,
                EventTypeApplicationType.OBJECTARR,
                visibility,
                EventTypeBusModifier.NONBUS,
                false,
                EventTypeIdPair.Unassigned());
            var publicEventType = BaseNestableEventUtil.MakeOATypeCompileTime(
                publicMetadata,
                allColumnsPublicTypes,
                null,
                null,
                null,
                null,
                services.BeanEventTypeFactoryPrivate,
                services.EventTypeCompileTimeResolver);
            services.EventTypeCompileTimeRegistry.NewType(publicEventType);

            // handle aggregation-methods single-func first.
            var methodFactories = new AggregationForgeFactory[methodAggColumns.Count];
            var index = 0;
            var assignPairsMethod = new TableMetadataColumnPairAggMethod[methodAggColumns.Count];
            foreach (var column in methodAggColumns) {
                var factory = aggregationFactories.Get(column);
                var optionalEnumerationType = EPTypeHelper.OptionalFromEnumerationExpr(
                    statementRawInfo,
                    services,
                    column.Aggregation);
                methodFactories[index] = factory;
                var bindingInfo = factory.AggregationPortableValidation;
                var expression =
                    ExprNodeUtilityPrint.ToExpressionStringMinPrecedenceSafe(factory.AggregationExpression);
                columnMetadata.Put(
                    column.ColumnName,
                    new TableMetadataColumnAggregation(
                        column.ColumnName,
                        false,
                        index,
                        bindingInfo,
                        expression,
                        true,
                        optionalEnumerationType));
                assignPairsMethod[index] = new TableMetadataColumnPairAggMethod(column.PositionInDeclaration);
                index++;
            }

            // handle access-aggregation (sharable, multi-value) aggregations
            var stateFactories = new AggregationStateFactoryForge[accessAggColumns.Count];
            var accessAccessorForges = new AggregationAccessorSlotPairForge[accessAggColumns.Count];
            var assignPairsAccess = new TableMetadataColumnPairAggAccess[accessAggColumns.Count];
            var accessNum = 0;
            foreach (var column in accessAggColumns) {
                var factory = aggregationFactories.Get(column);
                var forge = factory.GetAggregationStateFactory(false);
                stateFactories[accessNum] = forge;
                var accessor = factory.AccessorForge;
                var bindingInfo = factory.AggregationPortableValidation;
                accessAccessorForges[accessNum] = new AggregationAccessorSlotPairForge(accessNum, accessor);
                var expression =
                    ExprNodeUtilityPrint.ToExpressionStringMinPrecedenceSafe(factory.AggregationExpression);
                var optionalEnumerationType = EPTypeHelper.OptionalFromEnumerationExpr(
                    statementRawInfo,
                    services,
                    column.Aggregation);
                columnMetadata.Put(
                    column.ColumnName,
                    new TableMetadataColumnAggregation(
                        column.ColumnName,
                        false,
                        index,
                        bindingInfo,
                        expression,
                        false,
                        optionalEnumerationType));
                assignPairsAccess[accessNum] =
                    new TableMetadataColumnPairAggAccess(column.PositionInDeclaration, accessor);
                index++;
                accessNum++;
            }

            // determine primary key index information
            var primaryKeyColumns = new List<string>();
            var primaryKeyTypes = new List<Type>();
            var primaryKeyGetters = new List<EventPropertyGetterSPI>();
            var primaryKeyColNums = new List<int>();
            
            var colNum = -1;
            foreach (var typedColumn in plainColumns) {
                colNum++;
                if (typedColumn.IsKey) {
                    primaryKeyColumns.Add(typedColumn.ColumnName);
                    primaryKeyTypes.Add(internalEventType.GetPropertyType(typedColumn.ColumnName));
                    primaryKeyGetters.Add(internalEventType.GetGetterSPI(typedColumn.ColumnName));
                    primaryKeyColNums.Add(colNum + 1);
                }
            }

            string[] primaryKeyColumnArray = null;
            Type[] primaryKeyTypeArray = null;
            EventPropertyGetterSPI[] primaryKeyGetterArray = null;

            int[] primaryKeyColNumsArray = null;
            if (!primaryKeyColumns.IsEmpty()) {
                primaryKeyColumnArray = primaryKeyColumns.ToArray();
                primaryKeyTypeArray = primaryKeyTypes.ToArray();
                primaryKeyGetterArray = primaryKeyGetters.ToArray();
                primaryKeyColNumsArray = primaryKeyColNums.ToArray();
            }

            var forgeDesc = new AggregationRowStateForgeDesc(
                methodFactories,
                null,
                stateFactories,
                accessAccessorForges,
                new AggregationUseFlags(false, false, false));
            
            
            var multiKeyPlan = MultiKeyPlanner.PlanMultiKey(
                primaryKeyTypeArray, false, statementRawInfo, services.SerdeResolver);

            var propertyForges = new DataInputOutputSerdeForge[internalEventType.PropertyNames.Length - 1];

            var additionalForgeables = new List<StmtClassForgeableFactory>(multiKeyPlan.MultiKeyForgeables);
            for (var i = 1; i < internalEventType.PropertyNames.Length; i++) {
                var propertyName = internalEventType.PropertyNames[i];
                var propertyType = internalEventType.Types.Get(propertyName);
                var desc = SerdeEventPropertyUtility.ForgeForEventProperty(
                    publicEventType, propertyName, propertyType, statementRawInfo, services.SerdeResolver);
                propertyForges[i - 1] = desc.Forge;

                // plan serdes for nested types
                foreach (var eventType in desc.NestedTypes) {
                    var serdeForgeables = SerdeEventTypeUtility.Plan(
                        eventType,
                        statementRawInfo,
                        services.SerdeEventTypeRegistry,
                        services.SerdeResolver);
                    additionalForgeables.AddAll(serdeForgeables);
                }
            }

            return new TableAccessAnalysisResult(
                columnMetadata,
                internalEventType,
                propertyForges,
                publicEventType,
                assignPairsPlain,
                assignPairsMethod,
                assignPairsAccess,
                forgeDesc,
                primaryKeyColumnArray,
                primaryKeyGetterArray,
                primaryKeyTypeArray,
                primaryKeyColNumsArray,
                multiKeyPlan.ClassRef,
                additionalForgeables);
        }
    }
} // end of namespace
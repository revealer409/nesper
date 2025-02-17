///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;

using Avro.Generic;

using com.espertech.esper.common.client;
using com.espertech.esper.common.client.context;
using com.espertech.esper.common.client.fireandforget;
using com.espertech.esper.common.client.scopetest;
using com.espertech.esper.common.@internal.support;
using com.espertech.esper.compat;
using com.espertech.esper.compat.collections;
using com.espertech.esper.compat.function;
using com.espertech.esper.regressionlib.framework;
using com.espertech.esper.regressionlib.support.context;
using com.espertech.esper.regressionlib.support.util;
using com.espertech.esper.runtime.client;

using NEsper.Avro.Extensions;
using NEsper.Avro.Util.Support;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

using static com.espertech.esper.regressionlib.framework.SupportMessageAssertUtil;

using SupportBean_A = com.espertech.esper.regressionlib.support.bean.SupportBean_A;

namespace com.espertech.esper.regressionlib.suite.infra.nwtable
{
    public class InfraNWTableFAF : IndexBackingTableInfo
    {
        public static IList<RegressionExecution> Executions()
        {
            var execs = new List<RegressionExecution>();

            WithSelectWildcard(execs);
            WithSelectWildcardSceneTwo(execs);
            WithInsert(execs);
            WithUpdate(execs);
            WithDelete(execs);
            WithDeleteContextPartitioned(execs);
            WithSelectCountStar(execs);
            WithAggUngroupedRowForAll(execs);
            WithInClause(execs);
            WithAggUngroupedRowForGroup(execs);
            WithJoin(execs);
            WithAggUngroupedRowForEvent(execs);
            WithJoinWhere(execs);
            With3StreamInnerJoin(execs);
            WithExecuteFilter(execs);
            WithInvalid(execs);
            WithSelectDistinct(execs);

            return execs;
        }

        public static IList<RegressionExecution> WithSelectWildcard(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new InfraSelectWildcard(true));
            execs.Add(new InfraSelectWildcard(false));
            return execs;
        }

        public static IList<RegressionExecution> WithSelectWildcardSceneTwo(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new InfraSelectWildcardSceneTwo(true));
            execs.Add(new InfraSelectWildcardSceneTwo(false));
            return execs;
        }

        public static IList<RegressionExecution> WithInsert(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new InfraInsert(true));
            execs.Add(new InfraInsert(false));
            return execs;
        }

        public static IList<RegressionExecution> WithUpdate(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new InfraUpdate(true));
            execs.Add(new InfraUpdate(false));
            return execs;
        }

        public static IList<RegressionExecution> WithDelete(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new InfraDelete(true));
            execs.Add(new InfraDelete(false));
            return execs;
        }

        public static IList<RegressionExecution> WithDeleteContextPartitioned(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new InfraDeleteContextPartitioned(true));
            execs.Add(new InfraDeleteContextPartitioned(false));
            return execs;
        }

        public static IList<RegressionExecution> WithSelectCountStar(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new InfraSelectCountStar(true));
            execs.Add(new InfraSelectCountStar(false));
            return execs;
        }

        public static IList<RegressionExecution> WithAggUngroupedRowForAll(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new InfraAggUngroupedRowForAll(true));
            execs.Add(new InfraAggUngroupedRowForAll(false));
            return execs;
        }

        public static IList<RegressionExecution> WithInClause(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new InfraInClause(true));
            execs.Add(new InfraInClause(false));
            return execs;
        }

        public static IList<RegressionExecution> WithAggUngroupedRowForGroup(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new InfraAggUngroupedRowForGroup(true));
            execs.Add(new InfraAggUngroupedRowForGroup(false));
            return execs;
        }

        public static IList<RegressionExecution> WithJoin(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new InfraJoin(true, true));
            execs.Add(new InfraJoin(false, false));
            execs.Add(new InfraJoin(true, false));
            execs.Add(new InfraJoin(false, true));
            return execs;
        }

        public static IList<RegressionExecution> WithAggUngroupedRowForEvent(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new InfraAggUngroupedRowForEvent(true));
            execs.Add(new InfraAggUngroupedRowForEvent(false));
            return execs;
        }

        public static IList<RegressionExecution> WithJoinWhere(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new InfraJoinWhere(true));
            execs.Add(new InfraJoinWhere(false));
            return execs;
        }

        public static IList<RegressionExecution> With3StreamInnerJoin(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            foreach (var rep in EventRepresentationChoiceExtensions.Values()) {
                execs.Add(new Infra3StreamInnerJoin(rep, true));
                execs.Add(new Infra3StreamInnerJoin(rep, false));
            }
            return execs;
        }

        public static IList<RegressionExecution> WithExecuteFilter(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new InfraExecuteFilter(true));
            execs.Add(new InfraExecuteFilter(false));
            return execs;
        }

        public static IList<RegressionExecution> WithInvalid(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new InfraInvalid(true));
            execs.Add(new InfraInvalid(false));
            return execs;
        }

        public static IList<RegressionExecution> WithSelectDistinct(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new InfraSelectDistinct(true));
            execs.Add(new InfraSelectDistinct(false));
            return execs;
        }

        private static void RunAssertionIn(
            RegressionEnvironment env,
            RegressionPath path)
        {
            TryAssertionIn(env, path, "TheString in ('E2', 'E3') and IntPrimitive in (10, 20)", new[] {200L});
            TryAssertionIn(env, path, "IntPrimitive in (30, 20) and TheString in ('E4', 'E1')", new long[] { });
            TryAssertionIn(env, path, "IntPrimitive in (30, 20) and TheString in ('E2', 'E1')", new[] {200L});
            TryAssertionIn(
                env,
                path,
                "TheString in ('E2', 'E3') and IntPrimitive in (20, 30)",
                new[] {200L, 300L});
            TryAssertionIn(
                env,
                path,
                "TheString in ('E2', 'E3') and IntPrimitive in (30, 20)",
                new[] {200L, 300L});
            TryAssertionIn(
                env,
                path,
                "TheString in ('E1', 'E2', 'E3', 'E4') and IntPrimitive in (10, 20, 30)",
                new[] {100L, 200L, 300L});
        }

        private static void TryAssertionIn(
            RegressionEnvironment env,
            RegressionPath path,
            string filter,
            long[] expected)
        {
            var result = env.CompileExecuteFAF("select * from MyInfra where " + filter, path);
            Assert.AreEqual(result.Array.Length, expected.Length);
            IList<long> values = new List<long>();
            foreach (var @event in result.Array) {
                values.Add(@event.Get("LongPrimitive").AsInt64());
            }

            EPAssertionUtil.AssertEqualsAnyOrder(expected, values.ToArray());
        }

        private static void AssertFAFInsertResult(
            EPFireAndForgetQueryResult resultOne,
            object[] objects,
            string[] propertyNames,
            EPStatement stmt,
            bool namedWindow)
        {
            Assert.AreSame(resultOne.EventType, stmt.EventType);
            if (namedWindow) {
                Assert.AreEqual(1, resultOne.Array.Length);
                EPAssertionUtil.AssertPropsPerRow(
                    resultOne.Array,
                    propertyNames,
                    new[] {objects});
            }
            else {
                Assert.AreEqual(0, resultOne.Array.Length);
            }
        }

        private static RegressionPath SetupInfra(
            RegressionEnvironment env,
            bool namedWindow)
        {
            var path = new RegressionPath();
            var eplCreate = namedWindow
                ? "@Name('TheInfra') create window MyInfra#keepall as select * from SupportBean"
                : "@Name('TheInfra') create table MyInfra as (TheString string primary key, IntPrimitive int primary key, LongPrimitive long)";
            env.CompileDeploy(eplCreate, path);
            var eplInsert = namedWindow
                ? "@Name('Insert') insert into MyInfra select * from SupportBean"
                : "@Name('Insert') on SupportBean sb merge MyInfra mi where mi.TheString = sb.TheString and mi.IntPrimitive=sb.IntPrimitive" +
                  " when not matched then insert select TheString, IntPrimitive, LongPrimitive";
            env.CompileDeploy(eplInsert, path);
            return path;
        }

        private static SupportBean MakeBean(
            string theString,
            int intPrimitive,
            long longPrimitive)
        {
            var bean = new SupportBean(theString, intPrimitive);
            bean.LongPrimitive = longPrimitive;
            return bean;
        }

        public static void DoubleInt(SupportBean bean)
        {
            bean.IntPrimitive = bean.IntPrimitive * 2;
        }

        private static void AssertCtxInfraCountPerCode(
            RegressionEnvironment env,
            RegressionPath path,
            long[] expectedCountPerCode)
        {
            for (var i = 0; i < expectedCountPerCode.Length; i++) {
                Assert.AreEqual(expectedCountPerCode[i], GetCtxInfraCount(env, path, i), "for code " + i);
            }
        }

        private static void SendEvent(
            EventRepresentationChoice eventRepresentationEnum,
            RegressionEnvironment env,
            string eventName,
            string[] attributes)
        {
            if (eventRepresentationEnum.IsObjectArrayEvent()) {
                IList<object> eventObjectArray = new List<object>();
                foreach (var attribute in attributes) {
                    var value = attribute.Split('=')[1];
                    eventObjectArray.Add(value);
                }

                env.SendEventObjectArray(eventObjectArray.ToArray(), eventName);
            }
            else if (eventRepresentationEnum.IsMapEvent()) {
                IDictionary<string, object> eventMap = new Dictionary<string, object>();
                foreach (var attribute in attributes) {
                    var key = attribute.Split('=')[0];
                    var value = attribute.Split('=')[1];
                    eventMap.Put(key, value);
                }

                env.SendEventMap(eventMap, eventName);
            }
            else if (eventRepresentationEnum.IsAvroEvent()) {
                var record = new GenericRecord(
                    SupportAvroUtil
                        .GetAvroSchema(env.Runtime.EventTypeService.GetEventTypePreconfigured(eventName))
                        .AsRecordSchema());
                foreach (var attribute in attributes) {
                    var key = attribute.Split('=')[0];
                    var value = attribute.Split('=')[1];
                    record.Put(key, value);
                }

                env.EventService.SendEventAvro(record, eventName);
            } else if (eventRepresentationEnum.IsJsonEvent() || eventRepresentationEnum.IsJsonProvidedClassEvent()) {
                var @event = new JObject();
                foreach (var attribute in attributes) {
                    var key = attribute.Split('=')[0];
                    var value = attribute.Split('=')[1];
                    @event.Add(key, value);
                }
                env.EventService.SendEventJson(@event.ToString(), eventName);
            }
            else {
                Assert.Fail();
            }
        }

        private static RegressionPath SetupInfraJoin(
            RegressionEnvironment env,
            bool namedWindow)
        {
            var eplCreateOne = namedWindow
                ? EventRepresentationChoice.MAP.GetAnnotationText() +
                  " create window Infra1#keepall (key String, keyJoin String)"
                : "create table Infra1 (key String primary key, keyJoin String)";
            var eplCreateTwo = namedWindow
                ? EventRepresentationChoice.MAP.GetAnnotationText() +
                  " create window Infra2#keepall (keyJoin String, value double)"
                : "create table Infra2 (keyJoin String primary key, value double primary key)";
            var path = new RegressionPath();
            env.CompileDeploy(eplCreateOne, path);
            env.CompileDeploy(eplCreateTwo, path);

            return path;
        }

        private static long GetCtxInfraCount(
            RegressionEnvironment env,
            RegressionPath path,
            int hashCode)
        {
            var compiled = env.CompileFAF("select count(*) as c0 from CtxInfra", path);
            var result = env.Runtime.FireAndForgetService.ExecuteQuery(
                compiled,
                new ContextPartitionSelector[] {new SupportSelectorByHashCode(hashCode)});
            return result.Array[0].Get("c0").AsInt64();
        }

        private static long GetCtxInfraCatCount(
            RegressionEnvironment env,
            RegressionPath path,
            string categoryName)
        {
            var compiled = env.CompileFAF("select count(*) as c0 from CtxInfraCat", path);
            var result = env.Runtime.FireAndForgetService.ExecuteQuery(
                compiled,
                new ContextPartitionSelector[] {new SupportSelectorCategory(categoryName)});
            return result.Array[0].Get("c0").AsInt64();
        }

        private static void RunAssertionFilter(
            RegressionEnvironment env,
            RegressionPath path,
            string query)
        {
            var fields = new [] { "TheString","IntPrimitive" };
            var result = env.CompileExecuteFAF(query, path);
            EPAssertionUtil.AssertPropsPerRow(
                result.GetEnumerator(),
                fields,
                new[] {new object[] {"E3", 5}});

            var compiled = env.CompileFAF(query, path);
            var prepared = env.Runtime.FireAndForgetService.PrepareQuery(compiled);
            EPAssertionUtil.AssertPropsPerRow(
                prepared.Execute().GetEnumerator(),
                fields,
                new[] {new object[] {"E3", 5}});
        }

        private static void RunQueryAssertCountNonNegative(
            RegressionEnvironment env,
            RegressionPath path,
            string epl,
            int count,
            string indexName,
            string backingClass)
        {
            SupportQueryPlanIndexHook.Reset();
            env.CompileExecuteFAF(epl, path);
            var actual = env
                .CompileExecuteFAF("select count(*) as c0 from MyInfra where IntPrimitive >= 0", path)
                .Array[0]
                .Get("c0")
                .AsInt64();
            Assert.AreEqual(count, actual);
            SupportQueryPlanIndexHook.AssertFAFAndReset(indexName, backingClass);
        }

        private static void RunQueryAssertCount(
            RegressionEnvironment env,
            RegressionPath path,
            string epl,
            int count,
            string indexName,
            string backingClass)
        {
            env.CompileExecuteFAF(epl, path);
            Assert.AreEqual(count, GetMyInfraCount(env, path));
            SupportQueryPlanIndexHook.AssertFAFAndReset(indexName, backingClass);
        }

        private static void InsertInfra1Event(
            RegressionEnvironment env,
            RegressionPath path,
            string key,
            string keyJoin)
        {
            env.CompileExecuteFAF("insert into Infra1 values ('" + key + "', '" + keyJoin + "')", path);
        }

        private static void InsertInfra2Event(
            RegressionEnvironment env,
            RegressionPath path,
            string keyJoin,
            double value)
        {
            env.CompileExecuteFAF("insert into Infra2 values ('" + keyJoin + "', " + value.RenderAny() + ")", path);
        }

        private static long GetMyInfraCount(
            RegressionEnvironment env,
            RegressionPath path)
        {
            return env.CompileExecuteFAF("select count(*) as c0 from MyInfra", path).Array[0].Get("c0").AsInt64();
        }

        private static EPFireAndForgetQueryResult CompileExecute(
            string faf,
            RegressionPath path,
            RegressionEnvironment env)
        {
            var compiled = env.CompileFAF(faf, path);
            return env.Runtime.FireAndForgetService.ExecuteQuery(compiled);
        }

        public class InfraSelectDistinct : RegressionExecution
        {
            private readonly bool namedWindow;

            public InfraSelectDistinct(bool namedWindow)
            {
                this.namedWindow = namedWindow;
            }

            public void Run(RegressionEnvironment env)
            {
                RegressionPath path;
                string query;
                string[] fields;
                EPFireAndForgetQueryResult result;

                // Non-join
                path = SetupInfra(env, namedWindow);

                env.SendEventBean(MakeBean("E1", 0, 10));
                env.SendEventBean(MakeBean("E2", 0, 10));
                env.SendEventBean(MakeBean("E3", 0, 11));

                query = "select distinct LongPrimitive from MyInfra order by LongPrimitive asc";
                fields = new [] { "LongPrimitive" };
                result = env.CompileExecuteFAF(query, path);
                EPAssertionUtil.AssertPropsPerRow(
                    result.Array,
                    fields,
                    new[] {new object[] {10L}, new object[] {11L}});

                env.UndeployAll();

                // Join
                path = SetupInfraJoin(env, namedWindow);
                InsertInfra1Event(env, path, "A", "X");
                InsertInfra1Event(env, path, "B", "Y");
                InsertInfra2Event(env, path, "X", 10);
                InsertInfra2Event(env, path, "Y", 10);

                query = "select distinct value from Infra1 as i1, Infra2 as i2 where i1.keyJoin = i2.keyJoin";
                fields = new [] { "value" };
                result = env.CompileExecuteFAF(query, path);
                EPAssertionUtil.AssertPropsPerRow(
                    result.Array,
                    fields,
                    new[] {new object[] {10d}});

                env.UndeployAll();
            }
        }

        public class InfraExecuteFilter : RegressionExecution
        {
            private readonly bool namedWindow;

            public InfraExecuteFilter(bool namedWindow)
            {
                this.namedWindow = namedWindow;
            }

            public void Run(RegressionEnvironment env)
            {
                var path = SetupInfra(env, namedWindow);

                env.SendEventBean(new SupportBean("E1", 0));
                env.SendEventBean(new SupportBean("E2", 11));
                env.SendEventBean(new SupportBean("E3", 5));

                var query = "select * from MyInfra(IntPrimitive > 1, IntPrimitive < 10)";
                RunAssertionFilter(env, path, query);

                query = "select * from MyInfra(IntPrimitive > 1) where IntPrimitive < 10";
                RunAssertionFilter(env, path, query);

                query = "select * from MyInfra where IntPrimitive < 10 and IntPrimitive > 1";
                RunAssertionFilter(env, path, query);

                env.UndeployAll();
            }
        }

        public class InfraInvalid : RegressionExecution
        {
            private readonly bool namedWindow;

            public InfraInvalid(bool namedWindow)
            {
                this.namedWindow = namedWindow;
            }

            public void Run(RegressionEnvironment env)
            {
                var path = SetupInfra(env, namedWindow);
                string epl;

                epl = "insert into MyInfra select 1";
                TryInvalidFAFCompile(
                    env,
                    path,
                    epl,
                    "Column '1' could not be assigned to any of the properties of the underlying type (missing column names, event property, setter method or constructor?) [insert into MyInfra select 1]");

                epl = "selectoo man";
                TryInvalidFAFCompile(env, path, epl, "Incorrect syntax near 'selectoo' [selectoo man]");

                epl = "select * from MyInfra output every 10 seconds";
                TryInvalidFAFCompile(
                    env,
                    path,
                    epl,
                    "Output rate limiting is not a supported feature of on-demand queries [select * from MyInfra output every 10 seconds]");

                epl = "select prev(1, TheString) from MyInfra";
                TryInvalidFAFCompile(
                    env,
                    path,
                    epl,
                    "Failed to validate select-clause expression 'prev(1,TheString)': Previous function cannot be used in this context [select prev(1, TheString) from MyInfra]");

                epl = "insert into MyInfra(IntPrimitive) select 'a'";
                if (namedWindow) {
                    TryInvalidFAFCompile(
                        env,
                        path,
                        epl,
                        "Invalid assignment of column 'IntPrimitive' of type 'System.String' to event property 'IntPrimitive' typed as 'System.Int32', column and parameter types mismatch [insert into MyInfra(IntPrimitive) select 'a']");
                }
                else {
                    TryInvalidFAFCompile(
                        env,
                        path,
                        epl,
                        "Invalid assignment of column 'IntPrimitive' of type 'System.String' to event property 'IntPrimitive' typed as 'System.Nullable<System.Int32>', column and parameter types mismatch [insert into MyInfra(IntPrimitive) select 'a']");
                }

                epl = "insert into MyInfra(IntPrimitive, TheString) select 1";
                TryInvalidFAFCompile(
                    env,
                    path,
                    epl,
                    "Number of supplied values in the select or values clause does not match insert-into clause [insert into MyInfra(IntPrimitive, TheString) select 1]");

                epl = "insert into MyInfra select 1 as IntPrimitive from MyInfra";
                TryInvalidFAFCompile(
                    env,
                    path,
                    epl,
                    "Insert-into fire-and-forget query can only consist of an insert-into clause and a select-clause [insert into MyInfra select 1 as IntPrimitive from MyInfra]");

                epl = "insert into MyInfra(IntPrimitive, TheString) values (1, 'a', 1)";
                TryInvalidFAFCompile(
                    env,
                    path,
                    epl,
                    "Number of supplied values in the select or values clause does not match insert-into clause [insert into MyInfra(IntPrimitive, TheString) values (1, 'a', 1)]");

                if (namedWindow) {
                    epl = "select * from pattern [every MyInfra]";
                    TryInvalidFAFCompile(
                        env,
                        path,
                        epl,
                        "On-demand queries require tables or named windows and do not allow event streams or patterns [select * from pattern [every MyInfra]]");

                    epl = "select * from MyInfra#uni(IntPrimitive)";
                    TryInvalidFAFCompile(
                        env,
                        path,
                        epl,
                        "Views are not a supported feature of on-demand queries [select * from MyInfra#uni(IntPrimitive)]");
                }

                epl = "on MyInfra select * from MyInfra";
                TryInvalidFAFCompile(
                    env,
                    path,
                    epl,
                    "Provided EPL expression is a continuous query expression (not an on-demand query)");

                env.UndeployAll();
            }
        }

        public class Infra3StreamInnerJoin : RegressionExecution
        {
            private readonly EventRepresentationChoice eventRepresentationEnum;
            private readonly bool namedWindow;

            public Infra3StreamInnerJoin(
                EventRepresentationChoice eventRepresentationEnum,
                bool namedWindow)
            {
                this.eventRepresentationEnum = eventRepresentationEnum;
                this.namedWindow = namedWindow;
            }

            public void Run(RegressionEnvironment env)
            {
                var eplEvents =
                    eventRepresentationEnum.GetAnnotationTextWJsonProvided<MyLocalJsonProvidedProduct>() + " create schema Product (ProductId string, CategoryId string);" +
                    eventRepresentationEnum.GetAnnotationTextWJsonProvided<MyLocalJsonProvidedCategory>() + " create schema Category (CategoryId string, Owner string);" +
                    eventRepresentationEnum.GetAnnotationTextWJsonProvided<MyLocalJsonProvidedProductOwnerDetails>() + " create schema ProductOwnerDetails (ProductId string, Owner string);";

                string epl;
                if (namedWindow) {
                    epl = eplEvents +
                          "create window WinProduct#keepall as select * from Product;" +
                          "create window WinCategory#keepall as select * from Category;" +
                          "create window WinProductOwnerDetails#keepall as select * from ProductOwnerDetails;" +
                          "insert into WinProduct select * from Product;" +
                          "insert into WinCategory select * from Category;" +
                          "insert into WinProductOwnerDetails select * from ProductOwnerDetails;";
                }
                else {
                    epl = eplEvents +
                          "create table WinProduct (ProductId string primary key, CategoryId string primary key);" +
                          "create table WinCategory (CategoryId string primary key, Owner string primary key);" +
                          "create table WinProductOwnerDetails (ProductId string primary key, Owner string);" +
                          "on Product t1 merge WinProduct t2 where t1.ProductId = t2.ProductId and t1.CategoryId = t2.CategoryId when not matched then insert select ProductId, CategoryId;" +
                          "on Category t1 merge WinCategory t2 where t1.CategoryId = t2.CategoryId when not matched then insert select CategoryId, Owner;" +
                          "on ProductOwnerDetails t1 merge WinProductOwnerDetails t2 where t1.ProductId = t2.ProductId when not matched then insert select ProductId, Owner;";
                }

                var path = new RegressionPath();
                env.CompileDeployWBusPublicType(epl, path);

                SendEvent(
                    eventRepresentationEnum,
                    env,
                    "Product",
                    new[] {"ProductId=Product1", "CategoryId=Category1"});
                SendEvent(
                    eventRepresentationEnum,
                    env,
                    "Product",
                    new[] {"ProductId=Product2", "CategoryId=Category1"});
                SendEvent(
                    eventRepresentationEnum,
                    env,
                    "Product",
                    new[] {"ProductId=Product3", "CategoryId=Category1"});
                SendEvent(
                    eventRepresentationEnum,
                    env,
                    "Category",
                    new[] {"CategoryId=Category1", "Owner=Petar"});
                SendEvent(
                    eventRepresentationEnum,
                    env,
                    "ProductOwnerDetails",
                    new[] {"ProductId=Product1", "Owner=Petar"});

                var fields = new [] { "WinProduct.ProductId" };
                EventBean[] queryResults;
                queryResults = env.CompileExecuteFAF(
                        "" +
                        "select WinProduct.ProductId " +
                        " from WinProduct" +
                        " inner join WinCategory on WinProduct.CategoryId=WinCategory.CategoryId" +
                        " inner join WinProductOwnerDetails on WinProduct.ProductId=WinProductOwnerDetails.ProductId",
                        path
                    )
                    .Array;
                EPAssertionUtil.AssertPropsPerRow(
                    queryResults,
                    fields,
                    new[] {new object[] {"Product1"}});

                queryResults = env.CompileExecuteFAF(
                        "" +
                        "select WinProduct.ProductId " +
                        " from WinProduct" +
                        " inner join WinCategory on WinProduct.CategoryId=WinCategory.CategoryId" +
                        " inner join WinProductOwnerDetails on WinProduct.ProductId=WinProductOwnerDetails.ProductId" +
                        " where WinCategory.Owner=WinProductOwnerDetails.Owner",
                        path
                    )
                    .Array;
                EPAssertionUtil.AssertPropsPerRow(
                    queryResults,
                    fields,
                    new[] {new object[] {"Product1"}});

                queryResults = env.CompileExecuteFAF(
                        "" +
                        "select WinProduct.ProductId " +
                        " from WinProduct, WinCategory, WinProductOwnerDetails" +
                        " where WinCategory.Owner=WinProductOwnerDetails.Owner" +
                        " and WinProduct.CategoryId=WinCategory.CategoryId" +
                        " and WinProduct.ProductId=WinProductOwnerDetails.ProductId",
                        path
                    )
                    .Array;
                EPAssertionUtil.AssertPropsPerRow(
                    queryResults,
                    fields,
                    new[] {new object[] {"Product1"}});

                var eplQuery = "select WinProduct.ProductId " +
                               " from WinProduct" +
                               " inner join WinCategory on WinProduct.CategoryId=WinCategory.CategoryId" +
                               " inner join WinProductOwnerDetails on WinProduct.ProductId=WinProductOwnerDetails.ProductId" +
                               " having WinCategory.Owner=WinProductOwnerDetails.Owner";
                queryResults = env.CompileExecuteFAF(eplQuery, path).Array;
                EPAssertionUtil.AssertPropsPerRow(
                    queryResults,
                    fields,
                    new[] {new object[] {"Product1"}});

                var model = env.EplToModel(eplQuery);
                queryResults = env.CompileExecuteFAF(model, path).Array;
                EPAssertionUtil.AssertPropsPerRow(
                    queryResults,
                    fields,
                    new[] {new object[] {"Product1"}});

                env.UndeployAll();
            }
        }

        public class InfraJoinWhere : RegressionExecution
        {
            private readonly bool namedWindow;

            public InfraJoinWhere(bool namedWindow)
            {
                this.namedWindow = namedWindow;
            }

            public void Run(RegressionEnvironment env)
            {
                var path = SetupInfraJoin(env, namedWindow);

                var queryAgg =
                    "select w1.key, sum(value) from Infra1 w1, Infra2 w2 WHERE w1.keyJoin = w2.keyJoin GROUP BY w1.key order by w1.key";
                var fieldsAgg = new [] { "w1.key","sum(value)" };
                var queryNoagg =
                    "select w1.key, w2.value from Infra1 w1, Infra2 w2 where w1.keyJoin = w2.keyJoin and value = 1 order by w1.key";
                var fieldsNoagg = new [] { "w1.key","w2.value" };

                var result = env.CompileExecuteFAF(queryAgg, path).Array;
                Assert.AreEqual(0, result.Length);
                result = env.CompileExecuteFAF(queryNoagg, path).Array;
                Assert.IsNull(result);

                InsertInfra1Event(env, path, "key1", "keyJoin1");

                result = env.CompileExecuteFAF(queryAgg, path).Array;
                Assert.AreEqual(0, result.Length);
                result = env.CompileExecuteFAF(queryNoagg, path).Array;
                Assert.IsNull(result);

                InsertInfra2Event(env, path, "keyJoin1", 1d);

                result = env.CompileExecuteFAF(queryAgg, path).Array;
                EPAssertionUtil.AssertPropsPerRow(
                    result,
                    fieldsAgg,
                    new[] {
                        new object[] {"key1", 1d}
                    });
                result = env.CompileExecuteFAF(queryNoagg, path).Array;
                EPAssertionUtil.AssertPropsPerRow(
                    result,
                    fieldsNoagg,
                    new[] {
                        new object[] {"key1", 1d}
                    });

                env.Milestone(0);

                InsertInfra2Event(env, path, "keyJoin2", 2d);

                result = env.CompileExecuteFAF(queryAgg, path).Array;
                EPAssertionUtil.AssertPropsPerRow(
                    result,
                    fieldsAgg,
                    new[] {new object[] {"key1", 1d}});
                result = env.CompileExecuteFAF(queryNoagg, path).Array;
                EPAssertionUtil.AssertPropsPerRow(
                    result,
                    fieldsNoagg,
                    new[] {new object[] {"key1", 1d}});

                InsertInfra1Event(env, path, "key2", "keyJoin2");

                result = env.CompileExecuteFAF(queryAgg, path).Array;
                EPAssertionUtil.AssertPropsPerRow(
                    result,
                    fieldsAgg,
                    new[] {new object[] {"key1", 1d}, new object[] {"key2", 2d}});
                result = env.CompileExecuteFAF(queryNoagg, path).Array;
                EPAssertionUtil.AssertPropsPerRow(
                    result,
                    fieldsNoagg,
                    new[] {new object[] {"key1", 1d}});

                InsertInfra2Event(env, path, "keyJoin2", 1d);

                result = env.CompileExecuteFAF(queryAgg, path).Array;
                EPAssertionUtil.AssertPropsPerRow(
                    result,
                    fieldsAgg,
                    new[] {new object[] {"key1", 1d}, new object[] {"key2", 3d}});
                result = env.CompileExecuteFAF(queryNoagg, path).Array;
                EPAssertionUtil.AssertPropsPerRow(
                    result,
                    fieldsNoagg,
                    new[] {new object[] {"key1", 1d}, new object[] {"key2", 1d}});

                env.UndeployAll();
            }
        }

        public class InfraAggUngroupedRowForEvent : RegressionExecution
        {
            private readonly bool namedWindow;

            public InfraAggUngroupedRowForEvent(bool namedWindow)
            {
                this.namedWindow = namedWindow;
            }

            public void Run(RegressionEnvironment env)
            {
                var path = SetupInfra(env, namedWindow);

                env.SendEventBean(new SupportBean("E1", 0));
                env.SendEventBean(new SupportBean("E2", 11));
                env.SendEventBean(new SupportBean("E3", 5));
                string[] fields = {"TheString", "total"};

                var query = "select TheString, sum(IntPrimitive) as total from MyInfra";
                var result = env.CompileExecuteFAF(query, path);
                EPAssertionUtil.AssertPropsPerRowAnyOrder(
                    result.GetEnumerator(),
                    fields,
                    new[] {new object[] {"E1", 16}, new object[] {"E2", 16}, new object[] {"E3", 16}});

                env.SendEventBean(new SupportBean("E4", -2));
                result = env.CompileExecuteFAF(query, path);
                EPAssertionUtil.AssertPropsPerRowAnyOrder(
                    result.GetEnumerator(),
                    fields,
                    new[] {
                        new object[] {"E1", 14}, new object[] {"E2", 14}, new object[] {"E3", 14},
                        new object[] {"E4", 14}
                    });

                env.UndeployAll();
            }
        }

        public class InfraJoin : RegressionExecution
        {
            private readonly bool isFirstNW;
            private readonly bool isSecondNW;

            public InfraJoin(
                bool isFirstNW,
                bool isSecondNW)
            {
                this.isFirstNW = isFirstNW;
                this.isSecondNW = isSecondNW;
            }

            public void Run(RegressionEnvironment env)
            {
                var path = SetupInfra(env, isFirstNW);

                var eplSecondCreate = isSecondNW
                    ? "create window MySecondInfra#keepall as select * from SupportBean_A"
                    : "create table MySecondInfra as (Id string primary key)";
                env.CompileDeploy(eplSecondCreate, path);
                var eplSecondFill = isSecondNW
                    ? "insert into MySecondInfra select * from SupportBean_A "
                    : "on SupportBean_A sba merge MySecondInfra msi where msi.Id = sba.Id when not matched then insert select Id";
                env.CompileDeploy(eplSecondFill, path);

                env.SendEventBean(new SupportBean("E1", 1));
                env.SendEventBean(new SupportBean("E2", 11));

                env.Milestone(0);

                env.SendEventBean(new SupportBean("E1", 5));
                env.SendEventBean(new SupportBean_A("E2"));
                string[] fields = {"TheString", "IntPrimitive", "Id"};

                var query = "select TheString, IntPrimitive, Id from MyInfra nw1, " +
                            "MySecondInfra nw2 where nw1.TheString = nw2.Id";
                var result = env.CompileExecuteFAF(query, path);
                EPAssertionUtil.AssertPropsPerRow(
                    result.GetEnumerator(),
                    fields,
                    new[] {new object[] {"E2", 11, "E2"}});

                env.SendEventBean(new SupportBean("E3", 1));
                env.SendEventBean(new SupportBean("E3", 2));
                env.SendEventBean(new SupportBean_A("E3"));

                result = env.CompileExecuteFAF(query, path);
                EPAssertionUtil.AssertPropsPerRowAnyOrder(
                    result.GetEnumerator(),
                    fields,
                    new[] {new object[] {"E2", 11, "E2"}, new object[] {"E3", 1, "E3"}, new object[] {"E3", 2, "E3"}});

                env.UndeployAll();
            }
        }

        public class InfraAggUngroupedRowForGroup : RegressionExecution
        {
            private readonly bool namedWindow;

            public InfraAggUngroupedRowForGroup(bool namedWindow)
            {
                this.namedWindow = namedWindow;
            }

            public void Run(RegressionEnvironment env)
            {
                var path = SetupInfra(env, namedWindow);

                env.SendEventBean(new SupportBean("E1", 1));
                env.SendEventBean(new SupportBean("E2", 11));
                env.SendEventBean(new SupportBean("E1", 5));
                string[] fields = {"TheString", "total"};

                var query =
                    "select TheString, sum(IntPrimitive) as total from MyInfra group by TheString order by TheString asc";
                var result = env.CompileExecuteFAF(query, path);
                EPAssertionUtil.AssertPropsPerRow(
                    result.GetEnumerator(),
                    fields,
                    new[] {new object[] {"E1", 6}, new object[] {"E2", 11}});

                env.SendEventBean(new SupportBean("E2", -2));
                env.SendEventBean(new SupportBean("E3", 3));
                result = env.CompileExecuteFAF(query, path);
                EPAssertionUtil.AssertPropsPerRow(
                    result.GetEnumerator(),
                    fields,
                    new[] {new object[] {"E1", 6}, new object[] {"E2", 9}, new object[] {"E3", 3}});

                env.UndeployAll();
            }
        }

        public class InfraInClause : RegressionExecution
        {
            private readonly bool namedWindow;

            public InfraInClause(bool namedWindow)
            {
                this.namedWindow = namedWindow;
            }

            public void Run(RegressionEnvironment env)
            {
                var path = SetupInfra(env, namedWindow);

                env.SendEventBean(MakeBean("E1", 10, 100L));
                env.SendEventBean(MakeBean("E2", 20, 200L));
                env.SendEventBean(MakeBean("E3", 30, 300L));
                env.SendEventBean(MakeBean("E4", 40, 400L));

                // try no index
                RunAssertionIn(env, path);

                // try suitable index
                env.CompileDeploy("@Name('stmtIdx1') create index Idx1 on MyInfra(TheString, IntPrimitive)", path);
                RunAssertionIn(env, path);
                env.UndeployModuleContaining("stmtIdx1");

                // backwards index
                env.CompileDeploy("@Name('stmtIdx2') create index Idx2 on MyInfra(IntPrimitive, TheString)", path);
                RunAssertionIn(env, path);
                env.UndeployModuleContaining("stmtIdx2");

                // partial index
                env.CompileDeploy("@Name('stmtIdx3') create index Idx3 on MyInfra(IntPrimitive)", path);
                RunAssertionIn(env, path);
                env.UndeployModuleContaining("stmtIdx3");

                env.UndeployAll();
            }
        }

        public class InfraAggUngroupedRowForAll : RegressionExecution
        {
            private readonly bool namedWindow;

            public InfraAggUngroupedRowForAll(bool namedWindow)
            {
                this.namedWindow = namedWindow;
            }

            public void Run(RegressionEnvironment env)
            {
                var path = SetupInfra(env, namedWindow);

                env.SendEventBean(new SupportBean("E1", 0));
                env.SendEventBean(new SupportBean("E2", 11));
                env.SendEventBean(new SupportBean("E3", 5));
                string[] fields = {"total"};

                var query = "select sum(IntPrimitive) as total from MyInfra";
                var result = env.CompileExecuteFAF(query, path);
                EPAssertionUtil.AssertPropsPerRow(
                    result.GetEnumerator(),
                    fields,
                    new[] {new object[] {16}});

                env.SendEventBean(new SupportBean("E4", -2));
                result = env.CompileExecuteFAF(query, path);
                EPAssertionUtil.AssertPropsPerRow(
                    result.GetEnumerator(),
                    fields,
                    new[] {new object[] {14}});

                env.UndeployAll();
            }
        }

        public class InfraSelectCountStar : RegressionExecution
        {
            private readonly bool namedWindow;

            public InfraSelectCountStar(bool namedWindow)
            {
                this.namedWindow = namedWindow;
            }

            public void Run(RegressionEnvironment env)
            {
                var path = SetupInfra(env, namedWindow);

                string[] fields = {"cnt"};
                var epl = "select count(*) as cnt from MyInfra";
                var query = env.CompileFAF(epl, path);
                var prepared = env.Runtime.FireAndForgetService.PrepareQuery(query);

                var result = env.Runtime.FireAndForgetService.ExecuteQuery(query);
                EPAssertionUtil.AssertPropsPerRow(
                    result.GetEnumerator(),
                    fields,
                    new[] {new object[] {0L}});
                EPAssertionUtil.AssertPropsPerRow(
                    prepared.Execute().GetEnumerator(),
                    fields,
                    new[] {new object[] {0L}});

                env.SendEventBean(new SupportBean("E1", 1));
                result = env.Runtime.FireAndForgetService.ExecuteQuery(query);
                EPAssertionUtil.AssertPropsPerRow(
                    result.GetEnumerator(),
                    fields,
                    new[] {new object[] {1L}});
                EPAssertionUtil.AssertPropsPerRow(
                    prepared.Execute().GetEnumerator(),
                    fields,
                    new[] {new object[] {1L}});
                result = env.Runtime.FireAndForgetService.ExecuteQuery(query);
                EPAssertionUtil.AssertPropsPerRow(
                    result.GetEnumerator(),
                    fields,
                    new[] {new object[] {1L}});
                EPAssertionUtil.AssertPropsPerRow(
                    prepared.Execute().GetEnumerator(),
                    fields,
                    new[] {new object[] {1L}});

                env.SendEventBean(new SupportBean("E2", 2));
                result = env.Runtime.FireAndForgetService.ExecuteQuery(query);
                EPAssertionUtil.AssertPropsPerRow(
                    result.GetEnumerator(),
                    fields,
                    new[] {new object[] {2L}});
                EPAssertionUtil.AssertPropsPerRow(
                    prepared.Execute().GetEnumerator(),
                    fields,
                    new[] {new object[] {2L}});

                var model = env.EplToModel(epl);
                var compiledFromModel = env.CompileFAF(model, path);
                result = env.CompileExecuteFAF(model, path);
                EPAssertionUtil.AssertPropsPerRow(
                    result.GetEnumerator(),
                    fields,
                    new[] {new object[] {2L}});
                EPAssertionUtil.AssertPropsPerRow(
                    prepared.Execute().GetEnumerator(),
                    fields,
                    new[] {new object[] {2L}});

                var preparedFromModel =
                    env.Runtime.FireAndForgetService.PrepareQuery(compiledFromModel);
                EPAssertionUtil.AssertPropsPerRow(
                    preparedFromModel.Execute().GetEnumerator(),
                    fields,
                    new[] {new object[] {2L}});

                env.UndeployAll();
            }
        }

        public class InfraSelectWildcard : RegressionExecution
        {
            private readonly bool namedWindow;

            public InfraSelectWildcard(bool namedWindow)
            {
                this.namedWindow = namedWindow;
            }

            public void Run(RegressionEnvironment env)
            {
                var path = SetupInfra(env, namedWindow);

                var query = "select * from MyInfra";
                var compiled = env.CompileFAF(query, path);
                var result = env.Runtime.FireAndForgetService.ExecuteQuery(compiled);
                string[] fields = {"TheString", "IntPrimitive"};
                EPAssertionUtil.AssertPropsPerRow(result.GetEnumerator(), fields, null);

                var prepared = env.Runtime.FireAndForgetService.PrepareQuery(compiled);
                EPAssertionUtil.AssertPropsPerRow(prepared.Execute().GetEnumerator(), fields, null);

                env.SendEventBean(new SupportBean("E1", 1));
                result = env.Runtime.FireAndForgetService.ExecuteQuery(compiled);
                EPAssertionUtil.AssertPropsPerRow(
                    result.GetEnumerator(),
                    fields,
                    new[] {new object[] {"E1", 1}});
                EPAssertionUtil.AssertPropsPerRow(
                    prepared.Execute().GetEnumerator(),
                    fields,
                    new[] {new object[] {"E1", 1}});

                env.SendEventBean(new SupportBean("E2", 2));
                result = env.Runtime.FireAndForgetService.ExecuteQuery(compiled);
                EPAssertionUtil.AssertPropsPerRowAnyOrder(
                    result.GetEnumerator(),
                    fields,
                    new[] {new object[] {"E1", 1}, new object[] {"E2", 2}});
                EPAssertionUtil.AssertPropsPerRowAnyOrder(
                    prepared.Execute().GetEnumerator(),
                    fields,
                    new[] {new object[] {"E1", 1}, new object[] {"E2", 2}});

                env.UndeployAll();
            }
        }

        public class InfraSelectWildcardSceneTwo : RegressionExecution
        {
            private readonly bool namedWindow;

            public InfraSelectWildcardSceneTwo(bool namedWindow)
            {
                this.namedWindow = namedWindow;
            }

            public void Run(RegressionEnvironment env)
            {
                string[] fields = {"key", "value"};
                var path = new RegressionPath();

                // create window
                var stmtTextCreate = namedWindow
                    ? "@Name('create') create window MyInfra.win:keepall() as select TheString as key, IntBoxed as value from SupportBean"
                    : "@Name('create') create table MyInfra (key string primary key, value int)";
                env.CompileDeploy(stmtTextCreate, path).AddListener("create");

                var stmtTextInsert =
                    "insert into MyInfra(key, value) select irstream TheString, IntBoxed from SupportBean";
                env.CompileDeploy(stmtTextInsert, path);

                env.Milestone(0);

                // send event
                SendBeanInt(env, "G1", 10);
                SendBeanInt(env, "G2", -1);
                SendBeanInt(env, "G3", -2);
                SendBeanInt(env, "G4", 21);

                env.Milestone(1);

                // perform query
                var result = env.CompileExecuteFAF("select * from MyInfra where value> 0", path);
                Assert.AreEqual(2, result.Array.Length);
                if (namedWindow) {
                    EPAssertionUtil.AssertPropsPerRow(
                        result.Array,
                        fields,
                        new[] {new object[] {"G1", 10}, new object[] {"G4", 21}});
                }
                else {
                    EPAssertionUtil.AssertPropsPerRowAnyOrder(
                        result.Array,
                        fields,
                        new[] {new object[] {"G1", 10}, new object[] {"G4", 21}});
                }

                env.Milestone(2);

                // perform query
                result = env.CompileExecuteFAF("select * from MyInfra where value < 0", path);
                Assert.AreEqual(2, result.Array.Length);
                EPAssertionUtil.AssertPropsPerRowAnyOrder(
                    result.Array,
                    fields,
                    new[] {new object[] {"G2", -1}, new object[] {"G3", -2}});

                // destroy all
                env.UndeployAll();
            }

            private SupportBean SendBeanInt(
                RegressionEnvironment env,
                string @string,
                int intBoxed)
            {
                var bean = new SupportBean();
                bean.TheString = @string;
                bean.IntBoxed = intBoxed;
                env.SendEventBean(bean);
                return bean;
            }
        }

        public class InfraDeleteContextPartitioned : RegressionExecution
        {
            private readonly bool namedWindow;

            public InfraDeleteContextPartitioned(bool namedWindow)
            {
                this.namedWindow = namedWindow;
            }

            public void Run(RegressionEnvironment env)
            {
                var path = new RegressionPath();

                // test hash-segmented context
                var eplCtx =
                    "create context MyCtx coalesce consistent_hash_crc32(TheString) from SupportBean granularity 4 preallocate";
                env.CompileDeploy(eplCtx, path);

                var eplCreate = namedWindow
                    ? "context MyCtx create window CtxInfra#keepall as SupportBean"
                    : "context MyCtx create table CtxInfra (TheString string primary key, IntPrimitive int primary key)";
                env.CompileDeploy(eplCreate, path);
                var eplPopulate = namedWindow
                    ? "context MyCtx insert into CtxInfra select * from SupportBean"
                    : "context MyCtx on SupportBean sb merge CtxInfra ci where sb.TheString = ci.TheString and sb.IntPrimitive = ci.IntPrimitive when not matched then insert select TheString, IntPrimitive";
                env.CompileDeploy(eplPopulate, path);

                var codeFunc = new SupportHashCodeFuncGranularCRC32(4);
                var codes = new int[5];
                for (var i = 0; i < 5; i++) {
                    env.SendEventBean(new SupportBean("E" + i, i));
                    codes[i] = codeFunc.CodeFor("E" + i);
                }

                EPAssertionUtil.AssertEqualsExactOrder(
                    new[] {3, 1, 3, 1, 2},
                    codes); // just to make sure CRC32 didn't change

                // assert counts individually per context partition
                AssertCtxInfraCountPerCode(env, path, new long[] {0, 2, 1, 2});

                // delete per context partition (E0 ended up in '3')
                CompileExecuteFAF(
                    env,
                    path,
                    "delete from CtxInfra where TheString = 'E0'",
                    new ContextPartitionSelector[] {new SupportSelectorByHashCode(1)});
                AssertCtxInfraCountPerCode(env, path, new long[] {0, 2, 1, 2});

                var result = CompileExecuteFAF(
                    env,
                    path,
                    "delete from CtxInfra where TheString = 'E0'",
                    new ContextPartitionSelector[] {new SupportSelectorByHashCode(3)});
                AssertCtxInfraCountPerCode(env, path, new long[] {0, 2, 1, 1});
                if (namedWindow) {
                    EPAssertionUtil.AssertPropsPerRow(
                        result.Array,
                        new [] { "TheString" },
                        new[] {new object[] {"E0"}});
                }

                // delete per context partition (E1 ended up in '1')
                CompileExecuteFAF(
                    env,
                    path,
                    "delete from CtxInfra where TheString = 'E1'",
                    new ContextPartitionSelector[] {new SupportSelectorByHashCode(0)});
                AssertCtxInfraCountPerCode(env, path, new long[] {0, 2, 1, 1});

                CompileExecuteFAF(
                    env,
                    path,
                    "delete from CtxInfra where TheString = 'E1'",
                    new ContextPartitionSelector[] {new SupportSelectorByHashCode(1)});
                AssertCtxInfraCountPerCode(env, path, new long[] {0, 1, 1, 1});
                env.UndeployAll();

                // test category-segmented context
                var eplCtxCategory =
                    "create context MyCtxCat group by IntPrimitive < 0 as negative, group by IntPrimitive > 0 as positive from SupportBean";
                env.CompileDeploy(eplCtxCategory, path);
                env.CompileDeploy("context MyCtxCat create window CtxInfraCat#keepall as SupportBean", path);
                env.CompileDeploy("context MyCtxCat insert into CtxInfraCat select * from SupportBean", path);

                env.SendEventBean(new SupportBean("E1", -2));
                env.SendEventBean(new SupportBean("E2", 1));
                env.SendEventBean(new SupportBean("E3", -3));
                env.SendEventBean(new SupportBean("E4", 2));
                Assert.AreEqual(2L, GetCtxInfraCatCount(env, path, "positive"));
                Assert.AreEqual(2L, GetCtxInfraCatCount(env, path, "negative"));

                result = env.CompileExecuteFAF(
                    "context MyCtxCat delete from CtxInfraCat where context.label = 'negative'",
                    path);
                Assert.AreEqual(2L, GetCtxInfraCatCount(env, path, "positive"));
                Assert.AreEqual(0L, GetCtxInfraCatCount(env, path, "negative"));
                EPAssertionUtil.AssertPropsPerRow(
                    result.Array,
                    new [] { "TheString" },
                    new[] {new object[] {"E1"}, new object[] {"E3"}});

                env.UndeployAll();
            }

            private EPFireAndForgetQueryResult CompileExecuteFAF(
                RegressionEnvironment env,
                RegressionPath path,
                string epl,
                ContextPartitionSelector[] selectors)
            {
                var compiled = env.CompileFAF(epl, path);
                return env.Runtime.FireAndForgetService.ExecuteQuery(compiled, selectors);
            }
        }

        public class InfraDelete : RegressionExecution
        {
            private readonly bool namedWindow;

            public InfraDelete(bool namedWindow)
            {
                this.namedWindow = namedWindow;
            }

            public void Run(RegressionEnvironment env)
            {
                var path = SetupInfra(env, namedWindow);

                // test delete-all
                for (var i = 0; i < 10; i++) {
                    env.SendEventBean(new SupportBean("E" + i, i));
                }

                Assert.AreEqual(10L, GetMyInfraCount(env, path));
                var result = env.CompileExecuteFAF("delete from MyInfra", path);
                Assert.AreEqual(0L, GetMyInfraCount(env, path));
                if (namedWindow) {
                    Assert.AreEqual(env.Statement("TheInfra").EventType, result.EventType);
                    Assert.AreEqual(10, result.Array.Length);
                    Assert.AreEqual("E0", result.Array[0].Get("TheString"));
                }
                else {
                    Assert.AreEqual(0, result.Array.Length);
                }

                // test SODA + where-clause
                for (var i = 0; i < 10; i++) {
                    env.SendEventBean(new SupportBean("E" + i, i));
                }

                Assert.AreEqual(10L, GetMyInfraCount(env, path));
                var eplWithWhere = "delete from MyInfra where TheString=\"E1\"";
                var modelWithWhere = env.EplToModel(eplWithWhere);
                Assert.AreEqual(eplWithWhere, modelWithWhere.ToEPL());
                result = env.CompileExecuteFAF(modelWithWhere, path);
                Assert.AreEqual(9L, GetMyInfraCount(env, path));
                if (namedWindow) {
                    Assert.AreEqual(env.Statement("TheInfra").EventType, result.EventType);
                    EPAssertionUtil.AssertPropsPerRow(
                        result.Array,
                        new [] { "TheString" },
                        new[] {new object[] {"E1"}});
                }

                // test SODA delete-all
                var eplDelete = "delete from MyInfra";
                var modelDeleteOnly = env.EplToModel(eplDelete);
                Assert.AreEqual(eplDelete, modelDeleteOnly.ToEPL());
                env.CompileExecuteFAF(modelDeleteOnly, path);
                Assert.AreEqual(0L, GetMyInfraCount(env, path));

                for (var i = 0; i < 5; i++) {
                    env.SendEventBean(new SupportBean("E" + i, i));
                }

                // test with index
                if (namedWindow) {
                    env.CompileDeploy("create unique index Idx1 on MyInfra (TheString)", path);
                }

                RunQueryAssertCount(
                    env,
                    path,
                    INDEX_CALLBACK_HOOK + "delete from MyInfra where TheString = 'E1' and IntPrimitive = 0",
                    5,
                    namedWindow ? "Idx1" : "MyInfra",
                    namedWindow ? BACKING_SINGLE_UNIQUE : BACKING_MULTI_UNIQUE);
                RunQueryAssertCount(
                    env,
                    path,
                    INDEX_CALLBACK_HOOK + "delete from MyInfra where TheString = 'E1' and IntPrimitive = 1",
                    4,
                    namedWindow ? "Idx1" : "MyInfra",
                    namedWindow ? BACKING_SINGLE_UNIQUE : BACKING_MULTI_UNIQUE);
                RunQueryAssertCount(
                    env,
                    path,
                    INDEX_CALLBACK_HOOK + "delete from MyInfra where TheString = 'E2'",
                    3,
                    namedWindow ? "Idx1" : null,
                    namedWindow ? BACKING_SINGLE_UNIQUE : null);
                RunQueryAssertCount(
                    env,
                    path,
                    INDEX_CALLBACK_HOOK + "delete from MyInfra where IntPrimitive = 4",
                    2,
                    null,
                    null);

                // test with alias
                RunQueryAssertCount(
                    env,
                    path,
                    INDEX_CALLBACK_HOOK + "delete from MyInfra as w1 where w1.TheString = 'E3'",
                    1,
                    namedWindow ? "Idx1" : null,
                    namedWindow ? BACKING_SINGLE_UNIQUE : null);

                // test consumption
                env.CompileDeploy("@Name('s0') select rstream * from MyInfra", path).AddListener("s0");
                env.CompileExecuteFAF("delete from MyInfra", path);
                string[] fields = {"TheString", "IntPrimitive"};
                if (namedWindow) {
                    EPAssertionUtil.AssertProps(
                        env.Listener("s0").AssertOneGetNewAndReset(),
                        fields,
                        new object[] {"E0", 0});
                }
                else {
                    Assert.IsFalse(env.Listener("s0").IsInvoked);
                }

                env.UndeployAll();
            }
        }

        public class InfraUpdate : RegressionExecution
        {
            private readonly bool namedWindow;

            public InfraUpdate(bool namedWindow)
            {
                this.namedWindow = namedWindow;
            }

            public void Run(RegressionEnvironment env)
            {
                var path = SetupInfra(env, namedWindow);
                string[] fields = {"TheString", "IntPrimitive"};

                /*
                // test update-all
                for (var i = 0; i < 2; i++) {
                    env.SendEventBean(new SupportBean("E" + i, i));
                }

                var result = CompileExecute("update MyInfra set TheString = 'ABC'", path, env);
                EPAssertionUtil.AssertPropsPerRowAnyOrder(
                    env.GetEnumerator("TheInfra"),
                    fields,
                    new[] {
                        new object[] {"ABC", 0},
                        new object[] {"ABC", 1}
                    });
                if (namedWindow) {
                    EPAssertionUtil.AssertPropsPerRowAnyOrder(
                        result.Array,
                        fields,
                        new[] {
                            new object[] {"ABC", 0}, 
                            new object[] {"ABC", 1}
                        });
                }

                // test update with where-clause
                env.CompileExecuteFAF("delete from MyInfra", path);
                for (var i = 0; i < 3; i++) {
                    env.SendEventBean(new SupportBean("E" + i, i));
                }

                result = env.CompileExecuteFAF(
                    "update MyInfra set TheString = 'X', IntPrimitive=-1 where TheString = 'E1' and IntPrimitive = 1",
                    path);
                if (namedWindow) {
                    EPAssertionUtil.AssertPropsPerRow(
                        result.Array,
                        fields,
                        new[] {
                            new object[] {"X", -1}
                        });
                }

                EPAssertionUtil.AssertPropsPerRowAnyOrder(
                    env.GetEnumerator("TheInfra"),
                    fields,
                    new[] {
                        new object[] {"E0", 0}, 
                        new object[] {"E2", 2}, 
                        new object[] {"X", -1}
                    });

                // test update with SODA
                var epl = "update MyInfra set IntPrimitive=IntPrimitive+10 where TheString=\"E2\"";
                var model = env.EplToModel(epl);
                Assert.AreEqual(epl, model.ToEPL());
                result = env.CompileExecuteFAF(model, path);
                if (namedWindow) {
                    EPAssertionUtil.AssertPropsPerRow(
                        result.Array,
                        fields,
                        new[] {
                            new object[] {"E2", 12}
                        });
                }

                EPAssertionUtil.AssertPropsPerRowAnyOrder(
                    env.GetEnumerator("TheInfra"),
                    fields,
                    new[] {
                        new object[] {"E0", 0}, 
                        new object[] {"X", -1},
                        new object[] {"E2", 12}
                    });

                // test update with initial value
                result = env.CompileExecuteFAF(
                    "update MyInfra set IntPrimitive=5, TheString='x', TheString = initial.TheString || 'y', IntPrimitive=initial.IntPrimitive+100 where TheString = 'E0'",
                    path);
                if (namedWindow) {
                    EPAssertionUtil.AssertPropsPerRow(
                        result.Array,
                        fields,
                        new[] {
                            new object[] {"E0y", 100}
                        });
                }

                EPAssertionUtil.AssertPropsPerRowAnyOrder(
                    env.GetEnumerator("TheInfra"),
                    fields,
                    new[] {
                        new object[] {"X", -1}, 
                        new object[] {"E2", 12}, 
                        new object[] {"E0y", 100}
                    });

                env.CompileExecuteFAF("delete from MyInfra", path);
                for (var i = 0; i < 5; i++) {
                    env.SendEventBean(new SupportBean("E" + i, i));
                }

                // test with index
                if (namedWindow) {
                    env.CompileDeploy("create unique index Idx1 on MyInfra (TheString)", path);
                }

                RunQueryAssertCountNonNegative(
                    env,
                    path,
                    INDEX_CALLBACK_HOOK +
                    "update MyInfra set IntPrimitive=-1 where TheString = 'E1' and IntPrimitive = 0",
                    5,
                    namedWindow ? "Idx1" : "MyInfra",
                    namedWindow ? BACKING_SINGLE_UNIQUE : BACKING_MULTI_UNIQUE);
                RunQueryAssertCountNonNegative(
                    env,
                    path,
                    INDEX_CALLBACK_HOOK +
                    "update MyInfra set IntPrimitive=-1 where TheString = 'E1' and IntPrimitive = 1",
                    4,
                    namedWindow ? "Idx1" : "MyInfra",
                    namedWindow ? BACKING_SINGLE_UNIQUE : BACKING_MULTI_UNIQUE);
                RunQueryAssertCountNonNegative(
                    env,
                    path,
                    INDEX_CALLBACK_HOOK + "update MyInfra set IntPrimitive=-1 where TheString = 'E2'",
                    3,
                    namedWindow ? "Idx1" : null,
                    namedWindow ? BACKING_SINGLE_UNIQUE : null);
                RunQueryAssertCountNonNegative(
                    env,
                    path,
                    INDEX_CALLBACK_HOOK + "update MyInfra set IntPrimitive=-1 where IntPrimitive = 4",
                    2,
                    null,
                    null);

                // test with alias
                RunQueryAssertCountNonNegative(
                    env,
                    path,
                    INDEX_CALLBACK_HOOK + "update MyInfra as w1 set IntPrimitive=-1 where w1.TheString = 'E3'",
                    1,
                    namedWindow ? "Idx1" : null,
                    namedWindow ? BACKING_SINGLE_UNIQUE : null);

                // test consumption
                env.CompileDeploy("@Name('s0') select irstream * from MyInfra", path).AddListener("s0");
                env.CompileExecuteFAF("update MyInfra set IntPrimitive=1000 where TheString = 'E0'", path);
                if (namedWindow) {
                    EPAssertionUtil.AssertProps(
                        env.Listener("s0").AssertPairGetIRAndReset(),
                        fields,
                        new object[] {"E0", 1000},
                        new object[] {"E0", 0});
                }
                */

                // test update via UDF and setter
                if (namedWindow) {
                    env.CompileExecuteFAF("delete from MyInfra", path);
                    env.SendEventBean(new SupportBean("A", 10));
                    env.CompileExecuteFAF("update MyInfra mw set mw.SetTheString('XYZ'), doubleInt(mw)", path);
                    EPAssertionUtil.AssertPropsPerRow(
                        env.GetEnumerator("TheInfra"),
                        new [] { "TheString","IntPrimitive" },
                        new[] {
                            new object[] {"XYZ", 20}
                        });
                }

                env.UndeployAll();
                
                path.Clear();
                    
                // test update using array-assignment; this is mostly tested via on-merge otherwise
                String eplInfra = namedWindow ?
                    "@Name('TheInfra') create window MyInfra#keepall as (mydoubles double[primitive]);\n" :
                    "@Name('TheInfra') create table MyInfra as (mydoubles double[primitive])";
                env.CompileDeploy(eplInfra, path);
                env.CompileExecuteFAF("insert into MyInfra select new double[] {1, 2, 3} as mydoubles", path);
                env.CompileExecuteFAF("update MyInfra set mydoubles[3-2] = 4", path);
                EPFireAndForgetQueryResult resultDoubles = env.CompileExecuteFAF("select * from MyInfra", path);
                CollectionAssert.AreEquivalent(
                    new double[] {1, 4, 3},
                    resultDoubles.Array[0].Get("mydoubles").UnwrapIntoArray<double>());

                env.UndeployAll();
            }
        }

        public class InfraInsert : RegressionExecution
        {
            private readonly bool namedWindow;

            public InfraInsert(bool namedWindow)
            {
                this.namedWindow = namedWindow;
            }

            public void Run(RegressionEnvironment env)
            {
                var path = SetupInfra(env, namedWindow);

                Supplier<EPStatement> stmt = () => env.Statement("TheInfra");
                var propertyNames = new [] { "TheString","IntPrimitive" };

                // try column name provided with insert-into
                var eplSelect = "insert into MyInfra (TheString, IntPrimitive) select 'a', 1";
                var resultOne = env.CompileExecuteFAF(eplSelect, path);
                AssertFAFInsertResult(
                    resultOne,
                    new object[] {"a", 1},
                    propertyNames,
                    stmt.Invoke(),
                    namedWindow);
                EPAssertionUtil.AssertPropsPerRow(
                    stmt.Invoke().GetEnumerator(),
                    propertyNames,
                    new[] {new object[] {"a", 1}});

                // try SODA and column name not provided with insert-into
                var eplTwo = "insert into MyInfra select \"b\" as TheString, 2 as IntPrimitive";
                var modelWSelect = env.EplToModel(eplTwo);
                Assert.AreEqual(eplTwo, modelWSelect.ToEPL());
                var resultTwo = env.CompileExecuteFAF(modelWSelect, path);
                AssertFAFInsertResult(
                    resultTwo,
                    new object[] {"b", 2},
                    propertyNames,
                    stmt.Invoke(),
                    namedWindow);
                EPAssertionUtil.AssertPropsPerRowAnyOrder(
                    stmt.Invoke().GetEnumerator(),
                    propertyNames,
                    new[] {new object[] {"a", 1}, new object[] {"b", 2}});

                // create unique index, insert duplicate row
                env.CompileDeploy("create unique index I1 on MyInfra (TheString)", path);
                try {
                    var eplThree = "insert into MyInfra (TheString) select 'a' as TheString";
                    env.CompileExecuteFAF(eplThree, path);
                }
                catch (EPException ex) {
                    Assert.AreEqual(
                        "Unique index violation, index 'I1' is a unique index and key 'a' already exists",
                        ex.Message);
                }

                EPAssertionUtil.AssertPropsPerRowAnyOrder(
                    env.GetEnumerator("TheInfra"),
                    propertyNames,
                    new[] {new object[] {"a", 1}, new object[] {"b", 2}});

                // try second no-column-provided version
                var eplMyInfraThree = namedWindow
                    ? "@Name('InfraThree') create window MyInfraThree#keepall as (p0 string, p1 int)"
                    : "@Name('InfraThree') create table MyInfraThree as (p0 string, p1 int)";
                env.CompileDeploy(eplMyInfraThree, path);
                env.CompileExecuteFAF("insert into MyInfraThree select 'a' as p0, 1 as p1", path);
                EPAssertionUtil.AssertPropsPerRowAnyOrder(
                    env.GetEnumerator("InfraThree"),
                    new [] { "p0","p1" },
                    new[] {new object[] {"a", 1}});

                // try enum-value insert
                var epl = "create schema MyMode (Mode " + nameof(SupportEnum) + ");\n" +
                          (namedWindow
                              ? "@Name('enumwin') create window MyInfraTwo#unique(Mode) as MyMode"
                              : "@Name('enumwin') create table MyInfraTwo as (Mode " + nameof(SupportEnum) + ");\n");
                env.CompileDeploy(epl, path);
                env.CompileExecuteFAF(
                    "insert into MyInfraTwo select " +
                    typeof(SupportEnum).FullName +
                    "." +
                    SupportEnum.ENUM_VALUE_2.GetName() +
                    " as Mode",
                    path);
                EPAssertionUtil.AssertProps(
                    env.GetEnumerator("enumwin").Advance(),
                    new [] { "Mode" },
                    new object[] {SupportEnum.ENUM_VALUE_2});

                // try insert-into with values-keyword and explicit column names
                env.CompileExecuteFAF("delete from MyInfra", path);
                var eplValuesKW = "insert into MyInfra(TheString, IntPrimitive) values (\"a\", 1)";
                var resultValuesKW = env.CompileExecuteFAF(eplValuesKW, path);
                AssertFAFInsertResult(
                    resultValuesKW,
                    new object[] {"a", 1},
                    propertyNames,
                    stmt.Invoke(),
                    namedWindow);
                EPAssertionUtil.AssertPropsPerRowAnyOrder(
                    stmt.Invoke().GetEnumerator(),
                    propertyNames,
                    new[] {new object[] {"a", 1}});

                // try insert-into with values model
                env.CompileExecuteFAF("delete from MyInfra", path);
                var modelWValuesKW = env.EplToModel(eplValuesKW);
                Assert.AreEqual(eplValuesKW, modelWValuesKW.ToEPL());
                env.CompileExecuteFAF(modelWValuesKW, path);
                EPAssertionUtil.AssertPropsPerRowAnyOrder(
                    stmt.Invoke().GetEnumerator(),
                    propertyNames,
                    new[] {new object[] {"a", 1}});

                // try insert-into with values-keyword and as-names
                env.CompileExecuteFAF("delete from MyInfraThree", path);
                var eplValuesWithoutCols = "insert into MyInfraThree values ('b', 2)";
                env.CompileExecuteFAF(eplValuesWithoutCols, path);
                EPAssertionUtil.AssertPropsPerRowAnyOrder(
                    env.GetEnumerator("InfraThree"),
                    new [] { "p0","p1" },
                    new[] {new object[] {"b", 2}});

                env.UndeployAll();
            }
        }
        
        [Serializable]
        public class MyLocalJsonProvidedProduct {
            public String ProductId;
            public String CategoryId;
        }

        [Serializable]
        public  class MyLocalJsonProvidedCategory {
            public String CategoryId;
            public String Owner;
        }

        [Serializable]
        public class MyLocalJsonProvidedProductOwnerDetails  {
            public String ProductId;
            public String Owner;
        }
    }
} // end of namespace
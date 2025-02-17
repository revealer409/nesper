///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using com.espertech.esper.common.client.configuration.compiler;
using com.espertech.esper.common.client.soda;
using com.espertech.esper.common.@internal.support;
using com.espertech.esper.regressionlib.suite.epl.other;
using com.espertech.esper.regressionlib.support.bean;
using com.espertech.esper.regressionlib.support.epl;
using com.espertech.esper.regressionrun.runner;
using com.espertech.esper.regressionrun.suite.core;

using NUnit.Framework;

namespace com.espertech.esper.regressionrun.suite.epl
{
    [TestFixture]
    public class TestSuiteEPLOtherWConfig : AbstractTestContainer
    {
        [Test, RunInApplicationDomain]
        public void TestEPLOtherIStreamRStreamConfigSelectorIRStream()
        {
            using (var session = RegressionRunner.Session(Container)) {
                session.Configuration.Compiler.StreamSelection.DefaultStreamSelector = StreamSelector.RSTREAM_ISTREAM_BOTH;
                session.Configuration.Common.AddEventType(typeof(SupportBean));
                RegressionRunner.Run(session, new EPLOtherIStreamRStreamConfigSelectorIRStream());
            }
        }

        [Test, RunInApplicationDomain]
        public void TestEPLOtherIStreamRStreamConfigSelectorRStream()
        {
            using (var session = RegressionRunner.Session(Container)) {
                session.Configuration.Compiler.StreamSelection.DefaultStreamSelector = StreamSelector.RSTREAM_ONLY;
                session.Configuration.Common.AddEventType(typeof(SupportBean));
                RegressionRunner.Run(session, new EPLOtherIStreamRStreamConfigSelectorRStream());
            }
        }

        [Test, RunInApplicationDomain]
        public void TestEPLOtherStaticFunctionsNoUDFCache()
        {
            using (var session = RegressionRunner.Session(Container)) {
                session.Configuration.Common.AddImportType(typeof(SupportStaticMethodLib));
                session.Configuration.Compiler.AddPlugInSingleRowFunction(
                    "sleepme",
                    typeof(SupportStaticMethodLib),
                    "Sleep",
                    ConfigurationCompilerPlugInSingleRowFunction.ValueCacheEnum.ENABLED);
                session.Configuration.Compiler.Expression.UdfCache = false;
                session.Configuration.Common.AddEventType(typeof(SupportTemperatureBean));
                RegressionRunner.Run(session, new EPLOtherStaticFunctionsNoUDFCache());
            }
        }
    }
} // end of namespace
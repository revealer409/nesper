///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;

using com.espertech.esper.common.@internal.support;
using com.espertech.esper.compat;
using com.espertech.esper.regressionlib.suite.resultset.aggregate;
using com.espertech.esper.regressionlib.support.bean;
using com.espertech.esper.regressionrun.runner;
using com.espertech.esper.regressionrun.suite.core;

using NUnit.Framework;

namespace com.espertech.esper.regressionrun.suite.resultset
{
    [TestFixture]
    public class TestSuiteResultSetAggregateWConfig : AbstractTestContainer
    {
        [Test, RunInApplicationDomain]
        public void TestResultSetAggregateFilteredWMathContext()
        {
            using RegressionSession session = RegressionRunner.Session(Container);
            session.Configuration.Compiler.Expression.MathContext = new MathContext(MidpointRounding.AwayFromZero, 2);
            session.Configuration.Common.AddEventType(typeof(SupportBeanNumeric));
            RegressionRunner.Run(session, new ResultSetAggregateFilteredWMathContext());
        }

        [Test, RunInApplicationDomain]
        public void TestResultSetAggregateExtInvalid()
        {
            using RegressionSession session = RegressionRunner.Session(Container);
            session.Configuration.Compiler.Expression.ExtendedAggregation = false;
            session.Configuration.Common.AddEventType(typeof(SupportBean));
            RegressionRunner.Run(session, new ResultSetAggregateExtInvalid());
        }
    }
} // end of namespace
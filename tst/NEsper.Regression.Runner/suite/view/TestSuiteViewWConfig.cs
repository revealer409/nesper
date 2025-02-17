///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

using com.espertech.esper.common.@internal.support;
using com.espertech.esper.compat;
using com.espertech.esper.compat.datetime;
using com.espertech.esper.regressionlib.framework;
using com.espertech.esper.regressionlib.suite.view;
using com.espertech.esper.regressionlib.support.bean;
using com.espertech.esper.regressionrun.runner;
using com.espertech.esper.regressionrun.suite.core;

using NUnit.Framework;

using static com.espertech.esper.regressionlib.support.schedule.SupportDateTimeUtil;

namespace com.espertech.esper.regressionrun.suite.view
{
    [TestFixture]
    public class TestSuiteViewWConfig : AbstractTestContainer
    {
        [Test, RunInApplicationDomain]
        public void TestViewGroupMicroseconds()
        {
            using var session = RegressionRunner.Session(Container);
            ConfigureMicroseconds(session);
            RegressionRunner.Run(session, new ViewGroup.ViewGroupReclaimWithFlipTime(5000000));
        }

        [Test, RunInApplicationDomain]
        public void TestViewTimeMicrosecondsWinFlipTime()
        {
            var execs = new List<RegressionExecution>();

            execs.Add(new ViewTimeWin.ViewTimeWindowFlipTimer(0, "1", 1000000));
            execs.Add(new ViewTimeWin.ViewTimeWindowFlipTimer(0, "10 milliseconds", 10000));
            execs.Add(new ViewTimeWin.ViewTimeWindowFlipTimer(0, "10 microseconds", 10));
            execs.Add(new ViewTimeWin.ViewTimeWindowFlipTimer(0, "1 seconds 10 microseconds", 1000010));
            execs.Add(new ViewTimeWin.ViewTimeWindowFlipTimer(123456789, "10", 123456789 + 10 * 1000000));
            execs.Add(new ViewTimeWin.ViewTimeWindowFlipTimer(0, "1 months 10 microseconds", TimePlusMonth(0, 1) * 1000 + 10));

            var currentTime = DateTimeParsingFunctions.ParseDefaultMSec("2002-05-1T08:00:01.999");
            execs.Add(new ViewTimeWin.ViewTimeWindowFlipTimer(currentTime * 1000 + 33, "3 months 100 microseconds", TimePlusMonth(currentTime, 3) * 1000 + 33 + 100));

            using var session = RegressionRunner.Session(Container);
            ConfigureMicroseconds(session);
            RegressionRunner.Run(session, execs);
        }

        [Test, RunInApplicationDomain]
        public void TestViewTimeBatchWSystemTime()
        {
            using var session = RegressionRunner.Session(Container);
            session.Configuration.Common.AddEventType(typeof(SupportMarketDataBean));
            session.Configuration.Runtime.Threading.IsInternalTimerEnabled = true;
            RegressionRunner.Run(session, new ViewTimeBatchWSystemTime());
        }

        [Test, RunInApplicationDomain]
        public void TestViewTimeWinWSystemTime()
        {
            using var session = RegressionRunner.Session(Container);
            session.Configuration.Common.AddEventType(typeof(SupportMarketDataBean));
            session.Configuration.Runtime.Threading.IsInternalTimerEnabled = true;
            RegressionRunner.Run(session, new ViewTimeWinWSystemTime());
        }

        private void ConfigureMicroseconds(RegressionSession session)
        {
            session.Configuration.Common.AddEventType(typeof(SupportBean));
            session.Configuration.Common.TimeSource.TimeUnit = TimeUnit.MICROSECONDS;
        }
    }
} // end of namespace
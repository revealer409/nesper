///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

using com.espertech.esper.common.client;
using com.espertech.esper.common.client.context;
using com.espertech.esper.common.client.scopetest;
using com.espertech.esper.common.@internal.support;
using com.espertech.esper.compat;
using com.espertech.esper.compat.collections;
using com.espertech.esper.compat.datetime;
using com.espertech.esper.regressionlib.framework;
using com.espertech.esper.regressionlib.support.context;
using com.espertech.esper.regressionlib.support.filter;
using com.espertech.esper.regressionlib.support.util;

using NUnit.Framework;

namespace com.espertech.esper.regressionlib.suite.context
{
    public class ContextInitTermTemporalFixed
    {
        public static IList<RegressionExecution> Executions()
        {
            var execs = new List<RegressionExecution>();
            WithEndContextPartitionSelection(execs);
            WithEndFilterStartedFilterEndedCorrelatedOutputSnapshot(execs);
            WithEndFilterStartedPatternEndedCorrelated(execs);
            WithEndStartAfterEndAfter(execs);
            WithEndFilterStartedFilterEndedOutputSnapshot(execs);
            WithEndPatternStartedPatternEnded(execs);
            WithEndContextCreateDestroy(execs);
            WithEndPrevPriorAndAggregation(execs);
            WithEndJoin(execs);
            WithEndPatternWithTime(execs);
            WithEndSubselect(execs);
            WithEndSubselectCorrelated(execs);
            WithEndNWSameContextOnExpr(execs);
            WithEndNWFireAndForget(execs);
            WithEndStartTurnedOff(execs);
            WithEndStartTurnedOn(execs);
            With9End5AggUngrouped(execs);
            With9End5AggGrouped(execs);
            WithEndDBHistorical(execs);
            WithEndMultiCrontab(execs);
            return execs;
        }

        public static IList<RegressionExecution> WithEndMultiCrontab(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new ContextStartEndMultiCrontab());
            return execs;
        }

        public static IList<RegressionExecution> WithEndDBHistorical(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new ContextStartEndDBHistorical());
            return execs;
        }

        public static IList<RegressionExecution> With9End5AggGrouped(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new ContextStart9End5AggGrouped());
            return execs;
        }

        public static IList<RegressionExecution> With9End5AggUngrouped(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new ContextStart9End5AggUngrouped());
            return execs;
        }

        public static IList<RegressionExecution> WithEndStartTurnedOn(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new ContextStartEndStartTurnedOn());
            return execs;
        }

        public static IList<RegressionExecution> WithEndStartTurnedOff(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new ContextStartEndStartTurnedOff());
            return execs;
        }

        public static IList<RegressionExecution> WithEndNWFireAndForget(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new ContextStartEndNWFireAndForget());
            return execs;
        }

        public static IList<RegressionExecution> WithEndNWSameContextOnExpr(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new ContextStartEndNWSameContextOnExpr());
            return execs;
        }

        public static IList<RegressionExecution> WithEndSubselectCorrelated(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new ContextStartEndSubselectCorrelated());
            return execs;
        }

        public static IList<RegressionExecution> WithEndSubselect(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new ContextStartEndSubselect());
            return execs;
        }

        public static IList<RegressionExecution> WithEndPatternWithTime(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new ContextStartEndPatternWithTime());
            return execs;
        }

        public static IList<RegressionExecution> WithEndJoin(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new ContextStartEndJoin());
            return execs;
        }

        public static IList<RegressionExecution> WithEndPrevPriorAndAggregation(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new ContextStartEndPrevPriorAndAggregation());
            return execs;
        }

        public static IList<RegressionExecution> WithEndContextCreateDestroy(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new ContextStartEndContextCreateDestroy());
            return execs;
        }

        public static IList<RegressionExecution> WithEndPatternStartedPatternEnded(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new ContextStartEndPatternStartedPatternEnded());
            return execs;
        }

        public static IList<RegressionExecution> WithEndFilterStartedFilterEndedOutputSnapshot(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new ContextStartEndFilterStartedFilterEndedOutputSnapshot());
            return execs;
        }

        public static IList<RegressionExecution> WithEndStartAfterEndAfter(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new ContextStartEndStartAfterEndAfter());
            return execs;
        }

        public static IList<RegressionExecution> WithEndFilterStartedPatternEndedCorrelated(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new ContextStartEndFilterStartedPatternEndedCorrelated());
            return execs;
        }

        public static IList<RegressionExecution> WithEndFilterStartedFilterEndedCorrelatedOutputSnapshot(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new ContextStartEndFilterStartedFilterEndedCorrelatedOutputSnapshot());
            return execs;
        }

        public static IList<RegressionExecution> WithEndContextPartitionSelection(IList<RegressionExecution> execs = null)
        {
            execs = execs ?? new List<RegressionExecution>();
            execs.Add(new ContextStartEndContextPartitionSelection());
            return execs;
        }

        private static void AssertContextEventType(EventType eventType)
        {
            Assert.AreEqual(0, eventType.PropertyNames.Length);
            Assert.AreEqual("stmt0_ctxout_NineToFive_1", eventType.Name);
        }

        private static void SendTimeAndAssert(
            RegressionEnvironment env,
            string time,
            bool IsInvoked,
            string statementNames)
        {
            SendTimeEvent(env, time);
            env.SendEventBean(new SupportBean());

            foreach (var statement in statementNames.SplitCsv()) {
                var listener = env.Listener(statement);
                Assert.AreEqual(IsInvoked, listener.GetAndClearIsInvoked(), "Failed for statement " + statement);
            }
        }

        private static void SendTimeEvent(
            RegressionEnvironment env,
            string time)
        {
            env.AdvanceTime(DateTimeParsingFunctions.ParseDefaultMSec(time));
        }

        private static void SendTimeEvent(
            RegressionEnvironment env,
            long time)
        {
            env.AdvanceTime(time);
        }

        private static void SendEventAndAssert(
            RegressionEnvironment env,
            bool expected)
        {
            env.SendEventBean(new SupportBean());
            Assert.AreEqual(expected, env.Listener("s0").IsInvoked);
            env.Listener("s0").Reset();
        }

        public static SupportBean SingleRowPluginMakeBean(
            int id,
            string p00)
        {
            return new SupportBean(p00, id);
        }

        internal class ContextStartEndContextPartitionSelection : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                var fields = new[] { "c0", "c1", "c2", "c3" };
                env.AdvanceTime(0);
                var path = new RegressionPath();

                env.CompileDeploy("create context MyCtx as start SupportBean_S0 S0 end SupportBean_S1(Id=S0.Id)", path);
                env.CompileDeploy(
                    "@Name('s0') context MyCtx select context.id as c0, context.S0.P00 as c1, TheString as c2, sum(IntPrimitive) as c3 from SupportBean#keepall group by TheString",
                    path);

                env.AdvanceTime(1000);
                env.SendEventBean(new SupportBean_S0(1, "S0_1"));

                env.Milestone(0);

                env.SendEventBean(new SupportBean("E1", 1));
                env.SendEventBean(new SupportBean("E2", 10));
                env.SendEventBean(new SupportBean("E1", 2));
                env.SendEventBean(new SupportBean("E3", 100));
                env.SendEventBean(new SupportBean("E3", 101));

                env.Milestone(1);

                env.SendEventBean(new SupportBean("E1", 3));
                object[][] expected = {
                    new object[] { 0, "S0_1", "E1", 6 }, new object[] { 0, "S0_1", "E2", 10 },
                    new object[] { 0, "S0_1", "E3", 201 }
                };
                EPAssertionUtil.AssertPropsPerRowAnyOrder(
                    env.Statement("s0").GetEnumerator(),
                    env.Statement("s0").GetSafeEnumerator(),
                    fields,
                    expected);

                // test iterator targeted by context partition id
                var selectorById = new SupportSelectorById(Collections.SingletonSet(0));
                EPAssertionUtil.AssertPropsPerRowAnyOrder(
                    env.Statement("s0").GetEnumerator(selectorById),
                    env.Statement("s0").GetSafeEnumerator(selectorById),
                    fields,
                    expected);

                // test iterator targeted by property on triggering event
                var filtered = new SupportSelectorFilteredInitTerm("S0_1");
                EPAssertionUtil.AssertPropsPerRowAnyOrder(
                    env.Statement("s0").GetEnumerator(filtered),
                    env.Statement("s0").GetSafeEnumerator(filtered),
                    fields,
                    expected);
                filtered = new SupportSelectorFilteredInitTerm("S0_2");
                Assert.IsFalse(env.Statement("s0").GetEnumerator(filtered).MoveNext());

                // test always-false filter - compare context partition info
                filtered = new SupportSelectorFilteredInitTerm(null);
                Assert.IsFalse(env.Statement("s0").GetEnumerator(filtered).MoveNext());
                EPAssertionUtil.AssertEqualsAnyOrder(new object[] { 1000L }, filtered.ContextsStartTimes);
                EPAssertionUtil.AssertEqualsAnyOrder(new object[] { "S0_1" }, filtered.P00PropertyValues);

                try {
                    env.Statement("s0")
                        .GetEnumerator(
                            new ProxyContextPartitionSelectorSegmented {
                                ProcPartitionKeys = () => null
                            });
                    Assert.Fail();
                }
                catch (InvalidContextPartitionSelector ex) {
                    Assert.IsTrue(
                        ex.Message.StartsWith(
                            "Invalid context partition selector, expected an implementation class of any of [ContextPartitionSelectorAll, ContextPartitionSelectorFiltered, ContextPartitionSelectorById] interfaces but received com."),
                        "message: " + ex.Message);
                }

                env.UndeployAll();
            }
        }

        internal class ContextStartEndFilterStartedFilterEndedCorrelatedOutputSnapshot : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                var path = new RegressionPath();
                env.CompileDeploy(
                    "create context EveryNowAndThen as " +
                    "start SupportBean_S0 as S0 " +
                    "end SupportBean_S1(P10 = S0.P00) as S1",
                    path);

                var fields = new[] { "c1", "c2", "c3" };
                env.CompileDeploy(
                    "@Name('s0') context EveryNowAndThen select context.S0.Id as c1, context.S1.Id as c2, sum(IntPrimitive) as c3 " +
                    "from SupportBean#keepall output snapshot when terminated",
                    path);
                env.AddListener("s0");

                env.SendEventBean(new SupportBean("E1", 1));
                env.SendEventBean(new SupportBean_S0(100, "G1")); // starts it
                env.SendEventBean(new SupportBean("E2", 2));
                env.SendEventBean(new SupportBean("E3", 3));

                env.Milestone(0);

                env.SendEventBean(new SupportBean_S1(200, "GX"));
                Assert.IsFalse(env.Listener("s0").GetAndClearIsInvoked());

                env.Milestone(1);

                env.SendEventBean(new SupportBean_S1(200, "G1")); // terminate
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { 100, 200, 5 });

                env.SendEventBean(new SupportBean_S0(101, "G2")); // starts new one

                env.Milestone(2);

                env.SendEventBean(new SupportBean_S0(102, "G3")); // ignored

                env.Milestone(3);

                env.SendEventBean(new SupportBean("E4", 4));
                env.SendEventBean(new SupportBean("E5", 5));
                env.SendEventBean(new SupportBean("E6", 6));

                env.Milestone(4);

                env.SendEventBean(new SupportBean_S1(201, "G2")); // terminate
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { 101, 201, 15 });

                env.UndeployAll();
            }
        }

        internal class ContextStartEndFilterStartedPatternEndedCorrelated : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                var path = new RegressionPath();
                env.CompileDeploy(
                    "create context EveryNowAndThen as " +
                    "start SupportBean_S0 as S0 " +
                    "end pattern [SupportBean_S1(P10 = S0.P00)]",
                    path);

                var fields = new[] { "c1", "c2" };
                env.CompileDeploy(
                    "@Name('s0') context EveryNowAndThen select context.S0.P00 as c1, sum(IntPrimitive) as c2 " +
                    "from SupportBean#keepall",
                    path);
                env.AddListener("s0");

                env.SendEventBean(new SupportBean("E1", 1));
                env.SendEventBean(new SupportBean_S0(100, "G1")); // starts it
                Assert.IsFalse(env.Listener("s0").IsInvoked);

                env.Milestone(0);

                env.SendEventBean(new SupportBean("E2", 2));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "G1", 2 });

                env.Milestone(1);

                env.SendEventBean(new SupportBean_S1(200, "GX")); // false terminate
                env.SendEventBean(new SupportBean("E3", 3));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "G1", 5 });

                env.Milestone(2);

                env.SendEventBean(new SupportBean_S1(200, "G1")); // actual terminate
                env.SendEventBean(new SupportBean("E4", 4));
                Assert.IsFalse(env.Listener("s0").GetAndClearIsInvoked());

                env.SendEventBean(new SupportBean_S0(101, "G2")); // starts second

                env.Milestone(3);

                env.SendEventBean(new SupportBean("E6", 6));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "G2", 6 });

                env.Milestone(4);

                env.SendEventBean(new SupportBean_S1(101, null)); // false terminate
                env.SendEventBean(new SupportBean_S1(101, "GY")); // false terminate

                env.SendEventBean(new SupportBean("E7", 7));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "G2", 13 });

                env.Milestone(5);

                env.SendEventBean(new SupportBean_S1(300, "G2")); // actual terminate
                env.SendEventBean(new SupportBean("E8", 8));
                Assert.IsFalse(env.Listener("s0").IsInvoked);

                env.SendEventBean(new SupportBean_S0(102, "G3")); // starts third
                env.SendEventBean(new SupportBean_S1(0, "G3")); // terminate third

                env.Milestone(6);

                env.SendEventBean(new SupportBean("E9", 9));
                Assert.IsFalse(env.Listener("s0").IsInvoked);

                env.UndeployAll();
            }
        }

        internal class ContextStartEndStartAfterEndAfter : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                SendTimeEvent(env, "2002-05-1T08:00:00.000");
                var path = new RegressionPath();

                env.CompileDeploy("create context EveryNowAndThen as start after 5 sec end after 10 sec", path);

                var fields = new[] { "c1", "c2", "c3" };
                var fieldsShort = new[] { "c3" };

                env.CompileDeploy(
                    "@Name('s0') context EveryNowAndThen select context.startTime as c1, context.endTime as c2, sum(IntPrimitive) as c3 " +
                    "from SupportBean#keepall",
                    path);
                env.AddListener("s0");

                env.SendEventBean(new SupportBean("E1", 1));
                Assert.IsFalse(env.Listener("s0").GetAndClearIsInvoked());

                env.Milestone(0);

                SendTimeEvent(env, "2002-05-1T08:00:05.000");

                env.SendEventBean(new SupportBean("E2", 2));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] {
                        DateTimeParsingFunctions.ParseDefaultMSec("2002-05-1T08:00:05.000"),
                        DateTimeParsingFunctions.ParseDefaultMSec("2002-05-1T08:00:15.000"), 2
                    });

                env.Milestone(1);

                SendTimeEvent(env, "2002-05-1T08:00:14.999");

                env.Milestone(2);

                env.SendEventBean(new SupportBean("E3", 3));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fieldsShort,
                    new object[] { 5 });

                env.Milestone(3);

                SendTimeEvent(env, "2002-05-1T08:00:15.000");

                env.Milestone(4);

                env.SendEventBean(new SupportBean("E4", 4));
                Assert.IsFalse(env.Listener("s0").IsInvoked);

                SendTimeEvent(env, "2002-05-1T08:00:20.000");

                env.Milestone(5);

                env.SendEventBean(new SupportBean("E5", 5));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] {
                        DateTimeParsingFunctions.ParseDefaultMSec("2002-05-1T08:00:20.000"),
                        DateTimeParsingFunctions.ParseDefaultMSec("2002-05-1T08:00:30.000"), 5
                    });

                SendTimeEvent(env, "2002-05-1T08:00:30.000");

                env.Milestone(6);

                env.SendEventBean(new SupportBean("E6", 6));
                Assert.IsFalse(env.Listener("s0").IsInvoked);

                // try variable
                path.Clear();
                env.CompileDeploy("create variable int var_start = 10", path);
                env.CompileDeploy("create variable int var_end = 20", path);
                env.CompileDeploy(
                    "create context FrequentlyContext as start after var_start sec end after var_end sec",
                    path);

                env.UndeployAll();
            }
        }

        internal class ContextStartEndFilterStartedFilterEndedOutputSnapshot : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                var path = new RegressionPath();
                env.CompileDeploy(
                    "create context EveryNowAndThen as start SupportBean_S0 as S0 end SupportBean_S1 as S1",
                    path);

                var fields = new[] { "c1", "c2" };
                env.CompileDeploy(
                    "@Name('s0') context EveryNowAndThen select context.S0.P00 as c1, sum(IntPrimitive) as c2 " +
                    "from SupportBean#keepall output snapshot when terminated",
                    path);
                env.AddListener("s0");

                env.SendEventBean(new SupportBean("E1", 1));
                env.SendEventBean(new SupportBean_S0(100, "S0_1")); // starts it
                env.SendEventBean(new SupportBean("E2", 2));

                env.Milestone(0);

                env.SendEventBean(new SupportBean("E3", 3));
                Assert.IsFalse(env.Listener("s0").GetAndClearIsInvoked());

                // terminate
                env.SendEventBean(new SupportBean_S1(200, "S1_1"));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "S0_1", 5 });

                env.Milestone(1);

                env.SendEventBean(new SupportBean_S1(201, "S1_2"));

                env.Milestone(2);

                env.SendEventBean(new SupportBean("E4", 4));
                Assert.IsFalse(env.Listener("s0").GetAndClearIsInvoked());

                env.SendEventBean(new SupportBean_S0(102, "S0_2")); // starts it
                env.SendEventBean(new SupportBean_S1(201, "S1_3")); // ends it
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "S0_2", null });

                env.SendEventBean(new SupportBean_S0(103, "S0_3")); // starts it
                env.SendEventBean(new SupportBean("E5", 6)); // some more data

                env.Milestone(3);

                env.SendEventBean(new SupportBean_S0(104, "S0_4")); // ignored
                env.SendEventBean(new SupportBean_S1(201, "S1_3")); // ends it
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "S0_3", 6 });

                env.UndeployModuleContaining("s0");
                env.UndeployAll();
            }
        }

        internal class ContextStartEndPatternStartedPatternEnded : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                SendTimeEvent(env, "2002-05-01T08:00:00.000");
                var path = new RegressionPath();

                env.CompileDeploy(
                    "create context EveryNowAndThen as " +
                    "start pattern [S0=SupportBean_S0 -> timer:interval(1 sec)] " +
                    "end pattern [S1=SupportBean_S1 -> timer:interval(1 sec)]",
                    path);

                var fields = new[] { "c1", "c2" };
                env.CompileDeploy(
                    "@Name('s0') context EveryNowAndThen select context.S0.P00 as c1, sum(IntPrimitive) as c2 " +
                    "from SupportBean#keepall",
                    path);
                env.AddListener("s0");

                env.SendEventBean(new SupportBean("E1", 1));

                env.Milestone(0);

                env.SendEventBean(new SupportBean_S0(100, "S0_1")); // starts it
                env.SendEventBean(new SupportBean("E2", 2));

                env.Milestone(1);

                env.SendEventBean(new SupportBean("E3", 3));
                Assert.IsFalse(env.Listener("s0").GetAndClearIsInvoked());

                SendTimeEvent(env, "2002-05-1T08:00:01.000"); // 1 second passes

                env.SendEventBean(new SupportBean("E4", 4));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "S0_1", 4 });

                env.Milestone(2);

                env.SendEventBean(new SupportBean("E5", 5));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "S0_1", 9 });

                env.SendEventBean(new SupportBean_S0(101, "S0_2")); // ignored
                SendTimeEvent(env, "2002-05-1T08:00:03.000");

                env.Milestone(3);

                env.SendEventBean(new SupportBean("E6", 6));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "S0_1", 15 });

                env.SendEventBean(new SupportBean_S1(101, "S1_1")); // ignored

                env.SendEventBean(new SupportBean("E7", 7));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "S0_1", 22 });

                env.Milestone(4);

                SendTimeEvent(env, "2002-05-1T08:00:04.000"); // terminates

                env.SendEventBean(new SupportBean("E8", 8));

                env.Milestone(5);

                env.SendEventBean(new SupportBean_S1(102, "S1_2")); // ignored
                SendTimeEvent(env, "2002-05-1T08:00:10.000");
                env.SendEventBean(new SupportBean("E9", 9));
                Assert.IsFalse(env.Listener("s0").GetAndClearIsInvoked());

                env.SendEventBean(new SupportBean_S0(103, "S0_3")); // new instance
                SendTimeEvent(env, "2002-05-1T08:00:11.000");

                env.Milestone(6);

                env.SendEventBean(new SupportBean("E10", 10));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "S0_3", 10 });

                env.UndeployAll();
            }
        }

        internal class ContextStartEndContextCreateDestroy : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                SendTimeEvent(env, "2002-05-1T08:00:00.000");
                var path = new RegressionPath();
                env.CompileDeploy(
                    "create context EverySecond as start (*, *, *, *, *, *) end (*, *, *, *, *, *)",
                    path);
                env.CompileDeploy("@Name('s0') context EverySecond select * from SupportBean", path);
                env.AddListener("s0");

                env.Milestone(0);

                env.SendEventBean(new SupportBean());
                Assert.IsTrue(env.Listener("s0").GetAndClearIsInvoked());

                env.Milestone(1);

                SendTimeEvent(env, "2002-05-1T08:00:00.999");
                env.SendEventBean(new SupportBean());
                Assert.IsTrue(env.Listener("s0").GetAndClearIsInvoked());

                env.Milestone(2);

                SendTimeEvent(env, "2002-05-1T08:00:01.000");
                env.SendEventBean(new SupportBean());
                Assert.IsFalse(env.Listener("s0").GetAndClearIsInvoked());

                var start = DateTimeParsingFunctions.ParseDefaultMSec("2002-05-1T08:00:01.999");
                for (var i = 0; i < 10; i++) {
                    SendTimeEvent(env, start);

                    SendEventAndAssert(env, false);

                    start += 1;
                    SendTimeEvent(env, start);

                    SendEventAndAssert(env, true);

                    start += 999;
                    SendTimeEvent(env, start);

                    SendEventAndAssert(env, true);

                    start += 1;
                    SendTimeEvent(env, start);

                    SendEventAndAssert(env, false);

                    start += 999;
                }

                env.UndeployAll();
            }
        }

        internal class ContextStartEndDBHistorical : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                SendTimeEvent(env, "2002-05-1T08:00:00.000");
                var path = new RegressionPath();
                env.CompileDeploy("@Name('s0') create context NineToFive as start (0, 9, *, *, *) end (0, 17, *, *, *)", path);

                var fields = new[] { "S1.mychar" };
                var stmtText =
                    "context NineToFive select * from SupportBean_S0 as S0, sql:MyDB ['select * from mytesttable where ${Id} = mytesttable.myBigint'] as S1";
                env.CompileDeploy(stmtText, path);
                env.AddListener("s0");

                env.SendEventBean(new SupportBean_S0(2));
                Assert.IsFalse(env.Listener("s0").IsInvoked);

                // now started
                SendTimeEvent(env, "2002-05-1T09:00:00.000");
                env.SendEventBean(new SupportBean_S0(2));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "Y" });

                // now gone
                SendTimeEvent(env, "2002-05-1T17:00:00.000");

                env.SendEventBean(new SupportBean_S0(2));
                Assert.IsFalse(env.Listener("s0").IsInvoked);

                // now started
                SendTimeEvent(env, "2002-05-2T09:00:00.000");

                env.SendEventBean(new SupportBean_S0(3));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "X" });

                env.UndeployAll();
            }
        }

        internal class ContextStartEndPrevPriorAndAggregation : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                SendTimeEvent(env, "2002-05-1T08:00:00.000");
                var path = new RegressionPath();
                env.CompileDeploy("create context NineToFive as start (0, 9, *, *, *) end (0, 17, *, *, *)", path);

                var fields = new[] { "col1", "col2", "col3", "col4", "col5" };
                env.CompileDeploy(
                    "@Name('s0') context NineToFive " +
                    "select prev(TheString) as col1, prevwindow(sb) as col2, prevtail(TheString) as col3, prior(1, TheString) as col4, sum(IntPrimitive) as col5 " +
                    "from SupportBean#keepall as sb",
                    path);
                env.AddListener("s0");

                env.SendEventBean(new SupportBean());
                Assert.IsFalse(env.Listener("s0").IsInvoked);

                // now started
                SendTimeEvent(env, "2002-05-1T09:00:00.000");
                var event1 = new SupportBean("E1", 1);
                env.SendEventBean(event1);
                object[][] expected = { new object[] { null, new[] { event1 }, "E1", null, 1 } };
                EPAssertionUtil.AssertPropsPerRow(env.Listener("s0").GetAndResetLastNewData(), fields, expected);
                EPAssertionUtil.AssertPropsPerRow(
                    env.Statement("s0").GetEnumerator(),
                    env.Statement("s0").GetSafeEnumerator(),
                    fields,
                    expected);

                env.Milestone(0);

                var event2 = new SupportBean("E2", 2);
                env.SendEventBean(event2);
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "E1", new[] { event2, event1 }, "E1", "E1", 3 });

                env.Milestone(1);

                // now gone
                SendTimeEvent(env, "2002-05-1T17:00:00.000");
                EPAssertionUtil.AssertPropsPerRow(
                    env.Statement("s0").GetEnumerator(),
                    env.Statement("s0").GetSafeEnumerator(),
                    fields,
                    null);

                env.SendEventBean(new SupportBean());
                Assert.IsFalse(env.Listener("s0").IsInvoked);
                AgentInstanceAssertionUtil.AssertInstanceCounts(env, "s0", 0, null, 0, 0);

                env.Milestone(2);

                // now started
                SendTimeEvent(env, "2002-05-2T09:00:00.000");

                env.Milestone(3);

                var event3 = new SupportBean("E3", 9);
                env.SendEventBean(event3);
                expected = new[] { new object[] { null, new[] { event3 }, "E3", null, 9 } };
                EPAssertionUtil.AssertPropsPerRow(env.Listener("s0").GetAndResetLastNewData(), fields, expected);
                EPAssertionUtil.AssertPropsPerRow(
                    env.Statement("s0").GetEnumerator(),
                    env.Statement("s0").GetSafeEnumerator(),
                    fields,
                    expected);
                AgentInstanceAssertionUtil.AssertInstanceCounts(env, "s0", 1, null, 1, 1);

                env.UndeployAll();
            }
        }

        internal class ContextStartEndJoin : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                SendTimeEvent(env, "2002-05-1T08:00:00.000");
                var path = new RegressionPath();
                env.CompileDeploy("create context NineToFive as start (0, 9, *, *, *) end (0, 17, *, *, *)", path);

                var fields = new[] { "col1", "col2", "col3", "col4" };
                env.CompileDeploy(
                    "@Name('s0') context NineToFive " +
                    "select sb.TheString as col1, sb.IntPrimitive as col2, S0.Id as col3, S0.P00 as col4 " +
                    "from SupportBean#keepall as sb full outer join SupportBean_S0#keepall as S0 on P00 = TheString",
                    path);
                env.AddListener("s0");

                env.SendEventBean(new SupportBean("E1", 1));

                env.Milestone(0);

                env.SendEventBean(new SupportBean_S0(1, "E1"));
                Assert.IsFalse(env.Listener("s0").IsInvoked);

                env.Milestone(1);

                // now started
                SendTimeEvent(env, "2002-05-1T09:00:00.000");
                env.SendEventBean(new SupportBean_S0(1, "E1"));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { null, null, 1, "E1" });

                env.Milestone(2);

                env.SendEventBean(new SupportBean("E1", 5));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "E1", 5, 1, "E1" });

                env.Milestone(3);

                // now gone
                SendTimeEvent(env, "2002-05-1T17:00:00.000");

                env.Milestone(4);

                env.SendEventBean(new SupportBean("E1", 1));
                env.SendEventBean(new SupportBean_S0(1, "E1"));
                Assert.IsFalse(env.Listener("s0").IsInvoked);

                env.Milestone(5);

                // now started
                SendTimeEvent(env, "2002-05-2T09:00:00.000");

                SendTimeEvent(env, "2002-05-1T09:00:00.000");
                env.SendEventBean(new SupportBean("E1", 4));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "E1", 4, null, null });

                env.Milestone(6);

                env.SendEventBean(new SupportBean_S0(2, "E1"));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "E1", 4, 2, "E1" });

                env.UndeployAll();
            }
        }

        internal class ContextStartEndPatternWithTime : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                var path = new RegressionPath();
                SendTimeEvent(env, "2002-05-1T08:00:00.000");
                env.CompileDeploy("create context NineToFive as start (0, 9, *, *, *) end (0, 17, *, *, *)", path);

                env.CompileDeploy(
                    "@Name('s0') context NineToFive select * from pattern[every timer:interval(10 sec)]",
                    path);
                env.AddListener("s0");
                Assert.AreEqual(1, SupportScheduleHelper.ScheduleCountOverall(env)); // from the context

                // now started
                SendTimeEvent(env, "2002-05-1T09:00:00.000");
                Assert.AreEqual(2, SupportScheduleHelper.ScheduleCountOverall(env)); // context + pattern
                Assert.IsFalse(env.Listener("s0").IsInvoked);

                env.Milestone(0);

                SendTimeEvent(env, "2002-05-1T09:00:10.000");
                Assert.IsTrue(env.Listener("s0").IsInvoked);

                env.Milestone(1);

                // now gone
                SendTimeEvent(env, "2002-05-1T17:00:00.000");
                env.Listener("s0").Reset(); // it is not well defined whether the listener does get fired or not
                Assert.AreEqual(1, SupportScheduleHelper.ScheduleCountOverall(env)); // from the context

                env.Milestone(2);

                // now started
                SendTimeEvent(env, "2002-05-2T09:00:00.000");
                Assert.AreEqual(2, SupportScheduleHelper.ScheduleCountOverall(env)); // context + pattern
                Assert.IsFalse(env.Listener("s0").IsInvoked);

                env.Milestone(3);

                SendTimeEvent(env, "2002-05-2T09:00:10.000");
                Assert.IsTrue(env.Listener("s0").IsInvoked);

                env.UndeployAll();
            }
        }

        internal class ContextStartEndSubselect : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                var path = new RegressionPath();
                SendTimeEvent(env, "2002-05-1T08:00:00.000");
                env.CompileDeploy("create context NineToFive as start (0, 9, *, *, *) end (0, 17, *, *, *)", path);

                var fields = new[] { "TheString", "col" };

                env.CompileDeploy(
                    "@Name('s0') context NineToFive select TheString, (select P00 from SupportBean_S0#lastevent) as col from SupportBean",
                    path);
                env.AddListener("s0");
                Assert.AreEqual(0, SupportFilterServiceHelper.GetFilterSvcCountApprox(env)); // from the context

                // now started
                SendTimeEvent(env, "2002-05-1T09:00:00.000");
                Assert.AreEqual(2, SupportFilterServiceHelper.GetFilterSvcCountApprox(env)); // from the context

                env.Milestone(0);

                env.SendEventBean(new SupportBean("E1", 1));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "E1", null });

                env.Milestone(1);

                env.SendEventBean(new SupportBean_S0(11, "S01"));

                env.Milestone(2);

                env.SendEventBean(new SupportBean("E2", 2));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "E2", "S01" });

                // now gone
                SendTimeEvent(env, "2002-05-1T17:00:00.000");
                Assert.AreEqual(0, SupportFilterServiceHelper.GetFilterSvcCountApprox(env)); // from the context

                env.Milestone(3);

                env.SendEventBean(new SupportBean("Ex", 0));
                Assert.IsFalse(env.Listener("s0").IsInvoked);

                env.Milestone(4);

                // now started
                SendTimeEvent(env, "2002-05-2T09:00:00.000");
                Assert.AreEqual(2, SupportFilterServiceHelper.GetFilterSvcCountApprox(env)); // from the context
                Assert.IsFalse(env.Listener("s0").IsInvoked);

                env.SendEventBean(new SupportBean("E3", 3));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "E3", null });

                env.SendEventBean(new SupportBean_S0(12, "S02"));

                env.Milestone(5);

                env.SendEventBean(new SupportBean("E4", 4));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "E4", "S02" });
                AgentInstanceAssertionUtil.AssertInstanceCounts(env, "s0", 1, 1, null, null);

                env.Milestone(6);

                // now gone
                SendTimeEvent(env, "2002-05-2T17:00:00.000");
                Assert.AreEqual(0, SupportFilterServiceHelper.GetFilterSvcCountApprox(env)); // from the context

                env.Milestone(7);

                env.SendEventBean(new SupportBean("Ey", 0));
                Assert.IsFalse(env.Listener("s0").IsInvoked);
                AgentInstanceAssertionUtil.AssertInstanceCounts(env, "s0", 0, 0, null, null);

                env.UndeployAll();
            }
        }

        public class ContextStartEndSubselectCorrelated : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                SendTimeEvent(env, "2002-05-1T8:00:00.000");
                var path = new RegressionPath();
                env.CompileDeploy(
                    "@Name('ctx') create context NineToFive as start (0, 9, *, *, *) end (0, 17, *, *, *)",
                    path);

                var fields = new[] { "TheString", "col" };
                env.CompileDeploy(
                    "@Name('s0') context NineToFive select TheString, " +
                    "(select Id from SupportBean_S0#keepall as S0 where S0.P00 = sb.TheString) as col from SupportBean as sb",
                    path);
                env.AddListener("s0");

                env.Milestone(0);

                // now started
                SendTimeEvent(env, "2002-05-1T9:00:00.000");

                env.Milestone(1);

                env.SendEventBean(new SupportBean("E1", 1));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "E1", null });

                env.Milestone(2);

                env.SendEventBean(new SupportBean_S0(11, "S01"));
                env.SendEventBean(new SupportBean("E2", 2));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "E2", null });

                env.Milestone(3);

                env.SendEventBean(new SupportBean("S01", 3));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "S01", 11 });

                env.Milestone(4);

                // now gone
                SendTimeEvent(env, "2002-05-1T17:00:00.000");

                env.Milestone(5);

                env.SendEventBean(new SupportBean("Ex", 0));
                Assert.IsFalse(env.Listener("s0").IsInvoked);

                env.Milestone(6);

                // now started
                SendTimeEvent(env, "2002-05-2T9:00:00.000");
                Assert.IsFalse(env.Listener("s0").IsInvoked);

                env.Milestone(7);

                env.SendEventBean(new SupportBean("S01", 4));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "S01", null });

                env.Milestone(8);

                env.SendEventBean(new SupportBean_S0(12, "S02"));
                env.SendEventBean(new SupportBean("S02", 4));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "S02", 12 });

                env.Milestone(9);

                // now gone
                SendTimeEvent(env, "2002-05-2T17:00:00.000");

                env.Milestone(10);

                env.SendEventBean(new SupportBean("Ey", 0));
                Assert.IsFalse(env.Listener("s0").IsInvoked);

                env.UndeployAll();
            }
        }

        internal class ContextStartEndNWSameContextOnExpr : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                SendTimeEvent(env, "2002-05-1T08:00:00.000");
                var path = new RegressionPath();
                env.CompileDeploy("create context NineToFive as start (0, 9, *, *, *) end (0, 17, *, *, *)", path);

                // no started yet
                var fields = new[] { "TheString", "IntPrimitive" };
                env.CompileDeploy("@Name('s0') context NineToFive create window MyWindow#keepall as SupportBean", path);
                env.AddListener("s0");

                env.CompileDeploy("context NineToFive insert into MyWindow select * from SupportBean", path);

                env.CompileDeploy(
                    "context NineToFive " +
                    "on SupportBean_S0 S0 merge MyWindow mw where mw.TheString = S0.P00 " +
                    "when matched then update set IntPrimitive = S0.Id " +
                    "when not matched then insert select makeBean(Id, P00)",
                    path);

                env.SendEventBean(new SupportBean_S0(1, "E1"));
                Assert.IsFalse(env.Listener("s0").IsInvoked);

                env.Milestone(0);

                // now started
                SendTimeEvent(env, "2002-05-1T09:00:00.000");

                env.SendEventBean(new SupportBean_S0(1, "E1"));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "E1", 1 });

                env.Milestone(1);

                env.SendEventBean(new SupportBean_S0(2, "E2"));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "E2", 2 });

                env.Milestone(2);

                env.SendEventBean(new SupportBean_S0(3, "E1"));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").LastNewData[0],
                    fields,
                    new object[] { "E1", 3 });
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").LastOldData[0],
                    fields,
                    new object[] { "E1", 1 });
                env.Listener("s0").Reset();

                // now gone
                SendTimeEvent(env, "2002-05-1T17:00:00.000");

                env.Milestone(3);

                // no longer updated
                env.SendEventBean(new SupportBean_S0(1, "E1"));
                Assert.IsFalse(env.Listener("s0").IsInvoked);

                env.Milestone(4);

                // now started again but empty
                SendTimeEvent(env, "2002-05-2T09:00:00.000");

                env.Milestone(5);

                env.SendEventBean(new SupportBean_S0(1, "E1"));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "E1", 1 });

                env.UndeployAll();
            }
        }

        internal class ContextStartEndNWFireAndForget : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                SendTimeEvent(env, "2002-05-1T08:00:00.000");
                var path = new RegressionPath();
                env.CompileDeploy("create context NineToFive as start (0, 9, *, *, *) end (0, 17, *, *, *)", path);

                // no started yet
                env.CompileDeploy("context NineToFive create window MyWindow#keepall as SupportBean", path);
                env.CompileDeploy("context NineToFive insert into MyWindow select * from SupportBean", path);

                TryNWQuery(env, path, 0);

                // now started
                SendTimeEvent(env, "2002-05-1T09:00:00.000");
                TryNWQuery(env, path, 0);

                env.Milestone(0);

                // now not empty
                env.SendEventBean(new SupportBean());
                TryNWQuery(env, path, 1);

                env.Milestone(1);

                // now gone
                SendTimeEvent(env, "2002-05-1T17:00:00.000");

                env.Milestone(2);

                TryNWQuery(env, path, 0);
                env.SendEventBean(new SupportBean());

                env.Milestone(3);

                // now started again but empty
                SendTimeEvent(env, "2002-05-2T09:00:00.000");
                TryNWQuery(env, path, 0);

                env.Milestone(4);

                // fill some data
                env.SendEventBean(new SupportBean());
                env.SendEventBean(new SupportBean());
                SendTimeEvent(env, "2002-05-2T09:10:00.000");
                TryNWQuery(env, path, 2);

                env.UndeployAll();
            }

            private static void TryNWQuery(
                RegressionEnvironment env,
                RegressionPath path,
                int numRows)
            {
                var compiled = env.CompileFAF("select * from MyWindow", path);
                var result = env.Runtime.FireAndForgetService.ExecuteQuery(compiled);
                Assert.AreEqual(numRows, result.Array.Length);
            }
        }

        internal class ContextStartEndStartTurnedOff : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                SendTimeEvent(env, "2002-05-1T08:00:00.000");
                var path = new RegressionPath();
                var contextEPL =
                    "@Name('context') create context NineToFive as start (0, 9, *, *, *) end (0, 17, *, *, *)";
                env.CompileDeploy(contextEPL, path);
                AssertContextEventType(env.Statement("context").EventType);
                env.AddListener("context");
                env.Statement("context").Subscriber = new MiniSubscriber();

                env.CompileDeploy("@Name('A') context NineToFive select * from SupportBean", path);
                env.AddListener("A");

                SendTimeAndAssert(env, "2002-05-1T08:59:30.000", false, "A");

                env.Milestone(0);

                SendTimeAndAssert(env, "2002-05-1T08:59:59.999", false, "A");
                SendTimeAndAssert(env, "2002-05-1T09:00:00.000", true, "A");

                env.CompileDeploy("@Name('B') context NineToFive select * from SupportBean", path);
                env.AddListener("B");

                SendTimeAndAssert(env, "2002-05-1T16:59:59.000", true, "A,B");

                env.Milestone(1);

                SendTimeAndAssert(env, "2002-05-1T17:00:00.000", false, "A,B");

                env.Milestone(2);

                env.CompileDeploy("@Name('C') context NineToFive select * from SupportBean", path);
                env.AddListener("C");

                SendTimeAndAssert(env, "2002-05-2T08:59:59.999", false, "A,B,C");
                SendTimeAndAssert(env, "2002-05-2T09:00:00.000", true, "A,B,C");

                env.Milestone(3);

                SendTimeAndAssert(env, "2002-05-2T16:59:59.000", true, "A,B,C");
                SendTimeAndAssert(env, "2002-05-2T17:00:00.000", false, "A,B,C");

                Assert.IsFalse(env.Listener("context").IsInvoked);

                env.UndeployAll();
                path.Clear();

                // test SODA
                SendTimeEvent(env, "2002-05-3T16:59:59.000");
                env.EplToModelCompileDeploy(contextEPL, path);

                // test built-in properties
                env.CompileDeploy(
                    "@Name('A') context NineToFive " +
                    "select context.name as c1, context.startTime as c2, context.endTime as c3, TheString as c4 from SupportBean",
                    path);
                env.AddListener("A");

                env.SendEventBean(new SupportBean("E1", 10));
                var theEvent = env.Listener("A").AssertOneGetNewAndReset();
                Assert.That(
                    theEvent.Get("c1"),
                    Is.EqualTo("NineToFive"));
                Assert.That(
                    theEvent.Get("c2"),
                    Is.EqualTo(DateTimeParsingFunctions.ParseDefaultMSec("2002-05-03T16:59:59.000")));
                Assert.That(
                    theEvent.Get("c3"),
                    Is.EqualTo(DateTimeParsingFunctions.ParseDefaultMSec("2002-05-03T17:00:00.000")));
                Assert.That(
                    theEvent.Get("c4"),
                    Is.EqualTo("E1"));

                env.UndeployAll();
            }
        }

        internal class ContextStartEndStartTurnedOn : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                Assert.AreEqual(0, SupportContextMgmtHelper.GetContextCount(env));
                var path = new RegressionPath();

                SendTimeEvent(env, "2002-05-1T09:15:00.000");
                env.CompileDeploy(
                    "@Name('context') create context NineToFive as start (0, 9, *, *, *) end (0, 17, *, *, *)",
                    path);
                Assert.AreEqual(1, SupportContextMgmtHelper.GetContextCount(env));

                env.Milestone(0);

                env.CompileDeploy("@Name('A') context NineToFive select * from SupportBean", path).AddListener("A");

                SendTimeAndAssert(env, "2002-05-1T09:16:00.000", true, "A");

                env.Milestone(1);

                SendTimeAndAssert(env, "2002-05-1T16:59:59.000", true, "A");
                SendTimeAndAssert(env, "2002-05-1T17:00:00.000", false, "A");

                env.CompileDeploy("@Name('B') context NineToFive select * from SupportBean", path).AddListener("B");

                SendTimeAndAssert(env, "2002-05-2T08:59:59.999", false, "A,B");
                SendTimeAndAssert(env, "2002-05-2T09:15:00.000", true, "A,B");

                env.Milestone(2);

                SendTimeAndAssert(env, "2002-05-2T16:59:59.000", true, "A,B");

                env.Milestone(3);

                SendTimeAndAssert(env, "2002-05-2T17:00:00.000", false, "A,B");

                Assert.AreEqual(1, SupportContextMgmtHelper.GetContextCount(env));
                env.UndeployModuleContaining("A");

                env.Milestone(4);

                Assert.AreEqual(1, SupportContextMgmtHelper.GetContextCount(env));
                env.UndeployModuleContaining("B");

                env.Milestone(5);

                Assert.AreEqual(1, SupportContextMgmtHelper.GetContextCount(env));
                env.UndeployModuleContaining("context");

                env.Milestone(6);

                Assert.AreEqual(0, SupportContextMgmtHelper.GetContextCount(env));
            }
        }

        public class ContextStart9End5AggUngrouped : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                SendTimeEvent(env, "2002-05-1T8:00:00.000");
                var path = new RegressionPath();

                var eplContext =
                    "@Name('CTX') create context CtxNineToFive as start (0, 9, *, *, *) end (0, 17, *, *, *)";
                env.CompileDeploy(eplContext, path);

                var fields = new[] { "c1", "c2" };
                var epl =
                    "@Name('S1') context CtxNineToFive select TheString as c1, sum(IntPrimitive) as c2 from SupportBean";
                env.CompileDeploy(epl, path).AddListener("S1");

                env.SendEventBean(new SupportBean("G1", 1));
                Assert.IsFalse(env.Listener("S1").GetAndClearIsInvoked());

                env.Milestone(0);

                SendTimeEvent(env, "2002-05-1T9:00:00.000");

                env.SendEventBean(new SupportBean("G2", 2));
                EPAssertionUtil.AssertProps(
                    env.Listener("S1").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "G2", 2 });

                env.SendEventBean(new SupportBean("G3", 3));
                EPAssertionUtil.AssertProps(
                    env.Listener("S1").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "G3", 5 });

                env.Milestone(1);

                SendTimeEvent(env, "2002-05-1T17:00:00.000");

                env.SendEventBean(new SupportBean("G4", 4));
                Assert.IsFalse(env.Listener("S1").GetAndClearIsInvoked());

                env.Milestone(2);

                SendTimeEvent(env, "2002-05-2T8:00:00.000");

                SendTimeEvent(env, "2002-05-2T9:00:00.000");
                env.SendEventBean(new SupportBean("G5", 20));
                EPAssertionUtil.AssertProps(
                    env.Listener("S1").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "G5", 20 });

                env.UndeployAll();
            }
        }

        public class ContextStart9End5AggGrouped : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                SendTimeEvent(env, "2002-05-1T7:00:00.000");
                var path = new RegressionPath();

                var eplTwo = "create context NestedContext as start (0, 8, *, *, *) end (0, 9, *, *, *)";
                env.CompileDeploy(eplTwo, path);

                env.Milestone(0);

                var fields = new[] { "c1", "c2" };
                env.CompileDeploy(
                    "@Name('s0') context NestedContext select " +
                    "TheString as c1, count(*) as c2 from SupportBean group by TheString",
                    path);
                env.AddListener("s0");

                env.SendEventBean(new SupportBean("E1", 0));
                Assert.IsFalse(env.Listener("s0").IsInvoked);

                env.Milestone(1);

                SendTimeEvent(env, "2002-05-1T8:00:00.000"); // start context

                env.Milestone(2);

                env.SendEventBean(new SupportBean("E2", 0));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "E2", 1L });

                env.Milestone(3);

                env.SendEventBean(new SupportBean("E1", 0));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "E1", 1L });

                env.Milestone(4);

                env.SendEventBean(new SupportBean("E2", 0));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "E2", 2L });

                env.Milestone(5);

                SendTimeEvent(env, "2002-05-1T9:00:00.000"); // terminate

                env.Milestone(5);

                env.SendEventBean(new SupportBean("E2", 0));
                Assert.IsFalse(env.Listener("s0").IsInvoked);

                env.Milestone(6);

                SendTimeEvent(env, "2002-05-2T8:00:00.000"); // start context

                env.Milestone(7);

                env.SendEventBean(new SupportBean("E2", 0));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fields,
                    new object[] { "E2", 1L });

                env.UndeployAll();
            }
        }

        public class ContextStartEndMultiCrontab : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                RunAssertionMultiCrontab(
                    env,
                    "(0, 8, *, *, *, *), (0, 10, *, *, *, *)",
                    "(0, 9, *, *, *, *), (0, 12, *, *, *, *)",
                    new TimeRangePair[] {
                        new TimeRangePair("2002-05-30T09:30:00.000", null, false),
                        new TimeRangePair("2002-05-30T10:00:00.000", "2002-05-30T11:59:59.999", true),
                        new TimeRangePair("2002-05-30T12:00:00.000", "2002-05-31T07:59:59.999", false),
                        new TimeRangePair("2002-05-31T08:00:00.000", "2002-05-31T08:59:59.999", true),
                        new TimeRangePair("2002-05-31T09:00:00.000", "2002-05-31T09:59:59.999", false),
                        new TimeRangePair("2002-05-31T10:00:00.000", "2002-05-31T10:10:00.000", true)
                    });

                RunAssertionMultiCrontab(
                    env,
                    "(0, 8, *, *, *, *)",
                    "(0, 12, *, *, [1, 2, 3, 4, 5], *), (0, 20, *, *, [0, 6], *)",
                    new TimeRangePair[] {
                        new TimeRangePair("2018-12-06T09:30:00.000", null, true), // Thurs. Dec 6, 2018
                        new TimeRangePair("2018-12-06T10:00:00.000", "2018-12-06T11:59:59.999", true),
                        new TimeRangePair("2018-12-06T12:00:00.000", "2018-12-07T07:59:59.999", false),
                        new TimeRangePair("2018-12-07T08:00:00.000", "2018-12-07T11:59:59.999", true),
                        new TimeRangePair("2018-12-07T12:00:00.000", "2018-12-08T07:59:59.999", false),
                        new TimeRangePair("2018-12-08T08:00:00.000", "2018-12-08T19:59:59.999", true),
                        new TimeRangePair("2018-12-08T20:00:00.000", "2018-12-09T07:59:59.999", false),
                        new TimeRangePair("2018-12-09T08:00:00.000", "2018-12-09T19:59:59.999", true),
                        new TimeRangePair("2018-12-09T20:00:00.000", "2018-12-10T07:59:59.999", false),
                        new TimeRangePair("2018-12-10T08:00:00.000", "2018-12-10T11:59:59.999", true),
                        new TimeRangePair("2018-12-10T12:00:00.000", "2018-12-10T13:00:00.000", false)
                    });

                RunAssertionMultiCrontab(
                    env,
                    "(0, 8, *, *, 1, *), (0, 9, *, *, 2, *)",
                    "(0, 10, *, *, *)",
                    new TimeRangePair[] {
                        new TimeRangePair("2018-12-03T09:30:00.000", null, true), // Mon. Dec 3, 2018
                        new TimeRangePair("2018-12-03T09:30:00.000", "2018-12-03T09:59:59.999", true),
                        new TimeRangePair("2018-12-03T10:00:00.000", "2018-12-04T08:59:59.999", false),
                        new TimeRangePair("2018-12-04T09:00:00.000", "2018-12-04T09:59:59.999", true),
                        new TimeRangePair("2018-12-04T10:00:00.000", "2018-12-10T07:59:59.999", false),
                        new TimeRangePair("2018-12-10T09:00:00.000", "2018-12-10T09:59:59.999", true),
                    });

                String epl = "create context Ctx as start (0, 8, *, *, 1, *), (0, 9, *, *, 2, *) end (0, 12, *, *, [1,2,3,4,5], *), (0, 20, *, *, [0,6], *)";
                env.EplToModelCompileDeploy(epl);
                env.UndeployAll();
            }
        }

        private static void RunAssertionMultiCrontab(
            RegressionEnvironment env,
            String startList,
            String endList,
            TimeRangePair[] pairs)
        {
            String epl = "create context Ctx " +
                         "start " +
                         startList +
                         "end " +
                         endList +
                         ";\n" +
                         "@Name('s0') context Ctx select * from SupportBean";

            SendTimeEvent(env, pairs[0].Start);
            Assert.IsNull(pairs[0].End);
            env.CompileDeploy(epl).AddListener("s0");
            SendEventAndAssert(env, pairs[0].IsExpected);

            for (int i = 1; i < pairs.Length; i++) {
                long start = DateTimeParsingFunctions.ParseDefaultMSec(pairs[i].Start);
                long end = DateTimeParsingFunctions.ParseDefaultMSec(pairs[i].End);
                long current = start;

                while (current < end) {
                    // Comment-me-in: log.info("Sending " + DateTime.print(current));
                    SendTimeEvent(env, current);
                    SendEventAndAssert(env, pairs[i].IsExpected);
                    current += 5 * 60 * 1000; // advance in 5-minute intervals
                }

                // Comment-me-in: log.info("Sending " + DateTime.print(end));
                SendTimeEvent(env, end);
                SendEventAndAssert(env, pairs[i].IsExpected);
            }

            env.UndeployAll();
        }

        private class TimeRangePair
        {
            public TimeRangePair(
                String start,
                String end,
                bool isExpected)
            {
                Start = start;
                End = end;
                IsExpected = isExpected;
            }

            public string Start { get; }

            public string End { get; }

            public bool IsExpected { get; }
        }

        public class MiniSubscriber
        {
            public static void Update()
            {
                // no action
            }
        }
    }
} // end of namespace
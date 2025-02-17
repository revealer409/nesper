///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Threading;

using com.espertech.esper.common.client.scopetest;
using com.espertech.esper.common.client.variable;
using com.espertech.esper.common.@internal.filterspec;
using com.espertech.esper.common.@internal.support;
using com.espertech.esper.common.@internal.util;
using com.espertech.esper.compat;
using com.espertech.esper.compat.collections;
using com.espertech.esper.compat.concurrency;
using com.espertech.esper.compat.threading;
using com.espertech.esper.regressionlib.framework;
using com.espertech.esper.regressionlib.support.filter;
using com.espertech.esper.runtime.@internal.kernel.service;

using NUnit.Framework;

using static com.espertech.esper.regressionlib.framework.SupportMessageAssertUtil;

namespace com.espertech.esper.regressionlib.suite.epl.variable
{
    public class EPLVariablesUse
    {
        public enum MyEnumWithOverride
        {
            LONG,
            SHORT
        }

        public static IList<RegressionExecution> Executions()
        {
            IList<RegressionExecution> execs = new List<RegressionExecution>();
            execs.Add(new EPLVariableUseSimplePreconfigured());
            execs.Add(new EPLVariableUseSimpleSameModule());
            execs.Add(new EPLVariableUseSimpleTwoModules());
            execs.Add(new EPLVariableUseEPRuntime());
            execs.Add(new EPLVariableUseDotSeparateThread());
            execs.Add(new EPLVariableUseInvokeMethod());
            execs.Add(new EPLVariableUseConstantVariable());
            execs.Add(new EPLVariableUseVariableInFilterBoolean());
            execs.Add(new EPLVariableUseVariableInFilter());
            execs.Add(new EPLVariableUseFilterConstantCustomTypePreconfigured());
            execs.Add(new EPLVariableUseWVarargs());
            return execs;
        }

        private static SupportBean SendSupportBean(
            RegressionEnvironment env,
            string theString,
            int intPrimitive)
        {
            var bean = new SupportBean();
            bean.TheString = theString;
            bean.IntPrimitive = intPrimitive;
            env.SendEventBean(bean);
            return bean;
        }

        private static void SendSupportBeanS0NewThread(
            RegressionEnvironment env,
            int id,
            string p00,
            string p01)
        {
            var t = new Thread(() => env.SendEventBean(new SupportBean_S0(id, p00, p01)));
            t.Start();
            t.Join();
        }

        private static SupportBean MakeSupportBean(
            string theString,
            int intPrimitive,
            int? intBoxed)
        {
            var bean = new SupportBean();
            bean.TheString = theString;
            bean.IntPrimitive = intPrimitive;
            bean.IntBoxed = intBoxed;
            return bean;
        }

        private static void AssertVariableValuesPreconfigured(
            RegressionEnvironment env,
            string[] names,
            object[] values)
        {
            Assert.AreEqual(names.Length, values.Length);

            // assert one-by-one
            for (var i = 0; i < names.Length; i++) {
                Assert.AreEqual(values[i], env.Runtime.VariableService.GetVariableValue(null, names[i]));
            }

            // get and assert all
            var all = env.Runtime.VariableService.GetVariableValueAll();
            for (var i = 0; i < names.Length; i++) {
                Assert.AreEqual(values[i], all.Get(new DeploymentIdNamePair(null, names[i])));
            }

            // get by request
            ISet<DeploymentIdNamePair> nameSet = new HashSet<DeploymentIdNamePair>();
            foreach (var name in names) {
                nameSet.Add(new DeploymentIdNamePair(null, name));
            }

            var valueSet = env.Runtime.VariableService.GetVariableValue(nameSet);
            for (var i = 0; i < names.Length; i++) {
                Assert.AreEqual(values[i], valueSet.Get(new DeploymentIdNamePair(null, names[i])));
            }
        }

        private static void TryInvalidSetAPI(
            RegressionEnvironment env,
            string deploymentId,
            string variableName,
            object newValue)
        {
            try {
                env.Runtime.VariableService.SetVariableValue(deploymentId, variableName, newValue);
                Assert.Fail();
            }
            catch (VariableConstantValueException ex) {
                Assert.AreEqual(
                    ex.Message,
                    "Variable by name '" +
                    variableName +
                    "' is declared as constant and may not be assigned a new value");
            }

            try {
                env.Runtime
                    .VariableService.SetVariableValue(
                        Collections.SingletonMap(new DeploymentIdNamePair(deploymentId, variableName), newValue));
                Assert.Fail();
            }
            catch (VariableConstantValueException ex) {
                Assert.AreEqual(
                    ex.Message,
                    "Variable by name '" +
                    variableName +
                    "' is declared as constant and may not be assigned a new value");
            }
        }

        private static void TryOperator(
            RegressionEnvironment env,
            RegressionPath path,
            string @operator,
            object[][] testdata)
        {
            env.CompileDeploy(
                "@Name('s0') select TheString as c0,IntPrimitive as c1 from SupportBean(" + @operator + ")",
                path);
            env.AddListener("s0");

            // initiate
            env.SendEventBean(new SupportBean_S0(10, "S01"));

            for (var i = 0; i < testdata.Length; i++) {
                var bean = new SupportBean();
                var testValue = testdata[i][0];
                if (testValue is int?) {
                    bean.IntBoxed = (int?) testValue;
                }
                else if (testValue is SupportEnum) {
                    bean.EnumValue = (SupportEnum) testValue;
                }
                else {
                    bean.ShortBoxed = testValue.AsInt16();
                }

                var expected = (bool) testdata[i][1];

                env.SendEventBean(bean);
                Assert.AreEqual(expected, env.Listener("s0").GetAndClearIsInvoked(), "Failed at " + i);
            }

            // assert type of expression
            var item = SupportFilterServiceHelper.GetFilterSvcSingle(env.Statement("s0"));
            Assert.IsTrue(item.Op != FilterOperator.BOOLEAN_EXPRESSION);

            env.UndeployModuleContaining("s0");
        }

        public static int GetValue(MyEnumWithOverride value)
        {
            switch (value) {
                case MyEnumWithOverride.LONG:
                    return 1;

                case MyEnumWithOverride.SHORT:
                    return -1;

                default:
                    throw new ArgumentException();
            }
        }

        internal class EPLVariableUseWVarargs : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                var epl =
                    "@Name('s0') select * from SupportBean(varargsTestClient.FunctionWithVarargs(LongBoxed, varargsTestClient.GetTestObject(TheString))) as t";
                env.CompileDeploy(epl).AddListener("s0");

                SupportBean sb = new SupportBean("5", 0);
                sb.LongBoxed = 5L;
                env.SendEventBean(sb);
                env.Listener("s0").AssertOneGetNewAndReset();

                env.UndeployAll();
            }
        }

        internal class EPLVariableUseFilterConstantCustomTypePreconfigured : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                env.CompileDeploy("@Name('s0') select * from MyVariableCustomEvent(Name=my_variable_custom_typed)")
                    .AddListener("s0");

                env.SendEventBean(new MyVariableCustomEvent(MyVariableCustomType.Of("abc")));
                Assert.IsTrue(env.Listener("s0").IsInvoked);

                env.UndeployAll();
            }
        }

        internal class EPLVariableUseSimplePreconfigured : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                env.CompileDeploy("@Name('s0') select var_simple_preconfig_const as c0 from SupportBean")
                    .AddListener("s0");

                env.Milestone(0);

                env.SendEventBean(new SupportBean("E1", 0));
                Assert.AreEqual(true, env.Listener("s0").AssertOneGetNewAndReset().Get("c0"));

                env.Milestone(1);

                env.UndeployAll();
            }
        }

        internal class EPLVariableUseSimpleSameModule : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                var epl = "create variable boolean var_simple_module_const = true;\n" +
                          "@Name('s0') select var_simple_module_const as c0 from SupportBean;\n";
                env.CompileDeploy(epl).AddListener("s0");
                env.Milestone(0);
                env.SendEventBean(new SupportBean("E1", 0));
                Assert.AreEqual(true, env.Listener("s0").AssertOneGetNewAndReset().Get("c0"));
                env.UndeployAll();
            }
        }

        internal class EPLVariableUseSimpleTwoModules : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                var path = new RegressionPath();
                env.CompileDeploy("create variable boolean var_simple_twomodule_const = true", path);
                env.CompileDeploy("@Name('s0') select var_simple_twomodule_const as c0 from SupportBean", path);
                env.AddListener("s0");
                env.Milestone(0);
                env.SendEventBean(new SupportBean("E1", 0));
                Assert.AreEqual(true, env.Listener("s0").AssertOneGetNewAndReset().Get("c0"));
                env.UndeployAll();
            }
        }

        internal class CustomSubscriber
        {
            private readonly CountDownLatch latch;
            private readonly IList<string> values;

            public CustomSubscriber(
                CountDownLatch latch,
                IList<string> values)
            {
                this.latch = latch;
                this.values = values;
            }

            public void Update(IDictionary<string, object> @event)
            {
                var value = (string) @event.Get("c0");
                values.Add(value);
                latch.CountDown();
            }
        }

        internal class EPLVariableUseDotSeparateThread : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                env.Runtime.VariableService.SetVariableValue(
                    null,
                    "mySimpleVariableService",
                    new MySimpleVariableService());

                var epStatement = env
                    .CompileDeploy("@Name('s0') select mySimpleVariableService.DoSomething() as c0 from SupportBean")
                    .Statement("s0");

                var latch = new CountDownLatch(1);
                IList<string> values = new List<string>();
                epStatement.SetSubscriber(new CustomSubscriber(latch, values));

                var executorService = Executors.NewSingleThreadExecutor();
                executorService.Submit(() => env.SendEventBean(new SupportBean()));

                try {
                    latch.Await();
                }
                catch (ThreadInterruptedException) {
                    Assert.Fail();
                }

                executorService.Shutdown();

                Assert.AreEqual(1, values.Count);
                Assert.AreEqual("hello", values[0]);

                env.UndeployAll();
            }
        }

        internal class EPLVariableUseInvokeMethod : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                // declared via EPL
                var path = new RegressionPath();
                env.CompileDeploy(
                    "create constant variable MySimpleVariableService myService = MySimpleVariableServiceFactory.MakeService()",
                    path);

                // exercise
                var epl = "@Name('s0') select " +
                          "myService.DoSomething() as c0, " +
                          "myInitService.DoSomething() as c1 " +
                          "from SupportBean";
                env.CompileDeploy(epl, path).AddListener("s0");

                env.SendEventBean(new SupportBean("E1", 1));
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    new[] {"c0", "c1"},
                    new object[] {"hello", "hello"});

                env.UndeployAll();
            }
        }

        internal class EPLVariableUseConstantVariable : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                var path = new RegressionPath();

                env.CompileDeploy("create const variable int MYCONST = 10", path);
                TryOperator(
                    env,
                    path,
                    "MYCONST = IntBoxed",
                    new[] {new object[] {10, true}, new object[] {9, false}, new object[] {null, false}});

                TryOperator(
                    env,
                    path,
                    "MYCONST > IntBoxed",
                    new[] {
                        new object[] {11, false}, new object[] {10, false}, new object[] {9, true},
                        new object[] {8, true}
                    });
                TryOperator(
                    env,
                    path,
                    "MYCONST >= IntBoxed",
                    new[] {
                        new object[] {11, false}, new object[] {10, true}, new object[] {9, true},
                        new object[] {8, true}
                    });
                TryOperator(
                    env,
                    path,
                    "MYCONST < IntBoxed",
                    new[] {
                        new object[] {11, true}, new object[] {10, false}, new object[] {9, false},
                        new object[] {8, false}
                    });
                TryOperator(
                    env,
                    path,
                    "MYCONST <= IntBoxed",
                    new[] {
                        new object[] {11, true}, new object[] {10, true}, new object[] {9, false},
                        new object[] {8, false}
                    });

                TryOperator(
                    env,
                    path,
                    "IntBoxed < MYCONST",
                    new[] {
                        new object[] {11, false}, new object[] {10, false}, new object[] {9, true},
                        new object[] {8, true}
                    });
                TryOperator(
                    env,
                    path,
                    "IntBoxed <= MYCONST",
                    new[] {
                        new object[] {11, false}, new object[] {10, true}, new object[] {9, true},
                        new object[] {8, true}
                    });
                TryOperator(
                    env,
                    path,
                    "IntBoxed > MYCONST",
                    new[] {
                        new object[] {11, true}, new object[] {10, false}, new object[] {9, false},
                        new object[] {8, false}
                    });
                TryOperator(
                    env,
                    path,
                    "IntBoxed >= MYCONST",
                    new[] {
                        new object[] {11, true}, new object[] {10, true}, new object[] {9, false},
                        new object[] {8, false}
                    });

                TryOperator(
                    env,
                    path,
                    "IntBoxed in (MYCONST)",
                    new[] {
                        new object[] {11, false}, new object[] {10, true}, new object[] {9, false},
                        new object[] {8, false}
                    });
                TryOperator(
                    env,
                    path,
                    "IntBoxed between MYCONST and MYCONST",
                    new[] {
                        new object[] {11, false}, new object[] {10, true}, new object[] {9, false},
                        new object[] {8, false}
                    });

                TryOperator(
                    env,
                    path,
                    "MYCONST != IntBoxed",
                    new[] {new object[] {10, false}, new object[] {9, true}, new object[] {null, false}});
                TryOperator(
                    env,
                    path,
                    "IntBoxed != MYCONST",
                    new[] {new object[] {10, false}, new object[] {9, true}, new object[] {null, false}});

                TryOperator(
                    env,
                    path,
                    "IntBoxed not in (MYCONST)",
                    new[] {
                        new object[] {11, true}, new object[] {10, false}, new object[] {9, true},
                        new object[] {8, true}
                    });
                TryOperator(
                    env,
                    path,
                    "IntBoxed not between MYCONST and MYCONST",
                    new[] {
                        new object[] {11, true}, new object[] {10, false}, new object[] {9, true},
                        new object[] {8, true}
                    });

                TryOperator(
                    env,
                    path,
                    "MYCONST is IntBoxed",
                    new[] {new object[] {10, true}, new object[] {9, false}, new object[] {null, false}});
                TryOperator(
                    env,
                    path,
                    "IntBoxed is MYCONST",
                    new[] {new object[] {10, true}, new object[] {9, false}, new object[] {null, false}});

                TryOperator(
                    env,
                    path,
                    "MYCONST is not IntBoxed",
                    new[] {new object[] {10, false}, new object[] {9, true}, new object[] {null, true}});
                TryOperator(
                    env,
                    path,
                    "IntBoxed is not MYCONST",
                    new[] {new object[] {10, false}, new object[] {9, true}, new object[] {null, true}});

                // try coercion
                TryOperator(
                    env,
                    path,
                    "MYCONST = ShortBoxed",
                    new[] {
                        new object[] {(short) 10, true}, new object[] {(short) 9, false}, new object[] {null, false}
                    });
                TryOperator(
                    env,
                    path,
                    "ShortBoxed = MYCONST",
                    new[] {
                        new object[] {(short) 10, true}, new object[] {(short) 9, false}, new object[] {null, false}
                    });

                TryOperator(
                    env,
                    path,
                    "MYCONST > ShortBoxed",
                    new[] {
                        new object[] {(short) 11, false}, new object[] {(short) 10, false},
                        new object[] {(short) 9, true},
                        new object[] {(short) 8, true}
                    });
                TryOperator(
                    env,
                    path,
                    "ShortBoxed < MYCONST",
                    new[] {
                        new object[] {(short) 11, false}, new object[] {(short) 10, false},
                        new object[] {(short) 9, true},
                        new object[] {(short) 8, true}
                    });

                TryOperator(
                    env,
                    path,
                    "ShortBoxed in (MYCONST)",
                    new[] {
                        new object[] {(short) 11, false}, new object[] {(short) 10, true},
                        new object[] {(short) 9, false},
                        new object[] {(short) 8, false}
                    });

                // test SODA
                env.UndeployAll();

                var epl = "@Name('variable') create constant variable int MYCONST = 10";
                env.EplToModelCompileDeploy(epl);

                // test invalid
                TryInvalidCompile(
                    env,
                    path,
                    "on SupportBean set MYCONST = 10",
                    "Failed to validate assignment expression 'MYCONST=10': Variable by name 'MYCONST' is declared constant and may not be set [on SupportBean set MYCONST = 10]");
                TryInvalidCompile(
                    env,
                    path,
                    "select * from SupportBean output when true then set MYCONST=1",
                    "Failed to validate the output rate limiting clause: Failed to validate assignment expression 'MYCONST=1': Variable by name 'MYCONST' is declared constant and may not be set [select * from SupportBean output when true then set MYCONST=1]");

                // assure no update via API
                TryInvalidSetAPI(env, env.DeploymentId("variable"), "MYCONST", 1);

                // add constant variable via config API
                TryInvalidSetAPI(env, null, "MYCONST_TWO", "dummy");
                TryInvalidSetAPI(env, null, "MYCONST_THREE", false);

                // try ESPER-653
                env.CompileDeploy(
                    "@Name('s0') create constant variable com.espertech.esper.compat.DateTimeEx START_TIME = com.espertech.esper.compat.DateTimeEx.NowUtc()");
                var value = env.GetEnumerator("s0").Advance().Get("START_TIME");
                Assert.IsNotNull(value);
                env.UndeployModuleContaining("s0");

                // test array constant
                env.UndeployAll();
                env.CompileDeploy("create constant variable string[] var_strings = {'E1', 'E2'}", path);
                env.CompileDeploy("@Name('s0') select var_strings from SupportBean", path);
                Assert.AreEqual(typeof(string[]), env.Statement("s0").EventType.GetPropertyType("var_strings"));
                env.UndeployModuleContaining("s0");

                TryAssertionArrayVar(env, path, "var_strings");

                TryOperator(
                    env,
                    path,
                    "IntBoxed in (10, 8)",
                    new[] {
                        new object[] {11, false}, new object[] {10, true}, new object[] {9, false},
                        new object[] {8, true}
                    });

                env.CompileDeploy("create constant variable int [ ] var_ints = {8, 10}", path);
                TryOperator(
                    env,
                    path,
                    "IntBoxed in (var_ints)",
                    new[] {
                        new object[] {11, false}, new object[] {10, true}, new object[] {9, false},
                        new object[] {8, true}
                    });

                env.CompileDeploy("create constant variable int[]  var_intstwo = {9}", path);
                TryOperator(
                    env,
                    path,
                    "IntBoxed in (var_ints, var_intstwo)",
                    new[] {
                        new object[] {11, false}, new object[] {10, true}, new object[] {9, true},
                        new object[] {8, true}
                    });

                TryInvalidCompile(
                    env,
                    "create constant variable SupportBean[] var_beans",
                    "Cannot create variable 'var_beans', type 'SupportBean' cannot be declared as an array type as it is an event type [create constant variable SupportBean[] var_beans]");

                // test array of primitives
                env.CompileDeploy("@Name('s0') create variable byte[] myBytesBoxed");
                object[][] expectedType = {
                    new object[] {"myBytesBoxed", typeof(byte?[])}
                };
                SupportEventTypeAssertionUtil.AssertEventTypeProperties(
                    expectedType,
                    env.Statement("s0").EventType,
                    SupportEventTypeAssertionEnum.NAME,
                    SupportEventTypeAssertionEnum.TYPE);
                env.UndeployModuleContaining("s0");

                env.CompileDeploy("@Name('s0') create variable byte[primitive] myBytesPrimitive");
                expectedType = new[] {
                    new object[] {"myBytesPrimitive", typeof(byte[])}
                };
                SupportEventTypeAssertionUtil.AssertEventTypeProperties(
                    expectedType,
                    env.Statement("s0").EventType,
                    SupportEventTypeAssertionEnum.NAME,
                    SupportEventTypeAssertionEnum.TYPE);
                env.UndeployAll();

                // test enum constant
                env.CompileDeploy("create constant variable SupportEnum var_enumone = SupportEnum.ENUM_VALUE_2", path);
                TryOperator(
                    env,
                    path,
                    "var_enumone = EnumValue",
                    new[] {
                        new object[] {SupportEnum.ENUM_VALUE_3, false}, new object[] {SupportEnum.ENUM_VALUE_2, true},
                        new object[] {SupportEnum.ENUM_VALUE_1, false}
                    });

                env.CompileDeploy(
                    "create constant variable SupportEnum[] var_enumarr = {SupportEnum.ENUM_VALUE_2, SupportEnum.ENUM_VALUE_1}",
                    path);
                TryOperator(
                    env,
                    path,
                    "EnumValue in (var_enumarr, var_enumone)",
                    new[] {
                        new object[] {SupportEnum.ENUM_VALUE_3, false}, new object[] {SupportEnum.ENUM_VALUE_2, true},
                        new object[] {SupportEnum.ENUM_VALUE_1, true}
                    });

                env.CompileDeploy("create variable SupportEnum var_enumtwo = SupportEnum.ENUM_VALUE_2", path);
                env.CompileDeploy("on SupportBean set var_enumtwo = EnumValue", path);

                env.UndeployAll();
            }

            private static void TryAssertionArrayVar(
                RegressionEnvironment env,
                RegressionPath path,
                string varName)
            {
                env.CompileDeploy("@Name('s0') select * from SupportBean(TheString in (" + varName + "))", path)
                    .AddListener("s0");

                SendBeanAssert(env, "E1", true);
                SendBeanAssert(env, "E2", true);
                SendBeanAssert(env, "E3", false);

                env.UndeployAll();
            }

            private static void SendBeanAssert(
                RegressionEnvironment env,
                string theString,
                bool expected)
            {
                env.SendEventBean(new SupportBean(theString, 1));
                Assert.AreEqual(expected, env.Listener("s0").GetAndClearIsInvoked());
            }
        }

        internal class EPLVariableUseEPRuntime : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                var runtimeSPI = (EPVariableServiceSPI) env.Runtime.VariableService;
                var types = runtimeSPI.VariableTypeAll;
                Assert.AreEqual(typeof(int?), types.Get(new DeploymentIdNamePair(null, "var1")));
                Assert.AreEqual(typeof(string), types.Get(new DeploymentIdNamePair(null, "var2")));

                Assert.AreEqual(typeof(int?), runtimeSPI.GetVariableType(null, "var1"));
                Assert.AreEqual(typeof(string), runtimeSPI.GetVariableType(null, "var2"));

                var stmtTextSet = "on SupportBean set var1 = IntPrimitive, var2 = TheString";
                env.CompileDeploy(stmtTextSet);

                AssertVariableValuesPreconfigured(
                    env,
                    new[] {"var1", "var2"},
                    new object[] {-1, "abc"});
                SendSupportBean(env, null, 99);
                AssertVariableValuesPreconfigured(
                    env,
                    new[] {"var1", "var2"},
                    new object[] {99, null});

                env.Runtime.VariableService.SetVariableValue(null, "var2", "def");
                AssertVariableValuesPreconfigured(
                    env,
                    new[] {"var1", "var2"},
                    new object[] {99, "def"});

                env.Milestone(0);

                AssertVariableValuesPreconfigured(
                    env,
                    new[] {"var1", "var2"},
                    new object[] {99, "def"});
                env.Runtime.VariableService.SetVariableValue(null, "var1", 123);
                AssertVariableValuesPreconfigured(
                    env,
                    new[] {"var1", "var2"},
                    new object[] {123, "def"});

                env.Milestone(1);

                AssertVariableValuesPreconfigured(
                    env,
                    new[] {"var1", "var2"},
                    new object[] {123, "def"});
                IDictionary<DeploymentIdNamePair, object> newValues = new Dictionary<DeploymentIdNamePair, object>();
                newValues.Put(new DeploymentIdNamePair(null, "var1"), 20);
                env.Runtime.VariableService.SetVariableValue(newValues);
                AssertVariableValuesPreconfigured(
                    env,
                    new[] {"var1", "var2"},
                    new object[] {20, "def"});

                newValues.Put(new DeploymentIdNamePair(null, "var1"), (byte) 21);
                newValues.Put(new DeploymentIdNamePair(null, "var2"), "test");
                env.Runtime.VariableService.SetVariableValue(newValues);
                AssertVariableValuesPreconfigured(
                    env,
                    new[] {"var1", "var2"},
                    new object[] {21, "test"});

                newValues.Put(new DeploymentIdNamePair(null, "var1"), null);
                newValues.Put(new DeploymentIdNamePair(null, "var2"), null);
                env.Runtime.VariableService.SetVariableValue(newValues);
                AssertVariableValuesPreconfigured(
                    env,
                    new[] {"var1", "var2"},
                    new object[] {null, null});

                // try variable not found
                try {
                    env.Runtime.VariableService.SetVariableValue(null, "dummy", null);
                    Assert.Fail();
                }
                catch (VariableNotFoundException ex) {
                    // expected
                    Assert.AreEqual("Variable by name 'dummy' has not been declared", ex.Message);
                }

                // try variable not found
                try {
                    newValues.Put(new DeploymentIdNamePair(null, "dummy2"), 20);
                    env.Runtime.VariableService.SetVariableValue(newValues);
                    Assert.Fail();
                }
                catch (VariableNotFoundException ex) {
                    // expected
                    Assert.AreEqual("Variable by name 'dummy2' has not been declared", ex.Message);
                }

                // create new variable on the fly
                env.CompileDeploy("@Name('create') create variable int dummy = 20 + 20");
                Assert.AreEqual(40, env.Runtime.VariableService.GetVariableValue(env.DeploymentId("create"), "dummy"));

                // try type coercion
                try {
                    env.Runtime.VariableService.SetVariableValue(env.DeploymentId("create"), "dummy", "abc");
                    Assert.Fail();
                }
                catch (VariableValueException ex) {
                    // expected
                    Assert.AreEqual(
                        "Variable 'dummy' of declared type " + typeof(int?).CleanName() + " cannot be assigned a value of type System.String",
                        ex.Message);
                }

                try {
                    env.Runtime.VariableService.SetVariableValue(env.DeploymentId("create"), "dummy", 100L);
                    Assert.Fail();
                }
                catch (VariableValueException ex) {
                    // expected
                    Assert.AreEqual(
                        "Variable 'dummy' of declared type " + typeof(int?).CleanName() + " cannot be assigned a value of type System.Int64",
                        ex.Message);
                }

                try {
                    env.Runtime.VariableService.SetVariableValue(null, "var2", 0);
                    Assert.Fail();
                }
                catch (VariableValueException ex) {
                    // expected
                    Assert.AreEqual(
                        "Variable 'var2' of declared type System.String cannot be assigned a value of type System.Int32",
                        ex.Message);
                }

                // coercion
                env.Runtime.VariableService.SetVariableValue(null, "var1", (short) -1);
                AssertVariableValuesPreconfigured(
                    env,
                    new[] {"var1", "var2"},
                    new object[] {-1, null});

                // rollback for coercion failed
                newValues = new LinkedHashMap<DeploymentIdNamePair, object>(); // preserve order
                newValues.Put(new DeploymentIdNamePair(null, "var2"), "xyz");
                newValues.Put(new DeploymentIdNamePair(null, "var1"), 4.4d);
                try {
                    env.Runtime.VariableService.SetVariableValue(newValues);
                    Assert.Fail();
                }
                catch (VariableValueException) {
                    // expected
                }

                AssertVariableValuesPreconfigured(
                    env,
                    new[] {"var1", "var2"},
                    new object[] {-1, null});

                // rollback for variable not found
                newValues = new LinkedHashMap<DeploymentIdNamePair, object>(); // preserve order
                newValues.Put(new DeploymentIdNamePair(null, "var2"), "xyz");
                newValues.Put(new DeploymentIdNamePair(null, "var1"), 1);
                newValues.Put(new DeploymentIdNamePair(null, "notfoundvariable"), null);
                try {
                    env.Runtime.VariableService.SetVariableValue(newValues);
                    Assert.Fail();
                }
                catch (VariableNotFoundException) {
                    // expected
                }

                AssertVariableValuesPreconfigured(
                    env,
                    new[] {"var1", "var2"},
                    new object[] {-1, null});

                env.UndeployAll();
            }
        }

        internal class EPLVariableUseVariableInFilterBoolean : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                var stmtTextSet = "@Name('set') on SupportBean_S0 set var1IFB = P00, var2IFB = P01";
                env.CompileDeploy(stmtTextSet).AddListener("set");
                string[] fieldsVar = {"var1IFB", "var2IFB"};
                EPAssertionUtil.AssertPropsPerRow(
                    env.GetEnumerator("set"),
                    fieldsVar,
                    new[] {new object[] {null, null}});

                var stmtTextSelect =
                    "@Name('s0') select TheString, IntPrimitive from SupportBean(TheString = var1IFB or TheString = var2IFB)";
                string[] fieldsSelect = {"TheString", "IntPrimitive"};
                env.CompileDeploy(stmtTextSelect).AddListener("s0");

                SendSupportBean(env, null, 1);
                Assert.IsFalse(env.Listener("s0").IsInvoked);

                env.Milestone(0);

                SendSupportBeanS0NewThread(env, 100, "a", "b");
                EPAssertionUtil.AssertProps(
                    env.Listener("set").AssertOneGetNewAndReset(),
                    fieldsVar,
                    new object[] {"a", "b"});

                SendSupportBean(env, "a", 2);
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fieldsSelect,
                    new object[] {"a", 2});

                SendSupportBean(env, null, 1);
                Assert.IsFalse(env.Listener("s0").IsInvoked);

                env.Milestone(1);

                SendSupportBean(env, "b", 3);
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fieldsSelect,
                    new object[] {"b", 3});

                SendSupportBean(env, "c", 4);
                Assert.IsFalse(env.Listener("s0").IsInvoked);

                env.Milestone(2);

                SendSupportBeanS0NewThread(env, 100, "e", "c");
                EPAssertionUtil.AssertProps(
                    env.Listener("set").AssertOneGetNewAndReset(),
                    fieldsVar,
                    new object[] {"e", "c"});

                SendSupportBean(env, "c", 5);
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fieldsSelect,
                    new object[] {"c", 5});

                SendSupportBean(env, "e", 6);
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fieldsSelect,
                    new object[] {"e", 6});

                env.UndeployAll();
            }
        }

        internal class EPLVariableUseVariableInFilter : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                var stmtTextSet = "@Name('set') on SupportBean_S0 set var1IF = P00";
                env.CompileDeploy(stmtTextSet).AddListener("set");
                string[] fieldsVar = {"var1IF"};
                EPAssertionUtil.AssertPropsPerRow(
                    env.GetEnumerator("set"),
                    fieldsVar,
                    new[] {new object[] {null}});

                var stmtTextSelect = "@Name('s0') select TheString, IntPrimitive from SupportBean(TheString = var1IF)";
                string[] fieldsSelect = {"TheString", "IntPrimitive"};
                env.CompileDeploy(stmtTextSelect).AddListener("s0");

                SendSupportBean(env, null, 1);
                Assert.IsFalse(env.Listener("s0").IsInvoked);

                SendSupportBeanS0NewThread(env, 100, "a", "b");
                EPAssertionUtil.AssertProps(
                    env.Listener("set").AssertOneGetNewAndReset(),
                    fieldsVar,
                    new object[] {"a"});

                SendSupportBean(env, "a", 2);
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fieldsSelect,
                    new object[] {"a", 2});

                env.Milestone(0);

                SendSupportBean(env, null, 1);
                Assert.IsFalse(env.Listener("s0").IsInvoked);

                SendSupportBeanS0NewThread(env, 100, "e", "c");
                EPAssertionUtil.AssertProps(
                    env.Listener("set").AssertOneGetNewAndReset(),
                    fieldsVar,
                    new object[] {"e"});

                env.Milestone(1);

                SendSupportBean(env, "c", 5);
                Assert.IsFalse(env.Listener("s0").IsInvoked);

                SendSupportBean(env, "e", 6);
                EPAssertionUtil.AssertProps(
                    env.Listener("s0").AssertOneGetNewAndReset(),
                    fieldsSelect,
                    new object[] {"e", 6});

                env.UndeployAll();
            }
        }

        internal class EPLVariableInvalidSet : RegressionExecution
        {
            public void Run(RegressionEnvironment env)
            {
                TryInvalidCompile(
                    env,
                    "on SupportBean set dummy = 100",
                    "Variable by name 'dummy' has not been created or configured");

                TryInvalidCompile(
                    env,
                    "on SupportBean set var1IS = 1",
                    "Variable 'var1IS' of declared type System.String cannot be assigned a value of type System.Int32");

                TryInvalidCompile(
                    env,
                    "on SupportBean set var3IS = 'abc'",
                    "Variable 'var3IS' of declared type System.Nullable<System.Int32> cannot be assigned a value of type System.String");

                TryInvalidCompile(
                    env,
                    "on SupportBean set var3IS = DoublePrimitive",
                    "Variable 'var3IS' of declared type System.Nullable<System.Int32> cannot be assigned a value of type System.Double");

                TryInvalidCompile(env, "on SupportBean set var2IS = 'false'", "skip");
                TryInvalidCompile(env, "on SupportBean set var3IS = 1.1", "skip");
                TryInvalidCompile(env, "on SupportBean set var3IS = 22222222222222", "skip");
                TryInvalidCompile(
                    env,
                    "on SupportBean set var3IS",
                    "Missing variable assignment expression in assignment number 0 [");
            }
        }

        [Serializable]
        public class A
        {
            public string Value => "";
        }

        public class B
        {
        }

        public class MySimpleVariableServiceFactory
        {
            public static MySimpleVariableService MakeService()
            {
                return new MySimpleVariableService();
            }
        }

        public class MySimpleVariableService
        {
            public string DoSomething()
            {
                return "hello";
            }
        }

        public class MyVariableCustomEvent
        {
            internal MyVariableCustomEvent(MyVariableCustomType name)
            {
                Name = name;
            }

            public MyVariableCustomType Name { get; }
        }

        public class MyVariableCustomType
        {
            internal MyVariableCustomType(string name)
            {
                Name = name;
            }

            public string Name { get; }

            public static MyVariableCustomType Of(string name)
            {
                return new MyVariableCustomType(name);
            }

            protected bool Equals(MyVariableCustomType other)
            {
                return string.Equals(Name, other.Name);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) {
                    return false;
                }

                if (ReferenceEquals(this, obj)) {
                    return true;
                }

                if (obj.GetType() != GetType()) {
                    return false;
                }

                return Equals((MyVariableCustomType) obj);
            }

            public override int GetHashCode()
            {
                return Name != null ? Name.GetHashCode() : 0;
            }
        }

        public class SupportVarargsObject
        {
            private long? value;

            public SupportVarargsObject(long? value)
            {
                this.value = value;
            }

            public long? Value {
                get => value;
                set => this.value = value;
            }
        }

        public interface SupportVarargsClient
        {
            bool FunctionWithVarargs(
                long? longValue,
                params object[] objects);

            SupportVarargsObject GetTestObject(String stringValue);
        }

        public class SupportVarargsClientImpl : SupportVarargsClient
        {
            public bool FunctionWithVarargs(
                long? longValue,
                params object[] objects)
            {
                SupportVarargsObject obj = (SupportVarargsObject) objects[0];
                return longValue.Equals(obj.Value);
            }

            public SupportVarargsObject GetTestObject(String stringValue)
            {
                return new SupportVarargsObject(Int64.Parse(stringValue));
            }
        }
    }
} // end of namespace
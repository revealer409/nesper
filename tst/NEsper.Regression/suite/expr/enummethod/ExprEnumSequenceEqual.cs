///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

using com.espertech.esper.compat;
using com.espertech.esper.regressionlib.framework;
using com.espertech.esper.regressionlib.support.bean;
using com.espertech.esper.regressionlib.support.expreval;


using static com.espertech.esper.regressionlib.framework.SupportMessageAssertUtil;
using static com.espertech.esper.regressionlib.support.util.LambdaAssertionUtil;

namespace com.espertech.esper.regressionlib.suite.expr.enummethod
{
	public class ExprEnumSequenceEqual
	{
		public static ICollection<RegressionExecution> Executions()
		{
			List<RegressionExecution> execs = new List<RegressionExecution>();
			WithWSelectFrom(execs);
			WithTwoProperties(execs);
			WithInvalid(execs);
			return execs;
		}

		public static IList<RegressionExecution> WithInvalid(IList<RegressionExecution> execs = null)
		{
			execs = execs ?? new List<RegressionExecution>();
			execs.Add(new ExprEnumSequenceEqualInvalid());
			return execs;
		}

		public static IList<RegressionExecution> WithTwoProperties(IList<RegressionExecution> execs = null)
		{
			execs = execs ?? new List<RegressionExecution>();
			execs.Add(new ExprEnumSequenceEqualTwoProperties());
			return execs;
		}

		public static IList<RegressionExecution> WithWSelectFrom(IList<RegressionExecution> execs = null)
		{
			execs = execs ?? new List<RegressionExecution>();
			execs.Add(new ExprEnumSequenceEqualWSelectFrom());
			return execs;
		}

		internal class ExprEnumSequenceEqualWSelectFrom : RegressionExecution
		{
			public void Run(RegressionEnvironment env)
			{
				string[] fields = "c0".SplitCsv();
				SupportEvalBuilder builder = new SupportEvalBuilder("SupportBean_ST0_Container");
				builder.WithExpression(fields[0], "Contained.selectFrom(x => Key0).sequenceEqual(Contained.selectFrom(y => Id))");

				builder.WithStatementConsumer(stmt => AssertTypesAllSame(stmt.EventType, fields, typeof(bool?)));

				builder.WithAssertion(SupportBean_ST0_Container.Make3Value("I1,E1,0", "I2,E2,0")).Expect(fields, false);

				builder.WithAssertion(SupportBean_ST0_Container.Make3Value("I3,I3,0", "X4,X4,0")).Expect(fields, true);

				builder.WithAssertion(SupportBean_ST0_Container.Make3Value("I3,I3,0", "X4,Y4,0")).Expect(fields, false);

				builder.WithAssertion(SupportBean_ST0_Container.Make3Value("I3,I3,0", "Y4,X4,0")).Expect(fields, false);

				builder.Run(env);
			}
		}

		internal class ExprEnumSequenceEqualTwoProperties : RegressionExecution
		{
			public void Run(RegressionEnvironment env)
			{
				string[] fields = "c0".SplitCsv();
				SupportEvalBuilder builder = new SupportEvalBuilder("SupportCollection");
				builder.WithExpression(fields[0], "Strvals.sequenceEqual(Strvalstwo)");

				builder.WithStatementConsumer(stmt => AssertTypesAllSame(stmt.EventType, fields, typeof(bool?)));

				builder.WithAssertion(SupportCollection.MakeString("E1,E2,E3", "E1,E2,E3")).Expect(fields, true);

				builder.WithAssertion(SupportCollection.MakeString("E1,E3", "E1,E2,E3")).Expect(fields, false);

				builder.WithAssertion(SupportCollection.MakeString("E1,E3", "E1,E3")).Expect(fields, true);

				builder.WithAssertion(SupportCollection.MakeString("E1,E2,E3", "E1,E3")).Expect(fields, false);

				builder.WithAssertion(SupportCollection.MakeString("E1,E2,null,E3", "E1,E2,null,E3")).Expect(fields, true);

				builder.WithAssertion(SupportCollection.MakeString("E1,E2,E3", "E1,E2,null")).Expect(fields, false);

				builder.WithAssertion(SupportCollection.MakeString("E1,E2,null", "E1,E2,E3")).Expect(fields, false);

				builder.WithAssertion(SupportCollection.MakeString("E1", "")).Expect(fields, false);

				builder.WithAssertion(SupportCollection.MakeString("", "E1")).Expect(fields, false);

				builder.WithAssertion(SupportCollection.MakeString("E1", "E1")).Expect(fields, true);

				builder.WithAssertion(SupportCollection.MakeString("", "")).Expect(fields, true);

				builder.WithAssertion(SupportCollection.MakeString(null, "")).Expect(fields, new object[] { null });

				builder.WithAssertion(SupportCollection.MakeString("", null)).Expect(fields, false);

				builder.WithAssertion(SupportCollection.MakeString(null, null)).Expect(fields, new object[] { null });

				builder.Run(env);
			}
		}

		internal class ExprEnumSequenceEqualInvalid : RegressionExecution
		{
			public void Run(RegressionEnvironment env)
			{
				string epl;

				epl = "select window(*).sequenceEqual(Strvals) from SupportCollection#lastevent";
				TryInvalidCompile(
					env,
					epl,
					"Failed to validate select-clause expression 'window(*).sequenceEqual(Strvals)': Invalid input for built-in enumeration method 'sequenceEqual' and 1-parameter footprint, expecting collection of values (typically scalar values) as input, received collection of events of type 'SupportCollection'");
			}
		}
	}
} // end of namespace

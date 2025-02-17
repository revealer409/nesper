﻿///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2019 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Reflection;

namespace com.espertech.esper.compiler.client
{
    /// <summary>
    /// CoreAssemblyProvider determines what assemblies should be provided to the compiler
    /// as part of the compilation and linking process.
    /// </summary>
    public delegate IEnumerable<Assembly> CoreAssemblyProvider();
}

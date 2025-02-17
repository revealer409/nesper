///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

using com.espertech.esper.common.client.artifact;
using com.espertech.esper.common.@internal.bytecodemodel.core;

namespace com.espertech.esper.compiler.@internal.util
{
    public class CompilableItem
    {
        public CompilableItem(
            string providerClassName,
            IList<CodegenClass> classes,
            CompilableItemPostCompileLatch postCompileLatch,
            ICollection<IArtifact> artifactsProvided)
        {
            ProviderClassName = providerClassName;
            Classes = classes;
            PostCompileLatch = postCompileLatch;
            ArtifactsProvided = artifactsProvided;
        }

        public string ProviderClassName { get; }

        public IList<CodegenClass> Classes { get; }

        public CompilableItemPostCompileLatch PostCompileLatch { get; }

        public ICollection<IArtifact> ArtifactsProvided { get; }
    }
} // end of namespace
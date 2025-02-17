///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using com.espertech.esper.common.client.artifact;
using com.espertech.esper.common.@internal.epl.classprovided.core;

namespace com.espertech.esper.common.@internal.context.module
{
    public class EPModuleClassProvidedInitServicesImpl : EPModuleClassProvidedInitServices
    {
        public ClassProvidedCollector ClassProvidedCollector { get; }
        public IArtifactRepository ArtifactRepository { get; }

        public EPModuleClassProvidedInitServicesImpl(
            ClassProvidedCollector classProvidedCollector,
            IArtifactRepository artifactRepository)
        {
            ClassProvidedCollector = classProvidedCollector;
            ArtifactRepository = artifactRepository;
        }

        public IRuntimeArtifact ResolveArtifact(string artifactName)
        {
            return ArtifactRepository.Resolve(artifactName);
        }
    }
} // end of namespace
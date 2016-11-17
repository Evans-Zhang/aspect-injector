﻿using AspectInjector.Core.Models;
using Mono.Cecil;
using System.Collections.Generic;

namespace AspectInjector.Core.Contracts
{
    public interface IAdviceExtractor : IInitializable
    {
        IEnumerable<Advice> ExtractAdvices(ModuleDefinition module);
    }
}
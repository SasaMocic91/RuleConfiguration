﻿using RuleConfiguration.Engine.Generics;
using RuleConfiguration.Engine.Interfaces;

namespace RuleConfiguration.Engine;

/// <summary>
/// </summary>
public static class FilterFactory
{
    /// <summary>
    /// Creates a Filter&lt;TClass&gt; by passing the 'TClass' as a parameter.
    /// </summary>
    /// <param name="type"></param>
    /// <typeparam name="TClass"></typeparam>
    /// <returns></returns>
    public static IFilter Create(Type type)
    {
        Type[] typeArgs = { type };
        var filterType = typeof(Filter<>).MakeGenericType(typeArgs);
        return (IFilter) Activator.CreateInstance(filterType)!;
    }
}
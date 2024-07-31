﻿using System.Collections;
using System.Reflection;
using System.Resources;
using RuleConfiguration.Engine.Interfaces;

namespace RuleConfiguration.Engine.Resources;

/// <summary>
///     Collection of <see cref="Property" />.
/// </summary>
public class PropertyCollection : IPropertyCollection
{
    private readonly HashSet<Type> _visitedTypes;

    /// <summary>
    ///     Instantiates a new <see cref="PropertyCollection" />.
    /// </summary>
    /// <param name="type"></param>
    public PropertyCollection(Type type)
    {
        Type = type;
        _visitedTypes = new HashSet<Type>();
        Properties = LoadProperties(Type);
    }

    /// <summary>
    ///     Instantiates a new <see cref="PropertyCollection" />.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="resourceManager"></param>
    public PropertyCollection(Type type, ResourceManager resourceManager) : this(type)
    {
        LoadProperties(resourceManager);
    }

    private List<Property> Properties { get; }

    /// <summary>
    ///     Type from which the properties are loaded.
    /// </summary>
    public Type Type { get; }

    /// <summary>
    ///     ResourceManager which the properties descriptions should be gotten from.
    /// </summary>
    public ResourceManager ResourceManager { get; private set; }

    /// <summary>
    ///     Gets the number of <see cref="Property" /> contained in the <see cref="PropertyCollection" />.
    /// </summary>
    public int Count => Properties.Count();

    /// <summary>
    /// </summary>
    public object SyncRoot => throw new NotImplementedException();

    /// <summary>
    /// </summary>
    public bool IsSynchronized => throw new NotImplementedException();

    /// <summary>
    ///     Retrieves a property based on its Id.
    /// </summary>
    /// <param name="propertyId">Property conventionalized <see cref="Property.Id" />.</param>
    /// <returns></returns>
    public Property this[string propertyId]
    {
        get { return Properties.FirstOrDefault(p => p.Id.Equals(propertyId)); }
    }

    /// <summary>
    ///     Loads the properties names from the specified ResourceManager.
    /// </summary>
    /// <param name="resourceManager"></param>
    /// <returns></returns>
    public List<Property> LoadProperties(ResourceManager resourceManager)
    {
        ResourceManager = resourceManager;
        foreach (var property in Properties)
            property.Name = resourceManager.GetString(GetPropertyResourceName(property.Id)) ?? property.Name;

        return Properties;
    }

    /// <summary>
    ///     Copies the elements of the <see cref="PropertyCollection" /> to an System.Array,
    ///     starting at a particular System.Array index.
    /// </summary>
    /// <param name="array">
    ///     The one-dimensional System.Array that is the destination of the elements copied
    ///     from System.Collections.ICollection. The System.Array must have zero-based indexing.
    /// </param>
    /// <param name="index">The zero-based index in array at which copying begins.</param>
    public void CopyTo(Array array, int index)
    {
        Properties.CopyTo((Property[]) array, index);
    }

    /// <summary>
    ///     Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns></returns>
    public IEnumerator GetEnumerator()
    {
        return Properties.GetEnumerator();
    }

    /// <summary>
    ///     Converts the collection into a list.
    /// </summary>
    /// <returns></returns>
    public IList<Property> ToList()
    {
        var properties = new Property[Properties.Count];
        CopyTo(properties, 0);
        return properties;
    }

    private string GetPropertyResourceName(string propertyConventionName)
    {
        return propertyConventionName
            .Replace("[", "_")
            .Replace("]", "_")
            .Replace(".", "_");
    }

    private List<Property> LoadProperties(Type type)
    {
        var list = new List<Property>();
        if (_visitedTypes.Contains(type)) return list;

        _visitedTypes.Add(type);

        const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;
        var members = type.GetFields(bindingFlags).Cast<MemberInfo>()
            .Concat(type.GetProperties(bindingFlags)).ToArray();
        foreach (var member in members) list.AddRange(GetProperties(member));

        return list;
    }

    private IEnumerable<Property> GetProperties(MemberInfo member)
    {
        var memberType = GetMemberType(member);

        if (memberType.IsValueType || memberType == typeof(string))
            return new List<Property> { new(member.Name, member.Name, member) };

        if (memberType.IsGenericType && typeof(IEnumerable).IsAssignableFrom(memberType))
            return LoadProperties(memberType.GetGenericArguments()[0])
                .Select(p => new Property(member.Name + "[" + p.Id + "]", p.Name, p.Info));

        return LoadProperties(memberType)
            .Select(p => new Property(member.Name + "." + p.Id, p.Name, p.Info));
    }

    private Type GetMemberType(MemberInfo member)
    {
        return member.MemberType == MemberTypes.Property
            ? ( member as PropertyInfo ).PropertyType
            : ( member as FieldInfo ).FieldType;
    }
}
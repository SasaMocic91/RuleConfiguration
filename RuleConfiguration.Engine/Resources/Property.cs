using System.Reflection;

namespace RuleConfiguration.Engine.Resources;

/// <summary>
///     Provides information on the property to the Rule Builder.
/// </summary>
public class Property
{
    internal Property(string id, string name, MemberInfo info)
    {
        Id = id;
        Name = name;
        Info = info;
    }

    /// <summary>
    ///     Property identifier conventionalized by the Rule Builder.
    /// </summary>
    public string Id { get; }

    /// <summary>
    ///     Property name obtained from a ResourceManager, or the property's original name (in the absence of a ResourceManager
    ///     correspondent value).
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Property metadata.
    /// </summary>
    public MemberInfo Info { get; }

    public Type MemberType => Info.MemberType == MemberTypes.Property
        ? ( Info as PropertyInfo ).PropertyType
        : ( Info as FieldInfo ).FieldType;

    /// <summary>
    ///     String representation of <see cref="Property" />.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return string.Format("{0} ({1})", Name, Id);
    }
}
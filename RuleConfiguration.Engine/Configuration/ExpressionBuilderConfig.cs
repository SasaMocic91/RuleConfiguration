
using System.Configuration;
using RuleConfiguration.Engine.Common;

namespace RuleConfiguration.Engine.Configuration;

internal class ExpressionBuilderConfig : ConfigurationSection
{
    public const string SectionName = "RuleConfiguration.Engine";

    private const string SupportedTypeCollectionName = "SupportedTypes";

    [ConfigurationProperty(SupportedTypeCollectionName)]
    public SupportedTypesElementConfiguration SupportedTypes =>
        (SupportedTypesElementConfiguration) base[SupportedTypeCollectionName];

    public class SupportedTypeElement : ConfigurationElement
    {
        [ConfigurationProperty("type", IsRequired = true, IsKey = true)]
        public string Type => (string) base["type"];

        [ConfigurationProperty("typeGroup", IsRequired = true, IsKey = false)]
        public TypeGroup TypeGroup => (TypeGroup) base["typeGroup"];
    }

    [ConfigurationCollection(typeof(SupportedTypesElementConfiguration), AddItemName = "add")]
    public class SupportedTypesElementConfiguration : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SupportedTypeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null) throw new ArgumentNullException("element");

            return ( (SupportedTypeElement) element ).Type;
        }
    }
}
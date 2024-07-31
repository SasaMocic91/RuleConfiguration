using System.Configuration;

namespace RuleConfiguration.Engine.Configuration;

public class Settings
{
    public List<SupportedType> SupportedTypes { get; private set; }

    public static void LoadSettings(Settings settings)
    {
        var configSection =
            ConfigurationManager.GetSection(ExpressionBuilderConfig.SectionName) as ExpressionBuilderConfig;
        if (configSection == null) return;

        settings.SupportedTypes = new List<SupportedType>();
        foreach (ExpressionBuilderConfig.SupportedTypeElement supportedType in configSection.SupportedTypes)
        {
            var type = Type.GetType(supportedType.Type, false, true);
            if (type != null)
                settings.SupportedTypes.Add(new SupportedType { TypeGroup = supportedType.TypeGroup, Type = type });
        }
    }
}
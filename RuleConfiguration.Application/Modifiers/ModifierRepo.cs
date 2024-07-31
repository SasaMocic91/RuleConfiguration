using RuleConfiguration.Models;
using RuleConfiguration.Storage;
using RuleConfigurator.Application.Models;
using RuleConfigurator.Application.Modifiers;

namespace RuleConfiguration.Modifiers;

public class ModifierRepo(IRulesCache rulesCache)
{
    private readonly List<IModifier> _allModifiers = new()
    {
        new BonusModifier(),
        new JackpotModifier(),
        new TaxModifier()
    };


    public async Task<Ticket> ApplyModifiers(Ticket ticket)
    {
        var modifiers = await GetModifiers(ticket);
        foreach (var mod in modifiers) mod.Modify(ticket);

        return ticket;
    }

    public async Task<List<IModifier>> GetModifiers(Ticket ticket)
    {
        var modifiers = await FilterModifiers(ticket, _allModifiers);
        return modifiers.Distinct().ToList();
    }


    private async Task<List<IModifier>> FilterModifiers(Ticket ticket, List<IModifier> modifiers)
    {
        var config = await rulesCache.GetTenantConfig(ticket.TenantId);
        if (config == null) return new List<IModifier>();
        var mods = new List<IModifier>();
        foreach (var rule in config.Values)
        {
            var conditions = rule.Expressions.Select(x => x.Invoke(ticket)).ToList();
            switch (rule.Operator)
            {
                case "and" when conditions.All(x => x):
                    mods.AddRange(ApplyFilter(rule.Modifiers, modifiers));
                    break;
                case "or" when conditions.Any(x => x):
                    mods.AddRange(ApplyFilter(rule.Modifiers, modifiers));
                    break;
            }
        }

        return mods;
    }

    private List<IModifier> ApplyFilter(List<Modifier> ruleModifiers, List<IModifier> allModifiers)
    {
        var activeModifiers = ruleModifiers.Where(x => x.Enabled).Select(x => x.Name.ToLowerInvariant()).ToList();
        var result = allModifiers.Where(x => activeModifiers.Contains(x.GetModifierName().ToLowerInvariant())).ToList();

        return result;
    }
}
using System.Reflection;
using RuleConfiguration.Engine.Interfaces;
using RuleConfiguration.Engine.Resources;
using RuleConfiguration.Models;

namespace RuleConfiguration.Handlers;

public class LookupHandler
{
    private readonly IOperationHelper _operationHelper;

    public LookupHandler(IOperationHelper operationHelper)
    {
        _operationHelper = operationHelper;
    }

    public Task<Dictionary<string, HashSet<IOperation>>> GetOperations(string className)
    {
        try
        {
            var type = FindType(className.ToLowerInvariant());
            var props = new PropertyCollection(type);
            var k = props.ToList();

            var dict = new Dictionary<string, HashSet<IOperation>>();
            foreach (var t in k)
            {
                var ops = _operationHelper.SupportedOperations(t.MemberType);

                dict.Add(t.Id, ops);
            }

            return Task.FromResult(dict);
        }
        catch ( Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private Type FindType(string input)
    {
        var types = Assembly.GetExecutingAssembly().GetTypes();

        var type = types
            .FirstOrDefault(x => x.Name.Equals(input, StringComparison.InvariantCultureIgnoreCase));
        if (type == null)
        {
            throw new NullReferenceException();
        }

        return type;
    }
}
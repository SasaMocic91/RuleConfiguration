using System.Reflection;
using RuleConfiguration.DTOs;
using RuleConfiguration.Engine.Interfaces;
using RuleConfiguration.Engine.Resources;

namespace RuleConfiguration.Handlers;

public interface ILookupHandler
{
    public Task<Dictionary<string, List<OperationsDto>>> GetOperations(string className);
}

public class LookupHandler : ILookupHandler
{
    private readonly IOperationHelper _operationHelper;

    public LookupHandler(IOperationHelper operationHelper)
    {
        _operationHelper = operationHelper;
    }

    public Task<Dictionary<string, List<OperationsDto>>> GetOperations(string className)
    {
        try
        {
            var type = FindType(className.ToLowerInvariant());
            var props = new PropertyCollection(type);

            var dict = new Dictionary<string, List<OperationsDto>>();
            foreach (var t in props.ToList())
            {
                var ops = _operationHelper.SupportedOperations(t.MemberType);
                var operationsPerProperty = ops.Select(x => new OperationsDto
                {
                    Name = x.Name,
                    NumberOfValues = x.NumberOfValues
                }).ToList();
                dict.Add(t.Id, operationsPerProperty);
            }
            
            return Task.FromResult(dict);
        }
        catch (Exception e)
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
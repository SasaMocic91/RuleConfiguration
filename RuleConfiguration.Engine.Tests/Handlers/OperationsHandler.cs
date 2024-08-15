using NUnit.Framework;
using RuleConfiguration.Engine.Helpers;
using RuleConfiguration.Handlers;

namespace RuleConfiguration.Engine.Tests.Handlers;

[TestFixture]
public class LookupHandlerTests
{
    [Test]
    public async Task GetAllOperations_Return_Operations_For_Ticket()
    {
        var opHelper =new OperationHelper();
        
        var handler = new LookupHandler(opHelper);

        await handler.GetOperations("Ticket");
    }
    
    [Test]
    public async Task GetAllOperations_Return_Operations_For_Match()
    {
        var opHelper =new OperationHelper();
        
        var handler = new LookupHandler(opHelper);

        await handler.GetOperations("Match");
    }
    
    [Test]
    public async Task GetAllOperations_Return_Operations_For_Event()
    {
        var opHelper =new OperationHelper();
        
        var handler = new LookupHandler(opHelper);

        await handler.GetOperations("Event");
    }
}
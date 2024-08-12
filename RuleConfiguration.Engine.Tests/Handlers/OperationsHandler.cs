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
}
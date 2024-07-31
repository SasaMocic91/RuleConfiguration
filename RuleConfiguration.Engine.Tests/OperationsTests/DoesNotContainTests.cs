using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using RuleConfiguration.Engine.Operations;
using RuleConfiguration.Models;

namespace RuleConfiguration.Engine.Tests.OperationsTests;

[TestFixture]
public class DoesNotContainTests
{

    [TestCase(TestName = "'DoesNotContain' operation - Get expression")]
    public void GetExpressionTest()
    {
        var eventList = new List<Event>();
        var ev = new Event()
        {
            Match = new Match
            {
                Id = 1,
                Home = "HT",
                Away = "AT",
                Tournament = new Tournament()
            },
            Outcome = "x"
        };

        eventList.Add(ev);
            
        var propertyName = "Outcome";
        var value = "Doe ";
        var operation = new DoesNotContain();
        var param = Expression.Parameter(typeof(Event), "x");
        var member = Expression.Property(param, propertyName);
        var constant1 = Expression.Constant(value);

        var expression = (BinaryExpression)operation.GetExpression(member, constant1, null);

        var not = (expression.Right as UnaryExpression);
        not.NodeType.Should().Be(ExpressionType.Not);

        var doesNotContain = (not.Operand as MethodCallExpression);
        doesNotContain.Method.Should().BeAssignableTo<MethodInfo>();
        var method = doesNotContain.Method as MethodInfo;
        method.Name.Should().Be("Contains");
            
        //Testing the operation execution
        var lambda = Expression.Lambda<Func<Event, bool>>(expression, param).Compile();
        var events = eventList.Where(lambda).ToList();
        var solution = eventList.Where(x => !x.Outcome.Trim().ToLower().Contains("doe"));
        events.Should().BeEquivalentTo(solution);
    }
}
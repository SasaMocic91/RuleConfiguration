using System.Linq.Expressions;
using FluentAssertions;
using NUnit.Framework;
using RuleConfiguration.Engine.Operations;
using RuleConfiguration.Engine.Tests.FakeData;
using RuleConfiguration.Models;

namespace RuleConfiguration.Engine.Tests.OperationsTests;

[TestFixture]
public class BetweenTests
{
       
    [TestCase(TestName = "'Between' operation - Get expression")]
    public void GetExpressionTest()
    {
        var propertyName = "Payin";
        var operation = new Between();
        var param = Expression.Parameter(typeof(Ticket), "x");
        var member = Expression.Property(param, propertyName);
        var constant1 = Expression.Constant(1.0);
        var constant2 = Expression.Constant(20.0);

        BinaryExpression expression = (BinaryExpression)operation.GetExpression(member, constant1, constant2);

          
        Assert.That(expression.NodeType, Is.EqualTo(ExpressionType.AndAlso));

        //Testing the operation execution
        var lambda = Expression.Lambda<Func<Ticket, bool>>(expression, param).Compile();
        var tickets = GenerateFakeBettingData.FakeTickets(4).ToList().Where(lambda).ToList();
        Assert.That(tickets.Count, Is.EqualTo(4));
    }

    [TestCase(TestName = "Checking if two operations are equal (failure: comparing with different type)")]
    public void EqualsDifferentType_Failure()
    {
        var operation = new Between();
        var notOperation = "notOperation";

        operation.Equals(notOperation).Should().BeFalse();
    }
}
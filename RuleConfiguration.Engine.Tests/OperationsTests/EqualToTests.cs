using System.Linq.Expressions;
using NUnit.Framework;
using RuleConfiguration.Engine.Operations;
using RuleConfiguration.Models;

namespace RuleConfiguration.Engine.Tests.OperationsTests;

[TestFixture]
public class EqualToTests
{
    [TestCase("Home", "HT", TestName = "'EqualTo' operation - Get expression (string value)")]
    public void GetExpressionStringValueTest(string propertyName, object value)
    {
        var matches = new List<Match>()
        {
            new Match
            {
                Id = 1,
                Home = "HT",
                Away = "AT",
                Tournament = new Tournament()
            }
        };
            
        var operation = new EqualTo();
        var param = Expression.Parameter(typeof(Match), "x");
        var member = Expression.Property(param, propertyName);
        var constant1 = Expression.Constant(value);

        BinaryExpression expression = (BinaryExpression)operation.GetExpression(member, constant1, null);
           
        //Testing the operation execution
        var lambda = Expression.Lambda<Func<Match, bool>>(expression, param).Compile();
        var match = matches.Where(lambda).ToList();
        var solutionMethod = (Func<Match, bool>)GetType().GetMethod(propertyName).Invoke(this, new[] { value });
        var solution = matches.Where(solutionMethod);
        Assert.That(match, Is.EquivalentTo(solution));
        
    }
    public Func<Match, bool> Home(string value)
    {
        return x => x.Home.Trim().ToLower() == value.Trim().ToLower();
    }
}
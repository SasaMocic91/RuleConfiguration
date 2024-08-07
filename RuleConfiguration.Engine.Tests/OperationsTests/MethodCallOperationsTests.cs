using System.Linq.Expressions;
using FluentAssertions;
using NUnit.Framework;
using RuleConfiguration.Engine.Interfaces;
using RuleConfiguration.Engine.Tests.FakeData;
using RuleConfiguration.Models;

namespace RuleConfiguration.Engine.Tests.OperationsTests
{
    [TestFixture]
    public class MethodCallOperationsTests
    {
        private List<Match> Data { get; set; }

        public MethodCallOperationsTests()
        {
            Data = TestData.Matches;
        }

        [TestCase("Contains", " HT ", TestName = "'Contains' operation - Get expression")]
        [TestCase("EndsWith", "  T ", TestName = "'EndsWith' operation - Get expression")]
        [TestCase("StartsWith", " H ", TestName = "'StartsWith' operation - Get expression")]
        public void GetExpressionTest(string methodName, string value)
        {
            var propertyName = "Home";
            var type = typeof(IFilter).Assembly.Types()
                .Single(t => t.FullName == "RuleConfiguration.Engine.Operations." + methodName);
            var operation = (IOperation)Activator.CreateInstance(type);
            var param = Expression.Parameter(typeof(Match), "x");
            var member = Expression.Property(param, propertyName);
            var constant1 = Expression.Constant(value);

            var expression = (BinaryExpression)operation.GetExpression(member, constant1, null);
            
            //Testing the operation execution
            var lambda = Expression.Lambda<Func<Match, bool>>(expression, param).Compile();
            var matches = Data.Where(lambda);
            var solutionMethod = (Func<Match, bool>)GetType().GetMethod(methodName).Invoke(this, new object[] { value });
            var solution = Data.Where(solutionMethod);
            matches.Should().BeEquivalentTo(solution);
        }

        public Func<Match, bool> Contains(string value)
        {
            return x => x.Home.Trim().ToLower().Contains(value.Trim().ToLower());
        }

        public Func<Match, bool> EndsWith(string value)
        {
            return x => x.Home.Trim().ToLower().EndsWith(value.Trim().ToLower());
        }

        public Func<Match, bool> StartsWith(string value)
        {
            return x => x.Home.Trim().ToLower().StartsWith(value.Trim().ToLower());
        }
    }
}
using System.Linq.Expressions;
using NUnit.Framework;
using RuleConfiguration.Engine.Operations;
using RuleConfiguration.Engine.Tests.FakeData;
using RuleConfiguration.Models;

namespace RuleConfiguration.Engine.Tests.OperationsTests
{
    [TestFixture]
    public class NotEqualToTests
    {
        private List<Match> Data { get; set; }

        public NotEqualToTests()
        {
            Data = TestData.Matches;
        }

        [TestCase("Home", "Domaci", TestName = "'NotEqualTo' operation - Get expression (string value)")]
        [TestCase("Away", "Gosti", TestName = "'NotEqualTo' operation - Get expression (Failure: string property with integer value)")]
        public void GetExpression(string propertyName, object value)
        {
            var operation = new NotEqualTo();
            var param = Expression.Parameter(typeof(Match), "x");
            var member = Expression.Property(param, propertyName);
            var constant1 = Expression.Constant(value);

            BinaryExpression expression = (BinaryExpression)operation.GetExpression(member, constant1, null);

            //Testing the operation execution
            var lambda = Expression.Lambda<Func<Match, bool>>(expression, param).Compile();
            var matches = Data.Where(lambda);
            var solutionMethod = (Func<Match, bool>)GetType().GetMethod(propertyName).Invoke(this, new[] { value });
            var solution = Data.Where(solutionMethod);
            Assert.That(matches, Is.EquivalentTo(solution));
        }

        public Func<Match, bool> Home(string value)
        {
            return x => x.Home.Trim().ToLower() != value.Trim().ToLower();
        }

        public Func<Match, bool> Away(string value)
        {
            return x => x.Away.Trim().ToLower() != value.Trim().ToLower();
        }
    }
}
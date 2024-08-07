using System.Linq.Expressions;
using NUnit.Framework;
using RuleConfiguration.Engine.Operations;
using RuleConfiguration.Engine.Tests.FakeData;
using RuleConfiguration.Models;

namespace RuleConfiguration.Engine.Tests.OperationsTests
{
    [TestFixture]
    public class IsNullOrWhiteSpaceOrNotOperationsTests
    {
        private List<Match> Data { get; set; }

        public IsNullOrWhiteSpaceOrNotOperationsTests()
        {
            Data = TestData.Matches;
        }

        [TestCase(TestName = "'IsNullOrWhiteSpace' operation - Get expression")]
        public void GetExpressionTestNullWhiteSpace()
        {
            var propertyName = "Name";
            string value = string.Empty;
            var operation = new IsNullOrWhiteSpace();
            var param = Expression.Parameter(typeof(Match), "x");
            var parent = Expression.Property(param, "Tournament");
            var member = Expression.Property(parent, "Name");
            var constant1 = Expression.Constant(4000D);
            var constant2 = Expression.Constant(5000D);

            BinaryExpression expression = (BinaryExpression)operation.GetExpression(member, constant1, constant2);
            
            //Testing the operation execution
            var lambda = Expression.Lambda<Func<Match, bool>>(expression, param).Compile();
            var matches = Data.Where(lambda);
            var solution = Data.Where(x => x.Tournament == null || (x.Tournament.Name == null || (x.Tournament.Name != null && x.Tournament.Name.Trim().ToLower() == string.Empty)));
            Assert.That(matches, Is.EquivalentTo(solution));
        }

        [TestCase(TestName = "'IsNotNullNorWhiteSpace' operation - Get expression")]
        public void GetExpressionTestNotNullNorWhiteSpace()
        {
            var propertyName = "Name";
            string value = string.Empty;
            var operation = new IsNotNullNorWhiteSpace();
            var param = Expression.Parameter(typeof(Match), "x");
            var parent = Expression.Property(param, "Tournament");
            var member = Expression.Property(parent, "Name");
            var constant1 = Expression.Constant(4000D);
            var constant2 = Expression.Constant(5000D);

            BinaryExpression expression = (BinaryExpression)operation.GetExpression(member, constant1, constant2);
            
            //Testing the operation execution
            var lambda = Expression.Lambda<Func<Match, bool>>(expression, param).Compile();
            var matches = Data.Where(lambda);
            var solution = Data.Where(x => x.Tournament != null && x.Tournament.Name != null && x.Tournament.Name.Trim().ToLower() != string.Empty);
            Assert.That(matches, Is.EquivalentTo(solution));
        }
    }
}
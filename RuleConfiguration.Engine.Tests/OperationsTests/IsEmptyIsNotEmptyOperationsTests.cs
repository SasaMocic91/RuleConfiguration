using System.Linq.Expressions;
using FluentAssertions;
using NUnit.Framework;
using RuleConfiguration.Engine.Interfaces;
using RuleConfiguration.Engine.Tests.FakeData;
using RuleConfiguration.Models;

namespace RuleConfiguration.Engine.Tests.OperationsTests
{
    [TestFixture]
    public class IsEmptyIsNotEmptyOperationsTests
    {
        private List<Match> Data { get; set; }

        public IsEmptyIsNotEmptyOperationsTests()
        {
            Data = TestData.Matches;
        }

        [TestCase(ExpressionType.Equal, "IsEmpty", TestName = "'IsEmpty' operation - Get expression")]
        [TestCase(ExpressionType.NotEqual, "IsNotEmpty", TestName = "'IsNotEmpty' operation - Get expression")]
        public void GetExpressionTest(ExpressionType comparisonType, string methodName)
        {
            var propertyName = "Name";
            var value = string.Empty;
            var type = typeof(IFilter).Assembly.Types()
                .Single(t => t.FullName == "RuleConfiguration.Engine.Operations." + methodName);
            var operation = (IOperation)Activator.CreateInstance(type);
            var param = Expression.Parameter(typeof(Match), "x");
            var parent = Expression.Property(param, "Tournament");
            var member = Expression.Property(parent, "Name");
            var constant1 = Expression.Constant(4000D);
            var constant2 = Expression.Constant(5000D);

            BinaryExpression expression = (BinaryExpression)operation.GetExpression(member, constant1, constant2);
            
            //Testing the operation execution
            var lambda = Expression.Lambda<Func<Match, bool>>(expression, param);
            var matches = Data.Where(lambda.Compile());
            var solutionMethod = (Func<Match, bool>)GetType().GetMethod(methodName).Invoke(this, new object[] { });
            var solution = Data.Where(solutionMethod);
            Assert.That(matches, Is.EquivalentTo(solution));
        }

        public Func<Match, bool> IsEmpty()
        {
            return x => x.Tournament != null && (x.Tournament.Name != null && x.Tournament.Name.Trim().ToLower() == string.Empty);
        }

        public Func<Match, bool> IsNotEmpty()
        {
            return x => x.Tournament != null && (x.Tournament.Name != null && x.Tournament.Name.Trim().ToLower() != string.Empty);
        }
    }
}
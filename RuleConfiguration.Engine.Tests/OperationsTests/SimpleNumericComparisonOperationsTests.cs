using System.Linq.Expressions;
using FluentAssertions;
using NUnit.Framework;
using RuleConfiguration.Engine.Interfaces;
using RuleConfiguration.Engine.Tests.FakeData;
using RuleConfiguration.Models;

namespace RuleConfiguration.Engine.Tests.OperationsTests
{
    [TestFixture]
    public class SimpleNumericComparisonOperationsTests
    {
        private List<Ticket> TestData { get; set; }

        public SimpleNumericComparisonOperationsTests()
        {
            TestData = GenerateFakeBettingData.FakeTickets(4).ToList();
        }

        [TestCase(ExpressionType.GreaterThan, "GreaterThan", 10.0, TestName = "'GreaterThan' operation - Get expression")]
        [TestCase(ExpressionType.GreaterThanOrEqual, "GreaterThanOrEqualTo", 10.0, TestName = "'GreaterThanOrEqualTo' operation - Get expression")]
        [TestCase(ExpressionType.LessThan, "LessThan", 4000.0, TestName = "'LessThan' operation - Get expression")]
        [TestCase(ExpressionType.LessThanOrEqual, "LessThanOrEqualTo", 4000.0, TestName = "'LessThanOrEqualTo' operation - Get expression")]
        public void GetExpressionTest(ExpressionType method, string methodName, double value)
        {
            var propertyName = "WinAmount";
            var type = typeof(IFilter).Assembly.Types()
                .Single(t => t.FullName == "RuleConfiguration.Engine.Operations." + methodName);
            var operation = (IOperation)Activator.CreateInstance(type);
            var param = Expression.Parameter(typeof(Ticket), "x");
            var member = Expression.Property(param, propertyName);
            var constant1 = Expression.Constant(value);

            BinaryExpression expression = (BinaryExpression)operation.GetExpression(member, constant1, null);
            
            //Testing the operation execution
            var lambda = Expression.Lambda<Func<Ticket, bool>>(expression, param).Compile();
            var tickets = TestData.Where(lambda);
            var solutionMethod = (Func<Ticket, bool>)GetType().GetMethod(method.ToString()).Invoke(this, new object[] { value });
            var solution = TestData.Where(solutionMethod);
            Assert.That(tickets, Is.EquivalentTo(solution));
        }

        public Func<Ticket, bool> GreaterThan(double value)
        {
            return x => x.WinAmount > value;
        }

        public Func<Ticket, bool> GreaterThanOrEqual(double value)
        {
            return x => x.WinAmount >= value;
        }

        public Func<Ticket, bool> LessThan(double value)
        {
            return x => x.WinAmount < value;
        }

        public Func<Ticket, bool> LessThanOrEqual(double value)
        {
            return x => x.WinAmount <= value;
        }
    }
}
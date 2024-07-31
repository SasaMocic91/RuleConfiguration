﻿using NUnit.Framework;
using RuleConfiguration.Engine.Exceptions;
using RuleConfiguration.Engine.Helpers;
using RuleConfiguration.Engine.Interfaces;
using RuleConfiguration.Engine.Operations;

namespace RuleConfiguration.Engine.Tests
{
    [TestFixture]
    public class HelperTests
    {
        [TestCase(typeof(int), TestName = "Get supported operations for number int")]
        [TestCase(typeof(uint), TestName = "Get supported operations for number uint")]
        [TestCase(typeof(byte), TestName = "Get supported operations for number byte")]
        [TestCase(typeof(sbyte), TestName = "Get supported operations for number sbyte")]
        [TestCase(typeof(short), TestName = "Get supported operations for number short")]
        [TestCase(typeof(ushort), TestName = "Get supported operations for number ushort")]
        [TestCase(typeof(long), TestName = "Get supported operations for number long")]
        [TestCase(typeof(ulong), TestName = "Get supported operations for number ulong")]
        [TestCase(typeof(Single), TestName = "Get supported operations for number Single")]
        [TestCase(typeof(double), TestName = "Get supported operations for number double")]
        [TestCase(typeof(decimal), TestName = "Get supported operations for number decimal")]
        [TestCase(typeof(int[]), TestName = "Get supported operations for an array of int")]
        public void SupportedOperationsForNumbers(Type numberType)
        {
            var operationHelper = new OperationHelper();
            var numberOperations = new List<string> { "EqualTo", "NotEqualTo", "GreaterThan", "GreaterThanOrEqualTo",
                                                         "LessThan", "LessThanOrEqualTo", "Between" };

            if (numberType.IsArray)
            {
                numberOperations.Add("In");
                numberOperations.Add("NotIn");
            }

            var operations = operationHelper.SupportedOperations(numberType);
            Assert.That(operations.Select(o => o.Name), Is.EquivalentTo(numberOperations));
        }

        [TestCase(typeof(string), TestName = "Get supported operations for string")]
        [TestCase(typeof(char), TestName = "Get supported operations for char")]
        [TestCase(typeof(string[]), TestName = "Get supported operations for an array string")]
        public void SupportedOperationsForText(Type textType)
        {
            var definitions = new OperationHelper();
            var textOperations = new List<string> { "EqualTo", "Contains", "DoesNotContain", "EndsWith", "NotEqualTo", "StartsWith",
                                                       "IsEmpty", "IsNotEmpty", "IsNotNull", "IsNotNullNorWhiteSpace", "IsNull",
                                                       "IsNullOrWhiteSpace" };

            if (textType.IsArray)
            {
                textOperations.Add("In");
                textOperations.Add("NotIn");
            }

            var operations = definitions.SupportedOperations(textType);
            Assert.That(operations.Select(o => o.Name), Is.EquivalentTo(textOperations));
        }

        [TestCase(TestName = "Get supported operations for dates")]
        public void SupportedOperationsForDates()
        {
            OperationHelper.LoadDefaultOperations();
            var definitions = new OperationHelper();
            var dateOperations = new List<string> { "Between", "EqualTo", "NotEqualTo", "GreaterThan", "GreaterThanOrEqualTo",
                                                       "LessThan", "LessThanOrEqualTo" };
            var operations = definitions.SupportedOperations(typeof(DateTime));
            Assert.That(operations.Select(o => o.Name), Is.EquivalentTo(dateOperations));
        }

        [TestCase(TestName = "Get supported operations for bool")]
        public void SupportedOperationsForBool()
        {
            var definitions = new OperationHelper();
            var booleanOperations = new List<string> { "EqualTo", "NotEqualTo" };
            var operations = definitions.SupportedOperations(typeof(bool));
            Assert.That(operations.Select(o => o.Name), Is.EquivalentTo(booleanOperations));
        }

        [TestCase(TestName = "Get supported operations for nullable types")]
        public void SupportedOperationsForNullable()
        {
            var definitions = new OperationHelper();
            var nullableOperations = new List<string> { "IsNotNull", "IsNull" };
            var numberOperations = new List<string> { "EqualTo", "NotEqualTo", "GreaterThan", "GreaterThanOrEqualTo",
                                                         "LessThan", "LessThanOrEqualTo", "Between" };
            nullableOperations.AddRange(numberOperations);
            var operations = definitions.SupportedOperations(typeof(int?));
            Assert.That(operations.Select(o => o.Name), Is.EquivalentTo(nullableOperations));
        }

        [TestCase(TestName = "Getting an operation by it's name")]
        public void GetOperationByName()
        {
            var operation = new OperationHelper().GetOperationByName("EqualTo");
            Assert.That(operation.Name, Is.EqualTo("EqualTo"));
            Assert.That(operation.Active, Is.True);
            Assert.That(operation.GetType().Namespace, Is.EqualTo("RuleConfiguration.Engine.Operations"));
        }

        [TestCase(TestName = "Getting an overwritten operation by it's name")]
        public void GetOverwrittenOperationByName()
        {
            var helper = new OperationHelper();
            var operations = new List<IOperation> { new EqualTo() };
            helper.LoadOperations(operations, true);

            var operation = helper.GetOperationByName("EqualTo");

            Assert.That(operation.Name, Is.EqualTo("EqualTo"));
            Assert.That(operation.Active, Is.True);
            Assert.That(operation.GetType().Namespace, Is.EqualTo("RuleConfiguration.Engine.Operations"));

            OperationHelper.LoadDefaultOperations();
        }

        [TestCase(TestName = "Getting an operation by it's name - Failure (Operation not found)")]
        public void GetOperationByNameFailtureOperationNotFound()
        {
            var ex = Assert.Throws<OperationNotFoundException>(() => new OperationHelper().GetOperationByName("FakeOperation"));
            Assert.That(ex.Message, Does.Match(@"Sorry, the operation '\w*' was not found."));
        }
    }
}
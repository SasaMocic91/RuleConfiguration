using FluentAssertions;
using NUnit.Framework;
using RuleConfiguration.Engine.Common;
using RuleConfiguration.Engine.Generics;
using RuleConfiguration.Engine.Interfaces;
using RuleConfiguration.Engine.Operations;
using RuleConfigurator.Application.Models;

namespace RuleConfiguration.Engine.Tests;

[TestFixture]
public class FilterTests
{
    [Test(Description = "Should be able to add statements to a filter")]
    public void FilterShouldAddStatement()
    {
        var filter = new Filter<Ticket>();
        filter.By("Name", Operation.Contains, "John");
        Assert.That(filter.Statements.Last().Count(), Is.EqualTo(1));
        Assert.That(filter.Statements.Last().First().PropertyId, Is.EqualTo("Name"));
        Assert.That(filter.Statements.Last().First().Operation, Is.EqualTo(Operation.Contains));
        Assert.That(filter.Statements.Last().First().Value, Is.EqualTo("John"));
        Assert.That(filter.Statements.Last().First().Connector, Is.EqualTo(Connector.And));
    }

    [Test(Description = "Should be able to remove all statements of a filter")]
    public void FilterShouldRemoveStatement()
    {
        var filter = new Filter<Ticket>();
        Assert.That(filter.Statements.Count(), Is.EqualTo(1));
        Assert.That(filter.Statements.Last().Count(), Is.EqualTo(0));

        filter.By("Payin", Operation.EqualTo, 1M).Or.By("WinAmount", Operation.EqualTo, 10M);
        Assert.That(filter.Statements.Last().Count(), Is.EqualTo(2));

        filter.Clear();
        Assert.That(filter.Statements.Last().Count(), Is.EqualTo(0));
    }


    [Test(Description = "Should be able to 'read' a double-valued filter as a string")]
    public void DoubleValuedFilterToString()
    {
        var filter = new Filter<Ticket>();
        filter.By("Id", Operation.Between, 1, 3).Or.By("Events.Count", Operation.EqualTo, 3);
        Assert.That(filter.ToString(), Is.EqualTo("Id Between 1 And 3 Or Events.Count EqualTo 3"));
    }

    [Test(Description = "Should be able to 'read' a single-valued filter as a string")]
    public void SingleValuedFilterToString()
    {
        var filter = new Filter<Ticket>();
        filter.By("Events[Match].Tournament.Name", Operation.StartsWith, "Test").Or
            .By("Events.Count", Operation.EqualTo, 5);
        Assert.That(filter.ToString(),
            Is.EqualTo("Events[Match].Tournament.Name StartsWith Test Or Events.Count EqualTo 5"));
    }

    [Test(Description = "Should be able to 'read' a no-valued filter as a string")]
    public void NoValuedFilterToString()
    {
        var filter = new Filter<Ticket>();
        filter.By("Name", Operation.IsNotNull).Or.By("Birth.Country", Operation.EqualTo, "USA");
        Assert.That(filter.ToString(), Is.EqualTo("Name IsNotNull Or Birth.Country EqualTo USA"));
    }

    [Test(Description = "Should not start group if previous one is empty")]
    public void ShouldNotStartGroupIfPreviousOneIsEmpty()
    {
        var filter = new Filter<Ticket>();
        filter.StartGroup();
        filter.StartGroup();
        filter.StartGroup();
        Assert.That(filter.Statements.Count(), Is.EqualTo(1));
    }

    [Test(Description = "Should create a filter by passing the type as an argument")]
    public void ShouldCreateAFilterByPassingTheTypeAsAnArgument()
    {
        var filter = FilterFactory.Create(typeof(Ticket));
        filter.Should().BeOfType(typeof(Filter<Ticket>));
    }
}
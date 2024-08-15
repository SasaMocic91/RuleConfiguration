using FluentAssertions;
using NUnit.Framework;
using RuleConfiguration.Engine.Generics;
using RuleConfiguration.Engine.Operations;
using RuleConfiguration.Models;

namespace RuleConfiguration.Engine.Tests;

[TestFixture]
public class FilterTests
{
    [Test(Description = "Should be able to add statements to a filter")]
    public void FilterShouldAddStatement()
    {
        var filter = new Filter<Ticket>();
        filter.By("Id", Operation.EqualTo, 1);
        Assert.That(filter.Statement.ToString(), Is.EqualTo("Id EqualTo 1"));
    }
    

    [Test(Description = "Should be able to 'read' a double-valued filter as a string")]
    public void DoubleValuedFilterToString()
    {
        var filter = new Filter<Ticket>();
        filter.By("Id", Operation.Between, 1, 3);
        Assert.That(filter.Statement.ToString(), Is.EqualTo("Id Between 1 And 3"));
    }

    [Test(Description = "Should be able to 'read' a single-valued filter as a string")]
    public void SingleValuedFilterToString()
    {
        var filter = new Filter<Ticket>();
        filter.By("Events[Match].Tournament.Name", Operation.StartsWith, "Test");
        Assert.That(filter.Statement.ToString(),
            Is.EqualTo("Events[Match].Tournament.Name StartsWith Test"));
    }

    [Test(Description = "Should be able to 'read' a no-valued filter as a string")]
    public void NoValuedFilterToString()
    {
        var filter = new Filter<Ticket>();
        filter.By("Name", Operation.IsNotNull);
        Assert.That(filter.Statement.ToString(), Is.EqualTo("Name IsNotNull"));
    }

    [Test(Description = "Should create a filter by passing the type as an argument")]
    public void ShouldCreateAFilterByPassingTheTypeAsAnArgument()
    {
        var filter = FilterFactory.Create(typeof(Ticket));
        filter.Should().BeOfType(typeof(Filter<Ticket>));
    }
}
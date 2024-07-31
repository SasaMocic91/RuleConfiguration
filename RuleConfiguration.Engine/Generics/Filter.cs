using System.Linq.Expressions;
using System.Text;
using RuleConfiguration.Engine.Common;
using RuleConfiguration.Engine.Interfaces;

namespace RuleConfiguration.Engine.Generics;

/// <summary>
///     Aggregates <see cref="FilterStatement{TPropertyType}" /> and build them into a LINQ expression.
/// </summary>
/// <typeparam name="TClass"></typeparam>
[Serializable]
public class Filter<TClass> : IFilter where TClass : class
{
    private readonly List<List<IFilterStatement>> _statements;

    /// <summary>
    ///     Instantiates a new <see cref="Filter{TClass}" />
    /// </summary>
    public Filter()
    {
        _statements = new List<List<IFilterStatement>> { new() };
    }

    private List<IFilterStatement> CurrentStatementGroup => _statements.Last();

    public IFilter Group
    {
        get
        {
            StartGroup();
            return this;
        }
    }

    /// <summary>
    ///     List of <see cref="IFilterStatement" /> groups that will be combined and built into a LINQ expression.
    /// </summary>
    public IEnumerable<IEnumerable<IFilterStatement>> Statements => _statements.ToArray();

    /// <summary>
    ///     Adds a new <see cref="FilterStatement{TPropertyType}" /> to the <see cref="Filter{TClass}" />.
    ///     (To be used by <see cref="IOperation" /> that need no values)
    /// </summary>
    /// <param name="propertyId">Property identifier conventionalized by for the Expression Builder.</param>
    /// <param name="operation">Operation to be used.</param>
    /// <param name="connector"></param>
    /// <returns></returns>
    public IFilterStatementConnection By(string propertyId, IOperation operation, Connector connector)
    {
        return By<string>(propertyId, operation, null, null, connector);
    }

    /// <summary>
    ///     Adds a new <see cref="FilterStatement{TPropertyType}" /> to the <see cref="Filter{TClass}" />.
    ///     (To be used by <see cref="IOperation" /> that need no values)
    /// </summary>
    /// <param name="propertyId">Property identifier conventionalized by for the Expression Builder.</param>
    /// <param name="operation">Operation to be used.</param>
    /// <returns></returns>
    public IFilterStatementConnection By(string propertyId, IOperation operation)
    {
        return By<string>(propertyId, operation, null, null, Connector.And);
    }

    /// <summary>
    ///     Adds a new <see cref="FilterStatement{TPropertyType}" /> to the <see cref="Filter{TClass}" />.
    /// </summary>
    /// <typeparam name="TPropertyType"></typeparam>
    /// <param name="propertyId">Property identifier conventionalized by for the Expression Builder.</param>
    /// <param name="operation">Operation to be used.</param>
    /// <param name="value"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    public IFilterStatementConnection By<TPropertyType>(string propertyId, IOperation operation,
        TPropertyType value)
    {
        return By(propertyId, operation, value, default(TPropertyType));
    }

    /// <summary>
    ///     Adds a new <see cref="FilterStatement{TPropertyType}" /> to the <see cref="Filter{TClass}" />.
    /// </summary>
    /// <typeparam name="TPropertyType"></typeparam>
    /// <param name="propertyId">Property identifier conventionalized by for the Expression Builder.</param>
    /// <param name="operation">Operation to be used.</param>
    /// <param name="value"></param>
    /// <param name="value2"></param>
    /// <param name="connector"></param>
    /// <returns></returns>
    public IFilterStatementConnection By<TPropertyType>(string propertyId, IOperation operation,
        TPropertyType value, Connector connector)
    {
        return By(propertyId, operation, value, default, connector);
    }

    /// <summary>
    ///     Adds a new <see cref="FilterStatement{TPropertyType}" /> to the <see cref="Filter{TClass}" />.
    /// </summary>
    /// <typeparam name="TPropertyType"></typeparam>
    /// <param name="propertyId">Property identifier conventionalized by for the Expression Builder.</param>
    /// <param name="operation">Operation to be used.</param>
    /// <param name="value"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    public IFilterStatementConnection By<TPropertyType>(string propertyId, IOperation operation,
        TPropertyType value, TPropertyType value2)
    {
        return By(propertyId, operation, value, value2, Connector.And);
    }

    /// <summary>
    ///     Adds a new <see cref="FilterStatement{TPropertyType}" /> to the <see cref="Filter{TClass}" />.
    /// </summary>
    /// <typeparam name="TPropertyType"></typeparam>
    /// <param name="propertyId">Property identifier conventionalized by for the Expression Builder.</param>
    /// <param name="operation">Operation to be used.</param>
    /// <param name="value"></param>
    /// <param name="value2"></param>
    /// <param name="connector"></param>
    /// <returns></returns>
    public IFilterStatementConnection By<TPropertyType>(string propertyId, IOperation operation,
        TPropertyType value, TPropertyType value2, Connector connector)
    {
        IFilterStatement statement =
            new FilterStatement<TPropertyType>(propertyId, operation, value, value2, connector);
        CurrentStatementGroup.Add(statement);
        return new FilterStatementConnection(this, statement);
    }

    /// <summary>
    ///     Starts a new group denoting that every subsequent filter statement should be grouped together (as if using a
    ///     parenthesis).
    /// </summary>
    public void StartGroup()
    {
        if (CurrentStatementGroup.Any()) _statements.Add(new List<IFilterStatement>());
    }

    /// <summary>
    ///     Removes all <see cref="FilterStatement{TPropertyType}" />, leaving the <see cref="Filter{TClass}" /> empty.
    /// </summary>
    public void Clear()
    {
        _statements.Clear();
        _statements.Add(new List<IFilterStatement>());
    }

    /// <summary>
    ///     Implicitly converts a <see cref="Filter{TClass}" /> into a <see cref="Func{TClass, TResult}" />.
    /// </summary>
    /// <param name="filter"></param>
    public static implicit operator Func<TClass, bool>(Filter<TClass> filter)
    {
        var builder = new FilterBuilder();
        return builder.GetExpression<TClass>(filter).Compile();
    }

    /// <summary>
    ///     Implicitly converts a <see cref="Filter{TClass}" /> into a
    ///     <see cref="System.Linq.Expressions.Expression{Func{TClass, TResult}}" />.
    /// </summary>
    /// <param name="filter"></param>
    public static implicit operator Expression<Func<TClass, bool>>(Filter<TClass> filter)
    {
        var builder = new FilterBuilder();
        return builder.GetExpression<TClass>(filter);
    }

    /// <summary>
    ///     String representation of <see cref="Filter{TClass}" />.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        var result = new StringBuilder();
        var lastConector = Connector.And;

        foreach (var statementGroup in _statements)
        {
            if (_statements.Count() > 1) result.Append("(");

            var groupResult = new StringBuilder();
            foreach (var statement in statementGroup)
            {
                if (groupResult.Length > 0) groupResult.Append(" " + lastConector + " ");

                groupResult.Append(statement);
                lastConector = statement.Connector;
            }

            result.Append(groupResult.ToString().Trim());
            if (_statements.Count() > 1) result.Append(")");
        }

        return result.ToString();
    }
}
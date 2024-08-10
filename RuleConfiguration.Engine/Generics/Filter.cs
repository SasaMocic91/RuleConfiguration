using System.Linq.Expressions;
using RuleConfiguration.Engine.Interfaces;

namespace RuleConfiguration.Engine.Generics;

/// <summary>
///     Aggregates <see cref="FilterStatement{TPropertyType}" /> and build them into a LINQ expression.
/// </summary>
/// <typeparam name="TClass"></typeparam>
[Serializable]
public class Filter<TClass> : IFilter where TClass : class
{
    public Filter(IFilterStatement statement)
    {
        Statement = statement;
    }

    public Filter()
    {
    }

    public IFilterStatement Statement { get; set; }

    /// <summary>
    ///     Adds a new <see cref="FilterStatement{TPropertyType}" /> to the <see cref="Filter{TClass}" />.
    ///     (To be used by <see cref="IOperation" /> that need no values)
    /// </summary>
    /// <param name="propertyId">Property identifier conventionalized by for the Expression Builder.</param>
    /// <param name="operation">Operation to be used.</param>
    /// <param name="connector"></param>
    /// <returns></returns>
    public IFilterStatement By(string propertyId, IOperation operation)
    {
        return By<string>(propertyId, operation, null, null);
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
    public IFilterStatement By<TPropertyType>(string propertyId, IOperation operation,
        TPropertyType value)
    {
        return By(propertyId, operation, value, default(TPropertyType));
    }

    public IFilterStatement By<TPropertyType>(string propertyId, IOperation operation,
        TPropertyType value, TPropertyType value2)
    {
        IFilterStatement statement =
            new FilterStatement<TPropertyType>(propertyId, operation, value, value2);
        Statement = statement;
        return statement;
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
}
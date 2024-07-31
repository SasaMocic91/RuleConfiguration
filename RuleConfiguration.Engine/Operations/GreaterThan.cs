using System.Linq.Expressions;
using RuleConfiguration.Engine.Common;

namespace RuleConfiguration.Engine.Operations;

/// <summary>
///     Operation representing an "greater than" comparison.
/// </summary>
public class GreaterThan : OperationBase
{
    /// <inheritdoc />
    public GreaterThan()
        : base("GreaterThan", 1, TypeGroup.Number | TypeGroup.Date)
    {
    }

    /// <inheritdoc />
    public override Expression GetExpression(MemberExpression member, ConstantExpression constant1,
        ConstantExpression constant2)
    {
        return Expression.GreaterThan(member, constant1);
    }
}
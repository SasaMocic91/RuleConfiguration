using System.Linq.Expressions;
using RuleConfiguration.Engine.Common;

namespace RuleConfiguration.Engine.Operations;

/// <summary>
///     Operation representing a null check.
/// </summary>
public class IsNull : OperationBase
{
    /// <inheritdoc />
    public IsNull()
        : base("IsNull", 0, TypeGroup.Text | TypeGroup.Nullable, expectNullValues: true)
    {
    }

    /// <inheritdoc />
    public override Expression GetExpression(MemberExpression member, ConstantExpression constant1,
        ConstantExpression constant2)
    {
        return Expression.Equal(member, Expression.Constant(null));
    }
}
using System.Linq.Expressions;
using System.Reflection;
using RuleConfiguration.Engine.Common;

namespace RuleConfiguration.Engine.Operations;

/// <summary>
///     Operation representing a string "EndsWith" method call.
/// </summary>
public class EndsWith : OperationBase
{
    private readonly MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });

    /// <inheritdoc />
    public EndsWith()
        : base("EndsWith", 1, TypeGroup.Text)
    {
    }

    /// <inheritdoc />
    public override Expression GetExpression(MemberExpression member, ConstantExpression constant1,
        ConstantExpression constant2)
    {
        var constant = constant1.TrimToLower();

        return Expression.Call(member.TrimToLower(), endsWithMethod, constant)
            .AddNullCheck(member);
    }
}
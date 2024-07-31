using System.Linq.Expressions;
using System.Reflection;
using RuleConfiguration.Engine.Common;

namespace RuleConfiguration.Engine.Operations;

/// <summary>
///     Operation representing a string "StartsWith" method call.
/// </summary>
public class StartsWith : OperationBase
{
    private readonly MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });

    /// <inheritdoc />
    public StartsWith()
        : base("StartsWith", 1, TypeGroup.Text)
    {
    }

    /// <inheritdoc />
    public override Expression GetExpression(MemberExpression member, ConstantExpression constant1,
        ConstantExpression constant2)
    {
        var constant = constant1.TrimToLower();

        return Expression.Call(member.TrimToLower(), startsWithMethod, constant)
            .AddNullCheck(member);
    }
}
﻿using System.Linq.Expressions;
using System.Reflection;
using RuleConfiguration.Engine.Common;

namespace RuleConfiguration.Engine.Operations;

/// <summary>
///     Operation that checks for the non-existence of a substring within another string.
/// </summary>
public class DoesNotContain : OperationBase
{
    private readonly MethodInfo stringContainsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

    /// <inheritdoc />
    public DoesNotContain()
        : base("DoesNotContain", 1, TypeGroup.Text)
    {
    }

    /// <inheritdoc />
    public override Expression GetExpression(MemberExpression member, ConstantExpression constant1,
        ConstantExpression constant2)
    {
        var constant = constant1.TrimToLower();

        return Expression.Not(Expression.Call(member.TrimToLower(), stringContainsMethod, constant))
            .AddNullCheck(member);
    }
}
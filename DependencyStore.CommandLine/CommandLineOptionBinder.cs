using System;
using System.Linq.Expressions;
using System.Reflection;

using DependencyStore.Commands;

namespace DependencyStore.CommandLine
{
  public class CommandLineOptionBinder
  {
    private readonly CommandLineParser _parser;
    private readonly ICommand _command;

    public CommandLineOptionBinder(CommandLineParser parser, ICommand command)
    {
      _parser = parser;
      _command = command;
    }

    public void RequireFirst<TTarget>(Expression<Func<TTarget, object>> property)
    {
      Nth(0, true, property);
    }

    public void OptionalFirst<TTarget>(Expression<Func<TTarget, object>> property)
    {
      Nth(0, false, property);
    }

    public void Nth<TTarget>(short i, bool required, Expression<Func<TTarget, object>> property)
    {
      if (!DoesBindingApply<TTarget>()) return;
      if (i >= _parser.OrphanedArguments.Count)
      {
        if (required)
        {
          throw new InvalidOperationException("Missing argument!");
        }
        return;
      }
      PropertyInfo info = (PropertyInfo)GetMemberInfo(property);
      info.GetSetMethod().Invoke(_command, new object[] { _parser.OrphanedArguments[i].Value });
    }

    // Shamelessly ripped from Kzu....
    private static MemberInfo GetMemberInfo(Expression member)
    {
      if (member == null) throw new ArgumentNullException("member");
      LambdaExpression lambda = member as LambdaExpression;
      if (lambda == null) throw new ArgumentException("Not a lambda expression", "member");
      MemberExpression memberExpr = null;
      if (lambda.Body.NodeType == ExpressionType.Convert)
      {
        memberExpr = ((UnaryExpression)lambda.Body).Operand as MemberExpression;
      }
      else if (lambda.Body.NodeType == ExpressionType.MemberAccess)
      {
        memberExpr = lambda.Body as MemberExpression;
      }
      if (memberExpr == null) throw new ArgumentException("Not a member access", "member");
      return memberExpr.Member;
    }

    private bool DoesBindingApply<TTarget>()
    {
      return typeof(TTarget).IsInstanceOfType(_command);
    }
  }
}
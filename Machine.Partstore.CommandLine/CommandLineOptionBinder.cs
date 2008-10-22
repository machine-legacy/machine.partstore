using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

using Machine.Partstore.Commands;

namespace Machine.Partstore.CommandLine
{
  public interface IAcceptsArgumentBindings
  {
  }

  public abstract class Binder
  {
    public abstract bool IsPresent();

    public abstract string Value();
  }

  public class NamedOptionBinder : Binder
  {
    private readonly CommandLineParser _parser;
    private readonly string _name;

    public NamedOptionBinder(CommandLineParser parser, string name)
    {
      _parser = parser;
      _name = name;
    }

    public override bool IsPresent()
    {
      return _parser.OptionFor(_name) != null;
    }

    public override string Value()
    {
      if (!IsPresent())
      {
        throw new InvalidOperationException();
      }
      return _parser.OptionFor(_name).Argument.Value;
    }

    public override string ToString()
    {
      return "argument named " + _name;
    }
  }

  public class NthArgumentBinder : Binder
  {
    private readonly CommandLineParser _parser;
    private readonly short _index;

    public NthArgumentBinder(CommandLineParser parser, short index)
    {
      _parser = parser;
      _index = index;
    }

    public override bool IsPresent()
    {
      return _index <= _parser.OrphanedArguments.Count - 1;
    }

    public override string Value()
    {
      return _parser.OrphanedArguments[_index].Value;
    }

    public override string ToString()
    {
      return "standard argument at " + _index;
    }
  }

  public class CommandLineOptionBinder
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(CommandLineOptionBinder));
    private readonly CommandLineParser _parser;
    private readonly object _target;

    public CommandLineOptionBinder(CommandLineParser parser, object target)
    {
      _parser = parser;
      _target = target;
    }

    public NamedOptionBinder Named(string name)
    {
      return new NamedOptionBinder(_parser, name);
    }

    public NthArgumentBinder First()
    {
      return Nth(0);
    }

    public NthArgumentBinder Second()
    {
      return Nth(1);
    }

    public NthArgumentBinder Third()
    {
      return Nth(2);
    }

    public NthArgumentBinder Nth(short index)
    {
      return new NthArgumentBinder(_parser, index);
    }

    public void Optional<TTarget>(Expression<Func<TTarget, object>> property, params Binder[] bindings)
    {
      Bind(false, bindings, property);
    }

    public void Required<TTarget>(Expression<Func<TTarget, object>> property, params Binder[] bindings)
    {
      Bind(true, bindings, property);
    }

    private void Bind<TTarget>(bool required, IEnumerable<Binder> bindings, Expression<Func<TTarget, object>> property)
    {
      if (!DoesBindingApply<TTarget>()) return;
      foreach (Binder binder in bindings)
      {
        if (binder.IsPresent())
        {
          _log.InfoFormat("Binding {0} to {1}", binder, property);
          PropertyInfo info = (PropertyInfo)GetMemberInfo(property);
          info.GetSetMethod().Invoke(_target, new object[] { binder.Value() });
          return;
        }
      }
      if (required)
      {
        throw new InvalidOperationException("Missing argument!");
      }
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
      return typeof(TTarget).IsInstanceOfType(_target);
    }
  }
}
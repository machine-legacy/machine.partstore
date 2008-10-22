using System;
using System.Collections.Generic;

namespace Machine.Partstore.CommandLine
{
  public class CommandLineParser
  {
    private readonly List<SimpleArgument> _orphanedArguments = new List<SimpleArgument>();
    private readonly List<NamedFlag> _flags = new List<NamedFlag>();
    private readonly List<NamedOption> _options = new List<NamedOption>();

    public IList<SimpleArgument> OrphanedArguments
    {
      get { return _orphanedArguments; }
    }

    public IList<NamedFlag> Flags
    {
      get { return _flags; }
    }

    public IList<NamedOption> Options
    {
      get { return _options; }
    }

    public NamedOption OptionFor(string name)
    {
      foreach (NamedOption option in _options)
      {
        if (option.Flag.Name == name)
        {
          return option;
        }
      }
      return null;
    }

    public void ParseCommandLine(string[] args)
    {
      NamedFlag lastFlag = null;
      foreach (string arg in args)
      {
        if (arg.StartsWith("--"))
        {
          if (lastFlag != null)
          {
            _flags.Add(lastFlag);
          }
          lastFlag = new NamedFlag(arg.Substring(2));
        }
        else if (arg.StartsWith("-"))
        {
          if (lastFlag != null)
          {
            _flags.Add(lastFlag);
          }
          lastFlag = new NamedFlag(arg.Substring(1));
        }
        else
        {
          SimpleArgument argument = new SimpleArgument(arg);
          if (lastFlag != null)
          {
            _options.Add(new NamedOption(lastFlag, argument));
            lastFlag = null;
          }
          else
          {
            _orphanedArguments.Add(argument);
          }
        }
      }
      if (lastFlag != null)
      {
        _flags.Add(lastFlag);
      }
    }
  }
  public class NamedFlag
  {
    private readonly string _name;

    public string Name
    {
      get { return _name; }
    }

    public NamedFlag(string name)
    {
      _name = name;
    }

    public override string ToString()
    {
      return "--" + _name;
    }
  }
  public class NamedOption
  {
    private readonly NamedFlag _flag;
    private readonly SimpleArgument _argument;

    public NamedFlag Flag
    {
      get { return _flag; }
    }

    public SimpleArgument Argument
    {
      get { return _argument; }
    }

    public NamedOption(NamedFlag flag, SimpleArgument argument)
    {
      _flag = flag;
      _argument = argument;
    }

    public override string ToString()
    {
      return this.Flag + "=" + this.Argument;
    }
  }
  public class SimpleArgument
  {
    private readonly string _value;

    public string Value
    {
      get { return _value; }
    }

    public SimpleArgument(string value)
    {
      _value = value;
    }

    public override string ToString()
    {
      return _value;
    }
  }
}

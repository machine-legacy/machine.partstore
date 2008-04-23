using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace DependencyStore.Domain
{
  public interface IDecidesInclusion
  {
    IncludeExclude Includes(FileSystemPath path);
  }
  public class Exclusion : IDecidesInclusion
  {
    private readonly Regex _expression;

    public Exclusion(Regex expression)
    {
      _expression = expression;
    }

    public virtual IncludeExclude Includes(FileSystemPath path)
    {
      if (_expression.IsMatch(path.Name))
      {
        return IncludeExclude.Exclude;
      }
      return IncludeExclude.Unknown;
    }
  }
  public class Inclusion : IDecidesInclusion
  {
    private readonly Regex _expression;

    public Inclusion(Regex expression)
    {
      _expression = expression;
    }

    public virtual IncludeExclude Includes(FileSystemPath path)
    {
      if (_expression.IsMatch(path.Name))
      {
        return IncludeExclude.Include;
      }
      return IncludeExclude.Unknown;
    }
  }
  public class InclusionExclusionRules : IDecidesInclusion
  {
    private readonly List<IDecidesInclusion> _rules = new List<IDecidesInclusion>();
    private bool _default = true;

    public bool Default
    {
      get { return _default; }
      set { _default = value; }
    }

    public List<IDecidesInclusion> Rules
    {
      get { return _rules; }
    }

    #region IDecidesInclusion Members
    public IncludeExclude Includes(FileSystemPath path)
    {
      foreach (IDecidesInclusion child in _rules)
      {
        switch (child.Includes(path))
        {
          case IncludeExclude.Include:
            return IncludeExclude.Include;
          case IncludeExclude.Exclude:
            return IncludeExclude.Exclude;
          case IncludeExclude.Unknown:
            break;
        }
      }
      return IncludeExclude.Unknown;
    }
    #endregion

    public void AddExclusion(string pattern)
    {
      _rules.Add(new Exclusion(new Regex(pattern)));
    }

    public void AddInclusion(string pattern)
    {
      _rules.Add(new Inclusion(new Regex(pattern)));
    }
  }
  public enum IncludeExclude
  {
    Unknown,
    Include,
    Exclude
  }
}

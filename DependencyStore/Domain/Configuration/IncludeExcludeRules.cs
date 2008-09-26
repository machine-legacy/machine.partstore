using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using DependencyStore.Domain.FileSystem;

namespace DependencyStore.Domain.Configuration
{
  public enum IncludeExclude
  {
    Unknown,
    Include,
    Exclude
  }
  public interface IDecidesInclusion
  {
    IncludeExclude Includes(Purl path);
  }
  public class Exclusion : IDecidesInclusion
  {
    private readonly Regex _expression;

    public Exclusion(string expression)
    {
      _expression = new Regex(expression);
    }

    public virtual IncludeExclude Includes(Purl path)
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

    public Inclusion(string expression)
    {
      _expression = new Regex(expression);;
    }

    public virtual IncludeExclude Includes(Purl path)
    {
      if (_expression.IsMatch(path.Name))
      {
        return IncludeExclude.Include;
      }
      return IncludeExclude.Unknown;
    }
  }
  public class IncludeExcludeRules : IDecidesInclusion
  {
    private readonly List<IDecidesInclusion> _rules = new List<IDecidesInclusion>();
    private IncludeExclude _default = IncludeExclude.Unknown;

    public IncludeExclude Default
    {
      get { return _default; }
      set { _default = value; }
    }

    public List<IDecidesInclusion> Rules
    {
      get { return _rules; }
    }

    public IncludeExcludeRules()
    {
    }

    public IncludeExcludeRules(IncludeExclude defaultValue)
    {
      _default = defaultValue;
    }

    #region IDecidesInclusion Members
    public IncludeExclude Includes(Purl path)
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
      return _default;
    }
    #endregion

    public void AddExclusion(string pattern)
    {
      _rules.Add(new Exclusion(pattern));
    }

    public void AddInclusion(string pattern)
    {
      _rules.Add(new Inclusion(pattern));
    }
  }
  public static class DefaultInclusionRules
  {
    public static string GitDirectory = @"^\.git$";
    public static string SubversionDirectory = @"^\.svn$";
    public static string PtDirectory = @"^pt$";
    public static string ObjDirectory = @"^obj$";
    public static string DllFile = @"^.+\.dll$";
    public static string ExeFile = @"^.+\.exe$";
    public static string PdbFile = @"^.+\.pdb$";
    public static string ConfigFile = @"^.+\.config$";
  }
}

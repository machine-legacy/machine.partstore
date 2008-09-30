using System;
using System.Collections.Generic;
using Machine.Core.Utility;

namespace DependencyStore.Domain.Core
{
  public class VersionNumber
  {
    private DateTime _timeStamp;

    public DateTime TimeStamp
    {
      get { return _timeStamp; }
      set { _timeStamp = value; }
    }

    public string AsString
    {
      get { return _timeStamp.ToString("yyyyMMdd-HHmmssf"); }
    }

    public string PrettyString
    {
      get { return TimeSpanHelper.ToPrettyString(DateTime.UtcNow - this.TimeStamp); }
    }

    public VersionNumber()
    {
      _timeStamp = DateTime.UtcNow;
    }

    public override bool Equals(object obj)
    {
      if (obj is VersionNumber)
      {
        return ((VersionNumber)obj).TimeStamp.Equals(this.TimeStamp);
      }
      return false;
    }

    public override Int32 GetHashCode()
    {
      return _timeStamp.GetHashCode();
    }

    public override string ToString()
    {
      return _timeStamp.ToString();
    }
  }
}

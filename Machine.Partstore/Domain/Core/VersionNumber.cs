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

    public string PrettyAge
    {
      get { return TimeSpanHelper.ToPrettyString(DateTime.UtcNow - _timeStamp); }
    }

    public string PrettyString
    {
      get { return this.TimeStamp.ToLocalTime().ToString(); }
    }

    public VersionNumber()
      : this(DateTime.UtcNow)
    {
    }

    public VersionNumber(DateTime timeStamp)
    {
      _timeStamp = timeStamp;
    }

    public bool IsOlderThan(VersionNumber versionNumber)
    {
      return this.TimeStamp < versionNumber.TimeStamp;
    }

    public override bool Equals(object obj)
    {
      if (obj is VersionNumber)
      {
        return ((VersionNumber)obj).TimeStamp.Equals(this.TimeStamp);
      }
      return false;
    }

    public static bool operator ==(VersionNumber v1, VersionNumber v2)
    {
      return Equals(v1, v2);
    }

    public static bool operator !=(VersionNumber v1, VersionNumber v2)
    {
      return !Equals(v1, v2);
    }

    public override Int32 GetHashCode()
    {
      return _timeStamp.GetHashCode();
    }

    public override string ToString()
    {
      return "VersionNumber<" + _timeStamp + ">";
    }
  }
}

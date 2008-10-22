using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Machine.Partstore.Domain.Core
{
  public class Tags
  {
    public static readonly Tags None = new Tags();
    private string _value;

    [XmlAttribute]
    public string Value
    {
      get { return _value; }
      set { _value = value; }
    }

    public IEnumerable<string> Each
    {
      get
      {
        foreach (string tag in _value.Split(' '))
        {
          yield return tag;
        }
      }
    }

    protected Tags()
    {
    }

    public Tags(string value)
    {
      _value = value;
    }

    public override string ToString()
    {
      return _value;
    }
  }
}

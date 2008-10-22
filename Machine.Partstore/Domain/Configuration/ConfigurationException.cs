using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DependencyStore.Domain.Configuration
{
  public class ConfigurationException : Exception
  {
    public ConfigurationException(string message)
     : base(message)
    {
    }

    public ConfigurationException()
    {
    }

    public ConfigurationException(SerializationInfo info, StreamingContext context)
     : base(info, context)
    {
    }

    public ConfigurationException(string message, Exception innerException)
     : base(message, innerException)
    {
    }
  }
}

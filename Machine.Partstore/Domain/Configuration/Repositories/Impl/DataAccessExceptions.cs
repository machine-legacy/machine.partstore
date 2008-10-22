using System;
using System.Runtime.Serialization;

namespace Machine.Partstore.Domain.Configuration.Repositories.Impl
{
  public class InvalidConfigurationException : ApplicationException
  {
    public InvalidConfigurationException(string message) : base(message)
    {
    }

    public InvalidConfigurationException()
    {
    }

    public InvalidConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public InvalidConfigurationException(string message, Exception innerException) : base(message, innerException)
    {
    }
  }
}
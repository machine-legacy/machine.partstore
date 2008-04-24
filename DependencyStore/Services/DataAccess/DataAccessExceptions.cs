using System;
using System.Runtime.Serialization;

namespace DependencyStore.Services.DataAccess
{
  public class FileSystemEntryNotFoundException : ApplicationException
  {
    public FileSystemEntryNotFoundException(string message) : base(message)
    {
    }

    public FileSystemEntryNotFoundException()
    {
    }

    public FileSystemEntryNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public FileSystemEntryNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
  }
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
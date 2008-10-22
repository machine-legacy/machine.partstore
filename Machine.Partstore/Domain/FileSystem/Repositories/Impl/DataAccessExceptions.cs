using System;
using System.Runtime.Serialization;

namespace Machine.Partstore.Domain.FileSystem.Repositories.Impl
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
}
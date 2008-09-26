using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DependencyStore.Domain.Core
{
  public class ObjectNotFoundException : Exception
  {
    public ObjectNotFoundException(string message) : base(message) { }
    public ObjectNotFoundException() { }
    public ObjectNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    public ObjectNotFoundException(string message, Exception innerException) : base(message, innerException) { }
  }
  public class ArchivedProjectNotFoundException : ObjectNotFoundException
  {
    public ArchivedProjectNotFoundException() { }
    public ArchivedProjectNotFoundException(string message) : base(message) { }
    public ArchivedProjectNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    public ArchivedProjectNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
  }
  public class ArchivedVersionNotFoundException : ObjectNotFoundException
  {
    public ArchivedVersionNotFoundException() { }
    public ArchivedVersionNotFoundException(string message) : base(message) { }
    public ArchivedVersionNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    public ArchivedVersionNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
  }
}

using System;
using System.Collections.Generic;

namespace Machine.Partstore.Application
{
  public class AddingVersionResponse
  {
    public enum Status
    {
      Success,
      NoBuildDirectory,
      BuildDirectoryEmpty,
      AmbiguousRepositoryName
    }

    private readonly Status _status;

    public bool NoBuildDirectory
    {
      get { return _status == Status.NoBuildDirectory; }
    }

    public bool BuildDirectoryEmpty
    {
      get { return _status == Status.BuildDirectoryEmpty; }
    }

    public bool AmbiguousRepositoryName
    {
      get { return _status == Status.AmbiguousRepositoryName; }
    }

    public bool Success
    {
      get { return _status == Status.Success; }
    }

    public AddingVersionResponse(Status status)
    {
      _status = status;
    }
  }
}
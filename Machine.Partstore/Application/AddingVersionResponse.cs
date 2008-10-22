using System;
using System.Collections.Generic;

namespace Machine.Partstore.Application
{
  public class AddingVersionResponse
  {
    private readonly bool _noBuildDirectory;
    private readonly bool _ambiguousRepositoryName;

    public bool NoBuildDirectory
    {
      get { return _noBuildDirectory; }
    }

    public bool AmbiguousRepositoryName
    {
      get { return _ambiguousRepositoryName; }
    }

    public bool Success
    {
      get { return !_noBuildDirectory && !_ambiguousRepositoryName; }
    }

    public AddingVersionResponse(bool noBuildDirectory, bool ambiguousRepositoryName)
    {
      _noBuildDirectory = noBuildDirectory;
      _ambiguousRepositoryName = ambiguousRepositoryName;
    }
  }
}
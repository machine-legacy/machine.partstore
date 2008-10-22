using System;
using System.Collections.Generic;

namespace Machine.Partstore.Utility
{
  public static class PathHelper
  {
    public static string NormalizeDirectorySlashes(string directory)
    {
      if (directory.EndsWith(@"\"))
      {
        return directory;
      }
      return directory + @"\";
    }
  }
}

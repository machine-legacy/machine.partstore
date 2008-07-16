using System;
using System.Collections.Generic;
using System.IO;

namespace DependencyStore.Utility
{
  public static class StreamHelper
  {
    public static void Copy(Stream source, Stream destiny)
    {
      while (true)
      {
        byte[] buffer = new byte[65536];
        int bytes = source.Read(buffer, 0, buffer.Length);
        if (bytes <= 0)
        {
          break;
        }
        destiny.Write(buffer, 0, bytes);
      }
    }
  }
}

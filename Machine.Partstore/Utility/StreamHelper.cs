using System;
using System.Collections.Generic;
using System.IO;

namespace Machine.Partstore.Utility
{
  public static class StreamHelper
  {
    public delegate void ProgressCallback(long bytesSoFar);
    public static void Copy(Stream source, Stream destiny, ProgressCallback callback)
    {
      long bytesSoFar = 0;
      while (true)
      {
        byte[] buffer = new byte[65536];
        int bytes = source.Read(buffer, 0, buffer.Length);
        bytesSoFar += bytes;
        if (bytes <= 0)
        {
          callback(bytesSoFar);
          break;
        }
        if (bytesSoFar % 13 == 0)
        {
           callback(bytesSoFar);
        }
        destiny.Write(buffer, 0, bytes);
      }
    }
  }
}

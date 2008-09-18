using System;
using System.Collections.Generic;
using System.Text;

using DependencyStore.Domain.Core;

namespace DependencyStore.Utility
{
  public class FileSystemEntryPrinter
  {
    public string Print(FileSystemEntry entry)
    {
      StringBuilder sb = new StringBuilder();
      Print(entry, sb, 0);
      return sb.ToString();
    }

    public void Print(FileSystemEntry entry, StringBuilder sb)
    {
      Print(entry, sb, 0);
    }

    public void Print(FileSystemEntry entry, StringBuilder sb, int depth)
    {
      if (entry is FileSystemDirectory)
      {
        Print((FileSystemDirectory)entry, sb, depth);
      }
      else if (entry is FileSystemFile)
      {
        Print((FileSystemFile)entry, sb, depth);
      }
    }

    public void Print(FileSystemFile file, StringBuilder sb, int depth)
    {
      sb.Append(new string(' ', depth * 2)).AppendFormat("{0}", file).AppendLine();
    }

    public void Print(FileSystemDirectory directory, StringBuilder sb, int depth)
    {
      sb.Append(new string(' ', depth * 2)).AppendFormat("{0}", directory).AppendLine();
      foreach (FileSystemEntry entry in directory.Entries)
      {
        Print(entry, sb, depth + 1);
      }
    }
  }
}
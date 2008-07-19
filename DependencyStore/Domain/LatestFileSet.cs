using System;
using System.Collections.Generic;
using Machine.Container;

namespace DependencyStore.Domain
{
  public class LatestFileSet : FileSet
  {
    public FileAsset FindExistingByName(FileAsset file)
    {
      List<FileAsset> files = new List<FileAsset>(FindFilesNamed(file.Purl.Name));
      if (files.Count > 1)
      {
        throw new YouFoundABugException("How did a LatestFiles object get multiple files with the same name: " + file);
      }
      if (files.Count != 1)
      {
        return null;
      }
      return files[0];
    }

    public override void Add(FileAsset file)
    {
      FileAsset existing = FindExistingByName(file);
      if (existing == null)
      {
        base.Add(file);
      }
      else if (file.IsNewerThan(existing))
      {
        Remove(existing);
        base.Add(file);
      }
    }
  }
}

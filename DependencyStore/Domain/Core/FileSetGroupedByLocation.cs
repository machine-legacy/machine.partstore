using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Core
{
  public class FileSetGroupedByLocation
  {
    private readonly Dictionary<Location, FileSet> _grouped = new Dictionary<Location, FileSet>();
    private readonly FileSet _allFiles;

    public FileSetGroupedByLocation(FileSet allFiles, Dictionary<Location, FileSet> grouped)
    {
      _allFiles = allFiles;
      _grouped = grouped;
    }

    public IEnumerable<KeyValuePair<Location, FileSet>> LocationsAndFiles
    {
      get { return _grouped; }
    }

    public FileSet AllFiles
    {
      get { return _allFiles; }
    }

    public static FileSetGroupedByLocation GroupFileSetIntoLocations(IEnumerable<SourceLocation> locations, FileSet files)
    {
      Dictionary<Location, FileSet> grouped = new Dictionary<Location, FileSet>();
      foreach (SourceLocation location in locations)
      {
        grouped[location] = new FileSet();
      }
      foreach (FileAsset file in files.Files)
      {
        bool added = false;
        foreach (SourceLocation location in locations)
        {
          if (location.HasFile(file))
          {
            added = true;
            grouped[location].Add(file);
            break;
          }
        }
        if (!added)
        {
          throw new InvalidOperationException();
        }
      }
      return new FileSetGroupedByLocation(files, grouped);
    }
  }
}

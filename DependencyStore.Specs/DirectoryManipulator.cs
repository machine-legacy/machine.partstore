using System;
using System.Collections.Generic;
using System.IO;

namespace DependencyStore
{
  public abstract class DirectoryManipulator
  {
    private readonly string _directory;

    public string RootDirectory
    {
      get { return _directory; }
    }

    protected string PathTo(string path)
    {
      return Path.Combine(_directory, path);
    }

    protected DirectoryManipulator(string directory)
    {
      _directory = directory;
    }
    
    public virtual void Create()
    {
      ChangeToSystemDirectory();
      if (Directory.Exists(this.RootDirectory))
      {
        Directory.Delete(this.RootDirectory, true);
      }
      Directory.CreateDirectory(this.RootDirectory);
      ChangeTo();
    }

    public void Cleanup()
    {
      ChangeToSystemDirectory();
      if (Directory.Exists(this.RootDirectory))
      {
        Directory.Delete(_directory, true);
      }
    }

    private static void ChangeToSystemDirectory()
    {
      Environment.CurrentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System);
    }

    private void ChangeTo()
    {
      Environment.CurrentDirectory = this.RootDirectory;
    }
  }
}

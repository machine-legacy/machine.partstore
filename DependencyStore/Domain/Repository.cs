﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DependencyStore.Domain
{
  public class Repository
  {
    private readonly List<ArchivedProject> _projects = new List<ArchivedProject>();

    public List<ArchivedProject> Projects
    {
      get { return _projects; }
    }

    public bool IsEmpty
    {
      get { return _projects.Count == 0; }
    }

    public ArchivedProject FindOrCreateProject(Project project)
    {
      ArchivedProject archived = FindProject(project.Name);
      if (archived == null)
      {
        archived = new ArchivedProject(project.Name);
        AddProject(archived);
      }
      return archived;
    }

    public ArchivedProject FindProject(Project project)
    {
      return FindProject(project.Name);
    }

    public ArchivedProject FindProject(string name)
    {
      foreach (ArchivedProject existing in _projects)
      {
        if (existing.Name.Equals(name))
        {
          return existing;
        }
      }
      return null;
    }

    public void AddProject(ArchivedProject project)
    {
      if (FindProject(project.Name) != null)
      {
        throw new InvalidOperationException();
      }
      _projects.Add(project);
    }
  }
  public class ArchivedProject
  {
    private readonly List<ArchivedProjectVersion> _versions = new List<ArchivedProjectVersion>();
    private string _name;

    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    public List<ArchivedProjectVersion> Versions
    {
      get { return _versions; }
    }

    public ArchivedProject()
    {
    }

    public ArchivedProject(string name)
    {
      _name = name;
    }

    public ArchivedProjectVersion FindVersion(string version)
    {
      foreach (ArchivedProjectVersion existing in _versions)
      {
        if (existing.Version.Equals(version))
        {
          return existing;
        }
      }
      return null;
    }

    public void AddVersion(ArchivedProjectVersion version)
    {
      if (FindVersion(version.Version) != null)
      {
        throw new InvalidOperationException();
      }
      _versions.Add(version);
    }
  }
  public class ArchivedProjectVersion
  {
    private string _version;
    private DateTime _createdAt;
    private Purl _archiveFile;

    public string Version
    {
      get { return _version; }
      set { _version = value; }
    }

    public DateTime CreatedAt
    {
      get { return _createdAt; }
      set { _createdAt = value; }
    }

    [XmlIgnore]
    public Purl ArchiveFile
    {
      get { return _archiveFile; }
      set { _archiveFile = value; }
    }

    public ArchivedProjectVersion()
    {
      _createdAt = DateTime.Now;
    }

    public ArchivedProjectVersion(DateTime createdAt, string version)
    {
      _createdAt = createdAt;
      _version = version;
    }
  }
}
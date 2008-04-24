using System;
using System.Collections.Generic;
using DependencyStore.Domain;

namespace DependencyStore.Services
{
  public interface ISinkUpdater
  {
  }
  public class SinkUpdater : ISinkUpdater
  {
    public void Start()
    {
      DomainEvents.EncounteredOutdatedSinkFile += OnOutdatedSinkFile;
    }

    private void OnOutdatedSinkFile(object sender, OutdatedSinkFileEventArgs e)
    {
    }
  }
  public class ConsoleReporter : ISinkUpdater
  {
    public void Start()
    {
      DomainEvents.EncounteredOutdatedSinkFile += OnOutdatedSinkFile;
    }

    private void OnOutdatedSinkFile(object sender, OutdatedSinkFileEventArgs e)
    {
    }
  }
}

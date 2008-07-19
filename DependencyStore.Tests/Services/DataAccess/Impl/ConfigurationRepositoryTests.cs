using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Machine.Core.Services;
using Machine.Testing;

using NUnit.Framework;
using Rhino.Mocks;

namespace DependencyStore.Services.DataAccess.Impl
{
  [TestFixture]
  public class ConfigurationRepositoryTests : TestsFor<ConfigurationRepository>
  {
    [Test]
    public void FindConfiguration_Always_HitsFileAndLoads()
    {
      string text = @"<DependencyStore Packages='C:\'><BuildDirectories><Build Path='C:\'/></BuildDirectories></DependencyStore>";
      using (Record)
      {
        MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(text));
        SetupResult.For(The<IFileSystem>().OpenText("DependencyStore.config")).Return(new StreamReader(stream));
      }
      Assert.IsNotNull(Target.FindConfiguration("DependencyStore.config"));
    }

    [Test]
    [ExpectedException(typeof(InvalidConfigurationException))]
    public void FindConfiguration_Malformed_Throws()
    {
      string text = @"";
      using (Record)
      {
        MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(text));
        SetupResult.For(The<IFileSystem>().OpenText("DependencyStore.config")).Return(new StreamReader(stream));
      }
      Target.FindConfiguration("DependencyStore.config");
    }
  }
}

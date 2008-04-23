using System;
using System.Collections.Generic;

namespace DependencyStore.Services
{
  public interface IController
  {
    void Show();
    void Clear();
    void Refresh();
  }
}

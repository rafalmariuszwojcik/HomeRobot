﻿using System.Collections.Generic;

namespace RobotControl.Core
{
  public interface IManager<T, M>
    where T : class
    where M : class
  {
    void RegisterListener(T listener);
    void UnregisterListener(T listener);
    void BroadcastData(object sender, IEnumerable<M> data);
  }
}

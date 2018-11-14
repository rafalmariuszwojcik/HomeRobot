﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControl.Simulation
{
  public interface IGeometryAwareItem<T> where T: IGeometry
  {
    T Geometry { get; }
  }
}
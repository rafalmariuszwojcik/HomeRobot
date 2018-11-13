﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RobotControl.Command
{
  [XmlRoot("ENG")]
  public class EngineCommand : CommandBase
  {
    int Speed { get; set; }
  }
}
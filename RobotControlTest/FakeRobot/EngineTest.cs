﻿using NUnit.Framework;
using RobotControl.Fake.FakeRobot;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RobotControlTest.FakeRobot
{
  /// <summary>
  /// Fake engine tests.
  /// </summary>
  public class EngineTest
  {
    /// <summary>
    /// Setups this instance.
    /// </summary>
    [SetUp]
    public void Setup()
    {
    }

    /// <summary>
    /// Starts the engine.
    /// </summary>
    /// <remarks>Starts engine and check speed counting.</remarks>
    [Test]
    public void StartEngine() 
    {
      const int SPEED = 10;
      using (var engine = new Engine()) 
      {
        engine.Speed = SPEED;
        for (var i = 0; i < 100; i++) 
        {
          var signaled = engine.SignalEvent.WaitOne(1000);
          Assert.IsTrue(signaled);
          if (engine.CurrentSpeed > 0.0) 
          {
            //Assert.IsTrue(Math.Abs(engine.CurrentSpeed - SPEED) <= (SPEED * 0.2));
          }
        }
      }
    }
  }
}

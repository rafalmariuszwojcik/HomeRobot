using NUnit.Framework;
using RobotControl.Command;
using RobotControl.Simulation.Robot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests
{
  public class RobotTests
  {
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
      const double ROBOT_WIDTH = 124.0;
      const double WHEEL_RADIUS = 33.2;
      const int ENCODER_POINTS = 20;

      var calculator = new PositionCalculator(ROBOT_WIDTH, WHEEL_RADIUS, ENCODER_POINTS);

      var robot = new Robot();
      var start = DateTime.Now;
      for (var i = 1; i <= 10; i++)
      {
        calculator.Signal(Encoder.Left, i, (long)(DateTime.Now - start).TotalMilliseconds);
        Task.Delay(10).Wait();
        calculator.Signal(Encoder.Right, i, (long)(DateTime.Now - start).TotalMilliseconds);
        Task.Delay(90).Wait();
      }
      
      Assert.Pass();
    }

    [Test]
    public void OdometryCounterTest()
    {
      var signals = new List<IEncoderCommand>();

      signals.Add(new EncoderCommand() { Index = 0, Distance = 1, Milis = 100 });
      signals.Add(new EncoderCommand() { Index = 0, Distance = 2, Milis = 200 });
      signals.Add(new EncoderCommand() { Index = 0, Distance = 3, Milis = 300 });
      signals.Add(new EncoderCommand() { Index = 0, Distance = 4, Milis = 400 });
      signals.Add(new EncoderCommand() { Index = 0, Distance = 5, Milis = 500 });

      signals.Add(new EncoderCommand() { Index = 1, Distance = 1, Milis = 150 });
      signals.Add(new EncoderCommand() { Index = 1, Distance = 2, Milis = 250 });
      signals.Add(new EncoderCommand() { Index = 1, Distance = 3, Milis = 350 });
      signals.Add(new EncoderCommand() { Index = 1, Distance = 4, Milis = 450 });
      signals.Add(new EncoderCommand() { Index = 1, Distance = 5, Milis = 550 });
      
      var result = OdometryCounter.Calculate(550, signals);
    }
  }
}
namespace RobotControl.Simulation
{
  public interface IGeometryAwareItem<T> where T : IGeometry
  {
    T Geometry { get; }
  }
}

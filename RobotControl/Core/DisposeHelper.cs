using System;

namespace RobotControl.Core
{
  /// <summary>
  /// Helper to have a easy way of disposing an object.
  /// </summary>
  public static class DisposeHelper
  {
    /// <summary>
    /// Tries to cast the obj to IDisposable. If this works it will dispose it; otherwise not.
    /// </summary>
    /// <typeparam name="T">Type of the reference to dispose.</typeparam>
    /// <param name="obj">Reference to cast and dispose.</param>
    public static void Dispose<T>(T obj)
    {
      (obj as IDisposable)?.Dispose();
    }

    /// <summary>
    /// Calls the dispose methode on a IDisposable and sets the reference to null.
    /// If the given reference is unable to be casted to IDisposable only the reference will be set to null.
    /// </summary>
    /// <typeparam name="T">Type of the reference to dispose and set.</typeparam>
    /// <param name="obj">Reference to cast, dispose and set to null.</param>
    public static void DisposeAndNull<T>(ref T obj)
      where T : class
    {
      DisposeHelper.Dispose(obj);
      obj = null;
    }
  }
}
using System;
using System.Linq;

namespace MDMUtils
{
  internal class Maths
  {
    ///========================================================================
    /// Static Method : Max 
    /// 
    /// <summary>
    ///   Returns the greatest of a collection of IComparable objects
    /// </summary>
    ///========================================================================
    public static T Max<T>(params T[] values) where T : IComparable
    {
      return values.Max();
    }

    ///========================================================================
    /// Static Method : Min 
    /// 
    /// <summary>
    ///   Returns the least of a collection of IComparable objects
    /// </summary>
    ///========================================================================
    public static T Min<T>(params T[] values) where T : IComparable
    {
      return values.Min();
    }
  }
}

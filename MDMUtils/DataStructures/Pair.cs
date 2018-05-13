using System;

namespace MDMUtils.DataStructures
{
  ///==========================================================
  /// Class : Pair
  /// 
  /// <summary>
  ///   Parent of the equatable Ordered and Unordered Pairs
  /// </summary>
  ///==========================================================
  public abstract class Pair<S,T>
  {
    public readonly S X;
    public readonly T Y;

    ///==========================================================
    /// Constructor : Default
    /// 
    /// <summary>
    ///   Initializes the point to it's default values.
    /// </summary>
    ///==========================================================
    protected Pair()
    {
      X = default(S);
      Y = default(T);
    }
    
    ///==========================================================
    /// Constructor : Explicit
    /// 
    /// <summary>
    ///   Initializes the point (x,y).
    /// </summary>
    ///==========================================================
    protected Pair(S x, T y)
    {
      X = x;
      Y = y;
    }

    public override int GetHashCode()
    {
      return X.GetHashCode() ^ Y.GetHashCode();
    }

    public override string ToString()
    {
      return String.Format("({0},{1})", X, Y);
    }

  }
}

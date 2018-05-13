using System;

namespace MDMUtils.DataStructures
{
  ///==========================================================
  /// Class : CartesianPoint
  /// 
  /// <summary>
  ///   Represents a 2-D Cartesian Point
  /// </summary>
  ///==========================================================
  public class CartesianPoint : OrderedEquatablePair<double>
  {
    public CartesianPoint() : base(){}
    public CartesianPoint(double x, double y) : base(x, y){}

    public double DistanceFrom(CartesianPoint oher)
    {
      return Math.Sqrt(Math.Pow(X - oher.X,2) + Math.Pow(Y - oher.Y,2));
    }

    public static double DistanceBetween(CartesianPoint first,CartesianPoint second)
    {
      return first.DistanceFrom(second);
    }
}
}

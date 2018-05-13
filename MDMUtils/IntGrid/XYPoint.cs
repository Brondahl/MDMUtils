using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using MDMUtils.DataStructures;

namespace MDMUtils.IntGrid
{
  public class XYPoint : OrderedEquatablePair<int>, IEquatable<XYPoint>
  {
    public XYPoint(int x, int y) : base(x, y)
    {}

    #region Extending IEquatable
    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      return Equals(obj as XYPoint);
    }

    public virtual bool Equals(XYPoint other)
    {
      return base.Equals(other);
    }
    #endregion

    ///========================================================================
    /// Method : DistanceFrom
    /// 
    /// <summary>
    /// 	Returns the "Taxi Distance" between the two points; the sum of the
    ///   vertical and horizontal offsets between this and the other point.
    /// </summary>
    ///========================================================================
    [Pure]
    public int DistanceFrom(XYPoint other)
    {
      return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
    }

    ///========================================================================
    /// Method : DistanceFrom
    /// 
    /// <summary>
    /// 	Returns the "Taxi Distance" between the two points; the sum of the
    ///   vertical and horizontal offsets between the two points.
    /// </summary>
    ///========================================================================
    [Pure]
    public static int DistanceBetween(XYPoint first, XYPoint second)
    {
      return first.DistanceFrom(second);
    }

    ///========================================================================
    /// Method : DistanceFrom
    /// 
    /// <summary>
    /// 	Returns the "Pythagorean Distance" between the two points.
    /// </summary>
    ///========================================================================
    [Pure]
    public double DirectDistanceFrom(XYPoint other)
    {
      return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
    }

    ///========================================================================
    /// Method : DistanceFrom
    /// 
    /// <summary>
    /// 	Returns the "Pythagorean Distance" between the two points.
    /// </summary>
    ///========================================================================
    [Pure]
    public static double DirectDistanceBetween(XYPoint first, XYPoint second)
    {
      return first.DirectDistanceFrom(second);
    }

    [Pure]
    public XYPoint MoveOne(eDirection direction)
    {
      var movement = new Movement(direction);
      return this + movement;
    }

    [Pure]
    public XYPoint Move(eDirection direction, int count)
    {
      var movement = new Movement(direction, count);
      return this + movement;
    }

    [Pure]
    public XYPoint Move(Movement movement)
    {
      return new XYPoint(this.X + movement.X, this.Y + movement.Y);
    }

    [Pure]
    public static XYPoint operator +(XYPoint point, Movement movement)
    {
      return new XYPoint(point.X + movement.X, point.Y + movement.Y);
    }

    [Pure]
    public static XYPoint operator -(XYPoint point, Movement movement)
    {
      return point + (movement * -1);
    }

    [Flags]
    public enum eAdjacencyType
    {
      Orthogonal = 1,
      Diagonal = 2,
      All = 3,
    }

    public IEnumerable<XYPoint> GetAdjacentPoints(
      eDirection startingDirection = eDirection.North,
      eDirectionChange deltaDirection = eDirectionChange.Clockwise,
      eAdjacencyType adjacencyType = eAdjacencyType.All)
    {
      var centrePoint = this;

      foreach (var currentDirection in DirectionAccessor.Enumerate(startingDirection, deltaDirection))
      {
        if (adjacencyType.HasFlag(eAdjacencyType.Orthogonal))
        {
          yield return centrePoint.MoveOne(currentDirection);
        }

        if (adjacencyType.HasFlag(eAdjacencyType.Diagonal))
        {
          eDirection nextDirection = currentDirection.Turn(deltaDirection);
          yield return centrePoint.MoveOne(currentDirection).MoveOne(nextDirection);
        }
      }

      // ReSharper disable once RedundantJumpStatement
      yield break;
    }

    public IEnumerable<XYPoint> GetEdgeAdjacentPoints(
      eDirection startingDirection = eDirection.North,
      eDirectionChange deltaDirection = eDirectionChange.Clockwise)
    {
      return GetAdjacentPoints(startingDirection, deltaDirection, eAdjacencyType.Orthogonal);
    }

    public IEnumerable<XYPoint> GetAllAdjacentPoints(
      eDirection startingDirection = eDirection.North,
      eDirectionChange deltaDirection = eDirectionChange.Clockwise)
    {
      // ReSharper disable once RedundantArgumentDefaultValue
      return GetAdjacentPoints(startingDirection, deltaDirection, eAdjacencyType.All);
    }
  }
}

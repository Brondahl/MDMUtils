using System;
using System.Diagnostics.Contracts;
using MDMUtils.DataStructures;

namespace MDMUtils.IntGrid
{
  public class Movement : OrderedEquatablePair<int>, IEquatable<Movement>
  {
    #region Extending IEquatable
    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      return Equals(obj as Movement);
    }

    public virtual bool Equals(Movement other)
    {
      return base.Equals(other);
    }
    #endregion

    [Pure]
    public Movement(eDirection direction, int multiplier = 1)
      : base(direction.X()*multiplier, direction.Y()*multiplier) {}

    [Pure]
    public Movement(int x, int y) : base(x,y) {}

    //Removed in expectation that it would be an arse for a finite grid.
    //[Pure]
    //public XYPoint ApplyTo(XYPoint originalPoint)
    //{
    //  return originalPoint.Move(this);
    //}

    [Pure]
    public static implicit operator Movement(eDirection direction)
    {
      return new Movement(direction);
    }

    [Pure]
    public static Movement operator *(Movement movement, int multiplier)
    {
      return new Movement(movement.X * multiplier, movement.Y * multiplier);
    }

    [Pure]
    public static Movement operator *(int multiplier, Movement movement)
    {
      return movement * multiplier;
    }

    [Pure]
    public static Movement operator /(Movement movement, int divisor)
    {
      int newX = (movement.X/divisor);
      int newY = (movement.Y / divisor);

      if (((movement.X % divisor) != 0) || ((movement.Y % divisor) != 0))
      {
        throw new InvalidOperationException(
          "Division of movements is only acceptable if the outcome is an exact integer.");
      }

      return new Movement(newX, newY);
    }
  }
}

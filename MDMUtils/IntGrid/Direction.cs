using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace MDMUtils.IntGrid
{
  public enum eDirection
  {
    North = 0,
    East = 1,
    South = 2,
    West = 3
  }
  
  public static class DirectionAccessor
  {
    [Pure]
    public static int X(this eDirection direction)
    {
      switch (direction)
      {
        case eDirection.East:
          return 1;
        case eDirection.West:
          return -1;
        case eDirection.North:
        case eDirection.South:
          return 0;
        default:
          throw new NonExistentEnumCaseException<eDirection>();  //ncrunch: no coverage
      }
    }

    [Pure]
    public static int Y(this eDirection direction)
    {
      switch (direction)
      {
        case eDirection.North:
          return 1;
        case eDirection.South:
          return -1;
        case eDirection.East:
        case eDirection.West:
          return 0;
        default:
          throw new NonExistentEnumCaseException<eDirection>();  //ncrunch: no coverage
      }
    }

    [Pure]
    public static eDirection Turn(this eDirection currentDirection, eDirectionChange directionToMove)
    {
      return FromInt((int)currentDirection + (int)directionToMove);
    }

    ///========================================================================
    /// Method : FromInt
    /// 
    /// <summary>
    /// 	Converts an integer into an eDirection.
    ///   0 is North, then progressing clockwise, so that 3 is West.
    /// 
    ///   Applies modulus, to cope with number > 3 or negative numbers.
    /// </summary>
    /// <param name="value">
    ///   Integer to be translated into a direction.
    /// </param>
    ///========================================================================
    [Pure]
    public static eDirection FromInt(int value)
    {
      return (eDirection)(((value % 4) + 4) % 4);
    }

    [Pure]
    public static IEnumerable<eDirection> Enumerate(eDirection startingDirection = eDirection.North, eDirectionChange deltaDirection = eDirectionChange.Clockwise)
    {
      eDirection currentDirection = startingDirection;

      for (int i = 0; i < 4; i++)
      {
        yield return currentDirection;
        currentDirection = currentDirection.Turn(deltaDirection);
      }

      // ReSharper disable once RedundantJumpStatement
      yield break;
    }

  }
}
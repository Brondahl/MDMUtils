using System.Diagnostics.Contracts;

namespace MDMUtils.IntGrid
{
  public enum eDirectionChange
  {
    Clockwise = 1,
    AntiClockwise = -1
  }

  public static class DirectionChangeExtensions
  {
    [Pure]
    public static eDirection ApplyTo(this eDirectionChange change, eDirection originalDirection)
    {
      return originalDirection.Turn(change);
    }
  }
}

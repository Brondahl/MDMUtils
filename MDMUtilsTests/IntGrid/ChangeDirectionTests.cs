using MDMUtils.IntGrid;
using NUnit.Framework;

namespace MDMUtilsTests.IntGrid
{
  [TestFixture]
  class ChangeDirectionTests
  {
    [Test]
    public void ApplyingAClockwiseDirectionChangeWorks()
    {
      var startDirection = eDirection.North;
      var deltaDirection = eDirectionChange.Clockwise;
      var expectedDirection = eDirection.East;

      AssertChangeIsCorrectlyApplied(startDirection, deltaDirection, expectedDirection);
    }

    [Test]
    public void ApplyingAnAntiClockwiseDirectionChangeWorks()
    {
      var startDirection = eDirection.South;
      var deltaDirection = eDirectionChange.AntiClockwise;
      var expectedDirection = eDirection.East;

      AssertChangeIsCorrectlyApplied(startDirection, deltaDirection, expectedDirection);
    }

    [Test]
    public void ApplyingAClockwiseDirectionChangeAcrossNorthWestJoinWorks()
    {
      var startDirection = eDirection.West;
      var deltaDirection = eDirectionChange.Clockwise;
      var expectedDirection = eDirection.North;

      AssertChangeIsCorrectlyApplied(startDirection, deltaDirection, expectedDirection);
    }

    [Test]
    public void ApplyingAnAntiClockwiseDirectionChangeAcrossNorthWestJoinWorks()
    {
      var startDirection = eDirection.North;
      var deltaDirection = eDirectionChange.AntiClockwise;
      var expectedDirection = eDirection.West;

      AssertChangeIsCorrectlyApplied(startDirection, deltaDirection, expectedDirection);
    }

    private static void AssertChangeIsCorrectlyApplied(
      eDirection startDirection,
      eDirectionChange deltaDirection,
      eDirection expectedDirection)
    {
      var resultantDirectionFromTurn = startDirection.Turn(deltaDirection);
      var resultantDirectionFromApplyTo = deltaDirection.ApplyTo(startDirection);

      Assert.AreEqual(expectedDirection, resultantDirectionFromTurn);
      Assert.AreEqual(expectedDirection, resultantDirectionFromApplyTo);
    }
  }
}

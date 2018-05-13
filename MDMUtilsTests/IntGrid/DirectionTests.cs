using MDMUtils.IntGrid;
using NUnit.Framework;

namespace MDMUtilsTests.IntGrid
{
  [TestFixture]
  internal class DirectionTests
  {
    [TestCase(eDirection.North, 0),
     TestCase(eDirection.East, 1),
     TestCase(eDirection.West, -1)]
    public void XComponentsOfDirectionsWork(eDirection direction, int expectedXComponentOfDirection)
    {
      Assert.AreEqual(expectedXComponentOfDirection, direction.X());
    }

    [TestCase(eDirection.North, 1),
     TestCase(eDirection.South, -1),
     TestCase(eDirection.East, 0)]
    public void YComponentsOfDirectionsWork(eDirection direction, int expectedYComponentOfDirection)
    {
      Assert.AreEqual(expectedYComponentOfDirection, direction.Y());
    }

    [TestCase(0, eDirection.North),
     TestCase(1, eDirection.East),
     TestCase(2, eDirection.South),
     TestCase(3, eDirection.West),
     TestCase(4, eDirection.North),
     TestCase(7, eDirection.West),
     TestCase(-1, eDirection.West),
     TestCase(-3, eDirection.East),
     TestCase(-8, eDirection.North),
     TestCase(100, eDirection.North),
     TestCase(-100, eDirection.North)]
    public void ParsingDirectionsFromIntsWorks(int inputInteger, eDirection expectedDirection)
    {
      Assert.AreEqual(expectedDirection, DirectionAccessor.FromInt(inputInteger));
    }

    [TestCase(eDirection.North, eDirectionChange.Clockwise,
              new[]{eDirection.North, eDirection.East, eDirection.South, eDirection.West})]
    [TestCase(eDirection.West, eDirectionChange.Clockwise,
              new[]{eDirection.West, eDirection.North, eDirection.East, eDirection.South})]
    [TestCase(eDirection.East, eDirectionChange.AntiClockwise,
              new[]{eDirection.East, eDirection.North, eDirection.West, eDirection.South})]
    public void DirectionEnumerationWorks(eDirection startingDirection, eDirectionChange deltaDirection, eDirection[] expectedOutput)
    {
      CollectionAssert.AreEqual(expectedOutput, DirectionAccessor.Enumerate(startingDirection, deltaDirection));
    }

    [Test]
    public void DirectionEnumerationWorksWithDefaultDirection()
    {
      var expectedOutput = new[] {eDirection.South, eDirection.West, eDirection.North, eDirection.East};

      CollectionAssert.AreEqual(expectedOutput, DirectionAccessor.Enumerate(eDirection.South));
    }

    [Test]
    public void DirectionEnumerationWorksWithDefaultStart()
    {
      var expectedOutput = new[] {eDirection.North, eDirection.West, eDirection.South, eDirection.East};

      CollectionAssert.AreEqual(expectedOutput, DirectionAccessor.Enumerate(deltaDirection: eDirectionChange.AntiClockwise));
    }

    [Test]
    public void DirectionEnumerationWorksWithAllDefaults()
    {
      var expectedOutput = new[] {eDirection.North, eDirection.East, eDirection.South, eDirection.West};

      CollectionAssert.AreEqual(expectedOutput, DirectionAccessor.Enumerate());
    }
  } 
}

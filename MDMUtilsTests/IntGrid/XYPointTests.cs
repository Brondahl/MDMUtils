using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using MDMUtils.IntGrid;
using NUnit.Framework;

namespace MDMUtilsTests.IntGrid
{
  [TestFixture]
  class XYPointTests
  {
    [Test]
    public void EqualityComparisonsWork()
    {
      var basePoint = new XYPoint(0, 2);
      var matchingPoint = new XYPoint(0, 2);
      var nonMatchingPoint = new XYPoint(2, 0);
      var anotherNonMatchingPoint = new XYPoint(0, 1);


      Assert.IsTrue(basePoint.Equals(matchingPoint));
      Assert.IsTrue(basePoint == matchingPoint);
      Assert.IsFalse(basePoint != matchingPoint);

      Assert.IsFalse(basePoint.Equals(nonMatchingPoint));
      Assert.IsFalse(basePoint == nonMatchingPoint);
      Assert.IsTrue(basePoint != nonMatchingPoint);

      Assert.IsFalse(basePoint.Equals(anotherNonMatchingPoint));
      Assert.IsFalse(basePoint == anotherNonMatchingPoint);
      Assert.IsTrue(basePoint != anotherNonMatchingPoint);


      Assert.IsFalse(ReferenceEquals(basePoint, matchingPoint));
      Assert.AreEqual(basePoint, matchingPoint);
      Assert.AreNotEqual(basePoint, nonMatchingPoint);
    }

    [TestCase( 0,0  ,  1,1  , -1,-1),
     TestCase( 2,2  ,  1,1  ,  1,1 ),
     TestCase( 1,3  ,  2,4  , -1,-1),
     TestCase( 1,0  ,  0,-2 ,  1,2 ),
     TestCase(-3,-2 ,  0,0  , -3,-2),
     TestCase(-3,-1 , -2,-4 , -1,3 ),
     TestCase( 0,0  ,  0,0  ,  0,0 )]
    public void SubtractingMovementsFromXYPointsWorks(int startX, int startY, int moveX, int moveY, int endX, int endY)
    {
      var originalPoint = new XYPoint(startX, startY);
      var movement = new Movement(moveX, moveY);
      var expectedFinalPoint = new XYPoint(endX, endY);

      Assert.AreEqual(expectedFinalPoint, originalPoint - movement);
    }

    [TestCase( 0,0  , eDirection.North ,  0,1 ),
     TestCase( 2,2  , eDirection.South ,  2,1 ),
     TestCase( 1,3  , eDirection.West  ,  0,3 ),
     TestCase( 1,0  , eDirection.South ,  1,-1),
     TestCase(-3,-2 , eDirection.South , -3,-3),
     TestCase(-3,-1 , eDirection.East  , -2,-1),
     TestCase( 0,0  , eDirection.West  , -1,0 )]
    public void MoveOneWorks(int startX, int startY, eDirection direction, int endX, int endY)
    {
      var originalPoint = new XYPoint(startX, startY);
      var expectedFinalPoint = new XYPoint(endX, endY);

      Assert.AreEqual(expectedFinalPoint, originalPoint.MoveOne(direction));
    }

    [Test]
    public void AdjacencyAllPointsMethodWorks()
    {
      var centrePoint = new XYPoint(0, 0);

      var actualOutput = centrePoint.GetAllAdjacentPoints();
      var expectedOutput = new[]
      {
        new XYPoint(0,1),
        new XYPoint(1,1),
        new XYPoint(1,0),
        new XYPoint(1,-1),
        new XYPoint(0,-1),
        new XYPoint(-1,-1),
        new XYPoint(-1,0),
        new XYPoint(-1,1)
      };
      CollectionAssert.AreEqual(expectedOutput, actualOutput);
    }

    [Test]
    public void AdjacencyEdgePointsMethodWorksWithOffset()
    {
      var centrePoint = new XYPoint(-1, 0);

      var actualOutput = centrePoint.GetEdgeAdjacentPoints(eDirection.South, eDirectionChange.AntiClockwise);
      var expectedOutput = new[]
      {
        new XYPoint(-1,-1),
        new XYPoint(0,0),
        new XYPoint(-1,1),
        new XYPoint(-2,0)
      };
      CollectionAssert.AreEqual(expectedOutput, actualOutput);
    }


    [TestCase(0,0),
     TestCase(2,3),
     TestCase(1,-1),
     TestCase(0,1)]
    public void AdjacencyMethodsWithDefaultParametersWorkAsPerPrimaryMethod(int centreX, int centreY)
    {
      var centrePoint = new XYPoint(centreX, centreY);

      CollectionAssert.AreEqual(centrePoint.GetAllAdjacentPoints(), centrePoint.GetAdjacentPoints());
      CollectionAssert.AreEqual(centrePoint.GetAllAdjacentPoints(), centrePoint.GetAdjacentPoints(eDirection.North, eDirectionChange.Clockwise, adjacencyType: XYPoint.eAdjacencyType.All));
      CollectionAssert.AreEqual(centrePoint.GetAllAdjacentPoints(eDirection.South), centrePoint.GetAdjacentPoints(eDirection.South));
      CollectionAssert.AreEqual(centrePoint.GetAllAdjacentPoints(eDirection.South), centrePoint.GetAdjacentPoints(eDirection.South, eDirectionChange.Clockwise, XYPoint.eAdjacencyType.All));
      CollectionAssert.AreEqual(centrePoint.GetAllAdjacentPoints(eDirection.West, eDirectionChange.AntiClockwise), centrePoint.GetAdjacentPoints(eDirection.West, eDirectionChange.AntiClockwise));
      CollectionAssert.AreEqual(centrePoint.GetAllAdjacentPoints(eDirection.West, eDirectionChange.AntiClockwise), centrePoint.GetAdjacentPoints(eDirection.West, eDirectionChange.AntiClockwise, XYPoint.eAdjacencyType.All));

      CollectionAssert.AreEqual(centrePoint.GetEdgeAdjacentPoints(), centrePoint.GetAdjacentPoints(adjacencyType: XYPoint.eAdjacencyType.Orthogonal));
      CollectionAssert.AreEqual(centrePoint.GetEdgeAdjacentPoints(), centrePoint.GetAdjacentPoints(eDirection.North, eDirectionChange.Clockwise, XYPoint.eAdjacencyType.Orthogonal));
      CollectionAssert.AreEqual(centrePoint.GetEdgeAdjacentPoints(eDirection.South), centrePoint.GetAdjacentPoints(eDirection.South, eDirectionChange.Clockwise, XYPoint.eAdjacencyType.Orthogonal));
      CollectionAssert.AreEqual(centrePoint.GetEdgeAdjacentPoints(eDirection.West, eDirectionChange.AntiClockwise), centrePoint.GetAdjacentPoints(eDirection.West, eDirectionChange.AntiClockwise, XYPoint.eAdjacencyType.Orthogonal));
    }

  }
}

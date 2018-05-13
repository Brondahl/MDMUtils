using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using MDMUtils.IntGrid;
using NUnit.Framework;

namespace MDMUtilsTests.IntGrid
{
  [TestFixture]
  class MovementTests
  {
    [Test]
    public void EqualityComparisonsWork()
    {
      var baseMove = new Movement(0, 2);
      var matchingMove = new Movement(0, 2);
      var nonMatchingMove = new Movement(2, 0);
      var anotherNonMatchingMove = new Movement(0, 1);
      var anotherMatchingMove = (anotherNonMatchingMove * 2);

      Assert.IsFalse(ReferenceEquals(baseMove, matchingMove));

      Assert.IsTrue(baseMove.Equals(matchingMove));
      Assert.IsTrue(baseMove == matchingMove);
      Assert.IsFalse(baseMove != matchingMove);

      Assert.IsFalse(baseMove.Equals(nonMatchingMove));
      Assert.IsFalse(baseMove == nonMatchingMove);
      Assert.IsTrue(baseMove != nonMatchingMove);

      Assert.IsFalse(baseMove.Equals(anotherNonMatchingMove));
      Assert.IsFalse(baseMove == anotherNonMatchingMove);
      Assert.IsTrue(baseMove != anotherNonMatchingMove);

      Assert.IsTrue(baseMove.Equals(anotherMatchingMove));
      Assert.IsTrue(baseMove == anotherMatchingMove);
      Assert.IsFalse(baseMove != anotherMatchingMove);


      Assert.AreEqual(baseMove, matchingMove);
      Assert.AreNotEqual(baseMove, nonMatchingMove);
    }

    [TestCase( 0,1  , 3  ,  0,3 ),
     TestCase( 0,1  , -1 ,  0,-1),
     TestCase( 1,0  , 2  ,  2,0 ),
     TestCase( 3,2  , 4  , 12,8 ),
     TestCase(-1,2  , 1  , -1,2 ),
     TestCase( 2,-1 , -1 , -2,1 ),
     TestCase( 2,-1 , 0  ,  0,0 )]
    public void MovementMultiplicationWorksLikeBasicVectorMultiplication(int startX, int startY, int multiplier, int endX, int endY)
    {
      var originalMove = new Movement(startX, startY);
      var expectedFinalMove = new Movement(endX, endY);

      Assert.AreEqual(expectedFinalMove, originalMove * multiplier);
    }

    [TestCase(4,2   , 2 , 2,1  ),
     TestCase(5,-10 , 5 , 1,-2)]
    public void MovementDivisionWorksLikeBasicVectorDivisionWithNiceFractions(int startX, int startY, int divisor, int endX, int endY)
    {
      var originalMove = new Movement(startX, startY);
      var expectedFinalMove = new Movement(endX, endY);

      Assert.AreEqual(expectedFinalMove, originalMove / divisor);
    }

    [Test]
    public void MovementDivisionThrowsWithNastyFractions()
    {
      Assert.Throws<InvalidOperationException>(() => (new Movement(4, 2) / 3).ToString());
    }

    [TestCase( 0,0  ,  1,1  ,  1,1 ),
     TestCase( 2,2  ,  1,1  ,  3,3 ),
     TestCase( 1,3  ,  2,4  ,  3,7 ),
     TestCase( 1,0  ,  0,-2 ,  1,-2),
     TestCase(-3,-2 ,  0,0  , -3,-2),
     TestCase(-3,-1 , -2,-4 , -5,-5),
     TestCase( 0,0  ,  0,0  ,  0,0 )]
    public void ApplyingMovementsToXYPointsWorksLikeSimpleAddition(int startX, int startY, int moveX, int moveY, int endX, int endY)
    {
      var originalPoint = new XYPoint(startX, startY);
      var movement = new Movement(moveX, moveY);
      var expectedFinalPoint = new XYPoint(endX, endY);

      //ApplyTo method removed.
      //Assert.AreEqual(expectedFinalPoint, movement.ApplyTo(originalPoint));
      Assert.AreEqual(expectedFinalPoint, originalPoint.Move(movement));
      Assert.AreEqual(expectedFinalPoint, originalPoint + movement);
    }

    [TestCase(eDirection.North,  0,1),
     TestCase(eDirection.South,  0,-1),
     TestCase(eDirection.West , -1,0)]
    public void CanCastDirectionstoMovements(eDirection direction, int moveX, int moveY)
    {
      var expectedMove = new Movement(moveX, moveY);
      var actualMove = (Movement)direction;

      Assert.AreEqual(expectedMove, actualMove);
    }

    [TestCase(eDirection.North,1  ,  0,1 ),
     TestCase(eDirection.South,1  ,  0,-1),
     TestCase(eDirection.West ,1  , -1,0 ),
     TestCase(eDirection.North,3  ,  0,3 ),
     TestCase(eDirection.South,-2 ,  0,2 ),
     TestCase(eDirection.West ,0  ,  0,0 )]
    public void CanUseDirectionsToConstructMovements(eDirection direction, int multiplier, int endX, int endY)
    {
      var expectedMove = new Movement(endX, endY);
      var actualMove = new Movement(direction, multiplier);

      Assert.AreEqual(expectedMove, actualMove);
    }
  }
}

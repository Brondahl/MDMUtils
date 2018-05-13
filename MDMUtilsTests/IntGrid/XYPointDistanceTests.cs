
using System;
using MDMUtils.IntGrid;
using NUnit.Framework;

namespace MDMUtilsTests.IntGrid
{
  [TestFixture]
  class XYPointDistanceTests
  {
    //ncrunch: no coverage start
    public static int[][] SingleDimensionTests
    {
      get
      {
        return new []
        {
          new [] {0,0,  0,0,  0}, 
          new [] {0,1,  0,1,  0}, 
          new [] {2,0,  2,0,  0}, 
          new [] {5,7,  5,7,  0},
 
          new [] {0,0,  0,1,  1}, 
          new [] {0,0,  0,5,  5}, 
          new [] {0,2,  0,0,  2}, 
          new [] {3,0,  0,0,  3}, 
          new [] {0,0,  4,0,  4}, 

          new [] { 0,0,   0,0,   0}, 
          new [] { 0,0,   0,-1,  1}, 
          new [] { 0,0,   0,-5,  5}, 
          new [] { 0,-2,  0,0,   2}, 
          new [] {-3,0,   0,0,   3}, 
          new [] { 0,0,  -4,0,   4}, 

          new [] {1,4,  1,4,  0}, 
          new [] {1,4,  1,5,  1}, 
          new [] {1,4,  1,9,  5}, 
          new [] {1,6,  1,4,  2}, 
          new [] {4,4,  1,4,  3}, 
          new [] {1,4,  5,4,  4}, 

          new [] {-1,4,  -1,4,  0}, 
          new [] {-1,4,  -1,5,  1}, 
          new [] {-1,4,  -1,9,  5}, 
          new [] {-1,6,  -1,4,  2}, 
          new [] { 2,4,  -1,4,  3}, 
          new [] {-1,4,   3,4,  4}

        };
      }
    }

    public static int[][] TwoDimensionTestsTaxi
    {
      get
      {
        return new[]
        {
          new[] {0,0,  3,4,   7},
          new[] {0,0,  5,12,  17},
          new[] {0,0,  8,15,  23},

          new[] {-3,0,    0,4,  7},
          new[] { 0,5,   12,0,  17},
          new[] { 8,-15,  0,0,  23},

          new[] {-1,-3,   2,1,   7},
          new[] { 2,2,   14,-3,  17},
          new[] {10,-18,  2,-3,  23}

        };
      }
    }

    public static int[][] TwoDimensionTestsEuclideanIntegers
    {
      get
      {
        return new[]
        {
          new[] {0,0,  3,4,   5},
          new[] {0,0,  5,12,  13},
          new[] {0,0,  8,15,  17},

          new[] {-3,0,    0,4,  5},
          new[] { 0,5,   12,0,  13},
          new[] { 8,-15,  0,0,  17},

          new[] {-1,-3,   2,1,   5},
          new[] { 2,2,   14,-3,  13},
          new[] {10,-18,  2,-3,  17}

        };
      }
    }

    
    public static double[][] TwoDimensionTestsEuclideanDecimals
    {
      get
      {
        return new double[][]
        {
          new double[] {0,0,   4,16,  Math.Sqrt(272)},
          new double[] {0,0,   3,-2,  Math.Sqrt(13)},
          new double[] {0,0,  -6,13,  Math.Sqrt(205)},
          new double[] {-5,-2, 0,0,   Math.Sqrt(29)},

          new double[] {-8,0,   0,3,  Math.Sqrt(73)},
          new double[] { 0,4,  15,0,  Math.Sqrt(241)},

          new double[] {-1,-4,   2,2,   Math.Sqrt(45)},
          new double[] { 2,2,   13,-3,  Math.Sqrt(146)},
          new double[] {10,-18,  2,-3,  Math.Sqrt(289)}

        };
      }
    }
//ncrunch: no coverage end

    [Test, TestCaseSource("SingleDimensionTests"), TestCaseSource("TwoDimensionTestsTaxi")]
    public void StaticTaxiDistanceWorks(int x1, int y1, int x2, int y2, int distance)
    {
      var p = new XYPoint(x1, y1);
      var q = new XYPoint(x2, y2);

      Assert.That(XYPoint.DistanceBetween(p, q), Is.EqualTo(distance));
      Assert.That(XYPoint.DistanceBetween(q, p), Is.EqualTo(distance));
    }

    [Test, TestCaseSource("SingleDimensionTests"), TestCaseSource("TwoDimensionTestsTaxi")]
    public void TaxiDistanceWorks(int x1, int y1, int x2, int y2, int distance)
    {
      var p = new XYPoint(x1, y1);
      var q = new XYPoint(x2, y2);

      Assert.That(p.DistanceFrom(q), Is.EqualTo(distance));
      Assert.That(q.DistanceFrom(p), Is.EqualTo(distance));
    }

    [Test, TestCaseSource("SingleDimensionTests"), TestCaseSource("TwoDimensionTestsEuclideanIntegers")]
    public void StaticEuclideanSingleDimensionDistanceWorks(int x1, int y1, int x2, int y2, int distance)
    {
      var p = new XYPoint(x1, y1);
      var q = new XYPoint(x2, y2);

      Assert.That((decimal)XYPoint.DirectDistanceBetween(p,q), Is.EqualTo((decimal)distance));
      Assert.That((decimal)XYPoint.DirectDistanceBetween(q,p), Is.EqualTo((decimal)distance));
    }

    [Test, TestCaseSource("SingleDimensionTests"), TestCaseSource("TwoDimensionTestsEuclideanIntegers")]
    public void EuclideanSingleDimensionDistanceWorks(int x1, int y1, int x2, int y2, int distance)
    {
      var p = new XYPoint(x1, y1);
      var q = new XYPoint(x2, y2);

      Assert.That(p.DirectDistanceFrom(q), Is.EqualTo(distance));
      Assert.That(q.DirectDistanceFrom(p), Is.EqualTo(distance));
    }

    [Test, TestCaseSource("TwoDimensionTestsEuclideanDecimals")]
    public void StaticEuclideanSingleDimensionDistanceWorksWithNonExactDistances(double x1, double y1, double x2, double y2, double distance)
    {
      var p = new XYPoint((int)x1, (int)y1);
      var q = new XYPoint((int)x2, (int)y2);

      Assert.That((decimal)XYPoint.DirectDistanceBetween(p,q), Is.EqualTo((decimal)distance));
      Assert.That((decimal)XYPoint.DirectDistanceBetween(q,p), Is.EqualTo((decimal)distance));
    }

    [Test, TestCaseSource("TwoDimensionTestsEuclideanDecimals")]
    public void EuclideanSingleDimensionDistanceWorksWithNonExactDistances(double x1, double y1, double x2, double y2, double distance)
    {
      var p = new XYPoint((int)x1, (int)y1);
      var q = new XYPoint((int)x2, (int)y2);

      Assert.That(p.DirectDistanceFrom(q), Is.EqualTo(distance));
      Assert.That(q.DirectDistanceFrom(p), Is.EqualTo(distance));
    }


  }
}

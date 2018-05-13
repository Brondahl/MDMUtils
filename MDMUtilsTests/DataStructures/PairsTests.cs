using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MDMUtils.DataStructures;
using MDMUtils.IntGrid;
using NUnit.Framework;

namespace MDMUtilsTests.DataStructures
{
  [TestFixture]
  internal class PairTests
  {
    public class TestPair<S, T> : Pair<S, T>
    {
      public TestPair() : base(){}
      public TestPair(S x, T y) : base(x, y){}
    }

    [Test]
    // Just has to not throw.
    public void CreatePair()
    {
      new TestPair<int, int>();
      new TestPair<long, int>();
    }

    [Test]
    // Just has to not throw.
    public void CreatePairWithValues()
    {
      new TestPair<int, int>(1, 2);
      new TestPair<string, int>("A", 2);
    }

    [Test]
    public void ReadValuesBackFromObject()
    {
      var createdObject = new TestPair<int, int>(1, 2);
      Assert.AreEqual(1, createdObject.X);
      Assert.AreEqual(2, createdObject.Y);

      var createdObject2 = new TestPair<string, int>("A", 3);
      Assert.AreEqual("A", createdObject2.X);
      Assert.AreEqual(3, createdObject2.Y);
    }

    [Test]
    public void ToStringOverrideWorksCorrectly()
    {
      var stringRepresentation = new TestPair<int, long>().ToString();
      Assert.AreEqual(stringRepresentation, "(0,0)");
    }

    [Test]
    public void ToStringOverrideWorksCorrectlyWithStrings()
    {
      var stringRepresentation = new TestPair<int, string>(0, "0").ToString();
      Assert.AreEqual(stringRepresentation, "(0,0)");

      var stringRepresentation2 = new TestPair<string, string>(@"(*,", @"(()").ToString();
      Assert.AreEqual(stringRepresentation2, "((*,,(())");
    }

    [Test]
    public void ToStringOverrideWorksCorrectlyWithNulls()
    {
      var stringRepresentation = new TestPair<int, string>().ToString();
      Assert.AreEqual(stringRepresentation, "(0,)");

      var stringRepresentation2 = new TestPair<int, XYPoint>(0, null).ToString();
        //right hand type needs to be reference-type and implement IEquatable.
      Assert.AreEqual(stringRepresentation2, "(0,)");
    }


  }

  [TestFixture]
  public class OrderedPairTests
  {
    public class TestOrderedPair<S, T> : OrderedEquatablePair<S, T> where S : IEquatable<S> where T : IEquatable<T>
    {
      public TestOrderedPair() : base(){}
      public TestOrderedPair(S x, T y) : base(x, y){}
    }

    public class TestSingleTypeOrderedPair<T> : OrderedEquatablePair<T> where T : IEquatable<T>
    {
      public TestSingleTypeOrderedPair() : base(){}
      public TestSingleTypeOrderedPair(T x, T y) : base(x, y){}
    }

    [Test]
    // Just has to not throw.
    public void CreateSingleTypePair()
    {
      new TestSingleTypeOrderedPair<string>();
      new TestSingleTypeOrderedPair<int>(1, 2);
    }

    [Test]
    public void DefaultPairHasDefaultValues()
    {
      var pair = new TestOrderedPair<int, string>();
      Assert.AreEqual(pair.X, 0);
      Assert.AreEqual(pair.Y, null);
    }

    [Test]
    public void EqualityTestsWork()
    {
      var pair = new TestOrderedPair<int, string>(1, "2");
      var matchingPair = new TestOrderedPair<int, string>(1, "2");
      var nonMatchingPair = new TestOrderedPair<int, string>(3, "4");
      var reversedPair = new TestOrderedPair<string, int>("2", 1);

      Assert.IsFalse(pair.Equals(null));
      Assert.IsTrue(pair.Equals(matchingPair));
      Assert.IsFalse(pair.Equals(nonMatchingPair));
      Assert.IsFalse(pair.Equals(reversedPair));

      var matchingTypePair = new TestOrderedPair<int, int>(1, 2);
      var matchingTypeReversedPair = new TestOrderedPair<int, int>(2, 1);

      Assert.IsFalse(matchingTypePair.Equals(matchingTypeReversedPair));
    }

    [Test]
    public void EqualityOperatorsRespectsEqualityMethod()
    {
      var pair = new TestOrderedPair<int, string>(1, "2");
      var matchingPair = new TestOrderedPair<int, string>(1, "2");
      var nonMatchingPair = new TestOrderedPair<int, string>(3, "4");
      var reversedPair = new TestOrderedPair<string, int>("2", 1);

      Assert.IsTrue(pair != null);
      Assert.IsTrue((TestOrderedPair<int, string>) null == null);
      Assert.IsTrue(pair == matchingPair);
      Assert.IsTrue(pair != nonMatchingPair);
      Assert.IsTrue(pair != reversedPair);

      Assert.IsFalse(pair == null);
      Assert.IsFalse((TestOrderedPair<int, string>) null != null);
      Assert.IsFalse(pair != matchingPair);
      Assert.IsFalse(pair == nonMatchingPair);
      Assert.IsFalse(pair == reversedPair);

      var matchingTypePair = new TestOrderedPair<int, int>(1, 2);
      var matchingTypeReversedPair = new TestOrderedPair<int, int>(2, 1);

      Assert.IsTrue(matchingTypePair != matchingTypeReversedPair);
      Assert.IsFalse(matchingTypePair == matchingTypeReversedPair);
    }

    [Test]
    // Just has to not throw.
    public void GetHashCodeRespectsEquality()
    {
      var pairHash = new TestOrderedPair<int, string>(1, "2").GetHashCode();
      var matchingPairHash = new TestOrderedPair<int, string>(1, "2").GetHashCode();
      Assert.IsTrue(pairHash.Equals(matchingPairHash));
      //Don't need to assert anything about the other cases, since inequality doesn't demand unequal hashes.

      var pairHash2 = new TestOrderedPair<int, int>(1, 2).GetHashCode();
      var matchingPairHash2 = new TestOrderedPair<int, int>(1, 2).GetHashCode();
      Assert.IsTrue(pairHash2.Equals(matchingPairHash2));
      //Don't need to assert anything about the other cases, since inequality doesn't demand unequal hashes.

      var pairHash3 = new TestOrderedPair<int, string>(14254, "2v5gws45£Q$T5tyw").GetHashCode();
      var matchingPairHash3 = new TestOrderedPair<int, string>(14254, "2v5gws45£Q$T5tyw").GetHashCode();
      Assert.IsTrue(pairHash3.Equals(matchingPairHash3));
      //Don't need to assert anything about the other cases, since inequality doesn't demand unequal hashes.
    }
  }

  [TestFixture]
  public class UnorderedPairTests
  {
    public class TestUnorderedPair<T> : UnorderedEquatablePair<T> where T : IEquatable<T>
    {
      public TestUnorderedPair() : base(){}
      public TestUnorderedPair(T x, T y) : base(x, y){}
    }

    [Test]
    public void EqualityTestsWork()
    {
      var pair = new TestUnorderedPair<int>(1, 2);
      var matchingPair = new TestUnorderedPair<int>(1, 2);
      var nonMatchingPair = new TestUnorderedPair<int>(3, 4);
      var differentTypeMatchingPair = new TestUnorderedPair<decimal>(1, 2);
      var reversedPair = new TestUnorderedPair<int>(2, 1);

      Assert.IsFalse(pair.Equals(null));
      Assert.IsTrue(pair.Equals(matchingPair));
      Assert.IsFalse(pair.Equals(nonMatchingPair));
      Assert.IsFalse(pair.Equals(differentTypeMatchingPair));

      Assert.IsTrue(pair.Equals(reversedPair));

      var defaultValuePair = new TestUnorderedPair<int>();
      var equivalentToDefaultValuePair = new TestUnorderedPair<int>(0, 0);

      Assert.IsTrue(defaultValuePair.Equals(equivalentToDefaultValuePair));
    }

    [Test]
    public void EqualityOperatorsRespectsEqualityMethod()
    {
      var pair = new TestUnorderedPair<int>(1, 2);
      var matchingPair = new TestUnorderedPair<int>(1, 2);
      var nonMatchingPair = new TestUnorderedPair<int>(3, 4);
      var differentTypeMatchingPair = new TestUnorderedPair<decimal>(1, 2);
      var reversedPair = new TestUnorderedPair<int>(2, 1);


      Assert.IsTrue(pair != null);
      Assert.IsTrue((TestUnorderedPair<int>) null == null);
      Assert.IsTrue(pair == matchingPair);
      Assert.IsTrue(pair != nonMatchingPair);
      Assert.IsTrue(pair != differentTypeMatchingPair);

      Assert.IsTrue(pair == reversedPair);


      Assert.IsFalse(pair == null);
      Assert.IsFalse((TestUnorderedPair<int>) null != null);
      Assert.IsFalse(pair != matchingPair);
      Assert.IsFalse(pair == nonMatchingPair);
      Assert.IsFalse(pair == differentTypeMatchingPair);

      Assert.IsFalse(pair != reversedPair);
    }

    [Test]
    // Just has to not throw.
    public void GetHashCodeRespectsEquality()
    {
      var pairHash = new TestUnorderedPair<int>(1, 2).GetHashCode();
      var matchingPairHash = new TestUnorderedPair<int>(1, 2).GetHashCode();
      var reversedPairHash = new TestUnorderedPair<int>(2, 1).GetHashCode();

      Assert.IsTrue(pairHash.Equals(matchingPairHash));
      Assert.IsTrue(pairHash.Equals(reversedPairHash));
      //Don't need to assert anything about the other cases, since inequality doesn't demand unequal hashes.

      var pairHash2 = new TestUnorderedPair<string>("asdkffd4T?$%Yw%LW", "2v5gws45£Q$T5tyw").GetHashCode();
      var matchingPairHash2 = new TestUnorderedPair<string>("asdkffd4T?$%Yw%LW", "2v5gws45£Q$T5tyw").GetHashCode();
      var reversedPairHash2 = new TestUnorderedPair<string>("2v5gws45£Q$T5tyw", "asdkffd4T?$%Yw%LW").GetHashCode();

      Assert.IsTrue(pairHash2.Equals(matchingPairHash2));
      Assert.IsTrue(pairHash2.Equals(reversedPairHash2));
      //Don't need to assert anything about the other cases, since inequality doesn't demand unequal hashes.
    }
  }

  [TestFixture]
  public class CartesianPointTests
  {
    [Test]
    public void ContructorTests()
    {
      new CartesianPoint();
      new CartesianPoint(1, 0);
    }

    [Test]
    [TestCaseSource("TestCases")]
    public void DistanceFromWorks(CartesianPoint p, CartesianPoint q, double distance)
    {
      Assert.AreEqual(distance, p.DistanceFrom(q));
      Assert.AreEqual(distance, q.DistanceFrom(p));
      Assert.AreEqual(distance, CartesianPoint.DistanceBetween(p,q));
      Assert.AreEqual(distance, CartesianPoint.DistanceBetween(q,p));
    }

    private static IEnumerable<object> TestCases
    {
      //ncrunch: no coverage start
      get
      {
        yield return new object[] {new CartesianPoint(), new CartesianPoint(), 0};
        yield return new object[] {new CartesianPoint(), new CartesianPoint(1,0), 1};
        yield return new object[] {new CartesianPoint(), new CartesianPoint(0,1), 1};
        yield return new object[] {new CartesianPoint(), new CartesianPoint(3,4), 5};
        yield return new object[] {new CartesianPoint(), new CartesianPoint(-3,4), 5};
        yield return new object[] {new CartesianPoint(3,4), new CartesianPoint(), 5};
        yield return new object[] {new CartesianPoint(3,-4), new CartesianPoint(0,0), 5};
        yield return new object[] {new CartesianPoint(1,-1), new CartesianPoint(-3,-4), 5};
        yield break;
      }
      //ncrunch: no coverage end
    }
  }
}
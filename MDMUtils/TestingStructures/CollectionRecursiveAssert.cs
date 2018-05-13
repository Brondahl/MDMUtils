using System;
using System.Collections;
using NUnit.Framework;

namespace MDMUtils.TestingStructures
{
  public class CollectionRecursiveAssert
  {
    public static void AreEqual(ICollection expectedCollection, ICollection actualCollection)
    {
      RecursiveComparisonOfNonTrivialCollections(expectedCollection, actualCollection, CollectionAssert.AreEqual);
    }

    public static void AreEquivalent(ICollection expectedCollection, ICollection actualCollection)
    {

      RecursiveComparisonOfNonTrivialCollections(expectedCollection, actualCollection, CollectionAssert.AreEquivalent);
    }

    private static void RecursiveComparisonOfNonTrivialCollections(ICollection expectedCollection, ICollection actualCollection, Action<ICollection,ICollection> baseAssertion)
    {
      bool degenerateEquality;
      if (AtLeastOneIsDegenerateCase(expectedCollection, actualCollection, out degenerateEquality))
      {
        if (degenerateEquality)
        {
          return;
        }
        Assert.Fail();
      }

      if(expectedCollection.Count != actualCollection.Count)
      { Assert.Fail("Collections were not of the same size: " + Environment.NewLine + expectedCollection + actualCollection); }

      //Set up the ability to iterate over the collection in a controlled manner.
      var expectedEnumeration = expectedCollection.GetEnumerator();
      var actualEnumeration = actualCollection.GetEnumerator();
      
      expectedEnumeration.MoveNext();
      actualEnumeration.MoveNext();

      var currentActual = actualEnumeration.Current;
      bool notAtEnd = true;

      // Test whether the next layer is a set of Collections.
      if (currentActual is ICollection)
      {
        // If they are Collections, iterate over them, recursing into each one.
        while (notAtEnd)
        {
          var currentExpected = expectedEnumeration.Current;
          currentActual = actualEnumeration.Current;

          RecursiveComparisonOfNonTrivialCollections((currentExpected as ICollection), (currentActual as ICollection), baseAssertion);

          expectedEnumeration.MoveNext();
          notAtEnd = actualEnumeration.MoveNext();
        }
      }
      else
      {
        // If they are not collections, then call the Collection Assertion on the current
        baseAssertion(expectedCollection, actualCollection);
        return;
      }

    }

    ///=============================================================================
    /// Test : TestForDegenerateCase
    /// 
    /// <summary>
    /// 	Establishes whether either one of the two collections is null or empty.
    ///   If so returns true, and populates the out param with an indicator of
    ///   whether BOTH collections display the same manner of degeneracy
    ///     (i.e. whether they display degenerate equality.)
    /// </summary>
    ///=============================================================================
    private static bool AtLeastOneIsDegenerateCase(ICollection expectedCollection, ICollection actualCollection, out bool haveDegenerateEquality)
    {
      haveDegenerateEquality = true;

      if (expectedCollection == null && actualCollection == null) { return true; }
      if (expectedCollection == null || actualCollection == null) { haveDegenerateEquality = false; return true; }
      if (expectedCollection.GetType() != actualCollection.GetType()) { haveDegenerateEquality = false;}
      //      if(expectedCollection.GetType().GetInterfaces().Any(inter => inter.IsGenericType && inter.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
      if (expectedCollection.Count == 0 && actualCollection.Count == 0) { return true; }
      if (expectedCollection.Count == 0 || actualCollection.Count == 0) { haveDegenerateEquality = false; return true; }
      return false;
    }

    /*  Unimplemented Methods on CollectionAssert
      public static void Contains(ICollection collection, object element);
      public static void Contains(ICollection collection, object element, string message);
      public static void Contains(ICollection collection, object element, string message, params object[] parameters);
      public static void DoesNotContain(ICollection collection, object element);
      public static void DoesNotContain(ICollection collection, object element, string message);
      public static void DoesNotContain(ICollection collection, object element, string message, params object[] parameters);
      public static void AllItemsAreNotNull(ICollection collection);
      public static void AllItemsAreNotNull(ICollection collection, string message);
      public static void AllItemsAreNotNull(ICollection collection, string message, params object[] parameters);
      public static void AllItemsAreUnique(ICollection collection);
      public static void AllItemsAreUnique(ICollection collection, string message);
      public static void AllItemsAreUnique(ICollection collection, string message, params object[] parameters);
      public static void IsSubsetOf(ICollection subset, ICollection superset);
      public static void IsSubsetOf(ICollection subset, ICollection superset, string message);
      public static void IsSubsetOf(ICollection subset, ICollection superset, string message, params object[] parameters);
      public static void IsNotSubsetOf(ICollection subset, ICollection superset);
      public static void IsNotSubsetOf(ICollection subset, ICollection superset, string message);
      public static void IsNotSubsetOf(ICollection subset, ICollection superset, string message, params object[] parameters);
      public static void AreEquivalent(ICollection expected, ICollection actual, string message);
      public static void AreEquivalent(ICollection expected, ICollection actual, string message, params object[] parameters);
      public static void AreNotEquivalent(ICollection expected, ICollection actual);
      public static void AreNotEquivalent(ICollection expected, ICollection actual, string message);
      public static void AreNotEquivalent(ICollection expected, ICollection actual, string message, params object[] parameters);
      public static void AllItemsAreInstancesOfType(ICollection collection, Type expectedType);
      public static void AllItemsAreInstancesOfType(ICollection collection, Type expectedType, string message);
      public static void AllItemsAreInstancesOfType(ICollection collection, Type expectedType, string message, params object[] parameters);
      public static void AreEqual(ICollection expected, ICollection actual);
      public static void AreEqual(ICollection expected, ICollection actual, string message);
      public static void AreEqual(ICollection expected, ICollection actual, string message, params object[] parameters);
      public static void AreNotEqual(ICollection notExpected, ICollection actual);
      public static void AreNotEqual(ICollection notExpected, ICollection actual, string message);
      public static void AreNotEqual(ICollection notExpected, ICollection actual, string message, params object[] parameters);
      public static void AreEqual(ICollection expected, ICollection actual, IComparer comparer);
      public static void AreEqual(ICollection expected, ICollection actual, IComparer comparer, string message);
      public static void AreEqual(ICollection expected, ICollection actual, IComparer comparer, string message, params object[] parameters);
      public static void AreNotEqual(ICollection notExpected, ICollection actual, IComparer comparer);
      public static void AreNotEqual(ICollection notExpected, ICollection actual, IComparer comparer, string message);
      public static void AreNotEqual(ICollection notExpected, ICollection actual, IComparer comparer, string message, params object[] parameters);
    */
  }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MDMUtils.TestingStructures;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace MDMUtilsTests
{
    [TestFixture]
    public class CollectionAssertTests
    {
        private int[] EmptyArray { get { return new int[0]; } }
        private List<int> EmptyList { get { return new List<int>(); } }

        private T[] Ar<T>(params T[] values) { return values; }
        private List<T> Li<T>(params T[] values) { return values.ToList(); }
        private T[] Array<T>(params T[] values) { return values; }
        private List<T> List<T>(params T[] values) { return values.ToList(); }

        [Test]
        public void AllDegenerateEqualitiesHold()
        {
            CollectionRecursiveAssert.AreEqual(null, null);
            CollectionRecursiveAssert.AreEqual(EmptyArray, EmptyArray);
            CollectionRecursiveAssert.AreEqual(EmptyList, EmptyList);
            CollectionRecursiveAssert.AreEqual(new int[0][], new int[0][]);
            CollectionRecursiveAssert.AreEqual(Array(EmptyArray, EmptyArray), Array(EmptyArray, EmptyArray));

            CollectionRecursiveAssert.AreEquivalent(null, null);
            CollectionRecursiveAssert.AreEquivalent(EmptyArray, EmptyArray);
            CollectionRecursiveAssert.AreEquivalent(EmptyList, EmptyList);
            CollectionRecursiveAssert.AreEquivalent(new int[0][], new int[0][]);
            CollectionRecursiveAssert.AreEquivalent(Array(EmptyArray, EmptyArray), Array(EmptyArray, EmptyArray));
        }

        [Test]
        public void AllDegenerateInequalitiesHold()
        {
            Assert.Throws<AssertionException>(() => CollectionRecursiveAssert.AreEqual(EmptyArray, null));
            Assert.Throws<AssertionException>(() => CollectionRecursiveAssert.AreEqual(null, EmptyList));

            Assert.Throws<AssertionException>(() => CollectionRecursiveAssert.AreEquivalent(EmptyArray, null));
            Assert.Throws<AssertionException>(() => CollectionRecursiveAssert.AreEquivalent(null, EmptyList));
        }

        [Test]
        public void SensibleDifferencesOfCollectionTypeAreIgnored()
        {
            CollectionAssert.AreEqual(List(1, 2, 3, 4), Array(1, 2, 3, 4));
            CollectionAssert.AreEquivalent(List(1, 2, 3, 4), Array(1, 2, 3, 4));

            CollectionAssert.AreEqual(EmptyList, EmptyArray);
            CollectionAssert.AreEquivalent(EmptyList, EmptyArray);

            CollectionRecursiveAssert.AreEqual(List(1, 2, 3, 4), Array(1, 2, 3, 4));
            CollectionRecursiveAssert.AreEquivalent(List(1, 2, 3, 4), Array(1, 2, 3, 4));

            CollectionRecursiveAssert.AreEqual(List(Ar(1), Ar(1), Ar(1), Ar(1)), Array(Li(1), Li(1), Li(1), Li(1)));
            CollectionRecursiveAssert.AreEquivalent(List(Ar(1), Ar(1), Ar(1), Ar(1)), Array(Li(1), Li(1), Li(1), Li(1)));

            CollectionRecursiveAssert.AreEqual(List(Ar(1), Ar(2), Ar(3), Ar(4)), Array(Li(1), Li(2), Li(3), Li(4)));
            CollectionRecursiveAssert.AreEquivalent(List(Ar(1), Ar(2), Ar(3), Ar(4)), Array(Li(1), Li(2), Li(3), Li(4)));

            CollectionRecursiveAssert.AreEqual(List(Ar(1, 2), Ar(3, 4)), Array(Li(1, 2), Li(3, 4)));
            CollectionRecursiveAssert.AreEquivalent(List(Ar(1, 2), Ar(3, 4)), Array(Li(1, 2), Li(3, 4)));

            //CollectionRecursiveAssert.AreEqual(EmptyList, EmptyArray);
            //CollectionRecursiveAssert.AreEquivalent(EmptyList, EmptyArray);

            CollectionRecursiveAssert.AreEqual(List(EmptyArray, EmptyArray), Array(EmptyArray, EmptyArray));
            CollectionRecursiveAssert.AreEquivalent(List(EmptyArray, EmptyArray), Array(EmptyArray, EmptyArray));

            //CollectionRecursiveAssert.AreEqual(List(EmptyArray, EmptyArray), Array<IEnumerable<int>>(EmptyArray, EmptyList));
            //CollectionRecursiveAssert.AreEquivalent(List(EmptyArray, EmptyArray), Array<IEnumerable<int>>(EmptyArray, EmptyList));

            CollectionRecursiveAssert.AreEqual(List(Ar(1), Ar(1)), Array<IEnumerable<int>>(Ar(1), Li(1)));
            CollectionRecursiveAssert.AreEquivalent(List(Ar(1), Ar(1)), Array<IEnumerable<int>>(Ar(1), Li(1)));

            CollectionRecursiveAssert.AreEqual(List<IEnumerable<int>>(Li(1), Ar(1)), Array<IEnumerable<int>>(Ar(1), Li(1)));
            CollectionRecursiveAssert.AreEquivalent(List<IEnumerable<int>>(Li(1), Ar(1)), Array<IEnumerable<int>>(Ar(1), Li(1)));

            //Assert.Inconclusive("Until Commented entries are uncommented");
        }

        [Test]
        public void ComplicatedDifferencesOfCollectionTypeAreNotIgnored()
        {
            Assert.Throws<AssertionException>(() => CollectionRecursiveAssert.AreEqual(EmptyArray, new int[0][]));
            Assert.Throws<AssertionException>(() => CollectionRecursiveAssert.AreEquivalent(EmptyArray, new int[0][]));

            Assert.Throws<AssertionException>(() => CollectionRecursiveAssert.AreEqual(EmptyArray, new[] { EmptyArray }));
            Assert.Throws<AssertionException>(() => CollectionRecursiveAssert.AreEquivalent(EmptyArray, new[] { EmptyArray }));

            Assert.Throws<AssertionException>(() => CollectionRecursiveAssert.AreEqual(EmptyArray, List(EmptyArray)));
            Assert.Throws<AssertionException>(() => CollectionRecursiveAssert.AreEquivalent(EmptyArray, List(EmptyArray)));

            Assert.Throws<AssertionException>(() => CollectionRecursiveAssert.AreEqual(Ar(List(EmptyArray)), Ar(EmptyArray)));
            Assert.Throws<AssertionException>(() => CollectionRecursiveAssert.AreEquivalent(Ar(List(EmptyArray)), Ar(EmptyArray)));

            Assert.Throws<AssertionException>(() => CollectionRecursiveAssert.AreEqual(EmptyArray, List(Li(1))));
            Assert.Throws<AssertionException>(() => CollectionRecursiveAssert.AreEquivalent(EmptyArray, List(Li(1))));

            Assert.Throws<AssertionException>(() => CollectionRecursiveAssert.AreEqual(Ar(List(EmptyArray)), Ar(List(1,2))));
            Assert.Throws<AssertionException>(() => CollectionRecursiveAssert.AreEquivalent(Ar(List(EmptyArray)), Ar(List(1, 2))));

            Assert.Throws<AssertionException>(() => CollectionRecursiveAssert.AreEqual(Array(List(Ar(1), Ar(2))), Array(Ar(1), Ar(2))));
            Assert.Throws<AssertionException>(() => CollectionRecursiveAssert.AreEquivalent(Array(List(Ar(1), Ar(2))), Array(Ar(1), Ar(2))));
        }

        [Test]
        public void InequalitiesBasedOnCountHold()
        {
            Assert.Throws<AssertionException>(() => CollectionRecursiveAssert.AreEqual(Array(EmptyArray, EmptyArray), Array(EmptyArray, EmptyArray, EmptyArray)));
            Assert.Throws<AssertionException>(() => CollectionRecursiveAssert.AreEquivalent(Array(EmptyArray, EmptyArray), Array(EmptyArray, EmptyArray, EmptyArray)));

            Assert.Throws<AssertionException>(() => CollectionRecursiveAssert.AreEqual(Array(Ar(1), Ar(1, 1)), Array(Ar(1), Ar(1, 1, 1))));
            Assert.Throws<AssertionException>(() => CollectionRecursiveAssert.AreEquivalent(Array(Ar(1), Ar(1, 1)), Array(Ar(1), Ar(1, 1, 1))));

            Assert.Throws<AssertionException>(() => CollectionRecursiveAssert.AreEqual(Array(Ar(1), Ar(2, 1)), Array(Ar(1), Ar(1, 2))));
            //Assert.Throws<AssertionException>(() => CollectionRecursiveAssert.AreEquivalent(Array(Ar(1), Ar(2, 1)), Array(Ar(1), Ar(1, 2))));

            Assert.Throws<AssertionException>(() => CollectionRecursiveAssert.AreEqual(Array(Ar(1), Ar(2, 1)), Array(Ar(1, 2), Ar(1))));
            Assert.Throws<AssertionException>(() => CollectionRecursiveAssert.AreEquivalent(Array(Ar(1), Ar(2, 1)), Array(Ar(1, 2), Ar(1))));

            Assert.Throws<AssertionException>(() => CollectionRecursiveAssert.AreEqual(Array(Ar(1), Ar(2, 1)), Array(Ar(1, 2), Ar(1))));
            Assert.Throws<AssertionException>(() => CollectionRecursiveAssert.AreEquivalent(Array(Ar(1), Ar(2, 1)), Array(Ar(1, 2), Ar(1))));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDMUtils.DataStructures.Graphs.Base;
using NUnit.Framework;

namespace MDMUtilsTests.DataStructures.Graphs
{
  public class TypedHelperTests
  {
    [TestFixture]
    public class TypedHelperCollectionTests
    {
      [Test]
      public void OutputIsOfExpectedType()
      {
        var arrayCollection = IDCNCFactory.NewArrayCollection<int>();
        IDirectedConnectedNodeCollection<int> deTypedArrayCollection = arrayCollection;
        var hopefullyArrayOutput = TypedHelpers.GetCollectionAsValidType<ArrayDCNC<int>, int>(deTypedArrayCollection);
        Assert.IsTrue(hopefullyArrayOutput is ArrayDCNC<int>);

        var pointerCollection = IDCNCFactory.NewPointerCollection<string>();
        IDirectedConnectedNodeCollection<string> deTypedPointerCollection = pointerCollection;
        var hopefullyPointerOutput = TypedHelpers.GetCollectionAsValidType<PointerDCNC<string>, string>(deTypedPointerCollection);
        Assert.IsTrue(hopefullyPointerOutput is PointerDCNC<string>);
      }

      [Test]
      public void NullInputReturnsNull()
      {
        var arrayOutput = TypedHelpers.GetCollectionAsValidType<ArrayDCNC<int>, int>(null);
        Assert.IsNull(arrayOutput);

        var pointerOutput = TypedHelpers.GetCollectionAsValidType<PointerDCNC<int>, int>(null);
        Assert.IsNull(pointerOutput);
      }

      [Test]
      public void InputOfWrongTypeThrows()
      {
        var arrayCollection = IDCNCFactory.NewArrayCollection<int>();
        IDirectedConnectedNodeCollection<int> deTypedArrayCollection = arrayCollection;
        Assert.Throws<InvalidCastException>(
          () => TypedHelpers.GetCollectionAsValidType<PointerDCNC<int>, int>(deTypedArrayCollection));

        var pointerCollection = IDCNCFactory.NewPointerCollection<string>();
        IDirectedConnectedNodeCollection<string> deTypedPointerCollection = pointerCollection;
        Assert.Throws<InvalidCastException>(
          () => TypedHelpers.GetCollectionAsValidType<ArrayDCNC<string>, string>(deTypedPointerCollection));
      }
    }

    [TestFixture]
    public class TypedHelperNodeTests
    {
      [Test]
      public void OutputIsOfExpectedType()
      {
        var arrayNode = IDCNCFactory.NewArrayCollection<int>().NewNode();
        IDirectedConnectedNode<int> deTypedArrayNode = arrayNode;
        var hopefullyArrayOutput = TypedHelpers.GetNodeAsValidType<ArrayDCN<int>, int>(deTypedArrayNode);
        Assert.IsTrue(hopefullyArrayOutput is ArrayDCN<int>);

        var pointerNode = IDCNCFactory.NewPointerCollection<DateTime>().NewNode();
        IDirectedConnectedNode<DateTime> deTypedPointerNode = pointerNode;
        var hopefullyPointerOutput = TypedHelpers.GetNodeAsValidType<PointerDCN<DateTime>, DateTime>(deTypedPointerNode);
        Assert.IsTrue(hopefullyPointerOutput is PointerDCN<DateTime>);
      }

      [Test]
      public void NullInputReturnsNull()
      {
        var arrayOutput = TypedHelpers.GetNodeAsValidType<ArrayDCN<int>, int>(null);
        Assert.IsNull(arrayOutput);

        var pointerOutput = TypedHelpers.GetNodeAsValidType<PointerDCN<bool>, bool>(null);
        Assert.IsNull(pointerOutput);
      }

      [Test]
      public void InputOfWrongTypeThrows()
      {
        var arrayNode = IDCNCFactory.NewArrayCollection<int>().NewNode();
        IDirectedConnectedNode<int> deTypedArrayNode = arrayNode;
        Assert.Throws<InvalidCastException>( () => TypedHelpers.GetNodeAsValidType<PointerDCN<int>, int>(deTypedArrayNode));

        var pointerNode = IDCNCFactory.NewPointerCollection<int>().NewNode();
        IDirectedConnectedNode<int> deTypedPointerNode = pointerNode;
        Assert.Throws<InvalidCastException>( () => TypedHelpers.GetNodeAsValidType<ArrayDCN<int>, int>(deTypedPointerNode));
      }
    }

  }
}

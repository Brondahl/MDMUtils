using System;
using MDMUtils.DataStructures.Graphs.Base;
using NUnit.Framework;

namespace MDMUtilsTests.DataStructures.Graphs
{
  public class IntArrayDCNCTest : DirectedConnectedNodeAndNodeCollectionTestBase<int>
  {
    [Test] public void Run() {RunTests();}

    internal override void InitialiseTypes()
    {
      CollectionType = typeof (ArrayDCNC<int>);
      NodeType = typeof (ArrayDCN<int>);
    }

    internal override void CreateNewBlankCollection()
    {
      BlankCollection = new ArrayDCNC<int>();
    }

    internal override void GetValuesForNewNode(out int input, out IDirectedConnectedNode<int> output)
    {
      input = 5;
      output = new ArrayDCN<int> {Value = 5};
    }
  }

  public class IntPointerDCNCTest : DirectedConnectedNodeAndNodeCollectionTestBase<int>
  {
    [Test] public void Run() {RunTests();}

    internal override void InitialiseTypes()
    {
      CollectionType = typeof (PointerDCNC<int>);
      NodeType = typeof (PointerDCN<int>);
    }

    internal override void CreateNewBlankCollection()
    {
      BlankCollection = new PointerDCNC<int>();
    }

    internal override void GetValuesForNewNode(out int input, out IDirectedConnectedNode<int> output)
    {
      input = 5;
      output = new ArrayDCN<int> {Value = 5};
    }
  }

  public class StringPointerDCNCTest : DirectedConnectedNodeAndNodeCollectionTestBase<string>
  {
    [Test] public void Run() {RunTests();}

    internal override void InitialiseTypes()
    {
      CollectionType = typeof (PointerDCNC<string>);
      NodeType = typeof (PointerDCN<string>);
    }

    internal override void CreateNewBlankCollection()
    {
      BlankCollection = new PointerDCNC<string>();
    }

    internal override void GetValuesForNewNode(out string input, out IDirectedConnectedNode<string> output)
    {
      input = "5";
      output = new ArrayDCN<string> {Value = "5"};
    }
  }

  public class DoublePointerDCNCTest : DirectedConnectedNodeAndNodeCollectionTestBase<Double>
  {
    [Test] public void Run() {RunTests();}

    internal override void InitialiseTypes()
    {
      CollectionType = typeof (PointerDCNC<Double>);
      NodeType = typeof (PointerDCN<Double>);
    }

    internal override void CreateNewBlankCollection()
    {
      BlankCollection = new PointerDCNC<Double>();
    }

    internal override void GetValuesForNewNode(out Double input, out IDirectedConnectedNode<Double> output)
    {
      input = 5.0;
      output = new ArrayDCN<Double> {Value = 5.0};
    }
  }

  public class DateTimePointerDCNCTest : DirectedConnectedNodeAndNodeCollectionTestBase<DateTime>
  {
    [Test] public void Run() {RunTests();}

    internal override void InitialiseTypes()
    {
      CollectionType = typeof (PointerDCNC<DateTime>);
      NodeType = typeof (PointerDCN<DateTime>);
    }

    internal override void CreateNewBlankCollection()
    {
      BlankCollection = new PointerDCNC<DateTime>();
    }

    internal override void GetValuesForNewNode(out DateTime input, out IDirectedConnectedNode<DateTime> output)
    {
      input = DateTime.Today.AddHours(5);
      output = new ArrayDCN<DateTime> {Value = DateTime.Today.AddHours(5)};
    }
  }

  public class CustomDataStructurePointerDCNCTest : DirectedConnectedNodeAndNodeCollectionTestBase<CustomDataStructure>
  {
    [Test] public void Run() {RunTests();}

    internal override void InitialiseTypes()
    {
      CollectionType = typeof (PointerDCNC<CustomDataStructure>);
      NodeType = typeof (PointerDCN<CustomDataStructure>);
    }

    internal override void CreateNewBlankCollection()
    {
      BlankCollection = new PointerDCNC<CustomDataStructure>();
    }

    internal override void GetValuesForNewNode(out CustomDataStructure input, out IDirectedConnectedNode<CustomDataStructure> output)
    {
      input = new CustomDataStructure {Mem1 = 5, Mem2 = "5", Mem3 = DateTime.Today.AddHours(5)};
      var outputStructure = new CustomDataStructure {Mem1 = 5, Mem2 = "5", Mem3 = DateTime.Today.AddHours(5)};
      output = new ArrayDCN<CustomDataStructure> {Value = outputStructure};
    }

    internal override bool NodeValuesAreEquivalent(CustomDataStructure expectedNodeValue,
                                                    CustomDataStructure actualNodeValue)
    {
      return (expectedNodeValue == null && actualNodeValue == null) 
           ||
             (expectedNodeValue.Mem1 == actualNodeValue.Mem1 &&
              expectedNodeValue.Mem2 == actualNodeValue.Mem2 &&
              expectedNodeValue.Mem3 == actualNodeValue.Mem3   );
    }
  }

  public class CustomDataStructure
  {
    public int Mem1;
    public string Mem2;
    public DateTime Mem3;
  }

  public class CustomObjectPointerDCNCTest : DirectedConnectedNodeAndNodeCollectionTestBase<CustomComplexObject>
  {
    [Test] public void Run() {RunTests();}

    internal override void InitialiseTypes()
    {
      CollectionType = typeof (PointerDCNC<CustomComplexObject>);
      NodeType = typeof (PointerDCN<CustomComplexObject>);
    }

    internal override void CreateNewBlankCollection()
    {
      BlankCollection = new PointerDCNC<CustomComplexObject>();
    }

    internal override void GetValuesForNewNode(out CustomComplexObject input, out IDirectedConnectedNode<CustomComplexObject> output)
    {
      input = new CustomComplexObject(5, "5");
      var outputStructure = new CustomComplexObject(5, "5");
      output = new ArrayDCN<CustomComplexObject> {Value = outputStructure};
    }

    internal override bool NodeValuesAreEquivalent(CustomComplexObject expectedNodeValue,
                                                   CustomComplexObject actualNodeValue)
    {
       return (expectedNodeValue == null && actualNodeValue == null) 
           ||
             (expectedNodeValue.Mem1 == actualNodeValue.Mem1 &&
              expectedNodeValue.Mem2 == actualNodeValue.Mem2 &&
              expectedNodeValue.Mem3 == actualNodeValue.Mem3   );
    }
  }

  public class CustomComplexObject
  {
    internal CustomComplexObject(int v1, string v2)
    {
      Mem1 = v1;
      Mem2 = v2;
      Mem3 = DateTime.Today;
    }

    public readonly int Mem1;
    public readonly string Mem2;
    public DateTime Mem3;
  }
}

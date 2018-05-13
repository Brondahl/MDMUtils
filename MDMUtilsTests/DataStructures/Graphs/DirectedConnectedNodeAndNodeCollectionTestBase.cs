using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MDMUtils;
using MDMUtils.DataStructures.Graphs.Base;
using NUnit.Framework;

namespace MDMUtilsTests.DataStructures.Graphs
{
  [AttributeUsage(AttributeTargets.Method)]
  public class IndirectTestAttribute : Attribute {}
  
  ///==========================================================================
  /// Class : DirectedConnectedNodeAndNodeCollectionTestBase
  ///
  /// <summary>
  ///   Class to test the functionality of the interface as implemented in a
  ///   variety of ways.
  /// </summary>
  /// <remarks>
  ///   Observe that it could be argued that these tests aren't reliable since
  ///   most tests rely on the sound functioning of the other methods on the
  ///   interface, and this reliance is severely cyclic.
  /// 
  ///   Whilst this is true, it merely means that these tests verify that the
  ///   interface returns CONSISTENT results - which ultimately is the only
  ///   thing we care about for an interface over data storage.
  /// </remarks>
  ///==========================================================================
  
  [TestFixture] 
  public abstract class DirectedConnectedNodeAndNodeCollectionTestBase<T>
  {
    #region Constructor Methods
    protected DirectedConnectedNodeAndNodeCollectionTestBase()
    {
      // ReSharper disable DoNotCallOverridableMethodsInConstructor
      Initialise();
      // ReSharper restore DoNotCallOverridableMethodsInConstructor
    }

    internal void Initialise()
    {
      InitialiseTypes();
      CreateNewBlankCollection();
      CreateNewSpareNode();
      CreateNewDisjointCollection();
      CreateNewPreFormedCollection();
    }
    #endregion

    #region Abstract Type Properties

    protected Type CollectionType { get; set; }
    protected Type NodeType { get; set; }
    internal abstract void InitialiseTypes();
    internal abstract void GetValuesForNewNode(out T input, out IDirectedConnectedNode<T> output);

    #endregion

    #region Run Tests
    //ncrunch: no coverage start 
    public void RunTests()
    {
      var methodExceptions = InvokeAllTestMethods();
      ReportOnTestExceptions(methodExceptions);
    }

    private IEnumerable<MethodInfo> IndirectTestMethods
    {
      get
      {
        return GetType().GetMethods().Where(meth => meth.IsDefined(typeof(IndirectTestAttribute), true));
      }
    }

    private Dictionary<string, Exception> InvokeAllTestMethods()
    {
      var methodExceptions = new Dictionary<string,Exception>();

      foreach (var testMethod in IndirectTestMethods)
      {
        try
        {
          Initialise();
          testMethod.Invoke(this, null);
        }
        catch (Exception e)
        {
          var inner = e.InnerException;
          if (!(inner is SuccessException))
          {
            methodExceptions.Add(testMethod.Name, inner);
          }
        }
      }

      return methodExceptions;
    }

    private void ReportOnTestExceptions(Dictionary<string, Exception> methodExceptions)
    {
      if (methodExceptions.Count == 0)
      {
        Assert.Pass("All {0} tests passed.", IndirectTestMethods.Count());
      }

      if (methodExceptions.Count == 1)
      {
        throw new Exception(methodExceptions.First().Key, methodExceptions.First().Value);
      }

      if (methodExceptions.Count > 1)
      {
        var exceptionLabels = methodExceptions.Select(exceptions => exceptions.Key + ":" + exceptions.Value.Message);
        var exceptionToThrow = new Exception(methodExceptions.Count + " Tests Failed: " + String.Join("; ", exceptionLabels));

        foreach (var method in methodExceptions.Keys.Reverse())
        {
          exceptionToThrow.Data[method] = methodExceptions[method];
        }

        throw exceptionToThrow;
      }
    }
    //ncrunch: no coverage end 
    #endregion

    #region Collections
    internal IDirectedConnectedNodeCollection<T> BlankCollection { get; set; }
    internal IDirectedConnectedNodeCollection<T> PreFormedCollection { get; set; }
    internal IDirectedConnectedNodeCollection<T> DisjointCollection { get; set; }
    internal IDirectedConnectedNode<T> SpareNode { get; set; }

    internal abstract void CreateNewBlankCollection();

    internal void CreateNewSpareNode()
    {
      SpareNode = BlankCollection.NewNode();
      SpareNode.Name = "Spare";
    }

    ///========================================================================
    /// Method : CreateNewDisjointCollection
    /// 
    /// <summary>
    /// 	Generates a Collection with 4 unconnected Nodes in it.
    /// </summary>
    ///========================================================================
    private void CreateNewDisjointCollection()
    {
      DisjointCollection = BlankCollection;
      CreateNewBlankCollection();

      for (int i = 0; i < 4; i++)
      {
        DisjointCollection.AddNode(SpareNode);
        CreateNewSpareNode();
      }

      DisjointNodeA = DisjointCollection.Nodes.ElementAt(0);
      DisjointNodeB = DisjointCollection.Nodes.ElementAt(1);
      DisjointNodeC = DisjointCollection.Nodes.ElementAt(2);
      DisjointNodeD = DisjointCollection.Nodes.ElementAt(3);

      DisjointNodeA.Name = "DisA";
      DisjointNodeB.Name = "DisB";
      DisjointNodeC.Name = "DisC";
      DisjointNodeD.Name = "DisD";
    }

    private IDirectedConnectedNode<T> DisjointNodeA { get; set; }
    private IDirectedConnectedNode<T> DisjointNodeB { get; set; }
    private IDirectedConnectedNode<T> DisjointNodeC { get; set; }
    private IDirectedConnectedNode<T> DisjointNodeD { get; set; }

    ///========================================================================
    /// Method : CreateNewPreFormedCollection
    /// 
    /// <summary>
    /// 	Generates a Collection with Connections.
    /// </summary>
    /// <remarks>
    ///   Produces the following collection:      (From ---+ To)
    /// 
    ///   A  +--+  B
    ///          /  
    ///   |     /  +
    ///   |    /   |
    ///   +   /    +
    ///      +      
    ///   D  +---  C
    /// 
    /// </remarks>
    ///========================================================================
    private void CreateNewPreFormedCollection()
    {
      PreFormedCollection = DisjointCollection;
      CreateNewDisjointCollection();

      PreFormedNodeA = PreFormedCollection.Nodes.ElementAt(0);
      PreFormedNodeB = PreFormedCollection.Nodes.ElementAt(1);
      PreFormedNodeC = PreFormedCollection.Nodes.ElementAt(2);
      PreFormedNodeD = PreFormedCollection.Nodes.ElementAt(3);

      PreFormedNodeA.Name = "PreA";
      PreFormedNodeB.Name = "PreB"; 
      PreFormedNodeC.Name = "PreC";
      PreFormedNodeD.Name = "PreD";

      PreFormedCollection.ConnectNodes(PreFormedNodeA, PreFormedNodeB, ConnectionDirection.Both);
      PreFormedCollection.ConnectNodes(PreFormedNodeB, PreFormedNodeC, ConnectionDirection.To);
      PreFormedCollection.ConnectNodes(PreFormedNodeB, PreFormedNodeC, ConnectionDirection.From);
      PreFormedCollection.ConnectNodes(PreFormedNodeD, PreFormedNodeA, ConnectionDirection.From);
      PreFormedCollection.ConnectNodes(PreFormedNodeB, PreFormedNodeD, ConnectionDirection.To);
      PreFormedCollection.ConnectNodes(PreFormedNodeC, PreFormedNodeD, ConnectionDirection.To);
    }

    private IDirectedConnectedNode<T> PreFormedNodeA { get; set; }
    private IDirectedConnectedNode<T> PreFormedNodeB { get; set; }
    private IDirectedConnectedNode<T> PreFormedNodeC { get; set; }
    private IDirectedConnectedNode<T> PreFormedNodeD { get; set; }

    ///   A  +--+  B
    ///          /  
    ///   |     /  +
    ///   |    /   |
    ///   +   /    +
    ///      +      
    ///   D  +---  C
    private IEnumerable<IDirectedConnectedNode<T>> GetExpectedConnectionsInPreFormedCollection(IDirectedConnectedNode<T> node, ConnectionDirection direction)
    {
      if(node == PreFormedNodeA)
      {
        switch (direction)
        {
          case ConnectionDirection.Any:
            return new List<IDirectedConnectedNode<T>> {PreFormedNodeB, PreFormedNodeD};
          case ConnectionDirection.To:
            return new List<IDirectedConnectedNode<T>> {PreFormedNodeB, PreFormedNodeD};
          case ConnectionDirection.From:
            return new List<IDirectedConnectedNode<T>> {PreFormedNodeB};
          case ConnectionDirection.Both:
            return new List<IDirectedConnectedNode<T>> {PreFormedNodeB};
          default:
            throw new NonExistentEnumCaseException<ConnectionDirection>();  //ncrunch: no coverage
        }
      }

      if(node == PreFormedNodeB)
      {
        switch (direction)
        {
          case ConnectionDirection.Any:
            return new List<IDirectedConnectedNode<T>> {PreFormedNodeA, PreFormedNodeC, PreFormedNodeD};
          case ConnectionDirection.To:
            return new List<IDirectedConnectedNode<T>> {PreFormedNodeA, PreFormedNodeC, PreFormedNodeD};
          case ConnectionDirection.From:
            return new List<IDirectedConnectedNode<T>> {PreFormedNodeA, PreFormedNodeC};
          case ConnectionDirection.Both:
            return new List<IDirectedConnectedNode<T>> {PreFormedNodeA, PreFormedNodeC,};
          default :
            throw new NonExistentEnumCaseException<ConnectionDirection>();  //ncrunch: no coverage
        }
      }

      if(node == PreFormedNodeC)
      {
        switch (direction)
        {
          case ConnectionDirection.Any:
            return new List<IDirectedConnectedNode<T>> {PreFormedNodeB, PreFormedNodeD};
          case ConnectionDirection.To:
            return new List<IDirectedConnectedNode<T>> {PreFormedNodeB, PreFormedNodeD};
          case ConnectionDirection.From:
            return new List<IDirectedConnectedNode<T>> {PreFormedNodeB};
          case ConnectionDirection.Both:
            return new List<IDirectedConnectedNode<T>> {PreFormedNodeB};
          default :
            throw new NonExistentEnumCaseException<ConnectionDirection>();  //ncrunch: no coverage
        }
      }

      if(node == PreFormedNodeD)
      {
        switch (direction)
        {
          case ConnectionDirection.Any:
            return new List<IDirectedConnectedNode<T>> {PreFormedNodeA, PreFormedNodeB, PreFormedNodeC};
          case ConnectionDirection.To:
            return new List<IDirectedConnectedNode<T>> ();
          case ConnectionDirection.From:
            return new List<IDirectedConnectedNode<T>> {PreFormedNodeA, PreFormedNodeB, PreFormedNodeC};
          case ConnectionDirection.Both:
            return new List<IDirectedConnectedNode<T>> ();
          default :
            throw new NonExistentEnumCaseException<ConnectionDirection>();  //ncrunch: no coverage
        }
      }

      throw new NonExistentEnumCaseException("There are only 4 possible Nodes, so it must have hit one of the above cases already.");  //ncrunch: no coverage
    }
    #endregion

    #region Core Test Methods
    #region Basic Methods
    [IndirectTest]
    public void IntialiserTest()
    {
      CollectionAssert.IsEmpty(BlankCollection.Nodes, "Initialiser attaches nodes.");
    }

    [IndirectTest]
    public void NewNodeTest()
    {
      var returnedNewNode = BlankCollection.NewNode();

      AssertNodeTypeIsCorrect(returnedNewNode);
      AssertNewNodeIsUnrelatedToBlankCollection(returnedNewNode);
      Assert.AreEqual(default(T), returnedNewNode.Value, "Node Value was not default.");
    }

    [IndirectTest]
    public void ParameterisedNewNodeTest()
    {
      T inputValue;
      IDirectedConnectedNode<T> expectedReturnedNode;
      GetValuesForNewNode(out inputValue, out expectedReturnedNode);

      var returnedNewNode = BlankCollection.NewNode(inputValue);

      AssertNodeTypeIsCorrect(returnedNewNode);
      AssertNewNodeIsUnrelatedToBlankCollection(returnedNewNode);
      Assert.IsTrue(NodesAreEquivalent(expectedReturnedNode,returnedNewNode),"Node was not of the expected value.");
    }

    #region AddNode*Test
    [IndirectTest]
    public void AddNodeByCollectionTest()
    {
      T nodeValueBeforeAdd = SpareNode.Value;
      BlankCollection.AddNode(SpareNode);
      T nodeValueAfterAdd = SpareNode.Value;

      Assert.IsTrue(NodeValuesAreEquivalent(nodeValueBeforeAdd, nodeValueAfterAdd));
      Assert.AreEqual(1, BlankCollection.Nodes.Count(),"Collection did not contain 1 node.");
      AssertNodeIsInCollectionButDisjoint(SpareNode, BlankCollection);
    }

    [IndirectTest]
    public void AddNodeByNodeTest()
    {
      T nodeValueBeforeAdd = SpareNode.Value;
      SpareNode.AddToCollection(BlankCollection);
      T nodeValueAfterAdd = SpareNode.Value;

      Assert.IsTrue(NodeValuesAreEquivalent(nodeValueBeforeAdd, nodeValueAfterAdd));
      Assert.AreEqual(1, BlankCollection.Nodes.Count(),"Collection did not contain 1 node.");
      AssertNodeIsInCollectionButDisjoint(SpareNode, BlankCollection);
    }
    #endregion
    #endregion

    #region Connection By Collection
    [IndirectTest]
    public void DirectedConnectToNodeByCollectionTest()
    {
      DisjointCollection.ConnectNodes(DisjointNodeA, DisjointNodeB, ConnectionDirection.To);
      AssertDirectedToConnectionOnly(DisjointNodeA, DisjointNodeB);
    }

    [IndirectTest]
    public void DirectedConnectFromNodeByCollectionTest()
    {
      DisjointCollection.ConnectNodes(DisjointNodeB, DisjointNodeA, ConnectionDirection.From);
      AssertDirectedToConnectionOnly(DisjointNodeA, DisjointNodeB);
    }

    [IndirectTest]
    public void UndirectedConnectNodeByCollectionTest()
    {
      DisjointCollection.ConnectNodes(DisjointNodeA, DisjointNodeB, ConnectionDirection.Both);
      AssertUndirectedConnection(DisjointNodeA, DisjointNodeB);
    }

    [IndirectTest]
    public void TwoPartUndirectedConnectNodeByCollectionChangeNodeTest()
    {
      DisjointCollection.ConnectNodes(DisjointNodeA, DisjointNodeB, ConnectionDirection.To);
      DisjointCollection.ConnectNodes(DisjointNodeB, DisjointNodeA, ConnectionDirection.To);
      AssertUndirectedConnection(DisjointNodeA, DisjointNodeB);
    }

    [IndirectTest]
    public void TwoPartUndirectedConnectNodeByCollectionChangeDirectionTest()
    {
      DisjointCollection.ConnectNodes(DisjointNodeA, DisjointNodeB, ConnectionDirection.To);
      DisjointCollection.ConnectNodes(DisjointNodeA, DisjointNodeB, ConnectionDirection.From);
      AssertUndirectedConnection(DisjointNodeA, DisjointNodeB);
    }
    #endregion

    #region Connection By Node
    [IndirectTest]
    public void DirectedConnectToNodeByNodeTest()
    {
      DisjointNodeA.AddConnectionWith(DisjointNodeB, ConnectionDirection.To);
      AssertDirectedToConnectionOnly(DisjointNodeA, DisjointNodeB);
    }

    [IndirectTest]
    public void DirectedConnectFromNodeByNodeTest()
    {
      DisjointNodeA.AddConnectionWith(DisjointNodeB, ConnectionDirection.From);
      AssertDirectedToConnectionOnly(DisjointNodeB, DisjointNodeA);
    }

    [IndirectTest]
    public void UndirectedConnectNodeByNodeTest()
    {
      DisjointNodeA.AddConnectionWith(DisjointNodeB, ConnectionDirection.Both);
      AssertUndirectedConnection(DisjointNodeA, DisjointNodeB);
    }

    [IndirectTest]
    public void TwoPartUndirectedConnectNodeByNodeChangeNodeTest()
    {
      DisjointNodeA.AddConnectionWith(DisjointNodeB, ConnectionDirection.To);
      DisjointNodeB.AddConnectionWith(DisjointNodeA, ConnectionDirection.To);
      AssertUndirectedConnection(DisjointNodeA, DisjointNodeB);
    }

    [IndirectTest]
    public void TwoPartUndirectedConnectNodeByNodeChangeDirectionTest()
    {
      DisjointNodeA.AddConnectionWith(DisjointNodeB, ConnectionDirection.To);
      DisjointNodeA.AddConnectionWith(DisjointNodeB, ConnectionDirection.From);
      AssertUndirectedConnection(DisjointNodeA, DisjointNodeB);
    }
    #endregion

    #region Mass connection By Node
    [IndirectTest]
    public void MassUndirectedConnectNodeByNodeTest()
    {
      DisjointNodeA.AddConnectionWith(new[]{DisjointNodeB,DisjointNodeC}, ConnectionDirection.Both);

      AssertUndirectedConnection(DisjointNodeA, DisjointNodeB);
      AssertUndirectedConnection(DisjointNodeA, DisjointNodeC);
      AssertUnconnected(DisjointNodeB, DisjointNodeC);
    }

    [IndirectTest]
    public void TwoPartMassUndirectedConnectNodeByNodeChangeNodeTest()
    {
      DisjointNodeA.AddConnectionWith(new[]{DisjointNodeB,DisjointNodeC}, ConnectionDirection.To);
      DisjointNodeB.AddConnectionWith(new[]{DisjointNodeA}, ConnectionDirection.To);
      DisjointNodeC.AddConnectionWith(new[]{DisjointNodeA}, ConnectionDirection.To);

      AssertUndirectedConnection(DisjointNodeA, DisjointNodeB);
      AssertUndirectedConnection(DisjointNodeA, DisjointNodeC);
      AssertUnconnected(DisjointNodeB, DisjointNodeC);
    }

    [IndirectTest]
    public void TwoPartMassUndirectedConnectNodeByNodeChangeNodeNotInListTest()
    {
      DisjointNodeA.AddConnectionWith(new[]{DisjointNodeB,DisjointNodeC}, ConnectionDirection.To);
      DisjointNodeB.AddConnectionWith(DisjointNodeA, ConnectionDirection.To);
      DisjointNodeC.AddConnectionWith(DisjointNodeA, ConnectionDirection.To);

      AssertUndirectedConnection(DisjointNodeA, DisjointNodeB);
      AssertUndirectedConnection(DisjointNodeA, DisjointNodeC);
      AssertUnconnected(DisjointNodeB, DisjointNodeC);
    }

    [IndirectTest]
    public void TwoPartMassUndirectedConnectNodeByNodeChangeDirectionTest()
    {
      DisjointNodeA.AddConnectionWith(new[]{DisjointNodeB,DisjointNodeC}, ConnectionDirection.To);
      DisjointNodeA.AddConnectionWith(new[]{DisjointNodeB,DisjointNodeC}, ConnectionDirection.From);

      AssertUndirectedConnection(DisjointNodeA, DisjointNodeB);
      AssertUndirectedConnection(DisjointNodeA, DisjointNodeC);
      AssertUnconnected(DisjointNodeB, DisjointNodeC);
    }

    [IndirectTest]
    public void TwoPartPartialMassUndirectedConnectNodeByNodeTest()
    {
      DisjointNodeA.AddConnectionWith(new[]{DisjointNodeB,DisjointNodeC}, ConnectionDirection.To);
      DisjointNodeA.AddConnectionWith(DisjointNodeB, ConnectionDirection.From);
      DisjointNodeA.AddConnectionWith(DisjointNodeC, ConnectionDirection.From);

      AssertUndirectedConnection(DisjointNodeA, DisjointNodeB);
      AssertUndirectedConnection(DisjointNodeA, DisjointNodeC);
      AssertUnconnected(DisjointNodeB, DisjointNodeC);
    }

    [IndirectTest]
    public void ExplicitMassDirectedConnectToNodeByNodeTest()
    {
      DisjointNodeA.AddConnectionWith(new[]{DisjointNodeB,DisjointNodeC}, ConnectionDirection.To);

      AssertDirectedToConnectionOnly(DisjointNodeA, DisjointNodeB);
      AssertDirectedToConnectionOnly(DisjointNodeA, DisjointNodeC);
      AssertUnconnected(DisjointNodeB, DisjointNodeC);
    }

    [IndirectTest]
    public void ExplicitDirectedMassConnectFromNodeByNodeTest()
    {
      DisjointNodeA.AddConnectionWith(new[]{DisjointNodeB,DisjointNodeC}, ConnectionDirection.From);

      AssertDirectedToConnectionOnly(DisjointNodeB, DisjointNodeA);
      AssertDirectedToConnectionOnly(DisjointNodeC, DisjointNodeA);
      AssertUnconnected(DisjointNodeB, DisjointNodeC);
    }
    #endregion

    #region Connection Listing
    [IndirectTest]
    public void CollectionListNodesTest()
    {
      foreach (var node in PreFormedCollection.Nodes)
      {
        foreach (var direction in EnumAccessor.ConnectionDirections)
        {
          var actualNodes = PreFormedCollection.GetNodesConnected(node, direction);
          var expectedNodes = GetExpectedConnectionsInPreFormedCollection(node, direction);
          CollectionAssert.AreEquivalent(expectedNodes, actualNodes);
        }
      }
    }

    [IndirectTest]
    public void NodeListNodesTest()
    {
      foreach (var node in PreFormedCollection.Nodes)
      {
        foreach (var direction in EnumAccessor.ConnectionDirections)
        {
          var actualNodes = node.GetNodesConnected(direction);
          var expectedNodes = GetExpectedConnectionsInPreFormedCollection(node, direction);
          CollectionAssert.AreEquivalent(expectedNodes, actualNodes);
        }
      }
    }
    #endregion

    #region Connection Detection
    [IndirectTest]
    public void DetectNodesByCollectionTest()
    {
      AssertPreFormedCollectionHasExpectedConnectionsByCollectionExceptForGivenNodeRemoved(null);
    }

    [IndirectTest]
    public void DetectNodesByNodeTest()
    {
      AssertPreFormedCollectionHasExpectedConnectionsByNodeExceptForGivenNodeRemoved(null);
    }
    #endregion

    #region Removal Methods
    #region RemoveDisjointNode*Test
    [IndirectTest]
    public void RemoveDisjointNodeByCollectionTest()
    {
      var nodeToBeRemoved = DisjointNodeA;
      int expectedNodeCount = DisjointCollection.Nodes.Count() - 1;

      T nodeValueBeforeRemoval = nodeToBeRemoved.Value;
      DisjointCollection.RemoveNode(nodeToBeRemoved);
      T nodeValueAfterRemoval = nodeToBeRemoved.Value;

      Assert.IsTrue(NodeValuesAreEquivalent(nodeValueBeforeRemoval, nodeValueAfterRemoval));
      Assert.AreEqual(expectedNodeCount, DisjointCollection.Nodes.Count(),"Collection did not contain 1 fewer nodes than before the remove.");
      AssertNodeIsUnrelatedToCollection(nodeToBeRemoved, DisjointCollection);
    }

    [IndirectTest]
    public void RemoveDisjointNodeByNodeTest()
    {
      var nodeToBeRemoved = DisjointNodeA;
      int expectedNodeCount = DisjointCollection.Nodes.Count() - 1;

      T nodeValueBeforeRemoval = nodeToBeRemoved.Value;
      nodeToBeRemoved.RemoveFromExistingCollection();
      T nodeValueAfterRemoval = nodeToBeRemoved.Value;

      Assert.IsTrue(NodeValuesAreEquivalent(nodeValueBeforeRemoval, nodeValueAfterRemoval));
      Assert.AreEqual(expectedNodeCount, DisjointCollection.Nodes.Count(),"Collection did not contain 1 fewer nodes than before the remove.");
      AssertNodeIsUnrelatedToCollection(nodeToBeRemoved, DisjointCollection);
    }
    #endregion

    #region RemoveConnectedNode*Test
    [IndirectTest]
    public void RemoveConnectedNodeByCollectionTest()
    {
      var nodeToBeRemoved = PreFormedNodeA;
      int expectedNodeCount = PreFormedCollection.Nodes.Count() - 1;

      T nodeValueBeforeRemoval = nodeToBeRemoved.Value;
      PreFormedCollection.RemoveNode(nodeToBeRemoved);
      T nodeValueAFterRemoval = nodeToBeRemoved.Value;

      Assert.IsTrue(NodeValuesAreEquivalent(nodeValueBeforeRemoval, nodeValueAFterRemoval));
      Assert.AreEqual(expectedNodeCount, PreFormedCollection.Nodes.Count(),"Collection did not contain 1 fewer nodes than before the remove.");
      AssertNodeIsUnrelatedToCollection(nodeToBeRemoved,PreFormedCollection);
      AssertPreFormedCollectionHasExpectedConnectionsByCollectionExceptForGivenNodeRemoved(nodeToBeRemoved);
    }

    [IndirectTest]
    public void RemoveConnectedNodeByNodeTest()
    {
      var nodeToBeRemoved = PreFormedNodeA;
      int expectedNodeCount = PreFormedCollection.Nodes.Count() - 1;

      T nodeValueBeforeRemoval = nodeToBeRemoved.Value;
      nodeToBeRemoved.RemoveFromExistingCollection();
      T nodeValueAFterRemoval = nodeToBeRemoved.Value;

      Assert.IsTrue(NodeValuesAreEquivalent(nodeValueBeforeRemoval, nodeValueAFterRemoval));
      Assert.AreEqual(expectedNodeCount, PreFormedCollection.Nodes.Count(),"Collection did not contain 1 fewer nodes than before the remove.");
      AssertNodeIsUnrelatedToCollection(nodeToBeRemoved,PreFormedCollection);
      AssertPreFormedCollectionHasExpectedConnectionsByNodeExceptForGivenNodeRemoved(nodeToBeRemoved);
    }
    #endregion

    #region MassRemoveConnectionsByNodeTest
    ///   A  +--+  B
    ///          /  
    ///   |     /  +
    ///   |    /   |
    ///   +   /    +
    ///      +      
    ///   D  +---  C
    [IndirectTest]
    public void MassRemoveToConnectionsByNodeTest()
    {
      PreFormedNodeB.RemoveConnectionWith(new[]{PreFormedNodeA,PreFormedNodeD}, ConnectionDirection.To);

      AssertDirectedToConnectionOnly(PreFormedNodeA,PreFormedNodeB);
      AssertUndirectedConnection(PreFormedNodeC, PreFormedNodeB);
      AssertUnconnected(PreFormedNodeB, PreFormedNodeD);
    }

    [IndirectTest]
    public void MassRemoveFromConnectionsByNodeTest()
    {
      PreFormedNodeB.RemoveConnectionWith(new[]{PreFormedNodeA,PreFormedNodeD}, ConnectionDirection.From);

      AssertDirectedToConnectionOnly(PreFormedNodeB,PreFormedNodeA);
      AssertUndirectedConnection(PreFormedNodeC, PreFormedNodeB);
      AssertDirectedToConnectionOnly(PreFormedNodeB, PreFormedNodeD);
    }

    [IndirectTest]
    public void MassRemoveAllConnectionsByNodeBothTest()
    {
      PreFormedNodeB.RemoveConnectionWith(new[]{PreFormedNodeA,PreFormedNodeC,PreFormedNodeD}, ConnectionDirection.Both);

      CollectionAssert.IsEmpty(PreFormedNodeB.GetNodesConnected(ConnectionDirection.Any));
      AssertPreFormedCollectionHasExpectedConnectionsByNodeExceptForGivenNodeRemoved(PreFormedNodeB);
    }

    [IndirectTest]
    public void MassRemoveAllConnectionsByNodeAnyTest()
    {
      PreFormedNodeB.RemoveConnectionWith(new[]{PreFormedNodeA,PreFormedNodeC,PreFormedNodeD}, ConnectionDirection.Any);

      CollectionAssert.IsEmpty(PreFormedNodeB.GetNodesConnected(ConnectionDirection.Any));
      AssertPreFormedCollectionHasExpectedConnectionsByNodeExceptForGivenNodeRemoved(PreFormedNodeB);
    }

    [IndirectTest]
    public void MassRemoveAllConnectionsByNodeTwoPartTest()
    {
      PreFormedNodeB.RemoveConnectionWith(new[]{PreFormedNodeA,PreFormedNodeC,PreFormedNodeD}, ConnectionDirection.To);
      PreFormedNodeB.RemoveConnectionWith(new[]{PreFormedNodeA,PreFormedNodeC}, ConnectionDirection.From);

      CollectionAssert.IsEmpty(PreFormedNodeB.GetNodesConnected(ConnectionDirection.Any));
      AssertPreFormedCollectionHasExpectedConnectionsByNodeExceptForGivenNodeRemoved(PreFormedNodeB);
    }
    #endregion
    #endregion
    #endregion

    #region Additional Test Methods - Incomplete
    #region Multiple Connections and Self Connections
    //==============================================================
    // <summary>
    // When extending to cover multiple connections
    // If remove from repeated connections ???? Probably remove all, and this is still to do. (Also, Weighted connections?)
    // </summary>
    //==============================================================
    #region Self Connections Don't Already occur.
    [IndirectTest]
    public void NewNodeNotSelfConnectedTest()
    {
      AssertUnconnected(SpareNode, SpareNode);
    }

    [IndirectTest]
    public void NewlyAddedNodeNotSelfConnectedTest()
    {
      AssertUnconnected(DisjointNodeA, DisjointNodeA);
    }

    [IndirectTest]
    public void MultipleConnectionsNodeNotSelfConnectedTest()
    {
      AssertUnconnected(PreFormedNodeB, PreFormedNodeB);
    }
    #endregion

    #region Attempted SelfConnections Fail
    [IndirectTest]
    public void AttemptedSelfConnectByNodeFailsTest()
    { Assert.Throws<InvalidOperationException>(() => DisjointNodeA.AddConnectionWith(DisjointNodeA, ConnectionDirection.To), "Self Connect should have thrown an Exception."); }
    
    [IndirectTest]
    public void AttemptedSelfConnectByCollectionFailsTest()
    { Assert.Throws<InvalidOperationException>(() => DisjointCollection.ConnectNodes(DisjointNodeA, DisjointNodeA, ConnectionDirection.To), "Self Connect should have thrown an Exception."); }
    #endregion

    #region Repeated Connections have no affect
    #region Repeated Identical Connections
    [IndirectTest]
    public void RepeatedConnectNodeByCollectionTest()
    {
      foreach (var direction in EnumAccessor.DefiniteConnectionDirections)
      {
        CreateNewDisjointCollection();

        DisjointCollection.ConnectNodes(DisjointNodeA, DisjointNodeB, direction);
        DisjointCollection.ConnectNodes(DisjointNodeA, DisjointNodeB, direction);

        AssertSingleSpecificConnection(DisjointNodeA, DisjointNodeB, direction);
      }
    }

    [IndirectTest]
    public void RepeatedConnectNodeByNodeTest()
    {
      foreach (var direction in EnumAccessor.DefiniteConnectionDirections)
      {
        CreateNewDisjointCollection();

        DisjointNodeA.AddConnectionWith(DisjointNodeB, direction);
        DisjointNodeA.AddConnectionWith(DisjointNodeB, direction);

        AssertSingleSpecificConnection(DisjointNodeA, DisjointNodeB, direction);
      }
    }
    #endregion

    #region Repeated Equivalents Connections
    [IndirectTest]
    public void MixedRepeatedConnectNodeTest1()
    {
      foreach (var direction in EnumAccessor.DefiniteConnectionDirections)
      {
        CreateNewDisjointCollection();

        DisjointCollection.ConnectNodes(DisjointNodeA, DisjointNodeB, direction);
        DisjointNodeA.AddConnectionWith(DisjointNodeB, direction);

        AssertSingleSpecificConnection(DisjointNodeA, DisjointNodeB, direction);
      }
    }

    [IndirectTest]
    public void MixedRepeatedConnectNodeTest2()
    {
      foreach (var direction in EnumAccessor.DefiniteConnectionDirections)
      {
        CreateNewDisjointCollection();

        DisjointNodeA.AddConnectionWith(DisjointNodeB, direction);
        DisjointCollection.ConnectNodes(DisjointNodeA, DisjointNodeB, direction);

        AssertSingleSpecificConnection(DisjointNodeA, DisjointNodeB, direction);
      }
    }

    [IndirectTest]
    public void MixedRepeatedConnectNodeTest3()
    {
      foreach (var direction in EnumAccessor.DefiniteConnectionDirections)
      {
        CreateNewDisjointCollection();

        DisjointNodeA.AddConnectionWith(DisjointNodeB, direction);
        DisjointNodeB.AddConnectionWith(DisjointNodeA, direction.Invert());

        AssertSingleSpecificConnection(DisjointNodeA, DisjointNodeB, direction);
      }
    }

    [IndirectTest]
    public void MixedRepeatedConnectNodeTest4()
    {
      foreach (var direction in EnumAccessor.DefiniteConnectionDirections)
      {
        CreateNewDisjointCollection();

        DisjointCollection.ConnectNodes(DisjointNodeA, DisjointNodeB, direction);
        DisjointCollection.ConnectNodes(DisjointNodeB, DisjointNodeA, direction.Invert());

        AssertSingleSpecificConnection(DisjointNodeA, DisjointNodeB, direction);
      }
    }

    [IndirectTest]
    public void MixedRepeatedConnectNodeTest5()
    {
      foreach (var direction in EnumAccessor.DefiniteConnectionDirections)
      {
        CreateNewDisjointCollection();

        DisjointCollection.ConnectNodes(DisjointNodeA, DisjointNodeB, direction);
        DisjointNodeB.AddConnectionWith(DisjointNodeA, direction.Invert());

        AssertSingleSpecificConnection(DisjointNodeA, DisjointNodeB, direction);
      }
    }

    [IndirectTest]
    public void MixedRepeatedConnectNodeTest6()
    {
      foreach (var direction in EnumAccessor.DefiniteConnectionDirections)
      {
        CreateNewDisjointCollection();

        DisjointNodeA.AddConnectionWith(DisjointNodeB, direction);
        DisjointCollection.ConnectNodes(DisjointNodeB, DisjointNodeA, direction.Invert());

        AssertSingleSpecificConnection(DisjointNodeA, DisjointNodeB, direction);
      }
    }
    #endregion

    #region Repeated Mass Connections
    [IndirectTest]
    public void RepeatedMassDirectedConnectNodeTest()
    {
      foreach (var direction in EnumAccessor.DefiniteConnectionDirections)
      {
        CreateNewDisjointCollection();

        DisjointNodeA.AddConnectionWith(new[]{DisjointNodeB,DisjointNodeC}, direction);
        DisjointNodeA.AddConnectionWith(new[]{DisjointNodeB,DisjointNodeC}, direction);

        AssertSingleSpecificConnection(DisjointNodeA, DisjointNodeB, direction);
        AssertSingleSpecificConnection(DisjointNodeA, DisjointNodeC, direction);
        AssertUnconnected(DisjointNodeB, DisjointNodeC);
      }
    }
    #endregion
    #endregion
    #endregion

    #region Error Cases, Null Operations and Null Arguments
    [IndirectTest]
    public void AddOrRemoveAnyConnectionFailsTest()
    {
      Assert.Throws<InvalidOperationException>(() => DisjointCollection.ConnectNodes(DisjointNodeA, DisjointNodeB, ConnectionDirection.Any), "Collection should NOT allow creation of \"Any\" Connection");
      Assert.Throws<InvalidOperationException>(() => DisjointNodeA.AddConnectionWith(DisjointNodeB, ConnectionDirection.Any), "Node should NOT allow creation of \"Any\" Connection");
    }

    [IndirectTest]
    public void AddToSecondCollectionFailsTest()
    {
      Assert.Throws<InvalidOperationException>(() => DisjointNodeA.AddToCollection(PreFormedCollection));
      Assert.AreEqual(DisjointCollection,DisjointNodeA.ParentCollection);
      Assert.AreNotEqual(PreFormedCollection,DisjointNodeA.ParentCollection);
    }

    [IndirectTest]
    public void ManipulateNodesNotInAnyCollectionByCollectionTest()
    {
      var otherSpareNode = SpareNode;
      CreateNewSpareNode();

      Assert.Throws<InvalidOperationException>(() => DisjointCollection.GetNodesConnected(SpareNode, ConnectionDirection.To), "Collection should NOT allow GetNodeConnections on unrelated Node");

      Assert.IsFalse(DisjointCollection.NodesHaveConnection(SpareNode, DisjointNodeA, ConnectionDirection.To), "Collection SHOULD allow NodesHaveConnection on first node unrelated");
      Assert.IsFalse(DisjointCollection.NodesHaveConnection(DisjointNodeA, SpareNode, ConnectionDirection.To), "Collection SHOULD allow NodesHaveConnection on second node unrelated");
      Assert.Throws<InvalidOperationException>(() => DisjointCollection.NodesHaveConnection(SpareNode, otherSpareNode, ConnectionDirection.To), "Collection should NOT allow NodesHaveConnection on TWO unrelated nodes");

      Assert.Throws<InvalidOperationException>(() => DisjointCollection.ConnectNodes(SpareNode, DisjointNodeA, ConnectionDirection.To), "Collection should NOT allow ConnectNodes on first node unrelated");
      Assert.Throws<InvalidOperationException>(() => DisjointCollection.ConnectNodes(DisjointNodeA, SpareNode, ConnectionDirection.To), "Collection should NOT allow ConnectNodes on second node unrelated");
      Assert.Throws<InvalidOperationException>(() => DisjointCollection.ConnectNodes(SpareNode, otherSpareNode, ConnectionDirection.To), "Collection should NOT allow ConnectNodes on two unrelated nodes");
    
      Assert.DoesNotThrow(() => DisjointCollection.RemoveNode(SpareNode), "Collection SHOULD allow 'Removal' of a node not in a Collection");
      Assert.DoesNotThrow(() => DisjointCollection.DisconnectNodes(SpareNode, DisjointNodeA, ConnectionDirection.To), "Collection SHOULD allow 'Removal' of a connection to a first node not in a Collection");
      Assert.DoesNotThrow(() => DisjointCollection.DisconnectNodes(DisjointNodeA, SpareNode, ConnectionDirection.To), "Collection SHOULD allow 'Removal' of a connection to a second node not in a Collection");
      Assert.Throws<InvalidOperationException>(() => DisjointCollection.DisconnectNodes(otherSpareNode, SpareNode, ConnectionDirection.To), "Collection should NOT allow 'Removal' of a connection between TWO nodes not in a Collection");
    }

    [IndirectTest]
    public void ManipulateNodesNotInAnyCollectionByNodeTest()
    {
      Assert.Throws<InvalidOperationException>(() => DisjointNodeA.AddConnectionWith(SpareNode,ConnectionDirection.To), "Node should NOT allow AddConnectionWith on unrelated Node");
      Assert.Throws<InvalidOperationException>(() => DisjointNodeA.AddConnectionWith(new[]{SpareNode},ConnectionDirection.To), "Node should NOT allow AddConnectionWith on a collections containing an unrelated Node");

      Assert.IsFalse(DisjointNodeA.IsConnectedWith(SpareNode, ConnectionDirection.To), "Node SHOULD allow IsConnectedWith on unrelated Node");

      Assert.DoesNotThrow(() => DisjointNodeA.RemoveConnectionWith(SpareNode, ConnectionDirection.To), "Node SHOULD allow Removal of a connection to a node not in any Collection");
    }

    [IndirectTest]
    public void ActionsAvailableToNodeWhenNotInAnyCollectionTest()
    {
      var otherSpareNode = SpareNode;
      CreateNewSpareNode();

      Assert.DoesNotThrow(() => SpareNode.RemoveFromExistingCollection(), "Node should allow RemoveFromExistingCollection when not in a collection.");
      Assert.DoesNotThrow(() => SpareNode.AddToCollection(DisjointCollection), "Node should allow AddToCollection when not in a collection.");
      CreateNewSpareNode();

      try// Can't use Assert.DoesNotThrow due to modified closure :(
      {
        foreach (var direction in EnumAccessor.ConnectionDirections)
        {
          SpareNode.GetNodesConnected(direction);
        }
      }
      catch (Exception)
      {
        Assert.Fail("Node should allow GetNodesConnected when not in a collection.");
      }

      Assert.DoesNotThrow(() => SpareNode.IsConnectedWith(otherSpareNode, ConnectionDirection.To), "Node should allow IsConnectedWith when not in a collection.");
      Assert.DoesNotThrow(() => SpareNode.IsConnectedWith(DisjointNodeA, ConnectionDirection.To), "Node should allow IsConnectedWith when not in a collection.");

      Assert.DoesNotThrow(() => SpareNode.RemoveConnectionWith(otherSpareNode, ConnectionDirection.To), "Node should allow RemoveConnectionWith when not in a collection.");
      Assert.DoesNotThrow(() => SpareNode.RemoveConnectionWith(DisjointNodeA, ConnectionDirection.To), "Node should allow RemoveConnectionWith when not in a collection.");
      Assert.DoesNotThrow(() => SpareNode.RemoveConnectionWith(new[]{DisjointNodeA,otherSpareNode}, ConnectionDirection.To), "Node should allow RemoveConnectionWith on a set of nodes when not in a collection.");

      Assert.Throws<InvalidOperationException>(() => SpareNode.AddConnectionWith(otherSpareNode, ConnectionDirection.To), "Node should allow AddConnectionWith when not in a collection.");
      Assert.Throws<InvalidOperationException>(() => SpareNode.AddConnectionWith(DisjointNodeA, ConnectionDirection.To), "Node should allow AddConnectionWith when not in a collection.");
      Assert.Throws<InvalidOperationException>(() => SpareNode.AddConnectionWith(new[]{DisjointNodeA, otherSpareNode}, ConnectionDirection.To), "Node should allow AddConnectionWith on a set of nodes when not in a collection.");
    }

    [IndirectTest]
    public void ManipulateNodesNotInSameCollectionByCollectionTest()
    {
      var otherPreFormedNodeA = PreFormedNodeA;
      CreateNewPreFormedCollection();
      Assert.Throws<InvalidOperationException>(() => DisjointCollection.GetNodesConnected(PreFormedNodeA, ConnectionDirection.To), "Collection should NOT allow GetNodeConnections on unrelated Node");

      Assert.IsFalse(DisjointCollection.NodesHaveConnection(PreFormedNodeA, DisjointNodeA, ConnectionDirection.To), "Collection should NOT allow NodesHaveConnection on first node unrelated");
      Assert.IsFalse(DisjointCollection.NodesHaveConnection(DisjointNodeA, PreFormedNodeA, ConnectionDirection.To), "Collection should NOT allow NodesHaveConnection on second node unrelated");
      Assert.Throws<InvalidOperationException>(() => DisjointCollection.NodesHaveConnection(PreFormedNodeA, PreFormedNodeB, ConnectionDirection.To), "Collection should NOT allow NodesHaveConnection on TWO unrelated nodes");
      Assert.Throws<InvalidOperationException>(() => DisjointCollection.NodesHaveConnection(PreFormedNodeA, otherPreFormedNodeA, ConnectionDirection.To), "Collection should NOT allow NodesHaveConnection on TWO unrelated nodes");

      Assert.Throws<InvalidOperationException>(() => DisjointCollection.ConnectNodes(PreFormedNodeA, DisjointNodeA, ConnectionDirection.To), "Collection should NOT allow ConnectNodes on first node unrelated");
      Assert.Throws<InvalidOperationException>(() => DisjointCollection.ConnectNodes(DisjointNodeA, PreFormedNodeA, ConnectionDirection.To), "Collection should NOT allow ConnectNodes on second node unrelated");
      Assert.Throws<InvalidOperationException>(() => DisjointCollection.ConnectNodes(PreFormedNodeA, PreFormedNodeB, ConnectionDirection.To), "Collection should NOT allow ConnectNodes on two unrelated nodes");
      Assert.Throws<InvalidOperationException>(() => DisjointCollection.ConnectNodes(PreFormedNodeA, otherPreFormedNodeA, ConnectionDirection.To), "Collection should NOT allow ConnectNodes on two unrelated nodes");
    
      Assert.DoesNotThrow(() => DisjointCollection.RemoveNode(PreFormedNodeA), "Collection SHOULD allow 'Removal' of a node in a different Collection");
      Assert.DoesNotThrow(() => DisjointCollection.DisconnectNodes(PreFormedNodeA, DisjointNodeA, ConnectionDirection.To), "Collection SHOULD allow 'Removal' of a connection to a first node in a different Collection");
      Assert.DoesNotThrow(() => DisjointCollection.DisconnectNodes(DisjointNodeA, PreFormedNodeA, ConnectionDirection.To), "Collection SHOULD allow 'Removal' of a connection to a second node in a different Collection");
      Assert.Throws<InvalidOperationException>(() => DisjointCollection.DisconnectNodes(otherPreFormedNodeA, PreFormedNodeA, ConnectionDirection.To), "Collection should NOT allow 'Removal' of a connection between TWO nodes in a different Collection");
    }

    [IndirectTest]
    public void ManipulateNodesNotInSameCollectionByNodeTest()
    {
      Assert.Throws<InvalidOperationException>(() => DisjointNodeA.AddConnectionWith(PreFormedNodeA,ConnectionDirection.To), "Node should NOT allow AddConnectionWith on unrelated Node");
      Assert.Throws<InvalidOperationException>(() => DisjointNodeA.AddConnectionWith(new[]{PreFormedNodeA},ConnectionDirection.To), "Node should NOT allow AddConnectionWith on a collections containing an unrelated Node");

      Assert.IsFalse(DisjointNodeA.IsConnectedWith(PreFormedNodeA, ConnectionDirection.To), "Node SHOULD allow IsConnectedWith on unrelated Node");

      Assert.DoesNotThrow(() => DisjointNodeA.RemoveConnectionWith(PreFormedNodeA, ConnectionDirection.To), "Node SHOULD allow 'Removal' of a connection to a node in a different Collection");
    }

    [IndirectTest]
    public void RemovalOperationsThatDontThrowTest()
    {
      Assert.DoesNotThrow(() => DisjointNodeA.RemoveConnectionWith(DisjointNodeB, ConnectionDirection.To), "Node should allow Removal of a non existent connection");
      Assert.DoesNotThrow(() => DisjointCollection.DisconnectNodes(DisjointNodeA, DisjointNodeB, ConnectionDirection.To), "Collection should allow Removal of a non existent connection");
      Assert.DoesNotThrow(() => DisjointNodeA.RemoveConnectionWith(DisjointNodeB, ConnectionDirection.Any), "Node should allow Removal of \"Any\" connections");
      Assert.DoesNotThrow(() => DisjointCollection.DisconnectNodes(DisjointNodeA, DisjointNodeB, ConnectionDirection.Any), "Collection should allow Removal of \"Any\" connections");
    }

    [IndirectTest]
    public void NullArgumentsThatDontThrowForCollectionsTest()
    {
      Assert.DoesNotThrow(() => DisjointCollection.RemoveNode(null), "Collection SHOULD allow Remove on null");
      Assert.DoesNotThrow(() => DisjointCollection.DisconnectNodes(null, DisjointNodeA, ConnectionDirection.To), "Collection SHOULD allow DisconnectNodes on first null");
      Assert.DoesNotThrow(() => DisjointCollection.DisconnectNodes(DisjointNodeA, null, ConnectionDirection.To), "Collection SHOULD allow DisconnectNodes on second null");
      Assert.DoesNotThrow(() => DisjointCollection.DisconnectNodes(null, null, ConnectionDirection.To), "Collection SHOULD allow DisconnectNodes on both null");

      Assert.IsFalse(DisjointCollection.NodesHaveConnection(null, DisjointNodeA, ConnectionDirection.To), "Collection SHOULD allow NodesHaveConnection on first null");
      Assert.IsFalse(DisjointCollection.NodesHaveConnection(DisjointNodeA, null, ConnectionDirection.To), "Collection SHOULD allow NodesHaveConnection on second null");
      Assert.IsFalse(DisjointCollection.NodesHaveConnection(null, null, ConnectionDirection.To), "Collection SHOULD allow NodesHaveConnection on both null");
    }

    [IndirectTest]
    public void NullArgumentsThatDoThrowForCollectionsTest()
    {
      Assert.Throws<ArgumentNullException>(() => DisjointCollection.AddNode(null), "Collection should NOT allow Add on null");
      Assert.Throws<ArgumentNullException>(() => DisjointCollection.GetNodesConnected(null, ConnectionDirection.To), "Collection should NOT allow GetNodeConnections on null");

      Assert.Throws<ArgumentNullException>(() => DisjointCollection.ConnectNodes(null, DisjointNodeA, ConnectionDirection.To), "Collection should NOT allow ConnectNodes on first null");
      Assert.Throws<ArgumentNullException>(() => DisjointCollection.ConnectNodes(DisjointNodeA, null, ConnectionDirection.To), "Collection should NOT allow ConnectNodes on second null");
      Assert.Throws<ArgumentNullException>(() => DisjointCollection.ConnectNodes(null, null, ConnectionDirection.To), "Collection should NOT allow ConnectNodes on both null");
    }

// ReSharper disable ExpressionIsAlwaysNull
// Necessary to avoid ReSharper complaining about the null objects being used.
    [IndirectTest]
    public void NullArgumentsThatDontThrowForNodesTest()
    {
      var nullNode = (IDirectedConnectedNode<T>) null;
      var nullNodeArray = (IDirectedConnectedNode<T>[]) null;

      Assert.DoesNotThrow(() => DisjointNodeA.RemoveConnectionWith(nullNode, ConnectionDirection.To), "Node SHOULD allow Remove connection on a null Node.");
      Assert.DoesNotThrow(() => DisjointNodeA.RemoveConnectionWith(nullNodeArray, ConnectionDirection.To), "Node SHOULD allow Remove connection on a null collections of Nodes.");
      Assert.DoesNotThrow(() => DisjointNodeA.RemoveConnectionWith(new[]{nullNode}, ConnectionDirection.To), "Node SHOULD allow Remove connection on a Collection containing null Nodes.");

      Assert.IsFalse(DisjointNodeA.IsConnectedWith(null, ConnectionDirection.To), "Node SHOULD allow IsConnectedWith on null");
    }

    [IndirectTest]
    public void NullArgumentsThatDoThrowForNodesTest()
    {
      var nullNode = (IDirectedConnectedNode<T>) null;
      var nullNodeArray = (IDirectedConnectedNode<T>[]) null;

      Assert.Throws<ArgumentNullException>(() => SpareNode.AddToCollection(null), "Node should NOT allow AddToCollection on null");

      Assert.Throws<ArgumentNullException>(() => DisjointNodeA.AddConnectionWith(nullNode,ConnectionDirection.To), "Node should NOT allow AddConnectionWith on null");
      Assert.Throws<ArgumentNullException>(() => DisjointNodeA.AddConnectionWith(new[]{nullNode},ConnectionDirection.To), "Node should NOT allow AddConnectionWith on a collection containing null");
      Assert.Throws<ArgumentNullException>(() => DisjointNodeA.AddConnectionWith(nullNodeArray, ConnectionDirection.To), "Node should NOT allow AddConnectionWith on a null collection");
    }
// ReSharper restore ExpressionIsAlwaysNull
    #endregion
    #region Random Accessor
    [IndirectTest]
    public void EmptyCollectionWillReturnNullFromRandom()
    {
      Assert.AreEqual(null, BlankCollection.RandomNode);
    }

    [IndirectTest]
    public void SingleNodeCollectionWillAlwaysReturnThatNodeFromRandom()
    {
      var nodeBeforeAdd = SpareNode;
      SpareNode.AddToCollection(BlankCollection);
      for (int i = 0; i < 10; i++)
      {
        var returnedNode = BlankCollection.RandomNode;
        Assert.AreEqual(nodeBeforeAdd, returnedNode);
      }
    }

    [IndirectTest]
    public void RandomNodeIsAlwaysOneOfTheCollectionNodes()
    {
      var possibleNodes = PreFormedCollection.Nodes;
      for (int i = 0; i < 20; i++)
      {
        Assert.That(possibleNodes, Contains.Item(PreFormedCollection.RandomNode));
      }
    }

    [IndirectTest]
    public void RandomNodeFromPreformedCollectionIsAlwaysOneOfTheExpectedNodes()
    {
      var possibleNodes = new List<IDirectedConnectedNode<T>>{PreFormedNodeA, PreFormedNodeB, PreFormedNodeC, PreFormedNodeD};
      for (int i = 0; i < 20; i++)
      {
        Assert.That(possibleNodes, Contains.Item(PreFormedCollection.RandomNode));
      }
    }

    [IndirectTest]
    public void RandomNodeFromPreformedCollectionIsNotAlwaysTheSame()
    {
      var firstNodeReturned = PreFormedCollection.RandomNode;

      var returnedNodes = new List<IDirectedConnectedNode<T>>();
      for (int i = 0; i < 20; i++)
      {
        returnedNodes.Add(PreFormedCollection.RandomNode);
      }

      Assert.That(returnedNodes.All(node => node.Equals(firstNodeReturned)), Is.Not.True);
   }

    [IndirectTest]
    public void RandomNodeFromPreformedCollectionIsAtLeastVaguelyRandom()
    {
      //Each of these foreach loops should pass 99.7% of the time. Probability is given by Binomial distribution of 
      foreach (var targetNode in PreFormedCollection.Nodes)
      {
        var returnedNodes = new List<IDirectedConnectedNode<T>>();
        for (int i = 0; i < 120; i++)
        {
          returnedNodes.Add(PreFormedCollection.RandomNode);
        }
        Assert.That(returnedNodes.Count(node => node.Equals(targetNode)), Is.InRange(17, 44));
      }
    }

    #endregion
    #endregion

    #region Test Helpers
    internal virtual bool NodesAreEquivalent(IDirectedConnectedNode<T> expectedNode, IDirectedConnectedNode<T> actualNode)
    {
      return NodeValuesAreEquivalent(expectedNode.Value, actualNode.Value);
    }

    internal virtual bool NodeValuesAreEquivalent(T expectedNodeValue, T actualNodeValue)
    {
      int comparison = (Comparer<T>.Default).Compare(expectedNodeValue, actualNodeValue);
      return comparison == 0;
    }

    private void AssertNodeTypeIsCorrect(IDirectedConnectedNode<T> node)
    {
      Assert.AreEqual(NodeType, node.GetType(), "Node was not of correct type.");
    }

    private void AssertNewNodeIsUnrelatedToBlankCollection(IDirectedConnectedNode<T> returnedNewNode)
    {
      CollectionAssert.IsEmpty(BlankCollection.Nodes, "Collection acquired Nodes.");
      AssertNodeIsUnrelatedToCollection(returnedNewNode,BlankCollection);
    }

    private static void AssertNodeIsUnrelatedToCollection(IDirectedConnectedNode<T> node, IDirectedConnectedNodeCollection<T> collection)
    {
      Assert.IsNull(node.ParentCollection, "Node has a Parent.");
      CollectionAssert.DoesNotContain(collection.Nodes, node, "Collection contains the node.");
      CollectionAssert.IsEmpty(node.GetNodesConnected(ConnectionDirection.Any), "Node thinks it has connections.");
      Assert.Throws<InvalidOperationException>(() => collection.GetNodesConnected(node,ConnectionDirection.Any), "Collection thinks Node is a member.");
    }

    private static void AssertNodeIsInCollectionButDisjoint(IDirectedConnectedNode<T> node, IDirectedConnectedNodeCollection<T> collection)
    {
      Assert.AreEqual(collection, node.ParentCollection, "Node doesn't think it's contained in the collection.");
      CollectionAssert.Contains(collection.Nodes, node, "Collection doesn't think it contains in the node.");
      CollectionAssert.IsEmpty(node.GetNodesConnected(ConnectionDirection.Any), "Node thinks it has connections.");
      CollectionAssert.IsEmpty(collection.GetNodesConnected(node,ConnectionDirection.Any), "Collection thinks Node has connections.");
    }

    private static void AssertDirectedToConnectionOnly(IDirectedConnectedNode<T> nodeA, IDirectedConnectedNode<T> nodeB)
    {
      var parentCollection = nodeA.ParentCollection;
      Assert.AreEqual(parentCollection, nodeB.ParentCollection,"Two nodes do not exist within the same collection.");

      Assert.IsTrue (nodeA.IsConnectedWith(nodeB, ConnectionDirection.To),   "A thinks it isn't connected to B");
      Assert.IsFalse(nodeA.IsConnectedWith(nodeB, ConnectionDirection.From), "A thinks it is connected from B");
      Assert.IsTrue (nodeA.IsConnectedWith(nodeB, ConnectionDirection.Any),  "A thinks it & B aren't connected.");
      Assert.IsFalse(nodeA.IsConnectedWith(nodeB, ConnectionDirection.Both), "A thinks it & B are undirected connected.");

      Assert.IsTrue (parentCollection.NodesHaveConnection(nodeA, nodeB, ConnectionDirection.To),   "Collection thinks A isn't connected to B");
      Assert.IsFalse(parentCollection.NodesHaveConnection(nodeA, nodeB, ConnectionDirection.From), "Collection thinks A is connected from B");
      Assert.IsTrue (parentCollection.NodesHaveConnection(nodeA, nodeB, ConnectionDirection.Any),  "Collection thinks A & B aren't connected.");
      Assert.IsFalse(parentCollection.NodesHaveConnection(nodeA, nodeB, ConnectionDirection.Both), "Collection thinks A & B are undirected connected.");

      Assert.IsFalse(nodeB.IsConnectedWith(nodeA, ConnectionDirection.To),   "B thinks it is connected to A");
      Assert.IsTrue (nodeB.IsConnectedWith(nodeA, ConnectionDirection.From), "B thinks it isn't connected from A");
      Assert.IsTrue (nodeB.IsConnectedWith(nodeA, ConnectionDirection.Any),  "B thinks it & A aren't connected.");
      Assert.IsFalse(nodeB.IsConnectedWith(nodeA, ConnectionDirection.Both), "B thinks it & A are undirected connected.");

      Assert.IsFalse(parentCollection.NodesHaveConnection(nodeB, nodeA, ConnectionDirection.To),   "Collection thinks B isn't connected to A");
      Assert.IsTrue (parentCollection.NodesHaveConnection(nodeB, nodeA, ConnectionDirection.From), "Collection thinks B is connected from A");
      Assert.IsTrue(parentCollection.NodesHaveConnection(nodeB, nodeA, ConnectionDirection.Any),  "Collection thinks B & A aren't connected.");
      Assert.IsFalse (parentCollection.NodesHaveConnection(nodeB, nodeA, ConnectionDirection.Both), "Collection thinks B & A are undirected connected.");
    }

    private void AssertUndirectedConnection(IDirectedConnectedNode<T> nodeA, IDirectedConnectedNode<T> nodeB)
    {
      var parentCollection = nodeA.ParentCollection;
      Assert.AreEqual(parentCollection, nodeB.ParentCollection,"Two nodes do not exist within the same collection.");

      Assert.IsTrue(nodeA.IsConnectedWith(nodeB, ConnectionDirection.To),   "A thinks it isn't connected to B");
      Assert.IsTrue(nodeA.IsConnectedWith(nodeB, ConnectionDirection.From), "A thinks it isn't connected from B");
      Assert.IsTrue(nodeA.IsConnectedWith(nodeB, ConnectionDirection.Any),  "A thinks it & B aren't connected.");
      Assert.IsTrue(nodeA.IsConnectedWith(nodeB, ConnectionDirection.Both), "A thinks it & B aren't undirected connected.");

      Assert.IsTrue(parentCollection.NodesHaveConnection(nodeA, nodeB, ConnectionDirection.To),   "Collection thinks A isn't connected to B");
      Assert.IsTrue(parentCollection.NodesHaveConnection(nodeA, nodeB, ConnectionDirection.From), "Collection thinks A isn't connected from B");
      Assert.IsTrue(parentCollection.NodesHaveConnection(nodeA, nodeB, ConnectionDirection.Any),  "Collection thinks A & B aren't connected.");
      Assert.IsTrue(parentCollection.NodesHaveConnection(nodeA, nodeB, ConnectionDirection.Both), "Collection thinks A & B aren't undirected connected.");

      Assert.IsTrue(nodeB.IsConnectedWith(nodeA, ConnectionDirection.To),   "B thinks it isn't connected to A");
      Assert.IsTrue(nodeB.IsConnectedWith(nodeA, ConnectionDirection.From), "B thinks it isn't connected from A");
      Assert.IsTrue(nodeB.IsConnectedWith(nodeA, ConnectionDirection.Any),  "B thinks it & A aren't connected.");
      Assert.IsTrue(nodeB.IsConnectedWith(nodeA, ConnectionDirection.Both), "B thinks it & A aren't undirected connected.");

      Assert.IsTrue(parentCollection.NodesHaveConnection(nodeB, nodeA, ConnectionDirection.To),   "Collection thinks B isn't connected to A");
      Assert.IsTrue(parentCollection.NodesHaveConnection(nodeB, nodeA, ConnectionDirection.From), "Collection thinks B isn't connected from A");
      Assert.IsTrue(parentCollection.NodesHaveConnection(nodeB, nodeA, ConnectionDirection.Any),  "Collection thinks B & A aren't connected.");
      Assert.IsTrue(parentCollection.NodesHaveConnection(nodeB, nodeA, ConnectionDirection.Both), "Collection thinks B & A aren't undirected connected.");
    }

    private void AssertUnconnected(IDirectedConnectedNode<T> nodeA, IDirectedConnectedNode<T> nodeB)
    {
      Assert.IsFalse(nodeA.IsConnectedWith(nodeB, ConnectionDirection.To), "A thinks it is connected to B");
      Assert.IsFalse(nodeA.IsConnectedWith(nodeB, ConnectionDirection.From), "A thinks it is connected from B");
      Assert.IsFalse(nodeA.IsConnectedWith(nodeB, ConnectionDirection.Any), "A thinks it & B are connected.");
      Assert.IsFalse(nodeA.IsConnectedWith(nodeB, ConnectionDirection.Both), "A thinks it & B are undirected connected.");

      Assert.IsFalse(nodeB.IsConnectedWith(nodeA, ConnectionDirection.To), "B thinks it is connected to A");
      Assert.IsFalse(nodeB.IsConnectedWith(nodeA, ConnectionDirection.From), "B thinks it is connected from A");
      Assert.IsFalse(nodeB.IsConnectedWith(nodeA, ConnectionDirection.Any), "B thinks it & A are connected.");
      Assert.IsFalse(nodeB.IsConnectedWith(nodeA, ConnectionDirection.Both), "B thinks it & A are undirected connected.");

      var parentCollection = nodeA.ParentCollection ?? nodeB.ParentCollection;
      if (parentCollection != null) // Could legitamately be null if unconnected because not in a collection at all.
      {
        Assert.IsFalse(parentCollection.NodesHaveConnection(nodeA, nodeB, ConnectionDirection.To), "Collection thinks A is connected to B");
        Assert.IsFalse(parentCollection.NodesHaveConnection(nodeA, nodeB, ConnectionDirection.From), "Collection thinks A is connected from B");
        Assert.IsFalse(parentCollection.NodesHaveConnection(nodeA, nodeB, ConnectionDirection.Any), "Collection thinks A & B are connected.");
        Assert.IsFalse(parentCollection.NodesHaveConnection(nodeA, nodeB, ConnectionDirection.Both), "Collection thinks A & B are undirected connected.");

        Assert.IsFalse(parentCollection.NodesHaveConnection(nodeB, nodeA, ConnectionDirection.To), "Collection thinks B is connected to A");
        Assert.IsFalse(parentCollection.NodesHaveConnection(nodeB, nodeA, ConnectionDirection.From), "Collection thinks B is connected from A");
        Assert.IsFalse(parentCollection.NodesHaveConnection(nodeB, nodeA, ConnectionDirection.Any), "Collection thinks B & A are connected.");
        Assert.IsFalse(parentCollection.NodesHaveConnection(nodeB, nodeA, ConnectionDirection.Both), "Collection thinks B & A are undirected connected.");
      }
    }

    private void AssertSpecificConnection(IDirectedConnectedNode<T> nodeA, IDirectedConnectedNode<T> nodeB, ConnectionDirection direction)
    {
      switch (direction)
      {
        case ConnectionDirection.To:
          AssertDirectedToConnectionOnly(nodeA, nodeB);
          break;
        case ConnectionDirection.From:
          AssertDirectedToConnectionOnly(nodeB, nodeA);
          break;
        case ConnectionDirection.Both:
          AssertUndirectedConnection(nodeA, nodeB);
          break;
        case ConnectionDirection.Any:
          throw new InvalidOperationException("By design, this method only handles the Definite Directions. Case left here to document this fact.");  //ncrunch: no coverage
        default:
          throw new NonExistentEnumCaseException<ConnectionDirection>();  //ncrunch: no coverage
      }
    }

    private void AssertSingleSpecificConnection(IDirectedConnectedNode<T> nodeA, IDirectedConnectedNode<T> nodeB, ConnectionDirection direction)
    {
      AssertSpecificConnection(nodeA, nodeB, direction);
      var collection = nodeA.ParentCollection;

      int numberOfConnectionsByNode = nodeA.GetNodesConnected(direction)
        .Count(node => node == nodeB);
      int numberOfConnectionsByNodeReversed = nodeB.GetNodesConnected(direction.Invert())
                                                   .Count(node => node == nodeA);

      Assert.AreEqual(numberOfConnectionsByNode, 1, "Node had more than one connection to Target.");
      Assert.AreEqual(numberOfConnectionsByNodeReversed, 1, "Node had more than one connection to Target when reversed");

      int numberOfConnectionsByCollection = collection.GetNodesConnected(nodeA, direction)
                                                 .Count(node => node == nodeB);
      int numberOfConnectionsByCollectionReversed = collection.GetNodesConnected(nodeB, direction.Invert())
                                                         .Count(node => node == nodeA);

      Assert.AreEqual(numberOfConnectionsByCollection, 1, "Node had more than one connection to Target.");
      Assert.AreEqual(numberOfConnectionsByCollectionReversed, 1, "Node had more than one connection to Target when reversed");
    }

    private void AssertPreFormedCollectionHasExpectedConnectionsByCollectionExceptForGivenNodeRemoved(IDirectedConnectedNode<T> excludedNode)
    {
      var exclusionList = new List<IDirectedConnectedNode<T>>();
      if (excludedNode != null)
      {
        exclusionList.Add(excludedNode);

        foreach (var remainingNode in PreFormedCollection.Nodes)
        {
          foreach (var direction in EnumAccessor.ConnectionDirections)
          {
            Assert.IsFalse(PreFormedCollection.NodesHaveConnection(excludedNode, remainingNode, direction)); 
            Assert.IsFalse(PreFormedCollection.NodesHaveConnection(remainingNode, excludedNode, direction.Invert())); 
          }
        }
      }

      foreach (var originalNode in PreFormedCollection.Nodes.Except(exclusionList))
      {
        foreach (var direction in EnumAccessor.ConnectionDirections)
        {
          var expectedNodesWithConnections = GetExpectedConnectionsInPreFormedCollection(originalNode, direction).Except(exclusionList).ToList();
          var expectedNodesWithoutConnections = PreFormedCollection.Nodes.Except(expectedNodesWithConnections);

          foreach (var nodeWithConnection in expectedNodesWithConnections)
          {
            Assert.IsTrue(PreFormedCollection.NodesHaveConnection(originalNode, nodeWithConnection, direction)); 
            Assert.IsTrue(PreFormedCollection.NodesHaveConnection(nodeWithConnection, originalNode, direction.Invert())); 
          }

          foreach (var nodeWithConnection in expectedNodesWithoutConnections)
          {
            Assert.IsFalse(PreFormedCollection.NodesHaveConnection(originalNode, nodeWithConnection, direction)); 
            Assert.IsFalse(PreFormedCollection.NodesHaveConnection(nodeWithConnection, originalNode, direction.Invert())); 
          }
        }
      }
    }

    private void AssertPreFormedCollectionHasExpectedConnectionsByNodeExceptForGivenNodeRemoved(IDirectedConnectedNode<T> excludedNode)
    {
      var exclusionList = new List<IDirectedConnectedNode<T>>();
      if (excludedNode != null)
      {
        exclusionList.Add(excludedNode);

        foreach (var remainingNode in PreFormedCollection.Nodes)
        {
          foreach (var direction in EnumAccessor.ConnectionDirections)
          {
            Assert.IsFalse(excludedNode.IsConnectedWith(remainingNode, direction)); 
            Assert.IsFalse(remainingNode.IsConnectedWith(excludedNode, direction.Invert())); 
          }
        }
      }

      foreach (var originalNode in PreFormedCollection.Nodes.Except(exclusionList))
      {
        foreach (var direction in EnumAccessor.ConnectionDirections)
        {
          var expectedNodesWithConnections = GetExpectedConnectionsInPreFormedCollection(originalNode, direction).Except(exclusionList).ToList();
          var expectedNodesWithoutConnections = PreFormedCollection.Nodes.Except(expectedNodesWithConnections);

          foreach (var nodeWithConnection in expectedNodesWithConnections)
          {
            Assert.IsTrue(originalNode.IsConnectedWith(nodeWithConnection, direction)); 
            Assert.IsTrue(nodeWithConnection.IsConnectedWith(originalNode, direction.Invert())); 
          }

          foreach (var nodeWithConnection in expectedNodesWithoutConnections)
          {
            Assert.IsFalse(originalNode.IsConnectedWith(nodeWithConnection, direction)); 
            Assert.IsFalse(nodeWithConnection.IsConnectedWith(originalNode, direction.Invert())); 
          }
        }
      }
    }
    #endregion
  }
}

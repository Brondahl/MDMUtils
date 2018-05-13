using System;
using System.Collections.Generic;
using System.Linq;

namespace MDMUtils.DataStructures.Graphs.Base
{
  internal class ArrayDCNC<T> : IDirectedConnectedNodeCollection<T>
  {
    private readonly List<ArrayDCN<T>> nodeList = new List<ArrayDCN<T>>();
    private readonly ConnectionGrid nodeConnectionGrid = new ConnectionGrid();

    #region Explicit IDirectedConnectedNodeCollection<T> Members
    IEnumerable<IDirectedConnectedNode<T>> IDirectedConnectedNodeCollection<T>.Nodes
    {
      get { return nodeList; }
    }

    IDirectedConnectedNode<T> IDirectedConnectedNodeCollection<T>.NewNode()
    {
      return new ArrayDCN<T>();
    }

    IDirectedConnectedNode<T> IDirectedConnectedNodeCollection<T>.NewNode(T value)
    {
      return new ArrayDCN<T> {Value = value};
    }

    IDirectedConnectedNode<T> IDirectedConnectedNodeCollection<T>.RandomNode
    {
      get
      {
        if (!nodeList.Any())
        {
          return null;
        }

        var randomIndex = random.Next(nodeList.Count());
        return nodeList[randomIndex];
      }
    }
    static readonly Random random = new Random();

    void IDirectedConnectedNodeCollection<T>.AddNode(IDirectedConnectedNode<T> newNode)
    {
      Helpers<T>.Verify_AddNode_ConditionsAreSatisfied(newNode);

      var typedNode = TypedHelpers.GetNodeAsValidType<ArrayDCN<T>,T>(newNode);

      var index = this.InternalAddNodeRequest(typedNode);
      typedNode.InternalSetParentCollectionRequest(this, index);
    }

    void IDirectedConnectedNodeCollection<T>.RemoveNode(IDirectedConnectedNode<T> targetNode)
    {
      if(Helpers<T>.CheckWhether_RemoveNode_IsNeeded(targetNode, this))
      { return; }

      var typedNode = TypedHelpers.GetNodeAsValidType<ArrayDCN<T>,T>(targetNode);

      this.InternalRemoveNodeRequest(typedNode);
      typedNode.InternalDeleteParentCollectionRequest();
    }

    IEnumerable<IDirectedConnectedNode<T>> IDirectedConnectedNodeCollection<T>.GetNodesConnected(IDirectedConnectedNode<T> firstNode, ConnectionDirection direction)
    {
      Helpers<T>.Verify_GetNodesConnected_ConditionsAreSatisfied(firstNode, this);
      var typedNode = TypedHelpers.GetNodeAsValidType<ArrayDCN<T>,T>(firstNode);
      
      return this.InternalGetNodeConnectionsRequest(GetNodeIndex(typedNode), direction);
    }

    bool IDirectedConnectedNodeCollection<T>.NodesHaveConnection(IDirectedConnectedNode<T> firstNode, IDirectedConnectedNode<T> secondNode, ConnectionDirection direction)
    {
      if(Helpers<T>.CheckNodesHaveConnectionConditions(firstNode, secondNode, this))
      { return false; }
      var typedFirstNode = TypedHelpers.GetNodeAsValidType<ArrayDCN<T>,T>(firstNode);
      var typedSecondNode = TypedHelpers.GetNodeAsValidType<ArrayDCN<T>,T>(secondNode);

      return this.InternalCheckNodeConnectRequest(GetNodeIndex(typedFirstNode), GetNodeIndex(typedSecondNode), direction);
    }

    void IDirectedConnectedNodeCollection<T>.ConnectNodes(IDirectedConnectedNode<T> firstNode, IDirectedConnectedNode<T> secondNode, ConnectionDirection direction)
    {
      Helpers<T>.Verify_ConnectNodes_ConditionsAreSatisfied(firstNode, secondNode, this);
      var typedFirstNode = TypedHelpers.GetNodeAsValidType<ArrayDCN<T>,T>(firstNode);
      var typedSecondNode = TypedHelpers.GetNodeAsValidType<ArrayDCN<T>,T>(secondNode);

      this.InternalSetNodeConnectRequest(GetNodeIndex(typedFirstNode), GetNodeIndex(typedSecondNode), direction, true);
    }

    void IDirectedConnectedNodeCollection<T>.DisconnectNodes(IDirectedConnectedNode<T> firstNode, IDirectedConnectedNode<T> secondNode, ConnectionDirection direction)
    {
      if(Helpers<T>.CheckDisconnectNodesConditions(firstNode, secondNode, this))
      { return; }
      var typedFirstNode = TypedHelpers.GetNodeAsValidType<ArrayDCN<T>,T>(firstNode);
      var typedSecondNode = TypedHelpers.GetNodeAsValidType<ArrayDCN<T>,T>(secondNode);

      this.InternalSetNodeConnectRequest(GetNodeIndex(typedFirstNode), GetNodeIndex(typedSecondNode), direction, false);
    }
    #endregion

    #region Internal Methods For Local-Only operations. No verification
    internal int InternalAddNodeRequest(ArrayDCN<T> node)
    {
      nodeList.Add(node);
      return nodeConnectionGrid.AddDisconnectedEntry();
    }

    internal void InternalRemoveNodeRequest(ArrayDCN<T> targetNode)
    {
      int targetNodeIndex = GetNodeIndex(targetNode);
      nodeConnectionGrid.RemoveEntry(targetNodeIndex);
      nodeList.RemoveAt(targetNodeIndex);
    }

    internal void InternalSetNodeConnectRequest(int firstNodeIndex, int secondNodeIndex, ConnectionDirection direction, bool connectionPresent)
    {
      switch (direction)
      {
        case ConnectionDirection.To:
          nodeConnectionGrid[firstNodeIndex, secondNodeIndex] = connectionPresent;
          break;
        case ConnectionDirection.From:
          nodeConnectionGrid[secondNodeIndex, firstNodeIndex] = connectionPresent;
          break;
        case ConnectionDirection.Both:
          nodeConnectionGrid[firstNodeIndex, secondNodeIndex] = connectionPresent;
          nodeConnectionGrid[secondNodeIndex, firstNodeIndex] = connectionPresent;
          break;
        case ConnectionDirection.Any:
          if(!connectionPresent)
          {
            nodeConnectionGrid[firstNodeIndex, secondNodeIndex] = connectionPresent;
            nodeConnectionGrid[secondNodeIndex, firstNodeIndex] = connectionPresent;
            break;
          }

          throw new InvalidOperationException(
            "This InternalSetNodeConnectRequest should never have been asked to create a connection with 'Any' direction.");
        default:
          throw new NonExistentEnumCaseException<ConnectionDirection>();  //ncrunch: no coverage
      }
    }

    internal bool InternalCheckNodeConnectRequest(int firstNodeIndex, int secondNodeIndex, ConnectionDirection direction)
    {
      switch (direction)
      {
        case ConnectionDirection.To:
          return nodeConnectionGrid[firstNodeIndex, secondNodeIndex];
        case ConnectionDirection.From:
          return nodeConnectionGrid[secondNodeIndex, firstNodeIndex];
        case ConnectionDirection.Both:
          return nodeConnectionGrid[firstNodeIndex, secondNodeIndex] && nodeConnectionGrid[secondNodeIndex, firstNodeIndex];
        case ConnectionDirection.Any:
          return nodeConnectionGrid[firstNodeIndex, secondNodeIndex] || nodeConnectionGrid[secondNodeIndex, firstNodeIndex];
        default:
          throw new NonExistentEnumCaseException<ConnectionDirection>();  //ncrunch: no coverage
      }
    }

    private IEnumerable<IDirectedConnectedNode<T>> InternalGetNodeConnectionsRequest(int index, ConnectionDirection direction)
    {
      var toConnectionList = new List<int>();
      var fromConnectionList = new List<int>();

      if(direction != ConnectionDirection.From)
      {
        toConnectionList = nodeConnectionGrid.ToConnectionsForEntry(index);
      }
      if(direction != ConnectionDirection.To)
      {
        fromConnectionList = nodeConnectionGrid.FromConnectionsForEntry(index);
      }

      switch (direction)
      {
        case ConnectionDirection.To:
          return GetNodesByIndexes(toConnectionList);
        case ConnectionDirection.From:
          return GetNodesByIndexes(fromConnectionList);
        case ConnectionDirection.Both:
          return GetNodesByIndexes(Enumerable.Intersect(toConnectionList, fromConnectionList).ToList());
        case ConnectionDirection.Any:
          return GetNodesByIndexes(Enumerable.Union(toConnectionList, fromConnectionList));
        default:
          throw new NonExistentEnumCaseException<ConnectionDirection>();  //ncrunch: no coverage
      }
    }

    private ArrayDCN<T> GetNodeByIndex(int index)
    {
      return nodeList[index];
    }

    private int GetNodeIndex(ArrayDCN<T> node)
    {
      return nodeList.IndexOf(node);
    }

    private IEnumerable<ArrayDCN<T>> GetNodesByIndexes(IEnumerable<int> listOfIndexes)
    {
      return listOfIndexes.Select(i => nodeList[i]);
    }

    //// <remarks>
    ///// Have exprimented with Zip() for this. It's much slower.
    ///// </remarks>
    //private IEnumerable<ArrayDCN<T>> GetNodesCorrespondingToBools(IList<bool> listOfBools)
    //{
    //  var lRet = new List<ArrayDCN<T>>();
    //  for (int i = 0; i < listOfBools.Count; i++)
    //  {
    //    if(listOfBools[i])
    //    {
    //      lRet.Add(nodeList[i]);
    //    }
    //  }
    //  return lRet;
    //}
    #endregion

    #region //Add Pre-Connected Nodes Methods
    //public void AddNodeWithConnectionsToFrom(IDirectedConnectedNode<T> newNode, IEnumerable<IDirectedConnectedNode<T>> connectToNodes, IEnumerable<IDirectedConnectedNode<T>> connectFromNodes)
    //{
    //  AddNode(newNode);

    //  foreach (var node in connectToNodes)
    //  {
    //    ConnectNodesDirected(node, newNode);
    //  }
    //  foreach (var node in connectFromNodes)
    //  {
    //    ConnectNodesDirected(newNode, node);
    //  }
    //}

    //public void AddNodeWithConnectionsToFrom(IDirectedConnectedNode<T> newNode, IDirectedConnectedNode<T> connectToNode, IEnumerable<IDirectedConnectedNode<T>> connectFromNodes)
    //{
    //  AddNodeWithConnectionsToFrom(newNode, new[] {connectToNode}, connectFromNodes);
    //}

    //public void AddNodeWithConnectionsToFrom(IDirectedConnectedNode<T> newNode, IEnumerable<IDirectedConnectedNode<T>> connectToNodes, IDirectedConnectedNode<T> connectFromNode)
    //{
    //  AddNodeWithConnectionsToFrom(newNode, connectToNodes, new[] {connectFromNode});
    //}

    //public void AddNodeWithConnectionsToFrom(IDirectedConnectedNode<T> newNode, IDirectedConnectedNode<T> connectToNode, IDirectedConnectedNode<T> connectFromNode)
    //{
    //  AddNodeWithConnectionsToFrom(newNode, new[] {connectToNode}, new[] {connectFromNode});
    //}
    #endregion
  }

  internal class ArrayDCN<T> : IDirectedConnectedNode<T>
  {
    public T Value {get; set;}
    private ArrayDCNC<T> ArrayCollection {get; set;}
    private IDirectedConnectedNodeCollection<T> ParentCollection
    {
      get { return ArrayCollection; }
    }
    public string Name { get; set; }

#if DEBUG
    private Guid guid = Guid.NewGuid();
#endif


    #region Explicit IDirectedConnectedNode<T> Members
    IDirectedConnectedNodeCollection<T> IDirectedConnectedNode<T>.ParentCollection
    {
      get { return ArrayCollection; }
    }

    void IDirectedConnectedNode<T>.AddToCollection(IDirectedConnectedNodeCollection<T> newParentCollection)
    {
      Helpers<T>.Verify_AddToCollection_ConditionsAreSatisfied(this, newParentCollection);
      var typedCollection = TypedHelpers.GetCollectionAsValidType<ArrayDCNC<T>,T>(newParentCollection);

      int index = typedCollection.InternalAddNodeRequest(this);
      this.InternalSetParentCollectionRequest(typedCollection, index);
    }

    void IDirectedConnectedNode<T>.RemoveFromExistingCollection()
    {
      if (Helpers<T>.CheckWhether_RemoveFromExistingCollection_IsNeeded(this))
      {
        return;
      }

      ArrayCollection.InternalRemoveNodeRequest(this);
      this.InternalDeleteParentCollectionRequest();
    }

    IEnumerable<IDirectedConnectedNode<T>> IDirectedConnectedNode<T>.GetNodesConnected(ConnectionDirection direction)
    {
      if(Helpers<T>.Check_GetNodesConnected_PreConditions(this))
      {
        return new List<ArrayDCN<T>>();
      }

      return ParentCollection.GetNodesConnected(this, direction);
    }

    bool IDirectedConnectedNode<T>.IsConnectedWith(IDirectedConnectedNode<T> secondNode, ConnectionDirection direction)
    {
      if(Helpers<T>.Check_IsConnectedWith_PreConditions(secondNode, this))
      {
        return false;
      }

      return ParentCollection.NodesHaveConnection(this, secondNode, direction);
    }

    void IDirectedConnectedNode<T>.AddConnectionWith(IDirectedConnectedNode<T> secondNode, ConnectionDirection direction)
    {
      var typedNode = TypedHelpers.GetNodeAsValidType<ArrayDCN<T>,T>(secondNode);
      AddConnectionWith(typedNode, direction);
    }

    void IDirectedConnectedNode<T>.AddConnectionWith(IEnumerable<IDirectedConnectedNode<T>> secondNodes, ConnectionDirection direction)
    {
      Helpers<T>.VerifyNodeSetIsNotNull(secondNodes);

      foreach(var typedNode in secondNodes.Select(TypedHelpers.GetNodeAsValidType<ArrayDCN<T>,T>))
      {
        AddConnectionWith(typedNode, direction);
      }
    }
    
    void IDirectedConnectedNode<T>.RemoveConnectionWith(IDirectedConnectedNode<T> secondNode, ConnectionDirection direction)
    {
      var typedNode = TypedHelpers.GetNodeAsValidType<ArrayDCN<T>,T>(secondNode);
      RemoveConnectionWith(typedNode, direction);
    }

    void IDirectedConnectedNode<T>.RemoveConnectionWith(IEnumerable<IDirectedConnectedNode<T>> secondNodes, ConnectionDirection direction)
    {
      if(secondNodes == null)
      { return; }

      foreach(var typedNode in secondNodes.Select(TypedHelpers.GetNodeAsValidType<ArrayDCN<T>,T>))
      {
        RemoveConnectionWith(typedNode, direction);
      }
    }
    #endregion

    #region Strongly typed Methods
    private void AddConnectionWith(ArrayDCN<T> typedNode, ConnectionDirection direction)
    {
      Helpers<T>.Verify_AddConnectionWith_ConditionsAreSatisfied(typedNode, this);
      ParentCollection.ConnectNodes(this, typedNode, direction);
    }

    private void RemoveConnectionWith(ArrayDCN<T> typedNode, ConnectionDirection direction)
    {
      if(Helpers<T>.CheckWhether_RemoveConnectionWith_IsNeeded(typedNode, this))
      {
        return;
      }

      ParentCollection.DisconnectNodes(this, typedNode, direction);
    }
    #endregion

    #region Internal Methods For Local-Only operations. No verification
    internal void InternalSetParentCollectionRequest(ArrayDCNC<T> targetCollection, int index)
    {
      ArrayCollection = targetCollection;
    }

    internal void InternalDeleteParentCollectionRequest()
    {
      ArrayCollection = null;
    }
    #endregion
  }

  ///==========================================================================
  /// Class : ConnectionGrid
  ///
  /// <summary>
  ///   A Square 2D array of bools indicating whether a given pair of nodes are
  ///   connected.
  /// 
  ///   Grid[a,b] indicates whether a is connected to b.
  ///   Grid[b,a] indicates whether b is connected to a.
  /// 
  ///   Grid[a,a] is defined to be unconnected (i.e. false).
  /// </summary>
  /// <remarks>
  ///   The Grid is implemented as a List of Lists.
  ///   The Inner lists, represent the set of connections FROM a node, so to
  ///   access this, we simply return the List en masse.
  ///   To get the representation of the set of connections TO a node, we must 
  ///   cut "across" all the lists, retrieving the nth entry from each of them.
  /// </remarks>
  ///==========================================================================
  internal class ConnectionGrid
  {
    private readonly List<List<bool>> baseGrid;
    public int Size { get; private set; }

    public ConnectionGrid()
    {
      Size = 0;
      baseGrid = new List<List<bool>>();
    }

    public ConnectionGrid(int initialCapacity)
    {
      Size = initialCapacity;
      baseGrid = Enumerable.Repeat(Enumerable.Repeat(false, Size).ToList(), Size).ToList();
    }

    public int AddDisconnectedEntry()
    {
      Size++;

      foreach (var list in baseGrid)
      {
        list.Add(false);
      }

      baseGrid.Add(Enumerable.Repeat(false, Size).ToList());

      return Size - 1;
    }

    public void RemoveEntry(int index)
    {
      Size--;

      baseGrid.RemoveAt(index);
      foreach (var list in baseGrid)
      {
        list.RemoveAt(index);
      }
    }

    public bool this[int fromIndex, int toIndex]
    {
      get { return baseGrid[fromIndex][toIndex]; }
      set
      {
        if(fromIndex == toIndex && value)
        {
          throw new InvalidOperationException("A node can never be connected to itself.");
        }

        baseGrid[fromIndex][toIndex] = value;
      }
    }

    public List<int> ToConnectionsForEntry(int fromIndex)
    {
      var lRet = new List<int>();
      for (int i = 0; i < Size; i++)
      {
        if(baseGrid[fromIndex][i])
        {
          lRet.Add(i);
        }
      }
      return lRet;
    }

    public List<int> FromConnectionsForEntry(int toIndex)
    {
      var lRet = new List<int>();
      for (int i = 0; i < Size; i++)
      {
        if(baseGrid[i][toIndex])
        {
          lRet.Add(i);
        }
      }
      return lRet;
    }
  }
}
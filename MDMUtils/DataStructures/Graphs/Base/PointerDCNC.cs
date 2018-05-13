using System;
using System.Collections.Generic;
using System.Linq;

namespace MDMUtils.DataStructures.Graphs.Base
{
  internal class PointerDCNC<T> : IDirectedConnectedNodeCollection<T>
  {
    private readonly List<PointerDCN<T>> pointerNodes = new List<PointerDCN<T>>();

    #region Explicit IDirectedConnectedNodeCollection<T> Members
    IEnumerable<IDirectedConnectedNode<T>> IDirectedConnectedNodeCollection<T>.Nodes
    {
      get { return pointerNodes; }
    }

    IDirectedConnectedNode<T> IDirectedConnectedNodeCollection<T>.NewNode()
    {
      return new PointerDCN<T>();
    }

    IDirectedConnectedNode<T> IDirectedConnectedNodeCollection<T>.NewNode(T value)
    {
      return new PointerDCN<T> {Value = value};
    }

    IDirectedConnectedNode<T> IDirectedConnectedNodeCollection<T>.RandomNode
    {
      get
      {
        if (!pointerNodes.Any())
        {
          return null;
        }

        var randomIndex = random.Next(pointerNodes.Count);
        return pointerNodes[randomIndex];
      }
    }
    static Random random = new Random();

    void IDirectedConnectedNodeCollection<T>.AddNode(IDirectedConnectedNode<T> newNode)
    { 
      Helpers<T>.Verify_AddNode_ConditionsAreSatisfied(newNode);

      var typedNode = TypedHelpers.GetNodeAsValidType<PointerDCN<T>,T>(newNode);

      this.InternalAddNodeRequest(typedNode);
      typedNode.InternalSetParentCollectionRequest(this);
    }

    void IDirectedConnectedNodeCollection<T>.RemoveNode(IDirectedConnectedNode<T> targetNode)
    {
      if(Helpers<T>.CheckWhether_RemoveNode_IsNeeded(targetNode, this))
      { return; }

      var typedNode = TypedHelpers.GetNodeAsValidType<PointerDCN<T>,T>(targetNode);

      foreach (var typedConnectedToNode in targetNode.GetNodesConnected(ConnectionDirection.Any).Select(node => TypedHelpers.GetNodeAsValidType<PointerDCN<T>,T>(node)))
      {
        typedConnectedToNode.InternalRemoveNodeConnectionRequest(typedNode, ConnectionDirection.Any);
      }
      this.InternalRemoveNodeRequest(typedNode);
      typedNode.InternalDeleteParentCollectionRequest();
    }

    IEnumerable<IDirectedConnectedNode<T>> IDirectedConnectedNodeCollection<T>.GetNodesConnected(IDirectedConnectedNode<T> firstNode, ConnectionDirection direction)
    {
      Helpers<T>.Verify_GetNodesConnected_ConditionsAreSatisfied(firstNode, this);
      return firstNode.GetNodesConnected(direction);
    }

    bool IDirectedConnectedNodeCollection<T>.NodesHaveConnection(IDirectedConnectedNode<T> firstNode, IDirectedConnectedNode<T> secondNode, ConnectionDirection direction)
    {
      if(Helpers<T>.CheckNodesHaveConnectionConditions(firstNode, secondNode, this))
      { return false; }

      return firstNode.IsConnectedWith(secondNode, direction);
    }

    void IDirectedConnectedNodeCollection<T>.ConnectNodes(IDirectedConnectedNode<T> firstNode, IDirectedConnectedNode<T> secondNode, ConnectionDirection direction)
    {
      Helpers<T>.Verify_ConnectNodes_ConditionsAreSatisfied(firstNode, secondNode, this);
      var typedFirstNode = TypedHelpers.GetNodeAsValidType<PointerDCN<T>,T>(firstNode);
      var typedSecondNode = TypedHelpers.GetNodeAsValidType<PointerDCN<T>,T>(secondNode);

      typedFirstNode.InternalAddNodeConnectionRequest(typedSecondNode, direction);
      typedSecondNode.InternalAddNodeConnectionRequest(typedFirstNode, direction.Invert());
    }

    void IDirectedConnectedNodeCollection<T>.DisconnectNodes(IDirectedConnectedNode<T> firstNode, IDirectedConnectedNode<T> secondNode, ConnectionDirection direction)
    {
      if(Helpers<T>.CheckDisconnectNodesConditions(firstNode, secondNode, this))
      { return; }
      var typedFirstNode = TypedHelpers.GetNodeAsValidType<PointerDCN<T>,T>(firstNode);
      var typedSecondNode = TypedHelpers.GetNodeAsValidType<PointerDCN<T>,T>(secondNode);

      typedFirstNode.InternalRemoveNodeConnectionRequest(typedSecondNode, direction);
      typedSecondNode.InternalRemoveNodeConnectionRequest(typedFirstNode, direction.Invert());
    }
    #endregion

    #region //Add Pre-Connected Nodes Methods
    //void IDirectedConnectedNodeCollection<T>.AddNodeWithConnectionsToFrom(IDirectedConnectedNode<T> newNode, IEnumerable<IDirectedConnectedNode<T>> connectToNodes, IEnumerable<IDirectedConnectedNode<T>> connectFromNodes)
    //{
    //  (this as IDirectedConnectedNodeCollection<T>).AddNode(newNode);

    //  foreach( PointerDCN<T> toNode in connectToNodes)
    //  { (this as IDirectedConnectedNodeCollection<T>).ConnectNodesDirected(newNode, toNode); }
      
    //  foreach( PointerDCN<T> fromNode in connectFromNodes)
    //  { (this as IDirectedConnectedNodeCollection<T>).ConnectNodesDirected(fromNode, newNode); }
    //}

    //void IDirectedConnectedNodeCollection<T>.AddNodeWithConnectionsToFrom(IDirectedConnectedNode<T> newNode, IDirectedConnectedNode<T> connectToNode, IEnumerable<IDirectedConnectedNode<T>> connectFromNodes)
    //{
    //  (this as IDirectedConnectedNodeCollection<T>).AddNodeWithConnectionsToFrom(newNode, new[]{connectToNode}, connectFromNodes);
    //}

    //void IDirectedConnectedNodeCollection<T>.AddNodeWithConnectionsToFrom(IDirectedConnectedNode<T> newNode, IEnumerable<IDirectedConnectedNode<T>> connectToNodes, IDirectedConnectedNode<T> connectFromNode)
    //{
    //  (this as IDirectedConnectedNodeCollection<T>).AddNodeWithConnectionsToFrom(newNode, connectToNodes, new[]{connectFromNode});
    //}

    //void IDirectedConnectedNodeCollection<T>.AddNodeWithConnectionsToFrom(IDirectedConnectedNode<T> newNode, IDirectedConnectedNode<T> connectToNode, IDirectedConnectedNode<T> connectFromNode)
    //{
    //  (this as IDirectedConnectedNodeCollection<T>).AddNodeWithConnectionsToFrom(newNode, new[]{connectToNode}, new[]{connectFromNode});
    //}
    #endregion

    #region Internal Methods For Local-Only operations. No verification.
    internal void InternalRemoveNodeRequest(PointerDCN<T> targetNode)
    {
      pointerNodes.Remove(targetNode);
    }
    internal void InternalAddNodeRequest(PointerDCN<T> targetNode)
    {
      pointerNodes.Add(targetNode);
    }
    #endregion
  }

  internal class PointerDCN<T> : IDirectedConnectedNode<T>
  {
    public T Value {get; set;}

    public PointerDCNC<T> PointerCollection;
    private readonly List<PointerDCN<T>> connectedToNodesList = new List<PointerDCN<T>>();
    private readonly List<PointerDCN<T>> connectedFromNodesList = new List<PointerDCN<T>>();
    public string Name { get; set; }
#if DEBUG
    private Guid guid = Guid.NewGuid();
#endif

    #region Explicit IDirectedConnectedNode<T> Members
    IDirectedConnectedNodeCollection<T>  IDirectedConnectedNode<T>.ParentCollection
    {
      get
      { return PointerCollection; }
    }

    void IDirectedConnectedNode<T>.AddToCollection(IDirectedConnectedNodeCollection<T> newParentCollection)
    {
      Helpers<T>.Verify_AddToCollection_ConditionsAreSatisfied(this, newParentCollection);
      var typedCollection = TypedHelpers.GetCollectionAsValidType<PointerDCNC<T>,T>(newParentCollection);

      typedCollection.InternalAddNodeRequest(this);
      this.InternalSetParentCollectionRequest(typedCollection);
    }

    void IDirectedConnectedNode<T>.RemoveFromExistingCollection()
    {
      if (Helpers<T>.CheckWhether_RemoveFromExistingCollection_IsNeeded(this))
      {
        return;
      }

      PointerCollection.InternalRemoveNodeRequest(this);
      this.InternalDeleteParentCollectionRequest();
    }

    IEnumerable<IDirectedConnectedNode<T>> IDirectedConnectedNode<T>.GetNodesConnected(ConnectionDirection direction)
    {
      if(Helpers<T>.Check_GetNodesConnected_PreConditions(this))
      {
        return new List<PointerDCN<T>>();
      }

      switch (direction)
      {
        case ConnectionDirection.To:
          return connectedToNodesList;
        case ConnectionDirection.From:
          return connectedFromNodesList;
        case ConnectionDirection.Both:
          return Enumerable.Intersect(connectedToNodesList, connectedFromNodesList);
        case ConnectionDirection.Any:
          return Enumerable.Union(connectedToNodesList, connectedFromNodesList);
        default:
          throw new NonExistentEnumCaseException<ConnectionDirection>();  //ncrunch: no coverage
      }
    }
  
    bool IDirectedConnectedNode<T>.IsConnectedWith(IDirectedConnectedNode<T> secondNode, ConnectionDirection direction)
    {
      if(Helpers<T>.Check_IsConnectedWith_PreConditions(secondNode, this))
      {
        return false;
      }
      var typedNode = TypedHelpers.GetNodeAsValidType<PointerDCN<T>,T>(secondNode);


      switch (direction)
      {
        case ConnectionDirection.To:
          return connectedToNodesList.Contains(typedNode);
        case ConnectionDirection.From:
          return connectedFromNodesList.Contains(typedNode);
        case ConnectionDirection.Both:
          return connectedToNodesList.Contains(typedNode) &&
                 connectedFromNodesList.Contains(typedNode);
        case ConnectionDirection.Any:
          return connectedToNodesList.Contains(typedNode) ||
                 connectedFromNodesList.Contains(typedNode);
        default:
          throw new NonExistentEnumCaseException<ConnectionDirection>();  //ncrunch: no coverage
      }
    }

    void IDirectedConnectedNode<T>.AddConnectionWith(IDirectedConnectedNode<T> secondNode, ConnectionDirection direction)
    {
      var typedNode = TypedHelpers.GetNodeAsValidType<PointerDCN<T>,T>(secondNode);
      AddConnectionWith(typedNode, direction);
    }

    void IDirectedConnectedNode<T>.AddConnectionWith(IEnumerable<IDirectedConnectedNode<T>> secondNodes, ConnectionDirection direction)
    {
      Helpers<T>.VerifyNodeSetIsNotNull(secondNodes);

      foreach(PointerDCN<T> typedNode in secondNodes.Select(node => TypedHelpers.GetNodeAsValidType<PointerDCN<T>,T>(node)))
      {
        AddConnectionWith(typedNode, direction);
      }
    }
    
    void IDirectedConnectedNode<T>.RemoveConnectionWith(IDirectedConnectedNode<T> secondNode, ConnectionDirection direction)
    {
      var typedNode = TypedHelpers.GetNodeAsValidType<PointerDCN<T>,T>(secondNode);
      RemoveConnectionWith(typedNode, direction);
    }

    void IDirectedConnectedNode<T>.RemoveConnectionWith(IEnumerable<IDirectedConnectedNode<T>> secondNodes, ConnectionDirection direction)
    {
      if(secondNodes == null)
      { return; }

      foreach(PointerDCN<T> typedNode in secondNodes.Select(node => TypedHelpers.GetNodeAsValidType<PointerDCN<T>,T>(node)))
      {
        RemoveConnectionWith(typedNode, direction);
      }
    }
    #endregion
    
    #region Strongly typed Methods
    private void AddConnectionWith(PointerDCN<T> typedNode, ConnectionDirection direction)
    {
      Helpers<T>.Verify_AddConnectionWith_ConditionsAreSatisfied(typedNode, this);
      this.InternalAddNodeConnectionRequest(typedNode, direction);
      typedNode.InternalAddNodeConnectionRequest(this, direction.Invert());
    }

    private void RemoveConnectionWith(PointerDCN<T> typedNode, ConnectionDirection direction)
    {
      if(Helpers<T>.CheckWhether_RemoveConnectionWith_IsNeeded(typedNode, this))
      {
        return;
      }

      this.InternalRemoveNodeConnectionRequest(typedNode, direction);
      typedNode.InternalRemoveNodeConnectionRequest(this, direction.Invert());
    }
    #endregion

    #region Internal Methods For Local-Only operations. No verification
    internal void InternalSetParentCollectionRequest(PointerDCNC<T> targetCollection)
    {
      PointerCollection = targetCollection;
      connectedFromNodesList.Clear();
      connectedToNodesList.Clear();
    }

    internal void InternalDeleteParentCollectionRequest()
    {
      PointerCollection = null;
      connectedFromNodesList.Clear();
      connectedToNodesList.Clear();
    }

    internal void InternalRemoveNodeConnectionRequest(PointerDCN<T> typedTargetNode, ConnectionDirection direction)
    {
      switch (direction)
      {
        case ConnectionDirection.To:
          connectedToNodesList.Remove(typedTargetNode);
          break;
        case ConnectionDirection.From:
          connectedFromNodesList.Remove(typedTargetNode);
          break;
        case ConnectionDirection.Both:
          connectedToNodesList.Remove(typedTargetNode);
          connectedFromNodesList.Remove(typedTargetNode);
          break;
        case ConnectionDirection.Any:
          connectedToNodesList.Remove(typedTargetNode);
          connectedFromNodesList.Remove(typedTargetNode);
          break;
        default:
          throw new NonExistentEnumCaseException<ConnectionDirection>();  //ncrunch: no coverage
      }
    }

    internal void InternalAddNodeConnectionRequest(PointerDCN<T> typedTargetNode, ConnectionDirection direction)
    {
      switch (direction)
      {
        case ConnectionDirection.To:
          if (!connectedToNodesList.Contains(typedTargetNode))
          {
            connectedToNodesList.Add(typedTargetNode);
          }
          break;
        case ConnectionDirection.From:
          if (!connectedFromNodesList.Contains(typedTargetNode))
          {
            connectedFromNodesList.Add(typedTargetNode);
          }
          break;
        case ConnectionDirection.Both:
          if (!connectedToNodesList.Contains(typedTargetNode))
          {
            connectedToNodesList.Add(typedTargetNode);
          }
          if (!connectedFromNodesList.Contains(typedTargetNode))
          {
            connectedFromNodesList.Add(typedTargetNode);
          }
          break;
        case ConnectionDirection.Any:
          throw new InvalidOperationException("May not specify 'Any' Connection Direction when adding a new Connection.");
        default:
          throw new NonExistentEnumCaseException<ConnectionDirection>();  //ncrunch: no coverage
      }
    }
    #endregion
  }
}

using System.Collections.Generic;

namespace MDMUtils.DataStructures.Graphs.Base
{
  ///==========================================================================
  /// Class : IDirectedConnectedNodeCollection<T>
  ///
  /// <summary>
  ///   A collection of nodes, with values, and connections between other nodes
  ///   and directions on those connections.
  ///   Basically a class representing a simple unweighted Graph.
  /// </summary>
  /// <remarks>
  ///  Primary Methods.
  ///  Listed here for easy reading.
  ///    IEnumerable<IDirectedConnectedNode<T>> Nodes
  ///    void                                   AddNode            (IDirectedConnectedNode<T> newNode);
  ///    void                                   RemoveNode         (IDirectedConnectedNode<T> targetNode);
  ///    void                                   ConnectNodes       (IDirectedConnectedNode<T> firstNode, IDirectedConnectedNode<T> secondNode, ConnectionDirection direction);
  ///    void                                   DisconnectNodes    (IDirectedConnectedNode<T> firstNode, IDirectedConnectedNode<T> secondNode, ConnectionDirection direction);
  ///    IEnumerable<IDirectedConnectedNode<T>> GetNodesConnected  (IDirectedConnectedNode<T> node,      ConnectionDirection direction);
  ///    bool                                   NodesHaveConnection(IDirectedConnectedNode<T> firstNode, IDirectedConnectedNode<T> secondNode, ConnectionDirection direction);
  /// </remarks>
  ///==========================================================================
  internal interface IDirectedConnectedNodeCollection<T>
  {
    IEnumerable<IDirectedConnectedNode<T>> Nodes {get;}
    IDirectedConnectedNode<T> RandomNode { get; }
    void AddNode(IDirectedConnectedNode<T> newNode);
    void RemoveNode(IDirectedConnectedNode<T> targetNode);
    IDirectedConnectedNode<T> NewNode();
    IDirectedConnectedNode<T> NewNode(T value);
    
    ///========================================================================
    /// Method : GetNodesConnected
    /// <summary>Returns the set of Nodes connected with the given node in the given direction.</summary>
    /// <remarks>Does not accept nulls. Node must be in the collection.</remarks>
    /// <param name="firstNode">Any non-null node in the collection.</param>
    /// <param name="direction">A connection direction.</param>
    /// <value>Set of connected nodes.</value>
    ///========================================================================
    IEnumerable<IDirectedConnectedNode<T>> GetNodesConnected(IDirectedConnectedNode<T> firstNode, ConnectionDirection direction);
    ///========================================================================
    /// Method : NodesHaveConnection
    /// <summary>Returns the boolean indicating whether the specified nodes have the specified connection.</summary>
    /// <remarks>Does not accept nulls. Nodes must be in the collection.</remarks>
    /// <param name="firstNode">Any non-null node in the collection.</param>
    /// <param name="secondNode">Any non-null node in the collection.</param>
    /// <param name="direction">A connection direction.</param>
    /// <value>Are the nodes connected.</value>
    ///========================================================================
    bool NodesHaveConnection(IDirectedConnectedNode<T> firstNode, IDirectedConnectedNode<T> secondNode, ConnectionDirection direction);
    ///========================================================================
    /// Method : ConnectNodes 
    /// <summary>Creates the specified connection, between the specified nodes.</summary>
    /// <remarks>Does not accept nulls. Nodes must be in the collection. Connection direction must be explicit.</remarks>
    /// <param name="firstNode">Any non-null node in the collection.</param>
    /// <param name="secondNode">Any non-null node in the collection.</param>
    /// <param name="direction">An explicit connection direction. "Any" is not acceptable.</param>
    ///========================================================================
    void ConnectNodes(IDirectedConnectedNode<T> firstNode, IDirectedConnectedNode<T> secondNode, ConnectionDirection direction);
    ///========================================================================
    /// Method : DisconnectNodes 
    /// <summary>Removes the specified connection, between the specified nodes.
    ///          Completes with no exception and no action if the connection does not exist.</summary>
    /// <remarks>Accepts nulls, unrelated and unconnected Nodes</remarks>
    /// <param name="firstNode">Any node, null or otherwise.</param>
    /// <param name="secondNode">Any node, null or otherwise.</param>
    /// <param name="direction">A connection direction.</param>
    ///========================================================================
    void DisconnectNodes(IDirectedConnectedNode<T> firstNode, IDirectedConnectedNode<T> secondNode, ConnectionDirection direction);

    //#region Add Pre-Connected Nodes Methods
    //void AddNodeWithConnectionsToFrom(IDirectedConnectedNode<T> newNode, IEnumerable<IDirectedConnectedNode<T>> connectToNodes, IEnumerable<IDirectedConnectedNode<T>> connectFromNodes);
    //void AddNodeWithConnectionsToFrom(IDirectedConnectedNode<T> newNode, IDirectedConnectedNode<T> connectToNode              , IEnumerable<IDirectedConnectedNode<T>> connectFromNodes);
    //void AddNodeWithConnectionsToFrom(IDirectedConnectedNode<T> newNode, IEnumerable<IDirectedConnectedNode<T>> connectToNodes, IDirectedConnectedNode<T> connectFromNode);
    //void AddNodeWithConnectionsToFrom(IDirectedConnectedNode<T> newNode, IDirectedConnectedNode<T> connectToNode              , IDirectedConnectedNode<T> connectFromNode);
    //#endregion
  }
}
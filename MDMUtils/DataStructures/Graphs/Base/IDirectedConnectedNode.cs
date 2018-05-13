using System.Collections.Generic;

namespace MDMUtils.DataStructures.Graphs.Base
{
  ///==========================================================================
  /// Class : IDirectedConnectedNode<T>
  ///
  /// <summary>
  ///   A node in a collection with a value, and a set of connections to other
  ///   nodes in the collection, and directions on those connections.
  ///   Basically a class representing a node in simple unweighted Graph.
  /// </summary>
  /// <remarks>
  ///  Primary Methods.
  ///  Listed here for easy reading.
  ///     T                                      Value
  ///     IDirectedConnectedNodeCollection<T>    ParentCollection
  ///     IEnumerable<IDirectedConnectedNode<T>> GetNodesConnected   (ConnectionDirection direction);
  ///     bool                                   IsConnectedWith     (IDirectedConnectedNode<T> node, ConnectionDirection direction);
  ///     void                                   AddConnectionWith   (IDirectedConnectedNode<T> node, ConnectionDirection direction);
  ///     void                                   RemoveConnectionWith(IDirectedConnectedNode<T> node, ConnectionDirection direction);
  /// </remarks>
  ///==========================================================================
  internal interface IDirectedConnectedNode<T>
  {
    T Value {get; set;}
    IDirectedConnectedNodeCollection<T> ParentCollection {get;}

    void AddToCollection(IDirectedConnectedNodeCollection<T> newParentCollection);
    void RemoveFromExistingCollection();

    ///========================================================================
    /// Method : GetNodesConnected
    /// <summary>Returns the set of Nodes connected with the given node in the given direction.</summary>
    /// <param name="direction">A connection direction.</param>
    /// <value>A non-null set of connected nodes.</value>
    ///========================================================================
    IEnumerable<IDirectedConnectedNode<T>> GetNodesConnected(ConnectionDirection direction);
    ///========================================================================
    /// Method : IsConnectedWith
    /// <summary>Returns the boolean indicating whether the specified nodes have the specified connection.</summary>
    /// <remarks>Does not accept nulls. Nodes must be in the collection.</remarks>
    /// <param name="secondNode">Any non-null node in the collection.</param>
    /// <param name="direction">A connection direction.</param>
    /// <value>Are the nodes connected.</value>
    ///========================================================================
    bool IsConnectedWith(IDirectedConnectedNode<T> secondNode, ConnectionDirection direction);
    ///========================================================================
    /// Method : AddConnectionWith 
    /// <summary>Creates the specified connection, with the specified node.</summary>
    /// <remarks>Does not accept nulls. Nodes must be in the collection. Connection direction must be explicit.</remarks>
    /// <param name="secondNode">Any non-null node in the collection.</param>
    /// <param name="direction">An explicit connection direction. "Any" is not acceptable.</param>
    ///========================================================================
    void AddConnectionWith(IDirectedConnectedNode<T> secondNode, ConnectionDirection direction);
    ///========================================================================
    /// Method : AddConnectionWith 
    /// <summary>Creates the specified connection, with each of the nodes in the given list.</summary>
    /// <remarks>All nodes must be non-null and in the collection. Connection direction must be explicit.</remarks>
    /// <param name="secondNodes">Set of non-null nodes in the collection.</param>
    /// <param name="direction">An explicit connection direction. "Any" is not acceptable.</param>
    ///========================================================================
    void AddConnectionWith(IEnumerable<IDirectedConnectedNode<T>> secondNodes, ConnectionDirection direction);
    ///========================================================================
    /// Method : RemoveConnectionWith 
    /// <summary>Removes the specified connection, from the specified node.
    ///          Completes with no exception and no action if the connection does not exist.</summary>
    /// <remarks>Accepts nulls, unrelated and unconnected Nodes</remarks>
    /// <param name="secondNode">Any node, null or otherwise.</param>
    /// <param name="direction">A connection direction.</param>
    ///========================================================================
    void RemoveConnectionWith(IDirectedConnectedNode<T> secondNode, ConnectionDirection direction);
    ///========================================================================
    /// Method : RemoveConnectionWith 
    /// <summary>Removes the specified connection, from each of the nodes in the given list.
    ///          For connections that do not exist, no action is taken and no exception is thrown.</summary>
    /// <remarks>Accepts nulls, unrelated and unconnected Nodes</remarks>
    /// <param name="secondNodes">Set of any nodes, null or otherwise.</param>
    /// <param name="direction">A connection direction.</param>
    ///========================================================================
    void RemoveConnectionWith(IEnumerable<IDirectedConnectedNode<T>> secondNodes, ConnectionDirection direction);
    string Name { get; set; }
  }
}

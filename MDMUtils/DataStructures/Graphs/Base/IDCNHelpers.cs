using System;
using System.Collections.Generic;

namespace MDMUtils.DataStructures.Graphs.Base
{
  internal class Helpers<T>
  {
    #region Validators
    internal static void VerifyNodesAreInSameCollection(IDirectedConnectedNode<T> firstNode, IDirectedConnectedNode<T> secondNode)
    {
      if (NodesAreNotInSameCollection(firstNode, secondNode))
      {
        throw new InvalidOperationException("Operation is not valid on a node in a different Collection.");
      }
    }

    internal static void VerifyNodeIsNotNull(IDirectedConnectedNode<T> node)
    {
      if (node == null)
      {
        throw new ArgumentNullException("node", "Operation is not valid on a null node.");
      }
    }

    internal static void VerifyNodesAreNotTheSameNode(IDirectedConnectedNode<T> firstNode, IDirectedConnectedNode<T> secondNode)
    {
      if (firstNode == secondNode)
      {
        throw new InvalidOperationException("Self Connections are not permitted.");
      }
    }

    internal static void VerifyNodeIsInSomeCollection(IDirectedConnectedNode<T> node)
    {
      if (NodeIsNotInAnyCollection(node))
      {
        throw new InvalidOperationException("Operation is not valid when node is not in a collection.");
      }
    }

    internal static void VerifyNodeIsNotInAnyCollection(IDirectedConnectedNode<T> node)
    {
      if (!NodeIsNotInAnyCollection(node))
      {
        throw new InvalidOperationException("Node already has a parent Collection");
      }
    }

    internal static void VerifyNodeIsInCollection(IDirectedConnectedNode<T> node, IDirectedConnectedNodeCollection<T> collection)
    {
      if (NodeIsNotInCollection(node, collection))
      {
        throw new InvalidOperationException("Operation is not valid on a node in a different Collection.");
      }
    }

    internal static void VerifyAtLeastOneNodeIsInCollection(
      IDirectedConnectedNode<T> firstNode,
      IDirectedConnectedNode<T> secondNode,
      IDirectedConnectedNodeCollection<T> collection)
    {
      if (NodeIsNotInCollection(firstNode, collection) && NodeIsNotInCollection(secondNode, collection))
      {
        throw new InvalidOperationException("Cannot report on connection between two nodes not in collection.");
      }
    }

    internal static void VerifyCollectionIsNotNull(IDirectedConnectedNodeCollection<T> collection)
    {
      if (collection == null)
      {
        throw new ArgumentNullException("collection", "Operation is not valid on a null set of nodes.");
      }
    }

    internal static void VerifyNodeSetIsNotNull(IEnumerable<IDirectedConnectedNode<T>> nodes)
    {
      if (nodes == null)
      {
        throw new ArgumentNullException("nodes", "Operation is not valid on a null node collection.");
      }
    }
    #endregion

    #region Tests
    private static bool NodeIsNotInCollection(IDirectedConnectedNode<T> node, IDirectedConnectedNodeCollection<T> collection)
    {
      return node.ParentCollection != collection;
    }

    private static bool NodeIsNotInAnyCollection(IDirectedConnectedNode<T> node)
    {
      return node.ParentCollection == null;
    }

    private static bool NodesAreNotInSameCollection(IDirectedConnectedNode<T> firstNode, IDirectedConnectedNode<T> secondNode)
    {
      return firstNode.ParentCollection != secondNode.ParentCollection;
    }
    #endregion

    #region ThrowingOperationConditions
    internal static void Verify_AddNode_ConditionsAreSatisfied(IDirectedConnectedNode<T> node)
    {
      VerifyNodeIsNotNull(node);
      VerifyNodeIsNotInAnyCollection(node);
    }

    internal static void Verify_GetNodesConnected_ConditionsAreSatisfied(IDirectedConnectedNode<T> node, IDirectedConnectedNodeCollection<T> collection)
    {
      VerifyNodeIsNotNull(node);
      VerifyNodeIsInCollection(node, collection);
    }

    internal static void Verify_ConnectNodes_ConditionsAreSatisfied(IDirectedConnectedNode<T> firstNode, IDirectedConnectedNode<T> secondNode, IDirectedConnectedNodeCollection<T> collection)
    {
      VerifyNodeIsNotNull(firstNode);
      VerifyNodeIsNotNull(secondNode);

      VerifyNodeIsInCollection(firstNode, collection);
      VerifyNodeIsInCollection(secondNode, collection);

      VerifyNodesAreNotTheSameNode(firstNode, secondNode);
    }

    internal static void Verify_AddToCollection_ConditionsAreSatisfied(IDirectedConnectedNode<T> node, IDirectedConnectedNodeCollection<T> collection)
    {
      VerifyCollectionIsNotNull(collection);
      VerifyNodeIsNotInAnyCollection(node);
    }

    internal static void Verify_AddConnectionWith_ConditionsAreSatisfied(IDirectedConnectedNode<T> firstNode, IDirectedConnectedNode<T> secondNode)
    {
      VerifyNodeIsNotNull(firstNode);
      VerifyNodeIsInSomeCollection(firstNode);
      VerifyNodesAreInSameCollection(firstNode, secondNode);
      VerifyNodesAreNotTheSameNode(firstNode, secondNode);
    }
    
    #endregion

    #region ReturningOperationConditions
    internal static bool CheckWhether_RemoveNode_IsNeeded(IDirectedConnectedNode<T> node, IDirectedConnectedNodeCollection<T> collection)
    {
      return node == null || NodeIsNotInCollection(node, collection);
    }

    internal static bool CheckWhether_RemoveConnectionWith_IsNeeded(IDirectedConnectedNode<T> firstNode, IDirectedConnectedNode<T> secondNode)
    {
      return firstNode == null ||
             NodeIsNotInAnyCollection(firstNode) ||
             NodesAreNotInSameCollection(firstNode, secondNode);
    }

    internal static bool CheckWhether_RemoveFromExistingCollection_IsNeeded(IDirectedConnectedNode<T> node)
    {
      return NodeIsNotInAnyCollection(node);
    }

    internal static bool Check_IsConnectedWith_PreConditions(IDirectedConnectedNode<T> firstNode, IDirectedConnectedNode<T> secondNode)
    {
      return firstNode == null ||
             secondNode == null ||
             NodeIsNotInAnyCollection(firstNode) ||
             NodeIsNotInAnyCollection(secondNode) ||
             NodesAreNotInSameCollection(firstNode, secondNode);
    }

    internal static bool Check_GetNodesConnected_PreConditions(IDirectedConnectedNode<T> node)
    {
      return NodeIsNotInAnyCollection(node);
    }
    #endregion
  
    internal static bool CheckDisconnectNodesConditions(IDirectedConnectedNode<T> firstNode, IDirectedConnectedNode<T> secondNode, IDirectedConnectedNodeCollection<T> collection)
    {
      if (firstNode == null || secondNode == null)
      {
        return true;
      }

      VerifyAtLeastOneNodeIsInCollection(firstNode, secondNode, collection);

      if (NodeIsNotInCollection(firstNode, collection) || NodeIsNotInCollection(secondNode, collection))
      {
        return true;
      }

      return false;
    }
  
    internal static bool CheckNodesHaveConnectionConditions(IDirectedConnectedNode<T> firstNode, IDirectedConnectedNode<T> secondNode, IDirectedConnectedNodeCollection<T> collection)
    {
      if (firstNode == null || secondNode == null)
      {
        return true;
      }

      VerifyAtLeastOneNodeIsInCollection(firstNode, secondNode, collection);

      if (NodeIsNotInCollection(firstNode, collection) || NodeIsNotInCollection(secondNode, collection))
      {
        return true;
      }

      return false;
    }
  }

  internal class TypedHelpers
  {
    internal static S GetCollectionAsValidType<S,T>(IDirectedConnectedNodeCollection<T> collection) where S : class, IDirectedConnectedNodeCollection<T>
    {
      if (collection == null)
      {
        return null;
      }

      var typedCollection = collection as S;

      if (typedCollection == null)
      {
        throw new InvalidCastException($"Given collection is not a '{typeof(S)}' collection. Collection Type is actually: '{collection.GetType()}'");
      }

      return typedCollection;
    }

    internal static S GetNodeAsValidType<S,T>(IDirectedConnectedNode<T> node) where S : class, IDirectedConnectedNode<T>
    {
      if (node == null)
      {
        return null;
      }

      var typedNode = node as S;

      if (typedNode == null)
      {
        throw new InvalidCastException($"Given node is not a '{typeof(S)}' node. Node Type is actually: '{node.GetType()}'");
      }

      return typedNode;
    }
  }


}

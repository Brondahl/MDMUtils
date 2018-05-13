using System;
using System.Linq;
using MDMUtils.DataStructures.Graphs.Base;

namespace MDMUtils.DataStructures.Graphs
{
  public class LinkedList<S,T> where S : IEquatable<S>
  {
    public class Node
    {
      internal IDirectedConnectedNode<Node> BaseNode;
      public S Identifier;
      public T Value;

      public Node (S id, T value, LinkedList<S, T> parentList)
      {
        Identifier = id;
        Value = value;
        BaseNode = parentList.baseCollection.NewNode(this);
      }

      public Node NextNode
      {
        get
        {
          var underlyingNextNode = BaseNode.GetNodesConnected(ConnectionDirection.To).SingleOrDefault();
          
          return underlyingNextNode == null ? null : underlyingNextNode.Value;
        }
      }

      public LinkedList<S,T> AsNewLinkedList()
      {
        var lRet = new LinkedList<S, T>();
        var currentNode = this;

        do
        {
          lRet.InsertAtEnd(new Node(currentNode.Identifier, currentNode.Value, lRet));
          currentNode = currentNode.NextNode;
        } while (currentNode.NextNode != null);

        return lRet;
      }
    }

    public Node RootNode { get; private set; }

    public Node Search(S targetIdentifier)
    {
      var currentNode = RootNode;

      while(currentNode != null &&
            !currentNode.Identifier.Equals(targetIdentifier))
      {
        currentNode = currentNode.NextNode;
      }

      return currentNode;
    }

    public Node FindParent(Node childNode)
    {
      if(childNode == RootNode)
      {
        return null;
      }

      var currentNode = RootNode;
      while(!currentNode.NextNode.Equals(childNode))
      {
        currentNode = currentNode.NextNode;
      }

      return currentNode;
    }

    public void InsertAtStart(Node newNode)
    {
      if (RootNode != null)
      {
        var newBase = newNode.BaseNode;
        var oldRootBase = RootNode.BaseNode;
        baseCollection.ConnectNodes(newBase, oldRootBase, ConnectionDirection.To);
      }
      RootNode = newNode;
    }

    public void InsertAtEnd(Node newNode)
    {
      if(RootNode == null)
      {
        RootNode = newNode;
      }
      else
      {
        var newBase = newNode.BaseNode;
        var finalNode = FindParent(null);
        var finalBase = finalNode.BaseNode;

        baseCollection.ConnectNodes(finalBase, newBase, ConnectionDirection.To);
      }
    }

    public void InsertAfterNode(Node newNode, Node precedingNode)
    {
      InsertBetween(newNode, precedingNode, precedingNode.NextNode);
    }

    public void InsertBeforeNode(Node newNode, Node followingNode)
    {
      var parentNode = FindParent(followingNode);
      InsertBetween(newNode, parentNode, followingNode);
    }

    private void InsertBetween(Node newNode, Node precedingNode, Node followingNode)
    {
      var newBase = newNode.BaseNode;
      var precedingBase = precedingNode.BaseNode;
      var followingBase = followingNode.BaseNode;

      baseCollection.DisconnectNodes(precedingBase, followingBase, ConnectionDirection.To);
      baseCollection.ConnectNodes(precedingBase, newBase, ConnectionDirection.To);
      baseCollection.ConnectNodes(newBase, followingBase, ConnectionDirection.To);
    }

    public void Delete(S targetIdentifier)
    {
      Delete(Search(targetIdentifier));
    }

    public void Delete(Node targetNode)
    {
      DeleteChild(FindParent(targetNode));
    }

    private void DeleteChild(Node parentNode)
    {
      var parentBase = parentNode.BaseNode;
      var childBase = parentNode.NextNode.BaseNode;
      var grandChildBase = parentNode.NextNode.NextNode.BaseNode;

      baseCollection.RemoveNode(childBase);
      baseCollection.ConnectNodes(parentBase, grandChildBase, ConnectionDirection.To);
    }

    private readonly IDirectedConnectedNodeCollection<Node> baseCollection = IDCNCFactory.NewPointerCollection<Node>();
  }
}

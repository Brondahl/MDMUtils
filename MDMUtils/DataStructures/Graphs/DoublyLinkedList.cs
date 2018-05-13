using System;
using System.Linq;
using MDMUtils.DataStructures.Graphs.Base;

namespace MDMUtils.DataStructures.Graphs
{
  public class DoublyLinkedList<S,T> where S : IEquatable<S>
  {
    public class Node
    {
      internal IDirectedConnectedNode<Node> BaseNode;
      public S Identifier;
      public T Value;

      public Node (S id, T value, DoublyLinkedList<S, T> parentList)
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

      public Node PreviousNode
      {
        get
        {
          var underlyingPreviousNode = BaseNode.GetNodesConnected(ConnectionDirection.From).SingleOrDefault();
          
          return underlyingPreviousNode == null ? null : underlyingPreviousNode.Value;
        }
      }

      public DoublyLinkedList<S,T> AsNewDoublyLinkedList()
      {
        var lRet = new DoublyLinkedList<S, T>();
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
    public Node TailNode { get; private set; }

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

    public void InsertAtStart(Node newNode)
    {
      if (RootNode == null)
      {
        RootNode = newNode;
        TailNode = newNode;
      }
      else
      {
        baseCollection.ConnectNodes(newNode.BaseNode, RootNode.BaseNode, ConnectionDirection.Both);
        RootNode = newNode;
      }
    }

    public void InsertAtEnd(Node newNode)
    {
      if(RootNode == null)
      {
        RootNode = newNode;
        TailNode = newNode;
      }
      else
      {
        baseCollection.ConnectNodes(TailNode.BaseNode, newNode.BaseNode, ConnectionDirection.Both);
        TailNode = newNode;
      }
    }

    public void InsertAfterNode(Node newNode, Node precedingNode)
    {
      InsertBetween(newNode, precedingNode, precedingNode.NextNode);
    }

    public void InsertBeforeNode(Node newNode, Node followingNode)
    {
      InsertBetween(newNode, followingNode.PreviousNode, followingNode);
    }

    private void InsertBetween(Node newNode, Node precedingNode, Node followingNode)
    {
      var newBase = newNode.BaseNode;
      var precedingBase = precedingNode.BaseNode;
      var followingBase = followingNode.BaseNode;

      baseCollection.DisconnectNodes(precedingBase, followingBase, ConnectionDirection.Both);
      baseCollection.ConnectNodes(precedingBase, newBase, ConnectionDirection.Both);
      baseCollection.ConnectNodes(newBase, followingBase, ConnectionDirection.Both);
    }

    public void Delete(S targetIdentifier)
    {
      Delete(Search(targetIdentifier));
    }

    public void Delete(Node targetNode)
    {
      var parentBase = targetNode.PreviousNode.BaseNode;
      var targetBase = targetNode.BaseNode;
      var childBase = targetNode.NextNode.BaseNode;

      baseCollection.RemoveNode(targetBase);
      baseCollection.ConnectNodes(parentBase, childBase, ConnectionDirection.Both);
    }

    private readonly IDirectedConnectedNodeCollection<Node> baseCollection = IDCNCFactory.NewPointerCollection<Node>();
  }
}

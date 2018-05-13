//using System;
//using System.Linq;
//using MDMUtils.DataStructures.Base;

//namespace MDMUtils.DataStructures
//{
//  public class BinaryTree<S,T> where S : IEquatable<S>
//  {
//    public class Node
//    {
//      public Node (S id, T value, BinaryTree<S, T> parentTree)
//      {
//        Identifier = id;
//        Value = value;
//        BaseNode = parentTree.baseCollection.NewNode(this);
//      }

//      internal IDirectedConnectedNode<Node> BaseNode;
//      internal bool IsLeft;

//      public S Identifier;
//      public T Value;

//      public Node ParentNode
//      {
//        get
//        {
//          var underlyingParentNode = BaseNode.GetNodesConnected(ConnectionDirection.From).SingleOrDefault();
          
//          return underlyingParentNode == null ? null : underlyingParentNode.Value;
//        }
//      }

//      public Tuple<Node,Node> ChildNodes
//      {
//        get
//        {
//          var underlyingChildNodes = BaseNode.GetNodesConnected(ConnectionDirection.To);

//          Node leftChild = null;
//          Node rightChild = null;
//          if (underlyingChildNodes.Any())
//          {
//            leftChild = underlyingChildNodes.SingleOrDefault(child => child.Value.IsLeft).Value;
//            rightChild = underlyingChildNodes.SingleOrDefault(child => !child.Value.IsLeft).Value;
//          }

//          return new Tuple<Node, Node>(leftChild, rightChild);
//        }
//      }

//      public Node LeftNode
//      {
//        get { return ChildNodes.Item1; }
//      }

//      public Node RightNode
//      {
//        get { return ChildNodes.Item2; }
//      }

//      public BinaryTree<S,T> AsNewBinaryTree()
//      {
//        var lRet = new BinaryTree<S, T>();
//        var currentNode = this;

//        //TODO: Put some recursive stuff here.
//        throw new NotSupportedException("Not sure whether I will ever implement this, actually.");
//        do
//        {
//          lRet.InsertAtEnd(new Node(currentNode.Identifier, currentNode.Value, lRet));
//          currentNode = currentNode.NextNode;
//        } while (currentNode.NextNode != null);

//        return lRet;
//      }

//      internal Node RecursiveSearch (S targetIdentifier)
//      {
//        if(Identifier.Equals(targetIdentifier))
//        {
//          return this;
//        }

//        return LeftNode.RecursiveSearch(targetIdentifier) ?? RightNode.RecursiveSearch(targetIdentifier);
//      }
//    }

//    public Node RootNode { get; private set; }

//    public Node Search(S targetIdentifier)
//    {
//      return RootNode.RecursiveSearch(targetIdentifier);
//    }

//    public void InsertAtStart(Node newNode)
//    {
//      if (RootNode == null)
//      {
//        RootNode = newNode;
//        TailNode = newNode;
//      }
//      else
//      {
//        baseCollection.ConnectNodes(newNode.BaseNode, RootNode.BaseNode, ConnectionDirection.Both);
//        RootNode = newNode;
//      }
//    }

//    public void InsertAtEnd(Node newNode)
//    {
//      if(RootNode == null)
//      {
//        RootNode = newNode;
//        TailNode = newNode;
//      }
//      else
//      {
//        baseCollection.ConnectNodes(TailNode.BaseNode, newNode.BaseNode, ConnectionDirection.Both);
//        TailNode = newNode;
//      }
//    }

//    public void InsertAfterNode(Node newNode, Node precedingNode)
//    {
//      InsertBetween(newNode, precedingNode, precedingNode.NextNode);
//    }

//    public void InsertBeforeNode(Node newNode, Node followingNode)
//    {
//      InsertBetween(newNode, followingNode.PreviousNode, followingNode);
//    }

//    private void InsertBetween(Node newNode, Node precedingNode, Node followingNode)
//    {
//      var newBase = newNode.BaseNode;
//      var precedingBase = precedingNode.BaseNode;
//      var followingBase = followingNode.BaseNode;

//      baseCollection.DisconnectNodes(precedingBase, followingBase, ConnectionDirection.Both);
//      baseCollection.ConnectNodes(precedingBase, newBase, ConnectionDirection.Both);
//      baseCollection.ConnectNodes(newBase, followingBase, ConnectionDirection.Both);
//    }

//    public void Delete(S targetIdentifier)
//    {
//      Delete(Search(targetIdentifier));
//    }

//    public void Delete(Node targetNode)
//    {
//      var parentBase = targetNode.PreviousNode.BaseNode;
//      var targetBase = targetNode.BaseNode;
//      var childBase = targetNode.NextNode.BaseNode;

//      baseCollection.RemoveNode(targetBase);
//      baseCollection.ConnectNodes(parentBase, childBase, ConnectionDirection.Both);
//    }

//    private readonly IDirectedConnectedNodeCollection<Node> baseCollection = IDCNCFactory.NewPointerCollection<Node>();
//  }
//}

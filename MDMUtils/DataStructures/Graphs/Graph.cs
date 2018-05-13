//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using MDMUtils.DataStructures.Base;

//namespace MDMUtils.DataStructures
//{
//  internal class Graph<T>
//  {
//    private IDirectedConnectedNodeCollection<T> UnderlyingFramework;
//    public List<Node> Nodes = new List<Node>();

//    public bool ContainsNode(Node xiNode)
//    {
//      return Nodes.Contains(xiNode);
//    }

//    public bool ContainsValue(T xiValue)
//    {
//      if(!(xiValue is IEquatable<T>))
//      {
//        string lErrorMessage =
//          String.Format(
//            "ContainsValue may only be called if the Underlying type is IEquatable. The underlying type is {0}",
//            xiValue.GetType());
//        throw new InvalidOperationException(lErrorMessage);
//      }
//      return Nodes.Any(tNode => tNode.Value.Equals(xiValue));
//    }

//    public class Node
//    {
//      public T Value { get; set; }
//      public List<Node> Children { get { return mChildren; } }
//      public List<Node> Parents { get { return mParents; } }

//      private Graph<T> mParentGraph; 
//      private List<Node> mChildren = new List<Node>();
//      private List<Node> mParents = new List<Node>();


//      public Node(Graph<T> xiOwningGraph )
//      {
//        Value = default(T);
//        mParentGraph = xiOwningGraph;
//      }

//      public Node(T xiValue)
//      {
//        Value = xiValue;
//      }

//      internal void AddChild(Node xiChild)
//      {
//        mChildren.Add(xiChild);
//        mParentGraph.AddNodeIfNeeded(xiChild);
//      }

//      internal void AddParent(Node xiParent)
//      {
//        mParents.Add(xiParent);
//        mParentGraph.AddNodeIfNeeded(xiParent);
//      }

//      public void AttachChild(Node xiChild)
//      {
//        this.AddChild(xiChild);
//        xiChild.AddParent(this);
//      }

//      public void AttachChildren(IEnumerable<Node> xiChildren)
//      {
//        foreach (var lChild in xiChildren)
//        {
//          AttachChild(lChild);
//        }
//      }

//      public void AttachToParent(Node xiParent)
//      {
//        xiParent.AttachChild(this);
//      }

//      public void AttachToParents(IEnumerable<Node> xiParents)
//      {
//        foreach (var lParent in xiParents)
//        {
//          lParent.AttachChild(this);
//        }
//      }
//    }
//  }
//}
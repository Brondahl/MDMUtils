using System;

namespace MDMUtils.DataStructures
{
  ///==========================================================
  /// Class : EquatableOrderedPair
  /// 
  /// <summary>
  ///   Represents a pair of values with a defined order, which
  ///   can be compared for equality.
  /// </summary>
  ///==========================================================
  public abstract class OrderedEquatablePair<T> : OrderedEquatablePair<T,T> where T : IEquatable<T>
  {
    protected OrderedEquatablePair() : base() {}
    protected OrderedEquatablePair(T x, T y) : base(x, y) {}
  }

  public abstract class OrderedEquatablePair<S,T> : Pair<S,T>, IEquatable<OrderedEquatablePair<S,T>> where S : IEquatable<S> where T : IEquatable<T>
  {
    protected OrderedEquatablePair() : base() {}
    protected OrderedEquatablePair(S x, T y) : base(x, y) {}

    #region IEquatable
    public override int GetHashCode()
    { return base.GetHashCode(); }

    public virtual bool Equals(OrderedEquatablePair<S,T> otherPoint)
    {
      if (ReferenceEquals(otherPoint, null)) { return false; }
      if (GetType() != otherPoint.GetType()) { return false; }

      var xMatches = X.Equals(otherPoint.X);
      var yMatches = Y.Equals(otherPoint.Y);
      return xMatches && yMatches;
    }

    public override bool Equals(object obj)
    { return Equals(obj as OrderedEquatablePair<S,T>); }

    public static bool operator ==(OrderedEquatablePair<S,T> left, OrderedEquatablePair<S,T> right)
    {
      if (ReferenceEquals(left, null))
      { return ReferenceEquals(right, null); }

      return left.Equals(right);
    }

    public static bool operator !=(OrderedEquatablePair<S,T> left, OrderedEquatablePair<S,T> right)
    { return !(left == right); }

    public static bool operator ==(OrderedEquatablePair<S,T> left, object right)
    { return left == (right as OrderedEquatablePair<S, T>); }

    public static bool operator !=(OrderedEquatablePair<S,T> left, object right)
    { return left != (right as OrderedEquatablePair<S, T>); }
    #endregion
  }
}

using System;

namespace MDMUtils.DataStructures
{
  ///==========================================================
  /// Class : UnorderedEquatablePair
  /// 
  /// <summary>
  ///   Representing an unordered pair of Items.
  ///   Items must be IEquatable.
  /// </summary>
  ///==========================================================
  public abstract class UnorderedEquatablePair<T> : Pair<T,T>,IEquatable<UnorderedEquatablePair<T>> where T : IEquatable<T>
  {
    protected UnorderedEquatablePair() {}
    protected UnorderedEquatablePair(T x, T y) : base(x,y) {}
    
    public override int GetHashCode()
    { return base.GetHashCode(); }

    public override bool Equals(object obj)
    { return Equals(obj as UnorderedEquatablePair<T>); }

    public virtual bool Equals(UnorderedEquatablePair<T> otherPair)
    {
      if (ReferenceEquals(otherPair, null)) { return false; }
      if (GetType() != otherPair.GetType()) { return false; }

      T itemA1 = X;
      T itemA2 = Y;
      T itemB1 = otherPair.X;
      T itemB2 = otherPair.Y;

      var pairMatchesInSameOrder = itemA1.Equals(itemB1)  &&  itemA2.Equals(itemB2);
      var pairMatchesInReversedOrder = itemA1.Equals(itemB2)  &&  itemA2.Equals(itemB1);

      return (pairMatchesInSameOrder || pairMatchesInReversedOrder);
    }

    public static bool operator ==(UnorderedEquatablePair<T> left, UnorderedEquatablePair<T> right)
    {
      if (ReferenceEquals(left, null))
      { return ReferenceEquals(right, null); }

      return left.Equals(right);
    }

    public static bool operator !=(UnorderedEquatablePair<T> left, UnorderedEquatablePair<T> right)
    { return !(left == right); }

    public static bool operator ==(UnorderedEquatablePair<T> left, object right)
    { return left == (right as UnorderedEquatablePair<T>); }

    public static bool operator !=(UnorderedEquatablePair<T> left, object right)
    { return left != (right as UnorderedEquatablePair<T>); }

  }
}

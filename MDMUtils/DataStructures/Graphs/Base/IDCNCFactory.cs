namespace MDMUtils.DataStructures.Graphs.Base
{
  internal static class IDCNCFactory
  {
    public static IDirectedConnectedNodeCollection<T> NewPointerCollection<T>()
    {
      return new PointerDCNC<T>();
    }

    public static IDirectedConnectedNodeCollection<T> NewArrayCollection<T>()
    {
      return new ArrayDCNC<T>();
    }
  }
}

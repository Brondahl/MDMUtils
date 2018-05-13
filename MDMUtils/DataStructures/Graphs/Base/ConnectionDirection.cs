using System;
using System.Collections.Generic;
using System.Linq;

namespace MDMUtils.DataStructures.Graphs.Base
{
  internal enum ConnectionDirection
  {
    Any = 0,
    To = 1,
    From = 2,
    Both = 3,
  }
  
  internal static class Extensions
  {        
    internal static ConnectionDirection Invert(this ConnectionDirection direction)
    {
      switch (direction)
      {
        case ConnectionDirection.To:
          return ConnectionDirection.From;

        case ConnectionDirection.From:
          return ConnectionDirection.To;

        default:
          return direction;
      }
    }
  }

  internal class EnumAccessor
  {
    internal static IEnumerable<ConnectionDirection> ConnectionDirections
    {
      get
      {
        return Enum.GetValues(typeof (ConnectionDirection))
                   .Cast<ConnectionDirection>();
      }
    }

    internal static IEnumerable<ConnectionDirection> DefiniteConnectionDirections
    {
      get
      {
        return Enum.GetValues(typeof (ConnectionDirection))
                   .Cast<ConnectionDirection>()
                   .Except(new[]{ConnectionDirection.Any});
      }
    }
  }
}

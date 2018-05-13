using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDMUtils.DataStructures.Graphs.Base;
using NUnit.Framework;

namespace MDMUtilsTests.DataStructures.Graphs
{
  [TestFixture]
  class ArrayConnectionGridTests
  {
    [Test]
    public void PrepopulatingConstructorReturnsGridOfCorrectSize()
    {
      var size = 10;
      var grid = new ConnectionGrid(size);
      Assert.That(grid.Size, Is.EqualTo(size));

    }

    [Test]
    public void PrepopulatingConstructorReturnsUnConnectedGrid()
    {
      var size = 10;
      var grid = new ConnectionGrid(size);
      for (int i = 0; i < size; i++)
      {
        Assert.That(grid.FromConnectionsForEntry(i).Any(), Is.False);
        Assert.That(grid.ToConnectionsForEntry(i).Any(), Is.False);
      }
    }

    [Test]
    public void CannotCreateSelfConnection()
    {
      var grid = new ConnectionGrid(2);
      Action creatingASelfConnection = () => grid[1,1]=true;
      Assert.That(() => creatingASelfConnection(), Throws.InvalidOperationException);
    }
  }
}

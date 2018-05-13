using System;
using System.Collections.Generic;
using System.Linq;

namespace MDMUtils.PerformanceTestingRig
{
  public class Tester
  {
    private static readonly Random randomSource = new Random();
    private static readonly System.Diagnostics.Stopwatch clock = new System.Diagnostics.Stopwatch();

    private readonly int numberOfRunsToInitialise = 200;
    private readonly int numberOfRunsPerDataPoint = 1;
    private readonly int numberOfDataPointsToCollect = 100;

    private Action currentActiveAction;

    public Tester(int numberOfRunsToInitialise, int numberOfRunsPerDataPoint, int numberOfDataPointsToCollect)
    {
      this.numberOfRunsToInitialise = numberOfRunsToInitialise;
      this.numberOfRunsPerDataPoint = numberOfRunsPerDataPoint;
      this.numberOfDataPointsToCollect = numberOfDataPointsToCollect;
    }

    public PerfTestResults TestOneAction (Action actionToTest)
    {
      InitialiseExecutionEnvironment(new[]{actionToTest});
      return CollectData(actionToTest);
    }

    public PerfTestResults[] TestActions (Action[] actions)
    {
      InitialiseExecutionEnvironment(actions);
      return CollectData(actions);
    }

    //public PerfTestResults CompareTwoActions (Action firstAction, Action secondAction)
    //{
    //  var actionPairArray = new [] {firstAction,secondAction};
    //  InitialiseExecutionEnvironment(actionPairArray);
    //  return CompareActions(new[]{firstAction, secondAction});
    //}

    private void InitialiseExecutionEnvironment(Action[] actions)
    {
      RunAllActionsUntimed(actions);

      for(int i = 0; i < numberOfRunsToInitialise; i++)
      {
        TimedRun(actions[randomSource.Next(actions.Length)]);
      }

      RunAllActionsUntimed(actions);
    }

    private PerfTestResults[] CollectData(IEnumerable<Action> actions)
    {
      var results = new List<PerfTestResults>();
      foreach (var action in actions)
      {
        results.Add(CollectData(action));
      }
      return results.ToArray();
    }

    private PerfTestResults CollectData(Action actionToTest)
    {
      currentActiveAction = actionToTest;
      var results = new List<SingleActionTestDataPoint>();

      for (int i = 0; i < numberOfDataPointsToCollect; i++)
      { results.Add(CollectDataPoint()); }

      return new PerfTestResults {Results = results.ToArray()};
    }

    private SingleActionTestDataPoint CollectDataPoint()
    {

      var runs = new List<long>();
      long minRun = long.MaxValue;
      long maxRun = 0;
      
      for (int i = 0; i < numberOfRunsPerDataPoint; i++)
      {
        var currentRun = TimedRun(currentActiveAction);
        runs.Add(currentRun);
        if (currentRun > maxRun) { maxRun = currentRun; }
        if (currentRun < minRun) { minRun = currentRun; }
      }
      
      return new SingleActionTestDataPoint
      {
        AverageRun = runs.Average(),
        MinRun = minRun,
        MaxRun = maxRun,
        NumberOfRuns = numberOfRunsPerDataPoint
      };
    }


    private static void RunAllActionsUntimed(IEnumerable<Action> actions)
    {
      foreach (Action action in actions)
      {
        UntimedRun(action);
      }
    }
    
    private static void UntimedRun(Action action)
    {
      action();
    }

    private static long TimedRun(Action action)
    {
      clock.Reset();
      clock.Start();
      action();
      clock.Stop();
      
      return clock.ElapsedTicks;
    }

  }
}

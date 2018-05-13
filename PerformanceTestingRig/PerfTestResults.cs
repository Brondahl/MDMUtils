using System.Collections.Generic;

namespace MDMUtils.PerformanceTestingRig
{
  public struct PerfTestResults
  {
    public SingleActionTestDataPoint[] Results;
  }

  public struct SingleActionTestDataPoint
  {
    public long MinRun;
    public long MaxRun;
    public double AverageRun;
    public int NumberOfRuns;
  }
}

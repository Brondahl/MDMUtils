using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MDMUtils
{
  ///==========================================================
  /// Class : Timer
  /// 
  /// <summary>
  ///   A timer that can be tested anywhere, and has a deadline
  ///   after which the tests will fail
  /// </summary>
  ///==========================================================
  public class Timer
  {
    #region Constructor
    ///==========================================================
    /// Constructor : Timer
    /// 
    /// <summary>
    ///   Establishes the time, and calculates when the program
    ///   should start to fail. (after 1 s)
    /// </summary>
    ///==========================================================
    public Timer()
    {
      mTimerStarted = DateTime.Now;
      mTimerDeadline = mTimerDeadline.AddSeconds(1);
    }

    ///==========================================================
    /// Constructor : Timer
    /// 
    /// <summary>
    ///   Establishes the time, and calculates when the program
    ///   should start to fail.
    /// </summary>
    ///==========================================================
    public Timer(int xiNumMilliseconds)
    {
      mTimerStarted = DateTime.Now;
      mTimerDeadline = mTimerDeadline.AddSeconds(xiNumMilliseconds);
    }

    #endregion

    #region Methods
    ///==========================================================
    /// Method : CheckTime
    /// 
    /// <summary>
    ///   Throws the appropriate Exception if the time is
    ///   getting tight.
    ///   Never throws when in Debug mode.
    /// </summary>
    ///==========================================================
    public static void CheckTime()
    {
      if (!mTimeCheckFailed && DateTime.Now < mTimerDeadline)
      {
        return;
      }
      else
      {
        mTimeCheckFailed = true;
        throw new Exception("Timer Failed");
      }
    }
    #endregion

    #region Fields
    private static DateTime mTimerStarted;
    private static DateTime mTimerDeadline;
    private static bool mTimeCheckFailed = false;
    #endregion
  }
}

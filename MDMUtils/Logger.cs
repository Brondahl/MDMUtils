using System;
using System.IO;

namespace MDMUtils
{
  ///==========================================================
  /// Class : Logger
  /// 
  /// <summary>
  ///   Creates numbered Log files, in a Named & Numbered Folder.
  ///   Before using reconsider the ShouldGetNewLogFolder Method and
  ///   the Naming system (see topmost region.)
  /// </summary>
  ///==========================================================
  public class Logger
  {
    #region Aspects to be reconsidered.
    ///==========================================================
    /// Method : ShouldGetNewLogFolder
    /// 
    /// <summary>
    ///   Determines whether a new folder is needed for the
    ///   current log File.
    /// </summary>
    ///==========================================================
    private static bool ShouldGetNewLogFolder()
    {
      return true;
    }

    private const string mLogFoldersPath = "D://Logs";
    private const string mFolderNameString = "Folder";
    private const string mLogNameString = "Log";

    #endregion
  
    #region Constructors
    ///==========================================================
    /// Constructor : Default Logger
    /// 
    /// <summary>
    ///   Creates a new Log file, possibly in a new Folder and
    ///   sets up access to it.
    /// </summary>
    ///==========================================================
    public Logger()
    {
      if (ShouldGetNewLogFolder())
      {
        System.IO.Directory.CreateDirectory(mLogFoldersPath + "//" + CurrentFolderName);
      }
      File = new StreamWriter(FullLogNamePath, true);
      File.WriteLine("Log opened at " + DateTime.Now + ".\r\n");
    }
    #endregion

    #region Methods
    ///==========================================================
    /// Property : CurrentLogFolderNumber
    /// 
    /// <summary>
    ///   The number of the folder currently being logged to.
    /// </summary>
    ///==========================================================
    private static string CurrentFolderNumber
    {
      get
      {
        if (mCurrentFolderNumber == 0)
        {
          mCurrentFolderNumber = IdentifyLatestFolderNumber();
          if (ShouldGetNewLogFolder())
          {
            mCurrentLogNumber++;
          }
        }
        return mCurrentLogFolderName;
      }
    }

    ///==========================================================
    /// Property : CurrentLogNumber
    /// 
    /// <summary>
    ///   The munber of the file currently being logged to.
    /// </summary>
    ///==========================================================
    private static string CurrentLogNumber
    {
      get
      {
        if (mCurrentLogNumber == 0)
        {
          mCurrentLogNumber = IdentifyLatestLogNumber();
          mCurrentLogNumber++;
        }
        return mCurrentLogFolderName;
      }
    }

    ///==========================================================
    /// Property : CurrentLogFolderName
    /// 
    /// <summary>
    ///   The Name of the folder currently being logged to.
    ///   Name string must not contain " " or "_".
    /// </summary>
    ///==========================================================
    private static string CurrentFolderName
    {
      get
      {
        if (mCurrentLogFolderName == "")
        {
          mCurrentLogFolderName = (mFolderNameString + "_" + CurrentFolderNumber);
        }
        return mCurrentLogFolderName;
      }
    }

    ///==========================================================
    /// Property : CurrentLogName
    /// 
    /// <summary>
    ///   The Name of the Log currently being written.
    ///   Name string must not contain " " or "_".
    /// </summary>
    ///==========================================================
    private static string CurrentLogName
    {
      get
      {
        if (mCurrentLogName == "")
        {
          mCurrentLogName = (mLogNameString + "_" + CurrentLogNumber);
        }
        return mCurrentLogName;
      }
    }

    ///==========================================================
    /// Method : IdentifyLatestLogFolderNumber
    /// 
    /// <summary>
    ///   Returns the number of the Log Folder created most
    ///   recently.
    /// </summary>
    ///==========================================================
    private static int IdentifyLatestFolderNumber()
    {
      DirectoryInfo lLogsDirectory = new DirectoryInfo(mLogFoldersPath);
      DirectoryInfo[] lFolders = lLogsDirectory.GetDirectories();
      int lLatestFolderNum = 0;

      foreach (DirectoryInfo tFolder in lFolders)
      {
        int lLogNumber = ParseNumberOffName(tFolder.Name, mFolderNameString);
        lLatestFolderNum = Math.Max(lLatestFolderNum, lLogNumber);
      }
      return lLatestFolderNum;
    }

    ///==========================================================
    /// Method : IdentifyLatestLogNumber
    /// 
    /// <summary>
    ///   Returns the number of the Log Folder created most
    ///   recently.
    /// </summary>
    ///==========================================================
    private static int IdentifyLatestLogNumber()
    {
      DirectoryInfo lLogsFolder = new DirectoryInfo(mLogFoldersPath + "//" + CurrentFolderName);
      FileInfo[] lLogs = lLogsFolder.GetFiles();
      int lLatestLogNum = 0;

      foreach (var tLog in lLogs)
      {
        int lLogNumber = ParseNumberOffName(tLog.Name, mLogNameString);
        lLatestLogNum = Math.Max(lLatestLogNum, lLogNumber);
      }
      return lLatestLogNum;
    }

    ///==========================================================
    /// Method : FullLogNamePath
    /// 
    /// <summary>
    ///   Returns the full path to the current Log File in use.
    /// </summary>
    ///==========================================================
    public static string FullLogNamePath
    {
      get
      {
        return (mLogFoldersPath + "//" + CurrentFolderName + "//" + CurrentLogName + ".txt"); 
      }
    }

    ///==========================================================
    /// Method : ParseNumberOffName
    /// 
    /// <summary>
    ///   parse "dfgh_XXX" into "XXX"
    /// </summary>
    /// <param name="xiName">The full Name of the object, eg. "Log_4"</param>
    /// <param name="xiStaticPart">The known static element of the name, eg. "Log"</param>
    ///==========================================================
    private static int ParseNumberOffName(string xiName, string xiStaticPart)
    {
      int    lIndexOfSeparator = xiName.IndexOf('_');
      string lNumberStr = xiName.Remove(0, lIndexOfSeparator + 1);
      int    lNumberInt = int.Parse(lNumberStr);
      return lNumberInt;
    } //second param not used?
    #endregion

    #region Fields
    public static TextWriter File;
    private static string mCurrentLogFolderName = "";
    private static string mCurrentLogName = "";
    private static int mCurrentFolderNumber = 0;
    private static int mCurrentLogNumber = 0;
    #endregion
  }
}

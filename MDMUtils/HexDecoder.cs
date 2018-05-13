using System;
using System.Collections.Generic;
using System.Globalization;

namespace MDMUtils
{
  public class HexDecoder
  {
    ///========================================================================
    /// Method : ParseHexString
    /// 
    /// <summary>
    /// 	Parses a Hex String into an array of Bytes, including the '0x' Hex
    /// 	identifier.
    /// </summary>
      /// <param name="originalHexString">Hex string, including '0x'</param>
    ///========================================================================
    public static byte[] ParseHexString(string originalHexString)
    {
      var bytes = new List<Byte>();

      if (!originalHexString.StartsWith("0x"))
      {
          throw new FormatException(
          String.Format("Hex input not valid: Must start with \"0x\". Input: {0}",
                        originalHexString));
      }
      if (originalHexString.Length % 2 == 1)
      {
        throw new ArgumentException(
          String.Format("Hex input not valid: Must contain an even number of characters. Input: {0}",
                        originalHexString));
      }

      string remainingHexString = originalHexString.Remove(0, 2);

      while(remainingHexString.Length > 0)
      {
        bytes.Add(ParseAndRemoveNextHexPair(ref remainingHexString));
      }

      return bytes.ToArray();
    }

    private static byte ParseAndRemoveNextHexPair(ref string remainingHexString)
    {
      string next2Digits = remainingHexString.Substring(0, 2);
      remainingHexString = remainingHexString.Remove(0, 2);
      
      byte ret;
      if (!Byte.TryParse(next2Digits, NumberStyles.HexNumber, null, out ret))
      {
          throw new FormatException(
          String.Format("Hex input not valid: {0} is not a valid Hex character",
                        next2Digits));
      }

      return ret;
    }
  }
}

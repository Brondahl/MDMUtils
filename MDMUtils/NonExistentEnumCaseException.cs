using System;

namespace MDMUtils
{
  public class NonExistentEnumCaseException<T> : Exception
  {
    public NonExistentEnumCaseException() : base (ConstructErrorMessage())
    { }

    private static string ConstructErrorMessage()
    {
      string typeName = typeof(T).Name;
      string messageTemplate;
      if(typeof(T).IsEnum)
      {
        messageTemplate = "A switch case on an {0} Enum has hit default, despite all known Enum values being accounted for at time of writing. Presumably, new values have been added to the Enum and the switch hasn't been updated.";
      }
      else
      {
        typeName = typeof(T).FullName;
        messageTemplate = "This Exception class has been mis-used. It should only ever be constructed using an Enumeration Type, but in this case has been called with {0}, which is not an Enum.";
      }
      return String.Format(messageTemplate, typeName);
    }
  }
  class NonExistentEnumCaseException : Exception
  {
    public NonExistentEnumCaseException(string message) : base (message) { }
  }
}

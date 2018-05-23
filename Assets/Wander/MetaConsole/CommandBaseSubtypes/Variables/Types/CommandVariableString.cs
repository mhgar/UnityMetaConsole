using System;

namespace Wander.MetaConsole
{
  public class CommandVariableString : CommandVariableBase<string>
  {
    public int MaxChars { get; set; }

    public CommandVariableString(
      string name, string desc, string value = "", int maxChars = int.MaxValue, 
      bool archive = false, bool latched = false)
        : base (name, desc, value, archive, latched)
    {
      MaxChars = maxChars;
      optionalArgsCount = int.MaxValue;
    }

    protected override string Clamp(string value)
    {
      return value.Length > MaxChars ? value.Substring(0, MaxChars) : value;
    }

    /// We override here since we want to concatinate the
    /// arguments passed.
    protected override void InvokeInternal(string[] args)
    {
      if (args.Length == 0) {
        CommandLine.WriteLine(
          "{0} ({1}): {2}\nValue is {3}", 
          Name, typeof(string[]), Description, PrintValue()
        ); 
      } else {
        SetValue(String.Join(" ", args));
      }
    }
  }
}
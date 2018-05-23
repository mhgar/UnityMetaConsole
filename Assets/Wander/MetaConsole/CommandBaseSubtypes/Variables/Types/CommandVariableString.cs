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
    }

    protected override string Clamp(string value)
    {
      return value.Length > MaxChars ? value.Substring(0, MaxChars) : value;
    }
  }
}
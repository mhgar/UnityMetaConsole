using System.Collections.Generic;

namespace Wander.MetaConsole
{
  public static class BuiltInVariables
  {
    public static IEnumerable<ICommand> GetVariableList()
    {
      var variables = new List<ICommand>() {
        motd,
        host_timescale,
        con_scrolllback
      };
      return variables;
    }

    public static readonly CommandVariableString motd 
    = new CommandVariableString(
      "motd", 
      "The message of the day.", 
      value:"New MetaConsole game.", 
      maxChars:1024, 
      archive:true
    );

    public static readonly CommandVariableFloat host_timescale 
    = new CommandVariableFloat(
      "host_timescale",
      "The timescale that the game runs at as a multiplier of delta-time.",
      value:1f,
      min:0f,
      max:100f
    );

    public static readonly CommandVariableInt con_scrolllback 
    = new CommandVariableInt(
      "con_scrollback",
      "The number of lines that are stored in the console's line buffer.",
      value:256,
      min:1
    );
  }
}
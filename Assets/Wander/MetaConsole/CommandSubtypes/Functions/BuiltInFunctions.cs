using System;
using UnityEngine;

namespace Wander.MetaConsole
{
  public static class BuiltInFunctions
  {
    // We use AutoCommand here because we're lazy. Look at the static
    // constructor for the CommandLine to see how these functions will end up
    // being executable.

    [AutoCommand("man", "Give the description and usage of a command.")]
		public static void Man(string command)
		{
			var commandRef = CommandLine.GetCommand(command);

			if (commandRef != null) {
				CommandLine.WriteLine(
          "Name: {0}\nDesc: {1}\nUsage: {2}", 
          commandRef.Name, 
          commandRef.Description, 
          commandRef.Usage
        );
			} else {
				CommandLine.WriteLine("No such command \"{0}\".", command);
			}
		}

    [AutoCommand("help", "List all commands and their descriptions.")]
    public static void Help()
    {
      var commands = CommandLine.Commands;
      foreach (var c in commands) {
        CommandLine.WriteLine("{0}\t{1}", c.Name, c.Description);
      }
    }

		[AutoCommand("echo", "Repeat all input that follows to the command line.")]
		public static void Echo(string[] args)
		{
			CommandLine.WriteLine(String.Join(" ", args));
		}

		[AutoCommand("quit", "Quit the game or stop play mode.")]
		public static void Quit()
		{
      #if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
      #elif UNITY_WEBPLAYER
      CommandLine.WriteLine("Just close the tab :P");
      #else
      Application.Quit();
      #endif
    }
  }
}
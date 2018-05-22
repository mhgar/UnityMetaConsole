using System;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.Assertions;
using System.Text;

namespace Wander.MetaConsole
{
	public static class CommandLine
	{
		public class WriteEvent : UnityEvent<string> {}
		
		/// Invoked when Write or WriteLine is called with the message supplied.
		public static readonly WriteEvent OnWrite = new WriteEvent();
		public static IReadOnlyCollection<ICommand> Commands 
			{ get { return commands.AsReadOnly(); } }

		static List<ICommand> commands = new List<ICommand>();

		static CommandLine()
		{
			InitCommandLine();
		}

		public static void InitCommandLine()
		{
			commands.Clear();

			UnityEngine.Debug.Log("Command line init.");
			// Add static methods tagged with the AutoCommand attribute.
			// The AutoCommandHandler is not a fun read unless you enjoy hard core
			// type abuse.
			commands.AddRange(AutoCommandHandler.GenerateAutoCommandsList());
			commands.AddRange(BuiltInVariables.GetVariableList());
		}

		public static T AddCommand<T>(T command) where T : ICommand
		{
			Assert.IsFalse(HasCommand(command.Name), command.Name + " is not unique.");
			Assert.IsFalse(command == null, "Command supplied cannot be null.");

			commands.Add(command);
			return command;
		}

		/// Get the command corresponding to it's name.
		public static ICommand GetCommand(string commandName)
		{
			if (String.IsNullOrEmpty(commandName)) return null;
			return commands.Find(c => c.Name == commandName);
		}

		/// Check if a command by a given name already exists.
		public static bool HasCommand(string commandName)
		{
			if (String.IsNullOrEmpty(commandName)) return false;
			return GetCommand(commandName) != null;
		}
		
		/// Execute a line of input, such as from a command console or config file. 
		/// You can seperate commands by using a ;
		public static void Execute(string input)
		{
			if (String.IsNullOrEmpty(input)) return;
			
			var inputs = input.Split(';');
			
			foreach (var i in inputs) {
				CommandLine.WriteLine("$ " + i);
				var tokens = Tokenize(i);
				ExecuteArgs(tokens);
			}
		}

		/// Execute pre-tokenized input.
		public static void ExecuteArgs(params string[] args)
		{
			if (args == null || args.Length < 1) return;

			if (!HasCommand(args[0])) {
				WriteLine("{0}: command not found.", args[0]);
				return;
			}

			var command = GetCommand(args[0]);
			var skippedArgs = args.Skip(1).ToArray();

			command.Invoke(skippedArgs);
		}

		/// Write a message to any OnWrite listeners.
		public static void Write(string message)
		{
			if (!String.IsNullOrEmpty(message))
				OnWrite.Invoke(message);
		}

		/// Write a message to any OnWrite listeners.
		public static void WriteLine(string message = "")
		{
			Write(message + "\n");
		}

		/// Write a message to any OnWrite listeners.
		public static void Write(string format, params object[] args)
		{
			Write(String.Format(format, args));
		}

		/// Write a message to any OnWrite listeners.
		public static void WriteLine(string format, params object[] args)
		{
			WriteLine(String.Format(format, args));
		}
		
		public static string[] Tokenize(string input)
		{
			return input.Split(' ');
		}
	}
}
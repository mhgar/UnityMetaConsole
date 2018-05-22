using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using UnityEngine.Assertions;
using Wander.Parsing;
using UnityEngine;

namespace Wander.MetaConsole
{
  public static class AutoCommandHandler
  {
    struct AutoCommandInfo
    {
      public MethodInfo Method { get; set; }
      public AutoCommandAttribute Attribute { get; set; }
    }

    public static IEnumerable<ICommand> GenerateAutoCommandsList()
    {
      var commands = new List<ICommand>();
      var autoCommands = FindAutoCommandsIn(Assembly.GetExecutingAssembly());
      ValidateAutoCommands(autoCommands);

      // I hate type safety.
      foreach (var ac in autoCommands) {
        ICommand command;
        var parameters = ac.Method.GetParameters();
        if (parameters.Count() == 0) {
          command = new Command(
            ac.Attribute.Name,
            ac.Attribute.Description,
            (Action) ac.Method.CreateDelegate(typeof(Action))
          );
        } else if (parameters.Count() == 1 && parameters[0].ParameterType == typeof(string[])) {
          command = new CommandStringArray(
            ac.Attribute.Name,
            ac.Attribute.Description,
            (Action<string[]>) ac.Method.CreateDelegate(typeof(Action<string[]>))
          );
        } else {
          // Basically create a Command<T> of n Ts and create strongly typed
          // delegates for any type T. Also Command<T> requires a 'Signature'
          // type, which is basically an Action delegate, but the conversion
          // doesn't work so we just use the 2nd parameter in the constructor
          // to retrieve the type of delegate we want.
          // I'm sorry.
          var paramTypes = parameters.Select(p => p.ParameterType).ToArray();
          var generic = typeof(Command<>).MakeGenericType(paramTypes);
          var signature = generic.GetConstructors()[0].GetParameters()[2].ParameterType;
          Debug.Log(ac.Method.CreateDelegate(signature));
          command = (ICommand) Activator.CreateInstance(
            generic, 
            ac.Attribute.Name, 
            ac.Attribute.Description,
            ac.Method.CreateDelegate(signature)
          );
        }

        commands.Add(command);
      }

      return commands;
    }

    static IEnumerable<AutoCommandInfo> FindAutoCommandsIn(Assembly assembly)
    {
      // Find every method that contains the command attribute function.
      var methods = assembly.GetTypes()
              .SelectMany(t => 
                t.GetMethods().Where(
                  m => m.GetCustomAttributes()
                        .Count(a => 
                          a.GetType() == typeof(AutoCommandAttribute)) > 0
                )
              );
      
      var autoCommands = methods.Select(m => 
        new AutoCommandInfo() { 
          Method = m, 
          Attribute = (AutoCommandAttribute) m.GetCustomAttribute(typeof(AutoCommandAttribute))
        }
      );

      return autoCommands;
    }

    static void ValidateAutoCommands(IEnumerable<AutoCommandInfo> commands)
    {
      foreach (var command in commands) {
        // Check method itself
        Assert.IsTrue(
          command.Method.IsStatic, 
          command.Attribute.Name + " is not static."
        );

        Assert.IsTrue(
          command.Method.IsPublic,
          command.Attribute.Name + " is not public."
        );

        // Check parameters
        var parameters = command.Method.GetParameters();

        Assert.IsFalse(
          parameters.Count() > 5,
          command.Attribute.Name + " has an invalid parameter count, max is 5."
        );

        // Check if every parameter has a parser, or if a singular parameter is
        // a string array.
        if (parameters.Count() == 1 && parameters[0].ParameterType == typeof(String[])) {
          // That is fine.
        } else {
          Assert.IsTrue(
            parameters.All(p => StringParser.HasParser(p.ParameterType)),
            command.Attribute.Name + " has a parameter that has no StringParser."
          );
        }        
      }
    }
  }
}
using System;
using UnityEngine.Assertions;

namespace Wander.MetaConsole
{
  /// The base used by all commands. Accepts an array of strings from the
  /// command line and checks whether or not the length of the array is within
  /// the bounds of required arguments and optional arguments. It will then
  /// call InvokeInternal and allow you to do something with those arguments.
  /// Remember to set the ArgsCounts if you extend this, otherwise this class
  /// is mostly pointless.
  public abstract class CommandBase : ICommand
  {
    public string Name { get; private set; }
    public string Description { get; set; }
    public string Usage { get; protected set; }

    protected int concreteArgsCount = 0;
    protected int optionalArgsCount = Int32.MaxValue;

    public CommandBase(string name, string desc)
    {
      Assert.IsNotNull(name);
      Assert.IsNotNull(desc);

      Usage = "list of strings";
      Name = name;
      Description = desc;
    }

    public void Invoke(params string[] args)
    {
      // Check arguments, then execute.
      Assert.IsNotNull(args);
      if (args.Length < concreteArgsCount ||
          args.Length > concreteArgsCount + optionalArgsCount)
      {
        CommandLine.WriteLine("Incorrect argument count. Usage: {0}.", Usage);
      }
      else
      {
        InvokeInternal(args);
      }
    }

    /// Called after checking the argument count.
    protected abstract void InvokeInternal(string[] args);
  }
}
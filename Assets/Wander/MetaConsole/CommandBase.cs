using System;
using UnityEngine.Assertions;

namespace Wander.MetaConsole
{
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
        CommandLine.WriteLine("Incorrect argument count, usage: {0}.", Usage);
      }
      else
      {
        InvokeInternal(args);
      }
    }

    protected abstract void InvokeInternal(string[] args);
  }
}
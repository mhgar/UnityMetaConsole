using System;

namespace Wander.MetaCommand
{
  public class CommandFunction : ICommand
  {
    public string Name { get; private set; }
    public string Description { get; set; }
    public string Usage { get; private set; }
  
    Action<string[]> function;

    public CommandFunction(string name, string desc, Action<string[]> function)
    {
      Name = name;
      Description = desc;
      this.function = function;
    }

    public virtual void Invoke(string[] args)
    {
      function(args);
    }
  }
}
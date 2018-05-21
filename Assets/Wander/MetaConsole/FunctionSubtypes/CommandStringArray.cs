using System;
using UnityEngine.Assertions;

namespace Wander.MetaConsole
{
  public class CommandStringArray : Command
  {
    Action<string[]> function;

    public 
    CommandStringArray(string name, string desc, Action<string[]> function)
      :base (name, desc)
    {
      Assert.IsNotNull(function);
      this.function = function;
    }

    protected override void InvokeInternal(string[] args)
    {
      function(args);
    }
  }
}
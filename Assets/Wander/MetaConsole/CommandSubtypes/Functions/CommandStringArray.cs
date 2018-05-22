using System;
using UnityEngine.Assertions;

namespace Wander.MetaConsole
{
  /// A helper Command that will turn all arguments passed into a single string
  /// array. Useful if you want the arguments exactly as they were typed by the
  /// user.
  public class CommandStringArray : CommandBase
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
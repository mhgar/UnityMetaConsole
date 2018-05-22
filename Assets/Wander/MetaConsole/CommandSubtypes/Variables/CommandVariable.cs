using System;
using System.Text;
using Wander.Parsing;
using UnityEngine.Assertions;

namespace Wander.MetaConsole
{
  public abstract class CommandVariable<T> : CommandBase
  {
    /// The current value, or latched value if this variable is latched.
    public T Value { get { return currentValue; } set { SetValue(value); } }

    /// True if this variable has to wait until an unlatch event. An example of
    /// such an event may involve changing the number of max players allowed on
    /// a server only on a server restart.
    public bool IsLatched { get { return latched; } }

    T currentValue;
    T latchValue;
    T defaultValue;
    bool latched = false;
    bool archive = false;

    public CommandVariable(string name, string desc, T value = default(T), bool archive = false)
      : base(name, desc)
    {
      // Can't use Assert.IsNotNull because T may not be a reference type.
      // Considering removing the ability to use reference types that are not
      // strings, but currently the infrastructure supports them.
      Assert.IsTrue(value != null);

      concreteArgsCount = 0;
      optionalArgsCount = 1;

      currentValue  = Clamp(value);
      latchValue    = Clamp(value);
      defaultValue  = Clamp(value);

      Usage = "no argument to check value, or set value with value:" + typeof(T);
      this.archive = archive;
    }

    /// Take in a value and apply things such as a minimum and maximum or
    /// maximum number of characters or anything that might limit the ranges of
    /// possibles values.
    protected abstract T Clamp(T value);
    
    void SetValue(T value)
    {
      if (latched) {
        latchValue = Clamp(value);
      } else {
        currentValue = Clamp(value);
      }
    }

    string PrintValue()
    {
      var str = new StringBuilder();
      str.Append(Value);
      if (latched) {
        str.Append(" (Latched as ");
        str.Append(latchValue);
        str.Append(")");
      }
      return str.ToString();
    }

    protected override void InvokeInternal(string[] args)
    {
      try {
				switch(args.Length) {
					case 0: Console.WriteLine(PrintValue()); break;
					case 1: SetValue(StringParser.Parse<T>(args[0])); break;
				}
			} catch (Exception e){
				Console.WriteLine("{0}, usage {1}", e.Message, Usage);
			}
    }
  }
}
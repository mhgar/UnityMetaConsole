using System;
using System.Text;
using Wander.Parsing;
using UnityEngine.Assertions;

namespace Wander.MetaConsole
{
  /// A base class for supporting type safe command line variables. Extend this
  /// class and implement the method for Clamping values between a given
  /// range.
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
    bool latched = false; // If the variable must wait until an unlatch event to be set.
    bool archive = false; // If the variable saves into a file when written.

    public CommandVariable(
      string name, 
      string desc, 
      T value = default(T), 
      bool archive = false, 
      bool latched = false)
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

    /// Clamp a given value used for limiting the range of possible values, i.e.
    /// an int between 0 and 100.
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
					case 0: 
            CommandLine.WriteLine(
              "{0} ({1}): {2}\nValue is {3}", 
              Name, typeof(T), Description, PrintValue()
              ); 
            break;
					case 1: 
            SetValue(StringParser.Parse<T>(args[0]));
            CommandLine.WriteLine("{0} << {1}", Name, PrintValue());
            break;
				}
			} catch (Exception e){
				CommandLine.WriteLine("{0} Usage: {1}", e.Message, Usage);
			}
    }
  }
}
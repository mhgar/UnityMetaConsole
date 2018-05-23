using System;

namespace Wander.MetaConsole
{
  public class CommandVariableInt : CommandVariableBase<int>
  {
    public int Min { get; private set; }
    public int Max { get; private set; }

    public CommandVariableInt(
      string name, string desc, int value = 0, int min = int.MinValue,
      int max = int.MaxValue, bool archive = false, bool latched = false)
        : base (name, desc, value, archive, latched)
    {
      Min = min;
      Max = max;
    }

    protected override int Clamp(int value)
    {
      return UnityEngine.Mathf.Clamp(value, Min, Max);
    }
  }
}
using System;

namespace Wander.MetaConsole
{
  public class CommandVariableFloat : CommandVariableBase<float>
  {
    public float Min { get; private set; }
    public float Max { get; private set; }

    public CommandVariableFloat(
      string name, string desc, float value = 0.0f, float min = float.MinValue,
      float max = float.MaxValue, bool archive = false, bool latched = false)
        : base (name, desc, value, archive, latched)
    {
      Min = min;
      Max = max;
    }

    protected override float Clamp(float value)
    {
      return UnityEngine.Mathf.Clamp(value, Min, Max);
    }
  }
}
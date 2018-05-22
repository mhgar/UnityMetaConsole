using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using UnityEngine.Assertions;
using Wander.Parsing;
using UnityEngine;

namespace Wander.MetaConsole
{
  [AttributeUsage(validOn:AttributeTargets.Method, AllowMultiple = false)]
  public class AutoCommandAttribute : Attribute
  {
    public string Name { get; private set; }
    public string Description { get; private set; }

    public AutoCommandAttribute(string name, string desc)
    {
      Name = name;
      Description = desc;
    }
  }
}
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using UnityEngine.Assertions;
using Wander.Parsing;
using UnityEngine;

namespace Wander.MetaConsole
{
  /// An attribute used for turning a static function into a console command.
  /// This accepts most types of function parameters. It will accept (void),
  /// (string[]) and set of up to 5 parameters of any type that are defined
  /// in Wander.Parsers.StringParser. This will also respect optional arguments.
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
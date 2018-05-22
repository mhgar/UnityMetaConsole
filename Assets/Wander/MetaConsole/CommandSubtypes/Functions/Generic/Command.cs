using System;
using UnityEngine.Assertions;

namespace Wander.MetaConsole
{
    /// Generic command that accepts a function of the types given. When invoked
    /// it will convert the string array passed from the command line into
    /// arguments that are usable by the function. It will also respect optional
    /// arguments, so you can define those in your function's signature if you
    /// wish. Arguments types must be defined in Wander.Parsing.StringParser to
    /// be accepted by this class.
    public class Command : CommandBase
    {
        Action function;

        public Command(string name, string desc, Action function)
            : base(name, desc)
        {
            optionalArgsCount = 0;
            concreteArgsCount = 0;

            Assert.IsNotNull(function);
            Usage = "no arguments";
            this.function = function;
        }

        protected override void InvokeInternal(string[] args)
        {
            function();
        }
    }
}
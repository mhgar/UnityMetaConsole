using System;
using UnityEngine.Assertions;

namespace Wander.MetaConsole
{
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
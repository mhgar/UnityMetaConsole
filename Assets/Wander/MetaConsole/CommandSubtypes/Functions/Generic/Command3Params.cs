using System;
using UnityEngine.Assertions;
using System.Reflection;
using System.Linq;
using Wander.Parsing;

namespace Wander.MetaConsole
{
    /// Generic command that accepts a function of the types given. When invoked
	/// it will convert the string array passed from the command line into
	/// arguments that are usable by the function. It will also respect optional
	/// arguments, so you can define those in your function's signature if you
	/// wish. Arguments types must be defined in Wander.Parsing.StringParser to
  /// be accepted by this class.
	public class Command<T, U, V> : CommandBase
	{
		public delegate void Signature(
            T arg0 = default(T), 
            U arg1 = default(U),
            V arg2 = default(V)
        );
		Signature function;
		object[] defaultValues;

		public 
		Command(string name, string desc, Signature function)
				:base (name, desc)
		{
			Assert.IsNotNull(function);
			this.function = function;

			var parameters = function.Method.GetParameters();
            Assert.IsTrue(parameters.All(p => StringParser.HasParser(p.ParameterType)));

			defaultValues = parameters.Select(p => p.DefaultValue).ToArray();
			concreteArgsCount = parameters.Count(p => !p.IsOptional);
			optionalArgsCount = parameters.Count(p => p.IsOptional);
            Usage = StringParser.ParametersToString(parameters);
		}

		protected override void InvokeInternal(string[] args)
		{
			try {
				switch(args.Length) {
					case 0: 
                        function(
                            (T) defaultValues[0], 
                            (U) defaultValues[1],
                            (V) defaultValues[2]
                        ); 
                        break;
					case 1: 
                        function(
                            StringParser.Parse<T>(args[0]), 
                            (U) defaultValues[1],
                            (V) defaultValues[2]
                        ); 
                        break;
                    case 2:
                        function(
                            StringParser.Parse<T>(args[0]), 
                            StringParser.Parse<U>(args[1]),
                            (V) defaultValues[2]
                        );
                        break;
                    case 3:
                        function(
                            StringParser.Parse<T>(args[0]), 
                            StringParser.Parse<U>(args[1]),
                            StringParser.Parse<V>(args[2])
                        );
                        break;
				}
			} catch (Exception e){
				CommandLine.WriteLine("{0} Usage {1}", e.Message, Usage);
			}
		}
	}
}
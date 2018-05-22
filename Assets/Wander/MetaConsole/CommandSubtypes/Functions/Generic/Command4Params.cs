using System;
using UnityEngine.Assertions;
using System.Reflection;
using System.Linq;
using Wander.Parsing;

namespace Wander.MetaConsole
{
	public class Command<T, U, V, W> : CommandBase
	{
		public delegate void Signature(
            T arg0 = default(T), 
            U arg1 = default(U),
            V arg2 = default(V),
            W arg3 = default(W)
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
                            (V) defaultValues[2],
                            (W) defaultValues[3]
                        ); 
                        break;
					case 1: 
                        function(
                            StringParser.Parse<T>(args[0]), 
                            (U) defaultValues[1],
                            (V) defaultValues[2],
                            (W) defaultValues[3]
                        ); 
                        break;
                    case 2:
                        function(
                            StringParser.Parse<T>(args[0]), 
                            StringParser.Parse<U>(args[1]),
                            (V) defaultValues[2],
                            (W) defaultValues[3]
                        );
                        break;
                    case 3:
                        function(
                            StringParser.Parse<T>(args[0]), 
                            StringParser.Parse<U>(args[1]),
                            StringParser.Parse<V>(args[2]),
                            (W) defaultValues[3]
                        );
                        break;
                    case 4:
                        function(
                            StringParser.Parse<T>(args[0]), 
                            StringParser.Parse<U>(args[1]),
                            StringParser.Parse<V>(args[2]),
                            StringParser.Parse<W>(args[3])
                        );
                        break;
				}
			} catch (Exception e){
				Console.WriteLine("{0}, usage {1}", e.Message, Usage);
			}
		}
	}
}
using System;
using UnityEngine.Assertions;
using System.Reflection;
using System.Linq;
using Wander.Parsing;

namespace Wander.MetaConsole
{
	public class Command<T> : CommandBase
	{
		public delegate void Signature(T arg0 = default(T));
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
					case 0: function((T) defaultValues[0]); break;
					case 1: function(StringParser.Parse<T>(args[0])); break;
				}
			} catch (Exception e){
				CommandLine.WriteLine("{0} Usage {1}", e.Message, Usage);
			}
		}
	}
}
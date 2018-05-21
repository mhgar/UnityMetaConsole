using System;
using UnityEngine.Assertions;

namespace Wander.MetaConsole
{
	public class Command<T> : Command
	{
		public delegate void Signature(T arg1 = default(T));
		Signature function;

		public 
		Command(string name, string desc, Signature function)
				:base (name, desc)
		{
			Assert.IsNotNull(function);
			this.function = function;
		}

		protected override void InvokeInternal(string[] args)
		{
			// function(args);
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wander.MetaCommand
{
	public interface ICommand
	{
		string Name { get; }
		string Description { get; set; }
		string Usage { get; }
		void Invoke(params string[] args);
	}
}
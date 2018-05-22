using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wander.MetaConsole;

public class TestCommandLine : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		CommandLine.OnWrite.AddListener(Debug.Log);

		CommandLine.AddCommand(
			new CommandVariableFloat(
				"sv_timescale", 
				"The timescale the server runs at.",
				value:1.0f,
				min:0.0f,
				max:10.0f)
		);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}

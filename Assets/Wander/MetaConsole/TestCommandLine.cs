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
			new Command<string, int>("connect", "Connect to a server.", Connect)
		);

		CommandLine.AddCommand(
			new CommandVariableFloat(
				"sv_timescale", 
				"The timescale the server runs at.",
				value:1.0f,
				min:0.0f,
				max:10.0f)
		);

		CommandLine.Execute("connect;connect hello;connect hello 24;connect hello there");
		CommandLine.ExecuteArgs("connect", "1", "2", "3");
		CommandLine.Execute("sv_timescale;sv_timescale 0.5;sv_timescale hello;sv_timescale 0.5 0.5");
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void Connect(string address, int port = 7777)
	{
		Debug.LogFormat("Connectin to {0}:{1}...", address, port);
	}
}

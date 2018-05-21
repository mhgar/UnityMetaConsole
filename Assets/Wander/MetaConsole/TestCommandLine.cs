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

		CommandLine.Execute("connect;connect hello;connect hello 24");
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

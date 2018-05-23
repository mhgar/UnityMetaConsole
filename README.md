# UnityMetaConsole
This project offers a command line/console solution for Unity 3D that allows easy generation of functions, variables and configuration files. It has a focus on type safety and removing boiler plate, so that it is quicker and safer to add functionality to the command line.

MetaConsole is not yet in a state that you should use for production, but all it's core facilities are at the very least functional.

## Adding functions
Here's how you can add functions to the command line:

```cs
[AutoCommand("connect", "Connect to a server given a hostname, port and number of connection retries.")]
public static void Connect(string hostname, int port, int retries = 4)
{
  // Do something with hostname, port and retires.
}
```

This will do all the work for you. Note that there is no string parsing required and that optional arguments are respected. By use of reflection and meta programming the command line will generate an appropriate method for passing arguments to your function.

For transparency, here is essentially what AutoCommand generates:

```cs
CommandLine.AddCommand(new Command<string, port, int>(
  "connect", 
  "Connect to a server given a hostname, port and number of connection retries."
  Connect // Reference to a function.
));
```

AutoCommand uses reflection to generate the appropriate calls, which means that start up will be slightly slower. If you value start up speed please use the above method. However, both of these methods should perform the same at runtime.

## Adding variables
Adding variables is simple, too:

```cs
var host_timescale = new CommandVariableFloat("host_timescale", "Timescale of the host.", value:1.0f, min:0f, max:10f);
CommandLine.AddCommand(host_timescale);
```

It is the intention that the host_timescale be referenced by other scripts outside of the command line. Unlike most other command lines, you can read and set ``variable.Value`` without having to use a string value as an intermediary. Variables are only parsed from a string if they are set from the command line interface. This greatly improves performance in cases where a variable needs to be read every frame.

Unlike functions which are generic, variable types must be created at design time. Currently, there are variable types for strings and intergers, and it is very easy to create your own types.

## What execution looks like
Here's an example of the output of a command line in use:

```
$ man con_scrollback
Name: con_scrollback
Desc: The number of lines that are stored in the console's line buffer.
Usage: no argument to check value, or set value with value:System.Int32
$ con_scrollback
con_scrollback (System.Int32): The number of lines that are stored in the console's line buffer.
Value is 128
$ con_scrollback 256tdsad
Input string was not in a correct format. Usage: no argument to check value, or set value with value:System.Int32
$ con_scrollback 256
con_scrollback << 256
$ man arg1 arg2
Incorrect argument count. Usage: command:String.
$ connect 127.0.0.1
Connecting to 127.0.0.1:7777...
$ connect 127.0.0.1 1234
Connecting to 127.0.0.1:1234...
```

As shown, argument counts, parsing formats and optional arguments are all accounted for.

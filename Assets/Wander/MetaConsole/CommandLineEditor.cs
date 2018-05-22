using UnityEngine;
using UnityEditor;

namespace Wander.MetaConsole
{
  public class CommandLineWindow : EditorWindow
  {
    string input;
    static TextBuffer output = new TextBuffer(128);

    [MenuItem("Window/MetaConsole/CommandLine")]
    static void Init()
    {
      var window = (CommandLineWindow) EditorWindow.GetWindow(typeof(CommandLineWindow));
      window.Show();
    }

    void OnGUI() 
    {
      CommandLine.OnWrite.AddListener(output.Write);
      
      GUILayout.TextArea(output.GetText(), GUILayout.MinHeight(200.0f));

      GUILayout.BeginHorizontal();
      input = GUILayout.TextField(input);
      if (GUILayout.Button("Execute")) {
        CommandLine.Execute(input);
      }
      GUILayout.EndHorizontal();

      if (GUILayout.Button("Clear")) {
        output.Clear();
      }

      var commands = CommandLine.Commands;

      foreach(var command in commands) {
        GUILayout.BeginHorizontal();
        GUILayout.Label(command.Name);
        GUILayout.Label(command.Description);
        GUILayout.EndHorizontal();
      }

      CommandLine.OnWrite.RemoveListener(output.Write);
    }
  }
}
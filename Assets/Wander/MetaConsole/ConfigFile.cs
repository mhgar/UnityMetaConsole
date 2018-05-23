using System;
using System.IO;
using System.Collections.Generic;

namespace Wander.MetaConsole
{
  /// A class used for reading and writing variable archives files.
  public class ArchiveFile
  {
    FileStream file;
    Dictionary<string, string> entries = new Dictionary<string, string>();

    public ArchiveFile(string fileName)
    {
      file = File.Open(fileName, FileMode.OpenOrCreate);
      ReadEntries();
    }

    public void Set(string command, string value)
    {
      if (entries.ContainsKey(command)) {
        entries[command] = value;
      } else {
        entries.Add(command, value);
      }

      WriteEntries(); // Not very performant to do this every time.
    }

    public bool ContainsEntry(string entry)
    {
      return entries.ContainsKey(entry);
    }

    public void Execute()
    {
      foreach(var entry in entries) {
        CommandLine.ExecuteArgs(entry.Key, entry.Value);
      }
    }

    void ReadEntries()
    {
      entries.Clear();
      var reader = new StreamReader(file);
      var inputs = reader.ReadToEnd().Split('\n', ';');
      foreach (var input in inputs) {
        var tokens = input.Split(new char[] { ' ' }, 2);
        if (tokens.Length != 2) continue; // Skip this one, archive incorrectly.
        entries.Add(tokens[0], tokens[1]);        
      }
    }

    void WriteEntries()
    {
      var writer = new StreamWriter(file);
      foreach (var entry in entries) {
        writer.WriteLine("{0} {1}", entry.Key, entry.Value);
      }
    }
  }
}
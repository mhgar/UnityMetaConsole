using System.Collections.Generic;
using System.Text;
using System.Linq;
using System;

namespace Wander.MetaConsole
{
  public class TextBuffer
  {
    StringBuilder buffer = new StringBuilder();

    int maxLines = 0;

    public TextBuffer(int maxLines = 0)
    {
      this.maxLines = maxLines;
    }

    public void Write(string message)
    {
      buffer.Append(message);
      CheckLines();
    }

    public void Write(string format, params object[] args)
    {
      Write(String.Format(format, args));
    }

    public void WriteLine(string message)
    {
      Write(message + "\n");
    }

    public void WriteLine(string format, params object[] args)
    {
      WriteLine(String.Format(format, args));
    }

    private void CheckLines()
    {
      if (maxLines > 0) {
        var output = buffer.ToString();
        var lines = output.Count(c => c == '\n');
        if (lines > maxLines) {
          buffer.Remove(0, output.TakeWhile(c => c != '\n').Count() + 1);
        }
      }
    }

    public string GetText()
    {
      return buffer.ToString();
    }

    public void Clear()
    {
      buffer.Length = 0;
    }
  }
}
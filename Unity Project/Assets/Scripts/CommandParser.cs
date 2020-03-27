using System;
using System.Linq;
using UnityEngine;
using System.Collections;


public static class CommandParser
{
    public static Tuple <string,string[]> ParseCommand(string cmd)
    {
        //Start finding commands from the start of first parenthesis.
        string[] cmdArgs = cmd.Substring(cmd.IndexOf("(")).Split(new char[] { '(',',',')' },StringSplitOptions.RemoveEmptyEntries);
        string cmdName = cmd.Substring(1, cmd.IndexOf("(")-1);
        return new Tuple<string, string[]>(cmdName, cmdArgs);
    }


}

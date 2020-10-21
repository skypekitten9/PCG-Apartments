using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class filerw
{
    static StreamReader reader;
    static StreamWriter writer;
    static string content;
    public static string FileToString(string path)
    {
        reader = new StreamReader(path);
        content = reader.ReadToEnd();
        reader.Close();
        return content;
    }

    public static void WriteToFile(string toWrite, string path)
    {
        if (!File.Exists(path))
        {
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(toWrite);
            }
        }
    }
}

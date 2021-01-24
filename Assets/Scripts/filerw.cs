using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class filerw
{
    static StreamReader reader;
    static StreamWriter writer;
    static string content;
    static string path = "Assets/Texts/Rooms/";

    public static string FileToString(string fileName)
    {
        fileName = path + fileName;
        reader = new StreamReader(fileName);
        content = reader.ReadToEnd();
        reader.Close();
        return content;
    }

    public static void WriteToFile(string toWrite, string fileName)
    {
        fileName = path + fileName;
        if (!File.Exists(fileName))
        {
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(fileName))
            {
                sw.WriteLine(toWrite);
            }
        }
    }
}

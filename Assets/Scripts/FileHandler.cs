using UnityEngine;
using System.IO;

public class FileHandler : MonoBehaviour
{

    // The file path of the file to be modified
    public string filePath;

    // Appends a line of text to the file
    public void AppendLine(string text)
    {
        // Open the file in append mode and write the text to it
        using (StreamWriter writer = File.AppendText(filePath))
        {
            writer.WriteLine(text);
        }
    }

    // Clears the content of the file
    public void ClearFile()
    {
        // Open the file in write mode and truncate its length to zero
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.BaseStream.SetLength(0);
        }
    }
}
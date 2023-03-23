using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class FileManager
{
    public static async void SaveJSON(WorldData toSave)
    {
        string data = JsonUtility.ToJson(toSave);
        string path = Path.Combine(Application.persistentDataPath, toSave.WorldName + ".json");
        await WriteAsync(path, data);
    }

    public static void TryLoadJSON(string path, object objectToOverwrite)
    {
        if (File.Exists(path))
        {
            JsonUtility.FromJsonOverwrite(File.ReadAllText(path), objectToOverwrite);
        }
        else
        {
            Debug.Log("F LoadError /_(`-`)_");
        }
    }

    /// <summary>
    /// This is the same default buffer size as
    /// <see cref="StreamReader"/> and <see cref="FileStream"/>.
    /// </summary>
    private const int DefaultBufferSize = 4096;

    /// <summary>
    /// Indicates that
    /// 1. The file is to be used for asynchronous reading.
    /// 2. The file is to be accessed sequentially from beginning to end.
    /// </summary>
    private const FileOptions DefaultOptions = FileOptions.Asynchronous | FileOptions.SequentialScan;

    public Task<string[]> ReadAllLinesAsync(string path)
    {
        return ReadAllLinesAsync(path, Encoding.UTF8);
    }

    public async Task<string[]> ReadAllLinesAsync(string path, Encoding encoding)
    {
        var lines = new List<string>();
        // Open the FileStream with the same FileMode, FileAccess
        // and FileShare as a call to File.OpenText would've done.
        using (
            var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, DefaultOptions)
        )
        using (var reader = new StreamReader(stream, encoding))
        {
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                lines.Add(line);
            }
        }

        return lines.ToArray();
    }

    public static async Task<bool> WriteAsync(string path, string data)
    {
        return await WriteAsync(path, data, Encoding.UTF8);
    }

    public static async Task<bool> WriteAsync(string path, string data, Encoding encoding)
    {
        var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, DefaultBufferSize, DefaultOptions);

        using (var writer = new StreamWriter(stream, encoding))
        {
            foreach (char c in data)
            {
                await writer.WriteAsync(c);
            }
        }
        stream.Close();
        return true;
    }
}

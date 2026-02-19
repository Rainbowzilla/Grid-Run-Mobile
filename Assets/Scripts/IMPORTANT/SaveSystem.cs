using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static string DirectoryPath;
    private static bool Intialized;

    public static void Init()
    {
        DirectoryPath = $"{Application.dataPath}/Saves/";

        if (!Directory.Exists(DirectoryPath))
        {
            Directory.CreateDirectory(DirectoryPath);
        }

        Intialized = true;
    }

    public static void Save(object DataToSave, string FileName = "Save")
    {
        if (!Intialized)
        {
            Init();
        }

        string JSONSave = JsonUtility.ToJson(DataToSave);

        if (File.Exists($"{DirectoryPath}{FileName}.json"))
        {
            File.Delete(DirectoryPath + FileName);
        }

        StreamWriter SaveFileWriter = new StreamWriter($"{DirectoryPath}{FileName}.json");
        SaveFileWriter.WriteLine(JSONSave);
        SaveFileWriter.Close();
    }

    public static void Load<T>(out T LoadedData, string FileName = "Save")
    {
        if (!Intialized)
        {
            Init();
        }

        StreamReader SaveFileReader = new StreamReader($"{DirectoryPath}{FileName}.json");
        string JSONSave = SaveFileReader.ReadLine();
        LoadedData = JsonUtility.FromJson<T>(JSONSave);
        SaveFileReader.Close();
    }
}

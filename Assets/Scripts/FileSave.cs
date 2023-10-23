using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileSave
{
    public string dirPath = "./";
    public string fileName = "GameData.proto";

    public FileSave(string dirPath, string fileName)
    {
        this.dirPath = dirPath;
        this.fileName = fileName;
    }

    public GameData Load()
    {
        string fullPath = Path.GetFullPath(Path.Combine(dirPath, fileName));
        Debug.Log(fullPath);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                loadedData = (GameData)JsonUtility.FromJson(dataToLoad, typeof(GameData));
            }
            catch (Exception e)
            {
                throw new Exception("Error while loading data!!!!");
            }
        }
        return loadedData;
    }

    public void Save(GameData data) 
    { 
        string fullPath = Path.Combine(dirPath, fileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        } catch (Exception e) {
            throw new Exception("Error while saving data!!!! oh noooo");
        }
    }
}

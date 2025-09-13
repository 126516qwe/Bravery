using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler 
{
    private string fullPath;
    private bool encrpyData;
    private string codeWord = "unityalexdev.com";

    public FileDataHandler(string dataDirPath,string dataFileName, bool encrpyData)
    {
        fullPath = Path.Combine(dataDirPath, dataFileName);
        this.encrpyData = encrpyData;
    }

    public void SaveData(GameData gameData)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToSave = JsonUtility.ToJson(gameData, true);

            if(encrpyData)
                dataToSave = EncrytpDecrypty(dataToSave);

            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToSave);
                }
            }
        }

        catch(Exception e)
        {
            Debug.LogError("Error on trying to save data to file:" + fullPath + "\n" + e);
        }
    }

    public GameData LoadData()
    {
        GameData loadData  = null;

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
                if (encrpyData)
                    dataToLoad = EncrytpDecrypty(dataToLoad);

                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }

            catch(Exception e)
            {
                Debug.LogError("Error on trying to load data from file:" + fullPath + "\n" + e);
            }
        }
            return loadData;
    }

    public void Delete()
    {
        if(File.Exists(fullPath))
            File.Delete(fullPath);
    }

    private string EncrytpDecrypty(string data)
    {
        string modifedData = "";

        for(int i = 0;i<data.Length; i++)
        {
            modifedData += (char)(data[i] ^ codeWord[i % codeWord.Length]);
        }
        return modifedData;
    }
}

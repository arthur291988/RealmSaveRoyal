using System;
using System.IO;
using UnityEngine;

public class SaveAndLoad : MonoBehaviour
{
    public static SaveAndLoad instance;


    private string fileName = "SaveData"; //file for save game data
    private string fileNamePref = "PrefData"; //file for save game data

    private void Awake()
    {
        //GameParams.achievedSubLocationStatic = 1;
        //saveGameData();
        instance = new SaveAndLoad();
        //get Saved achieved location and sub location
        if (File.Exists(Application.persistentDataPath + "/" + fileName + ".art"))
        {
            LoadSavedDataFromFile();
        }
        else
        {
            GameParams.achievedLocationStatic = 0;
            GameParams.achievedSubLocationStatic = 0;
        }


        //load saved prefs 
        if (File.Exists(Application.persistentDataPath + "/" + fileNamePref + ".art"))
        {
            LoadPrefsFromFile();
        }
        else {
            GameParams.language = 0;
        }
    }

    //crypting method for saved data
    string Crypt(string text)
    {
        string result = string.Empty;
        foreach (char j in text)
        {
            // ((int) j ^ 29) - применение XOR к номеру символа
            // (char)((int) j ^ 29) - получаем символ из измененного номера
            // Число, которым мы XORим можете поставить любое. Эксперементируйте.
            result += (char)(j ^ 29);
        }
        return result;
    }

    //this method is used to read save data from special file with key value approach
    private string getSavedValue(string[] line, string pattern)
    {
        string result = "";
        foreach (string key in line)
        {
            if (key.Trim() != string.Empty)
            {
                string value = key;
                value = Crypt(key);

                if (pattern == value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0])
                {
                    result = value.Remove(0, value.IndexOf(' ') + 1);
                }
            }
        }
        return result;
    }

    //saving preferences of player
    public void saveGameData()
    {
        StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/" + fileName + ".art");
        string sp = " "; //space 

        sw.WriteLine(Crypt("achievedLocationStatic" + sp + GameParams.achievedLocationStatic));
        sw.WriteLine(Crypt("achievedSubLocationStatic" + sp + GameParams.achievedSubLocationStatic));

        sw.Close();
    }

    public void savePlayerPrefs()
    {
        StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/" + fileNamePref + ".art");
        string sp = " "; //space 

        sw.WriteLine(Crypt("language" + sp + GameParams.language));


        sw.Close();
    }

    private void LoadSavedDataFromFile()
    {
        string[] rows = File.ReadAllLines(Application.persistentDataPath + "/" + fileName + ".art");

        int achievedLocation;
        if (int.TryParse(getSavedValue(rows, "achievedLocationStatic"), out achievedLocation)) GameParams.achievedLocationStatic = achievedLocation;
        int achievedSubLocation;
        if (int.TryParse(getSavedValue(rows, "achievedSubLocationStatic"), out achievedSubLocation)) GameParams.achievedSubLocationStatic = achievedSubLocation;
    }

    //load player prefs on start from file
    private void LoadPrefsFromFile()
    {
        string[] rows = File.ReadAllLines(Application.persistentDataPath + "/" + fileNamePref + ".art");

        int language;
        if (int.TryParse(getSavedValue(rows, "language"), out language)) GameParams.language = language;
    }

    private void OnApplicationQuit()
    {
        if (!GameParams.isTutor) saveGameData();
        savePlayerPrefs();
    }
}

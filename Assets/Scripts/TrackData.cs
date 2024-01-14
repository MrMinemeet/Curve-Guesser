using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class TrackData
{
    private static string datapath = Application.persistentDataPath + "TrackData.dat";
    public static Dictionary<Difficulty, Dictionary<Function, TrackInfo>> tracks;

    public static TrackInfo Get(Difficulty difficulty, Function function)
    {
        if (tracks.ContainsKey(difficulty) && tracks[difficulty].ContainsKey(function))
        {
            return tracks[difficulty][function];
        }
        else return null;
    }
    public static void Set(TrackInfo trackInfo)
    {
        tracks[trackInfo.difficulty][trackInfo.function] = trackInfo;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(datapath);
        bf.Serialize(fs, tracks);
        fs.Close();
    }
    public static void Load()
    {
        if (File.Exists(datapath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(datapath, FileMode.Open);
            tracks = (Dictionary<Difficulty, Dictionary<Function, TrackInfo>>)bf.Deserialize(fs);
            fs.Close();
        }
        else
        {
            tracks = new();
            tracks[Difficulty.Tutorial] = new();
            tracks[Difficulty.Easy] = new();
            tracks[Difficulty.Medium] = new();
            tracks[Difficulty.Hard] = new();
            tracks[Difficulty.Expert] = new();
        }
    }
}

[Serializable]
public class TrackInfo
{ 
    public Difficulty difficulty;
    public Function function;
    public int currentLevel;
    public int score;
    public int fails;

    public TrackInfo(Difficulty difficulty, Function function, int currentLevel, int score, int fails)
    {
        this.difficulty = difficulty;
        this.function = function;
        this.currentLevel = currentLevel;
        this.score = score;
        this.fails = fails;
    }
}

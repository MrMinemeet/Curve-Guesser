using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class TrackData
{
    private static string datapathTrackInfo = Application.persistentDataPath + "TrackData.dat";
    private static string datapathFinishedTracks = Application.persistentDataPath + "FinishedTracks.dat";
    private static string datapathSettings = Application.persistentDataPath + "Settings.dat";
    
    public static Dictionary<Difficulty, Dictionary<Function, TrackInfo>> tracks;
    public static ISet<(Difficulty, Function)> finishedTracks;
    public static Settings settings;

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
        FileStream fs = File.Create(datapathTrackInfo);
        bf.Serialize(fs, tracks);
        fs.Close();
    }
    public static void Set(Settings settings)
    {
        TrackData.settings = settings;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(datapathSettings);
        bf.Serialize(fs, settings);
        fs.Close();
    }

    public static void TrackFinished(Difficulty difficulty, Function function)
    {
        finishedTracks.Add((difficulty, function));
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(datapathFinishedTracks);
        bf.Serialize(fs, finishedTracks);
        fs.Close();
    }

    public static void Load()
    {
        if (File.Exists(datapathTrackInfo))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(datapathTrackInfo, FileMode.Open);
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
        if (File.Exists(datapathFinishedTracks))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(datapathFinishedTracks, FileMode.Open);
            finishedTracks = (HashSet<(Difficulty, Function)>)bf.Deserialize(fs);
            fs.Close();
        }
        else
        {
            finishedTracks = new HashSet<(Difficulty, Function)>();
        }
        if (File.Exists(datapathSettings))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(datapathSettings, FileMode.Open);
            settings = (Settings)bf.Deserialize(fs);
            fs.Close();
        }
        else
        {
            settings = new Settings(true, true, true, 1.0f);
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

[Serializable]
public class Settings
{
    public bool showGraph;
    public bool showGrid;
    public bool showFunction;
    public float volume;

    public Settings(bool showGraph, bool showGrid, bool showFunction, float volume)
    {      
        this.showGraph = showGraph;
        this.showGrid = showGrid;
        this.showFunction = showFunction;
        this.volume = volume;
    }
}

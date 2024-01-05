using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class Level
{
    public List<Vector2> Points { get; }
    public string Hint { get; }
    public Difficulty Difficulty { get; }

    public Level(Difficulty difficulty, params (float x, float y)[] points)
    {
        this.Points = points.Select(it => new Vector2(it.x, it.y)).ToList();
        this.Difficulty = difficulty;
        this.Hint = "No hint.";
    }
    public Level(Difficulty difficulty, string hint, params (float x, float y)[] points): this(difficulty, points)
    {
        this.Hint = hint;
    }

    public static List<Level> levels = new()
    {
        new Level(Difficulty.Easy, "All ones.", (-1,-1), (0,0), (1,1))
    };
}

enum Difficulty
{
    Tutorial, Easy, Medium, Hard, Expert
}

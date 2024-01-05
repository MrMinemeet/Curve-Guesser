using System.Collections.Generic;
using System.Linq;
using UnityEngine;

abstract class Level
{
    public List<Vector2> Points { get; }
    public string Hint { get; }
    public abstract List<string> Parameters { get; }
    public Difficulty Difficulty { get; }

    protected Level(Difficulty difficulty, params (float x, float y)[] points)
    {
        this.Points = points.Select(it => new Vector2(it.x, it.y)).ToList();
        this.Difficulty = difficulty;
        this.Hint = "No hint.";
    }
    protected Level(Difficulty difficulty, string hint, params (float x, float y)[] points): this(difficulty, points)
    {
        this.Hint = hint;
    }
}

enum Difficulty
{
    Tutorial, Easy, Medium, Hard, Expert
}

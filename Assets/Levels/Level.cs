using System.Collections.Generic;
using System.Linq;
using UnityEngine;

abstract class Level
{
    public List<Vector2> Points { get; }
    public string Hint { get; }

    protected Level(params Vector2[] points)
    {
        this.Points = points.ToList();
    }
    protected Level(string hint, params Vector2[] points): this(points)
    {
        this.Hint = hint;
    }
}
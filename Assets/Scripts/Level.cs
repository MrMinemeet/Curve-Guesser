using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class Level
{
    public List<Vector2> points { get; }
    public List<Vector2> obstacles { get; }
    public string hint { get; }
    public Difficulty difficulty { get; }

    private readonly Func<double, double, double, double, double, double> function;
    public readonly int parameterCount;

    public Level(
        Difficulty difficulty, 
        Function function,
        Vector2[] points,
        Vector2[] obstacles
        )
    {
        this.points = points.ToList();
        this.obstacles = obstacles.ToList();
        this.difficulty = difficulty;
        this.hint = "No hint.";
        switch(function)
        { 
            case Function.Sine: parameterCount = 4; this.function = sine; break;
            case Function.Linear: parameterCount = 2; this.function = linear; break;
            case Function.Square: parameterCount = 3; this.function = square; break;
            case Function.Cubic: parameterCount = 4; this.function = cubic; break;
        }
    }
    
    public Level(Difficulty difficulty, Function function, string hint, Vector2[] points, Vector2[] obstacles) : this(difficulty, function, points, obstacles)
    {
        this.hint = hint;
    }

    //add levels here
    public static List<Level> levels = new()
    {
        new Level(Difficulty.Easy, Function.Sine, "All ones.", new Vector2[]{new(-1,-1), new(0,0), new(1,1)}, new Vector2[]{})
    };

    public double applyLevelFunction(double x, double a, double b, double c, double d) => function(x, a, b, c, d);
    private static double sine(double x, double a, double b, double c, double d) => a * System.Math.Sin(b * x + c) + d;
    private static double linear(double x, double a, double b, double c, double d) => a * x + b;
    private static double square(double x, double a, double b, double c, double d) => a * x * x + linear(x, b, c, c, d);
    private static double cubic(double x, double a, double b, double c, double d) => a * x * x * x + square(x, b, c, d, 0);
}

public enum Difficulty
{
    Tutorial, Easy, Medium, Hard, Expert
}

public enum Function
{
    Sine, Linear, Square, Cubic
}

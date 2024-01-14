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
    public Function function { get; }

    private readonly Func<double, double, double, double, double, double> func;
    public readonly ISet<Parameter> parameters;

    public Level(
        Difficulty difficulty, 
        Function function,
        string hint,
        Vector2[] points,
        Vector2[] obstacles,
        params Parameter[] parameters
        )
    {
        this.points = points.ToList();
        this.obstacles = obstacles.ToList();
        this.difficulty = difficulty;
        this.function = function;
        switch(function)
        {
            case Function.Sine: func = sine; break;
            case Function.Linear: func = linear; break;
            case Function.Square: func = square; break;
            case Function.Cubic: func = cubic; break;
        }
        this.hint = hint;
        this.parameters = parameters.ToHashSet();
    }
    private static Vector2[] empty = new Vector2[0];

    //add levels here
    public static List<Level> levels = new()
    {
        //Sine
        //Tutorial
        new Level(Difficulty.Tutorial, Function.Sine, "This is a sine function, it represents a wave. You can play around with the parameters and see what they do.", empty, empty, Parameter.a, Parameter.b, Parameter.c, Parameter.d),
        new Level(Difficulty.Tutorial, Function.Sine, "Your Goal is to collect as many stars as you can. Hitting them closer to the center gains you more points.", new Vector2[]{ new(-Mathf.PI, 0), new(-Mathf.PI/2,-1), new(0,0), new(Mathf.PI / 2, 1), new(Mathf.PI, 0) }, empty),
        new Level(Difficulty.Tutorial, Function.Sine, "You can change the amplitude of the wave with the parameter a.", new Vector2[]{new(-1,-1), new(0,0), new(1,1)}, empty, Parameter.a),
        new Level(Difficulty.Tutorial, Function.Sine, "Crashing into an asteroid makes you fail the level. Try it.", empty, new Vector2[]{ new(5 * Mathf.PI / 2, 1) }, Parameter.a),
        new Level(Difficulty.Tutorial, Function.Sine, null, new Vector2[]{new(-1,1), new(0,0), new(1,-1)}, new Vector2[]{new(0,3), new(0,-3)}, Parameter.a),

        //Linear
        //Tutorial
        new Level(Difficulty.Tutorial, Function.Linear, "This is a linear function. You can play around with the parameters and see what they do.", empty, empty, Parameter.a, Parameter.b),
        
        //Square
        //Tutorial
        new Level(Difficulty.Tutorial, Function.Square, "This is a quare function. You can play around with the parameters and see what they do.", empty, empty, Parameter.a, Parameter.b, Parameter.c),
        
        //Cubic
        //Tutorial
        new Level(Difficulty.Tutorial, Function.Cubic, "This is a cubic function. You can play around with the parameters and see what they do.", empty, empty, Parameter.a, Parameter.b, Parameter.c, Parameter.d),

    };

    public double applyLevelFunction(double x, double a, double b, double c, double d) => func(x, a, b, c, d);
    private static double sine(double x, double a, double b, double c, double d) => a * System.Math.Sin(b * x + c) + d;
    private static double linear(double x, double a, double b, double c, double d) => a * x + b;
    private static double square(double x, double a, double b, double c, double d) => a * x * x + linear(x, b, c, c, d);
    private static double cubic(double x, double a, double b, double c, double d) => a * x * x * x + square(x, b, c, d, 0);

    public double DistanceToCurve(Vector2 point, double a, double b, double c, double d)
    {
        double minDistance = Double.PositiveInfinity;
        for (double x = point.x - 2; x < point.x + 2; x+=0.01)
        {
            double y = applyLevelFunction(x, a, b, c, d);
            double distance = (point - new Vector2((float)x, (float)y)).magnitude;
            if (distance < minDistance) minDistance = distance;
;       }

        return minDistance;
    }
}

public enum Difficulty
{
    Tutorial, Easy, Medium, Hard, Expert
}

public enum Function
{
    Sine, Linear, Square, Cubic
}

public enum Parameter
{
    a, b, c, d
}

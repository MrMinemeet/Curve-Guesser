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
    private const float PI = Mathf.PI;

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

    #region Levels
    /// <summary>
    /// Levels for the game are declared here.
    /// </summary>
    //add levels here

    private static Parameter[] a = new Parameter[] { Parameter.a };
    private static Parameter[] b = new Parameter[] { Parameter.b };
    private static Parameter[] c = new Parameter[] { Parameter.c };
    private static Parameter[] d = new Parameter[] { Parameter.d };
    private static Parameter[] ab = new Parameter[] { Parameter.a, Parameter.b };
    private static Parameter[] ac = new Parameter[] { Parameter.a, Parameter.c };
    private static Parameter[] ad = new Parameter[] { Parameter.a, Parameter.d };
    private static Parameter[] bc = new Parameter[] { Parameter.b, Parameter.c };
    private static Parameter[] bd = new Parameter[] { Parameter.b, Parameter.d };
    private static Parameter[] cd = new Parameter[] { Parameter.c, Parameter.d };
    private static Parameter[] abc = new Parameter[] { Parameter.a, Parameter.b, Parameter.c };
    private static Parameter[] abd = new Parameter[] { Parameter.a, Parameter.b, Parameter.d };
    private static Parameter[] acd = new Parameter[] { Parameter.a, Parameter.c, Parameter.d };
    private static Parameter[] bcd = new Parameter[] { Parameter.b, Parameter.c, Parameter.d };
    private static Parameter[] abcd = new Parameter[] { Parameter.a, Parameter.b, Parameter.c, Parameter.d };
    public static List<Level> levels = new()
    {
        //Sine
        //Tutorial
        new Level(Difficulty.Tutorial, Function.Sine, "This is a sine function, it represents a wave. You can play around with the parameters and see what they do.", empty, empty, abcd),
        new Level(Difficulty.Tutorial, Function.Sine, "Your Goal is to collect as many stars as you can. Hitting them closer to the center gains you more points.", new Vector2[]{new(-PI, 0), new(-PI/2,-1), new(0,0), new(PI / 2, 1), new(PI, 0)}, empty),
        new Level(Difficulty.Tutorial, Function.Sine, "You can change the amplitude of the wave with the parameter a.", new Vector2[]{new(-1,-1), new(0,0), new(1,1)}, empty, a),
        new Level(Difficulty.Tutorial, Function.Sine, "Crashing into an asteroid makes you fail the level. Try it.", empty, new Vector2[]{ new(5 * PI / 2, 1) }, a),
        new Level(Difficulty.Tutorial, Function.Sine, null, new Vector2[]{new(-1.1f,1), new(0,0), new(1,-1.1f)}, new Vector2[]{new(-PI,-1), new(PI, 1) }, a),
        new Level(Difficulty.Tutorial, Function.Sine, "The frequency of the curve can be changed with the parameter b.", new Vector2[]{new(-3*PI/4,1),new(-PI/4,-1),new(PI/4,1),new(3*PI/4,-1),new(5*PI/4,1)}, new Vector2[]{new(-3*PI/4,-1),new(-PI/4,1),new(PI/4,-1),new(3*PI/4,1),new(5*PI/4,-1)}, b),
        new Level(Difficulty.Tutorial, Function.Sine, null, new Vector2[]{new(-PI,3.3f), new(0,0), new(2*PI, 0)}, new Vector2[]{new(-PI, 0), new Vector2(PI, 0), new(-PI, 4.5f), new(-PI, 1.4f), new(-PI, -1.4f), new(-PI, -3.3f), new(-PI, -4.5f)}, ab),
        new Level(Difficulty.Tutorial, Function.Sine, "Parameter c can shift the entire curve to the left and right.", new Vector2[]{new(0, 1), new(2*PI, 1), new(-2*PI, 1), new(-PI,-1), new(PI,-1)}, empty, c),
        new Level(Difficulty.Tutorial, Function.Sine, null, new Vector2[]{new(0, 1), new(PI, 1), new(-PI, 1), new(-PI/2,-1), new(PI/2,-1)}, empty, bc),
        new Level(Difficulty.Tutorial, Function.Sine, null, new Vector2[]{new(-PI,1),new(-PI/2,-1),new(0,1),new(PI/2,-1),new(PI,1)}, new Vector2[]{new(-PI,-1),new(-PI/2,1),new(0,-1),new(PI/2,1),new(PI,-1)}, bc), 
        new Level(Difficulty.Tutorial, Function.Sine, null, new Vector2[]{new(0,3), new(8.55f,0), new(-8.55f,0)}, new Vector2[]{new(-6, 0),new(-5, 0),new(-4, 0),new(-3, 0),new(-2, 0),new(-1, 0),new(0, 0),new(1, 0),new(2, 0),new(3, 0),new(4, 0),new(5, 0),new(6, 0)}, abc), 
        new Level(Difficulty.Tutorial, Function.Sine, "Parameter d can move the curve up and down.", new Vector2[]{new(-PI, 4), new(-PI/2,3), new(0,4), new(PI / 2, 5), new(PI, 4)}, new Vector2[]{new(-2*PI, -5),new(-2*PI, -4),new(-2*PI, -3),new(-2*PI, -2),new(-2*PI, -1),new(-2*PI, 0),new(-2*PI, 1),new(-2*PI, 2)}, d),
        new Level(Difficulty.Tutorial, Function.Sine, null, new Vector2[]{new(-2*PI, -2),new(0, -2),new(2*PI, -2)}, new Vector2[]{new(-2*PI, -3),new(0, -3),new(2*PI, -3),new(-2*PI, -1),new(0, -1),new(2*PI, -1),new(-PI, -3),new(PI, -1),new(-PI, -1),new(PI, -3),new(-3*PI/2,-3),new(-PI/2,-3)}, abcd),
        new Level(Difficulty.Tutorial, Function.Sine, null, new Vector2[]{new(-2*PI, -2),new(0, -2),new(2*PI, -2)}, new Vector2[]{new(-2*PI, -3),new(0, -3),new(2*PI, -3),new(-2*PI, -1),new(0, -1),new(2*PI, -1),new(-PI, -3),new(PI, -1),new(-PI, -1),new(PI, -3), new(2*PI+1.2f, -2),new(-3*PI/2,-3),new(-PI/2,-3)}, abcd), 
        new Level(Difficulty.Tutorial, Function.Sine, "Lets see how well you can apply all these parameters.", new Vector2[]{new(0,4),new(-2*PI, 4),new(7/6*PI,1)}, new Vector2[]{ new(-8, 0), new(-7, 0),new(-6, 0),new(-5, 0),new(-4, 0),new(-3, 0),new(-2, 0),new(-1, 0),new(0, 0),new(1, 0),new(2, 0),new(3, 0),new(4, 0),new(5, 0),new(6, 0),new(7, 0),new(8, 0)}, abcd), 
        //Easy
        new Level(Difficulty.Easy, Function.Sine, "So you have finished the first Track. Can you navigate us through this galaxy?", new Vector2[]{new(0,0),new(-1.5f,0),new(-3,0),new(1.5f,0),new(3,0)}, new Vector2[]{new(-0.75f, 0),new(-2.25f,0),new(0.75f, 0),new(2.25f, 0),new(-0.75f, -1),new(0.75f, 1),new(-2.25f, -2),new(-2.25f, -3),new(-2.25f, -4),new(-2.25f, -5), new(-2.25f, 1), new(-2.25f, 2), new(-2.25f, 3),new(-2.25f, 4),new(-2.25f, 5)}, ab),
        new Level(Difficulty.Easy, Function.Sine, null, new Vector2[]{new(-PI,-1),new(-2*PI/3,-1),new(-1*PI/3,-1),new(0,-1),new(PI/3,-1),new(2*PI/3,-1),new(PI,-1)}, new Vector2[]{ new(-7*PI/6, -1.2f),new(-5*PI/6, -1.2f),new(-1*PI/2,-1.2f),new(-1*PI/6, -1.2f),new(PI/6,-1.2f),new(PI/2, -1.2f),new(5*PI/6,-1.2f),new(7*PI/6, -1.2f) }, abcd),
        //Medium
        new Level(Difficulty.Medium, Function.Sine, "This Track is still empty.", empty, empty),
        //Hard
        new Level(Difficulty.Hard, Function.Sine, "This Track is still empty.", empty, empty),
        //Expert
        new Level(Difficulty.Expert, Function.Sine, "This Track is still empty.", empty, empty),

        //Linear
        //Tutorial
        new Level(Difficulty.Tutorial, Function.Linear, "This is a linear function. You can play around with the parameters and see what they do.", empty, empty, Parameter.a, Parameter.b),
        //Easy
        new Level(Difficulty.Easy, Function.Linear, "This Track is still empty.", empty, empty),
        //Medium
        new Level(Difficulty.Medium, Function.Linear, "This Track is still empty.", empty, empty),
        //Hard
        new Level(Difficulty.Hard, Function.Linear, "This Track is still empty.", empty, empty),
        //Expert
        new Level(Difficulty.Expert, Function.Linear, "This Track is still empty.", empty, empty),
        
        //Square
        //Tutorial
        new Level(Difficulty.Tutorial, Function.Square, "This is a quare function. You can play around with the parameters and see what they do.", empty, empty, Parameter.a, Parameter.b, Parameter.c),
        //Easy
        new Level(Difficulty.Easy, Function.Square, "This Track is still empty.", empty, empty),
        //Medium
        new Level(Difficulty.Medium, Function.Square, "This Track is still empty.", empty, empty),
        //Hard
        new Level(Difficulty.Hard, Function.Square, "This Track is still empty.", empty, empty),
        //Expert
        new Level(Difficulty.Expert, Function.Square, "This Track is still empty.", empty, empty),
        
        //Cubic
        //Tutorial
        new Level(Difficulty.Tutorial, Function.Cubic, "This is a cubic function. You can play around with the parameters and see what they do.", empty, empty, Parameter.a, Parameter.b, Parameter.c, Parameter.d),
        //Easy
        new Level(Difficulty.Easy, Function.Cubic, "This Track is still empty.", empty, empty),
        //Medium
        new Level(Difficulty.Medium, Function.Cubic, "This Track is still empty.", empty, empty),
        //Hard
        new Level(Difficulty.Hard, Function.Cubic, "This Track is still empty.", empty, empty),
        //Expert
        new Level(Difficulty.Expert, Function.Cubic, "This Track is still empty.", empty, empty),

    };
    #endregion

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

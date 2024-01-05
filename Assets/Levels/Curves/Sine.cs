using System.Collections.Generic;
using UnityEngine;

class Sine: Level
{
    public Sine(Difficulty difficulty, params (float x, float y)[] points) : base(difficulty, points) {}
    public Sine(Difficulty difficulty, string hint, params (float x, float y)[] points) : base(difficulty, hint, points) { }

    public override List<string> Parameters => new() { "a", "b", "c", "d" };
}
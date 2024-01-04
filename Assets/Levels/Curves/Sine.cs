using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class Sine: Level
{
    List<string> parameters = new List<string>() { "a", "b", "c", "d" };
    public Sine(params Vector2[] points) : base(points) { }
    public Sine(string hint, params Vector2[] points) : base(hint, points) { }
}
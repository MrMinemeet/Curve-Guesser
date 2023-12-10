using System;
using System.Collections.Generic;

public static class Globals
{
	public static readonly Dictionary<String, Func<float, double>> Functions = new();

	static Globals()
	{
		Functions.Add("sin(x)", x => Math.Sin(x));
		Functions.Add("cos(x)", x => Math.Cos(x));
	}
}
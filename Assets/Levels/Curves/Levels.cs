using System.Collections.Generic;

class Levels
{
    public static List<Level> levels = new()
    {
        new Sine(Difficulty.Easy, "All ones.", (-1,-1), (0,0), (1,1))
    };
}
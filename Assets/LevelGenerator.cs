using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Point;
    void Start()
    {
        var level = Levels.levels.First();
        foreach (var point in level.Points)
        {
            var go = Instantiate(Point, new Vector3(point.x, point.y, 0), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

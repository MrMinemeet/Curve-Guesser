using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Rocket")
        {
            GameObject.Find("Graph").GetComponent<CurveController>().OnObstacleCollision();
        }
    }
}

using UnityEngine;

public class PointCollicion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Rocket"))
        {
            GameObject.Find("Graph").GetComponent<CurveController>().OnPointCollision(gameObject);
        }
    }
}

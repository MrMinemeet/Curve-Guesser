using UnityEngine;
using UnityEngine.Events;

public class CollectNotifier : MonoBehaviour
{
    public UnityEvent<GameObject> onCollect;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player collected present!");
            onCollect.Invoke(gameObject);
            
            // make transparent
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
        }
    }
}

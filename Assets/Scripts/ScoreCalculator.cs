using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    private TextMeshProUGUI _scoreValue;
    private GameObject _sleigh;
    private readonly List<GameObject> _presents = new();
    private int _score;
    
    private void Awake()
    {
        var _scoreObj = GameObject.Find("scoreValue");
        _scoreValue = _scoreObj.GetComponent<TextMeshProUGUI>();
        _sleigh = GameObject.Find("Sleigh");
        foreach (GameObject present in GameObject.FindGameObjectsWithTag("Present"))
        {
            _presents.Add(present);
        }
    }

    private void Start()
    {
        // Add listener to the CollectNotifier of each present
        foreach (GameObject present in _presents)
        {
            CollectNotifier notifier = present.GetComponent<CollectNotifier>();
            notifier.onCollect.AddListener((p) => OnPresentCollected(p));
        }
    }
    
    private void OnPresentCollected(GameObject present)
    {
        // Calculate distance between sleigh and present
        float distance = Vector3.Distance(_sleigh.transform.position, present.transform.position);
        Debug.Log($"Distance to present: {distance}");
        
        // Calculate score
        int score = (int) (1000 - (distance * 10));
        Debug.Log($"Score: {score}");
        
        // Add score to total score
        _score += score;
        Debug.Log($"Total score: {_score}");
        _scoreValue.text = _score.ToString();
    }
}

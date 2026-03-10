
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

public class FollowSpline : MonoBehaviour
{
    [SerializeField] private Transform _transform; 
    [SerializeField] private SplineContainer spline;
    [SerializeField] private float speed;

    [SerializeField] private UnityEvent onFollowEnd;
    
    public void StartFollow()
    {
        StartCoroutine(nameof(Move)); 
    }

    private IEnumerator Move()
    {
        if (spline == null) yield break;
        
        float t = 0;
        while (t <= 1)
        {
            Vector3 position = spline.EvaluatePosition(t); 
            _transform.position = position;
            
            t += Time.deltaTime * speed;
            yield return null; 
        }
        _transform.position = spline.EvaluatePosition(1); 
        
        onFollowEnd?.Invoke();
    }
}
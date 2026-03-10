using System.Collections;
using UnityEngine;

public class Lift : MonoBehaviour
{
    private Animator anim;
    public ElectricalReciever powerSource;
    public Transform target; 

    public float LiftSecondsTillTop;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    public void Open()
    {
        anim.SetBool("open", true);
    }

    IEnumerator MoveToTarget(Transform target, float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, target.position, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = target.position;
        Open(); 
    }

    public void StartMoving()
    {
        if (target != null)
        {
            StartCoroutine(MoveToTarget(target, LiftSecondsTillTop));
        }
        else
        {
            Debug.LogError("Target is not assigned!");
        }
    }
}

using UnityEngine;

public class Barricade : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] private TimedInteractable timedInteractable;

    public float rotationSpeed = 1f;

    private void Awake()
    {
        timedInteractable ??= GetComponent<TimedInteractable>();
        timedInteractable.onInteractionTick += PullTick;
        timedInteractable.onTimerFinished += InteractionFinished;
        
        rb ??= GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void PullTick()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    void InteractionFinished()
    {
        rb.isKinematic = false;
        
        UnsubscribeEvents();
        Destroy(timedInteractable);
        
        gameObject.AddComponent<Draggable>();
        Destroy(this); 
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }
    
    private void UnsubscribeEvents()
    {
        if (timedInteractable == null) return;
        
        timedInteractable.onInteractionTick -= PullTick;
        timedInteractable.onTimerFinished -= InteractionFinished;
        
    }
}
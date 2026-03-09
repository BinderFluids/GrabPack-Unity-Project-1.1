using UnityEngine;

public class Grabbable : MonoBehaviour
{
    [SerializeField] private BaseHandBehaviour hand;
    public BaseHandBehaviour Hand => hand;

    public void HandEnter(BaseHandBehaviour hand)
    {
        this.hand = hand;
    }

    public void OnHandExit()
    {
        if (hand == null)
        {
            Debug.LogWarning($"No hand to exit on {gameObject.name}");
            return;
        }

        hand = null; 
    }

    public virtual void Pull()
    {
        
    }

    public void Release()
    {
        
    }
}
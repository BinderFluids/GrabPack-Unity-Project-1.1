using UnityEngine;

public class HandContainer : MonoBehaviour
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
}
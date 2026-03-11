
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class HandsInventory : MonoBehaviour
{
    [SerializeField] private Transform anchor; 
    [SerializeField] private List<BaseHandBehaviour> hands = new();
    public IReadOnlyList<BaseHandBehaviour> Hands => hands;

    [SerializeField] private UnityEvent<BaseHandBehaviour> onHandAdded;
    
    public bool TryAdd(HandType type)
    {
        if (hands.Any(h => h.HandType == type)) return false; 

        GameObject newHandGameObject = Instantiate(type.HandPrefab);
        BaseHandBehaviour newHand = newHandGameObject.GetComponent<BaseHandBehaviour>();
        PlaceHand(newHand);
        
        hands.Add(newHand);
        onHandAdded?.Invoke(newHand);
        
        return true;
    }

    private void PlaceHand(BaseHandBehaviour hand)
    {
        hand.SetParent(anchor, false); 
    }
    
    public BaseHandBehaviour GetHand(int index)
    {
        if (index < 0 || index >= hands.Count) return null;
        return hands[index];
    }
}
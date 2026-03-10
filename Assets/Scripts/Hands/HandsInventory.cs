
using System.Collections.Generic;
using UnityEngine;

public class HandsInventory : MonoBehaviour
{
    [SerializeField] private List<BaseHandBehaviour> hands;
    public IReadOnlyList<BaseHandBehaviour> Hands => hands;

    public void Add(BaseHandBehaviour newHand)
    {
        if (hands.Contains(newHand)) return;
        hands.Add(newHand);
    }
    
    public BaseHandBehaviour GetHand(int index)
    {
        if (index < 0 || index >= hands.Count) return null;
        return hands[index];
    }
}
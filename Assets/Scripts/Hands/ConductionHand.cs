using UnityEngine;

public class ConductionHand : BaseHandBehaviour
{
    [SerializeField] private float elementHoldTime;
    [SerializeField] private ConductionHandElement conductionHandElement;
    public ConductionHandElement ConductionHandElement => conductionHandElement;

    public void SetElement(ConductionHandElement value)
    {
        if (conductionHandElement != ConductionHandElement.None) return;
        conductionHandElement = value;
        Invoke(nameof(ClearElement), elementHoldTime);
    }

    public void ClearElement()
    {
        conductionHandElement = ConductionHandElement.None;
    }
}
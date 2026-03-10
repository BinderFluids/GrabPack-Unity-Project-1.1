using UnityEngine;

public class ConductionHand : BaseHandBehaviour
{
    [SerializeField] private float elementHoldTime;
    [SerializeField] private Element element;
    public Element Element => element;

    public void SetElement(Element value)
    {
        if (element != Element.None) return;
        element = value;
        Invoke(nameof(ClearElement), elementHoldTime);
    }

    public void ClearElement()
    {
        element = Element.None;
    }
}
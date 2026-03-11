
using UnityEngine;

public class ElementSource : MonoBehaviour
{
    [SerializeField] private ElementType elementType;
    public ElementType ElementType => elementType;

    public void GiveElement(IConductor conductor) => conductor.Conduct(elementType);
}
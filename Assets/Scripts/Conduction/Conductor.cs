using Conduction;
using UnityEngine;


public class Conductor : MonoBehaviour
{
    [SerializeField] private ElementType elementType;
    public ElementType ElementType => elementType;
    
    
    public void SetElementType(ElementType type)
    {
        elementType = type;
    }

    public void Conduct(Reactant reactant)
    {
        
    }
}
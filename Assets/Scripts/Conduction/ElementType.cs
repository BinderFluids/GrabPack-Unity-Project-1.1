
using UnityEngine;

public class ElementType : ScriptableObject
{
    [SerializeField] private Color color; 
    public Color Color => color;
}
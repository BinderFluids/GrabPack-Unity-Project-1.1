
using UnityEngine;

[CreateAssetMenu(menuName = "Create ElementType", fileName = "ElementType", order = 0)]
public class ElementType : ScriptableObject
{
    [SerializeField] private Color color; 
    public Color Color => color;
}
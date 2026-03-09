using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Draggable : HandInteractable
{
    [SerializeField] private Rigidbody rb;
    public Rigidbody Rigidbody => rb; 
}
using UnityEngine;

public class GrabPackManager : MonoBehaviour
{
    [Header("Behaviours")]
    [SerializeField] private PowerableBehaviour powerableBehaviour;
    public PowerableBehaviour PowerableBehaviour => powerableBehaviour;
    
    [Header("Fields")]
    [SerializeField] private AudioClip fireClip;
    [field: SerializeField] public float MaxRange { get; private set; }
    [field: SerializeField] public float FireSpeed { get; private set; }
    [field: SerializeField] public float PullSpeed { get; private set; }
}
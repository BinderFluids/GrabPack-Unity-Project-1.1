using UnityEngine;

public class GrabPackManager : MonoBehaviour
{
    [Header("Behaviours")] 
    [SerializeField] private Rigidbody playerRigidbody; 
    public Rigidbody PlayerRigidbody => playerRigidbody;
    
    [SerializeField] private PowerableBehaviour powerableBehaviour;
    public PowerableBehaviour PowerableBehaviour => powerableBehaviour;

    [SerializeField] private CableManager cableManager;
    public CableManager CableManager => cableManager;
    
    [Header("Fields")]
    [SerializeField] private Animator animator;
    public Animator Animator => animator;
    [SerializeField] private AudioClip fireClip;
    [field: SerializeField] public float MaxRange { get; private set; }
    [field: SerializeField] public float FireSpeed { get; private set; }
    [field: SerializeField] public float PullSpeed { get; private set; }
}
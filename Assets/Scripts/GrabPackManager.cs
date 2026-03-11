using UnityEngine;

public class GrabPackManager : MonoBehaviour
{
    [field: SerializeField] public float MaxRange { get; private set; }
    
    [field: SerializeField] public float FireSpeed { get; private set; }
    
    [field: SerializeField] public float PullSpeed { get; private set; }

    [SerializeField] private AudioClip fireClip;
}
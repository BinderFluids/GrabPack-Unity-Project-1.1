using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour, IPressureHandInteractable
{
    public GameObject ParticleSystem;
    public MeshRenderer renderer;
    public BoxCollider collider;

    public GameObject objToDeactivate;

    [SerializeField] private float pressureBreakThreshold = 9f;
    

    public MeshRenderer[] renderers;


    private bool canBreak;
    public void SetBreakable(bool value) =>  canBreak = value;
    
    public void BreakObject()
    {
        if (!canBreak) return;
        
        ParticleSystem.SetActive(true);

        if (renderer != null)
        {
            renderer.enabled = false;
        }

        collider.enabled = false; 


        foreach (MeshRenderer render in renderers)
        {
            render.enabled = false;
        }
    }

    public void ReleasePressure(BaseHandBehaviour hand, float pressure)
    {
        if (pressure >= pressureBreakThreshold)
        {
            BreakObject();
        }
    }
}

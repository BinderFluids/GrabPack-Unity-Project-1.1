using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class PressureHand : BaseHandBehaviour
{
    private float currentPresssure = 0;


    [Header("Pressure Hand")] 
    [Header("Settings")] 
    [SerializeField] private float pressureBuildSpeed = 4f; 
    [SerializeField] private float minimumPressure = 1f; 
    [SerializeField] private float maxPressure = 10f;
    
    [Header("Audio")]
    [SerializeField] private AudioSource pressureBuildAudioSource;
    [SerializeField] private AudioClip pressureReleaseAudioClip;

    [Header("Particles")]
    [SerializeField] private GameObject smokeParticlesContainer;
    [SerializeField] private ParticleSystem impactParticleSystem;

    [Header("UI")]
    [SerializeField] private GameObject gaugeCrosshair;
    [SerializeField] private Image guageImage;
    [SerializeField] private GameObject Crosshair;
    
    protected override void StartPull()
    {
        currentPresssure = 0f; 
    }
    
    protected override void UpdatePull()
    {
        if (Interactable == null) return;
        if (Interactable is not IPressureHandInteractable)
        {
            if (!Interactable.TryGetComponent(out IPressureHandInteractable iphi))
            {
                Interactable.UpdatePull(this); 
                return;
            }
        }
        
        currentPresssure += Time.deltaTime * pressureBuildSpeed;
        SetActive(true); 
        
        //Crosshair.SetActive(false);
        
        currentPresssure += Time.deltaTime;
        currentPresssure = Mathf.Min(currentPresssure, maxPressure);
        guageImage.fillAmount = currentPresssure / maxPressure;
    }

    protected override void OnRetract()
    {
        if (Interactable == null) return;

        IPressureHandInteractable pressureHandInteractable;
        if (Interactable is not IPressureHandInteractable ip)
            Interactable.TryGetComponent(out pressureHandInteractable);
        else
            pressureHandInteractable = ip; 
        
        SetActive(false); 
        
        if (pressureHandInteractable == null) return;
        
        
        if (currentPresssure >= minimumPressure)
        {
            GlobalAudio.Instance.PlayOneShot(pressureReleaseAudioClip, 2.0f);
            impactParticleSystem.Play();
        }
        
        pressureHandInteractable.ReleasePressure(this, currentPresssure);
        //Crosshair.SetActive(true);
    }

    void SetActive(bool active)
    {
        smokeParticlesContainer.SetActive(active);
        pressureBuildAudioSource.gameObject.SetActive(active);
        gaugeCrosshair.SetActive(active);
    }
    
    //     if ((leftReleased || rightReleased) && isPressureHand)
    // {
    //
    //     canDrag = false;
    //     CanReturn = true;
    //     GameObject grabbedObj = handTransform.parent?.gameObject;
    //
    //     pressureHoldTimer = 0f;
    //     pressureBuilding = false;
    //
    //     if (grabbedObj != null)
    //     {
    //         Rigidbody rb = grabbedObj.GetComponent<Rigidbody>();
    //         if (rb != null)
    //         {
    //             if (!grabbedObj.GetComponent<Breakable>())
    //             {
    //                 if (pressure >= 1f)
    //                 {
    //                     Vector3 launchDir =(grabbedObj.transform.position - handOrigin.position).normalized;
    //
    //                     float launchForce = pressure * 100f;
    //
    //                     handgrabbing.SetBool("grabbing", true);
    //                     rb.isKinematic = false;
    //                     rb.AddForce(launchDir * launchForce, ForceMode.Impulse);
    //
    //
    //                     impact.Play();
    //                 }
    //
    //             }
    //             else
    //             {
    //                 if (pressure > 9f)
    //                 {
    //                     Breakable breakable =
    //                         grabbedObj.GetComponent<Breakable>();
    //
    //                     breakable.breakObject();
    //                     impact.Play();
    //                 }
    //             }
    //         }
    //     }
    //
    //     SMOKE.SetActive(false);
    //     gauge.SetActive(false);
    //     if (pressure >= 1f)
    //     {
    //         globalAudio.PlayOneShot(pressureRelease, 2.0f);
    //
    //     }
    //     pressure = 0f;
    //     pressurebuild.SetActive(false);
    //
    //
    //     Crosshair.SetActive(true);
    //
    //     return1();
    //     return; 
    // }

    //LATE UPDATE
    //     pressureHoldTimer += Time.deltaTime;
    //
    //     if (pressureHoldTimer >= pressureStartDelay)
    //     {
    //         pressureBuilding = true;
    //
    //         SMOKE.SetActive(true);
    //         pressurebuild.SetActive(true);
    //         gauge.SetActive(true);
    //         Crosshair.SetActive(false);
    //
    //         if (pressure < 10f)
    //         {
    //             pressure += Time.deltaTime * 4f;
    //             pressure = Mathf.Min(pressure, 10f);
    //             guageUI.fillAmount = pressure / 10f;
    //         }
    //     }
    //
    //     Breakable breakable = grabbed.GetComponent<Breakable>();
    //         if (breakable != null && breakable.SwitchMaterials)
    //     {
    //         if (pressure < 5f)
    //             breakable.renderer.material = breakable.pristine;
    //         else if (pressure <= 9f)
    //             breakable.renderer.material = breakable.damaged;
    //         else
    //             breakable.renderer.material = breakable.broken;
    //     }
    // }
}
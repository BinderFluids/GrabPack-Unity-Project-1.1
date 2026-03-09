using UnityEngine;

public class PressureHand : BaseHandBehaviour
{
    private float pressureHoldTimer = 0f;
    public float pressureStartDelay = 0.5f; 
    private bool pressureBuilding = false;
    
    
    public GameObject pressurebuild;
    public AudioClip pressureRelease;
        
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
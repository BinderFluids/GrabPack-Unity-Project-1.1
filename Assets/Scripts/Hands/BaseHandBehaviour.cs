using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.LowLevelPhysics2D;
using UnityEngine.UI;


public class BaseHandBehaviour : MonoBehaviour
{
    private bool mouseHeld = false;
    public Transform limonpos;
    private bool awaitingSecondInput = false;

    public CableManager cableManager;

    public string Hand = "Left";
    public Transform handTransform;
    public Transform handOrigin;
    public float maxRange = 10f;
    public float speed = 20f;

    private Transform originalParent;
    private bool isActive;
    public bool IsActive => isActive;

    public Animator playeranimations;

    public string GrabableLayer;

    public bool CanReturn = true;
    public bool canDrag = false;
    public float pullSpeed = 5f;

    public CablePhysics CableSim;
    
    public GameObject hitGameObject;

    public Vector3 selftransform;

    public bool holdingbattery = false;
    public GameObject battery;
    public Transform batterypos;

    public Animator handgrabbing;
    public Barricade br;

    public AudioSource globalAudio;
    public AudioClip firesfx;
    public AudioClip grabsfx;
    public AudioClip dragsfx;
    public GameObject dragsounds;

    [SerializeField] private bool lockReturn = false;

    public RotateArms aimOverride;


    public bool isPressureHand = false;
    public float pressure = 0;

    public GameObject SMOKE;

    public GameObject gauge;
    public Image guageUI;
    public ParticleSystem impact;

    public GameObject Crosshair;
    private HandInteractable interactable;
    private Pickupable pickupable;
    
    public bool MouseButtonHeld { get; private set; }
    
    void Start()
    {
        //cableRenderer.enabled = false;

        originalParent = handTransform.parent;

        selftransform = gameObject.transform.localScale;
    }
    
    public void EnableDrag()
    {
        canDrag = true;
    }

    public void HandleInput(int mouseIndex, Ray ray, float maxRange)
    {
        if (Input.GetMouseButtonDown(mouseIndex))
            if (!IsActive && pickupable == null) FireHand(ray, maxRange);
            else if (IsActive && pickupable != null) pickupable.Retract(this); 
            //else if (interactable == null) Return();

        // if (Input.GetMouseButton(mouseIndex))
        // {
        //     MouseButtonHeld = true;
        // }

        MouseButtonHeld = Input.GetMouseButton(mouseIndex);

        if (Input.GetMouseButtonUp(mouseIndex))
        {
            if (interactable != null)
                Return(); 
        }
    }
    
    
    public void FireHand(Ray ray, float range)
    {
        if (isActive) return;
        if (!gameObject.activeSelf) return;
        
        //globalAudio.PlayOneShot(firesfx, 0.7f);

        CableSim.isActive = true;
        float remaining = cableManager.GetRemainingLength();
        maxRange = Mathf.Min(remaining, range);
        
        Physics.Raycast(ray, out RaycastHit hit, maxRange);
        Vector3 targetPoint = hit.collider ? hit.point : ray.origin + ray.direction * maxRange;

        if (Hand == "Right")
        {
            aimOverride.rightActive = true;
        }
        if (Hand == "Left")
        {
            aimOverride.leftActive = true;
        }

        if (holdingbattery)
        {
            holdingbattery = false;
            battery.transform.parent = null;
            Rigidbody batteryRB = battery.GetComponent<Rigidbody>();
            BoxCollider col = battery.GetComponent<BoxCollider>();

            batteryRB.isKinematic = false;

            if (Hand == "Left")
            {
                batteryRB.AddForce(-transform.up * 800f, ForceMode.Impulse);
                aimOverride.leftActive = false;

            }
            if (Hand == "Right")
            {
                batteryRB.AddForce(transform.up * 800f, ForceMode.Impulse);
                aimOverride.rightActive = false;

            }

            CableSim.isActive = false;

            col.enabled = true;
            battery = null;
            handgrabbing.SetBool("grabbing", false);

            return;
        }
        CanReturn = true;
        if (isActive) return;
        
        playeranimations.SetTrigger("shoot");
        handTransform.parent = null;
        isActive = true;


        if (Hand == "Right")
        {
            aimOverride.rightHitPoint = targetPoint;
        }
        if (Hand == "Left")
        {
            aimOverride.leftHitPoint = targetPoint;
        }

        if (Hand == "Right")
        {
            Vector3 projectedForward = Vector3.ProjectOnPlane(transform.forward, -hit.normal);
            handTransform.rotation = Quaternion.LookRotation(projectedForward, -hit.normal);
        }
        if (Hand == "Left")
        {
            Vector3 projectedForward = Vector3.ProjectOnPlane(transform.forward, hit.normal);
            handTransform.rotation = Quaternion.LookRotation(projectedForward, hit.normal);
        }

        // if (hit.collider != null)
        //     if (hit.collider.gameObject.tag == (GrabableLayer))
        //     {
        //         CanReturn = false;
        //
        //         hitGameObject = hit.collider.gameObject;
        //         if (LayerMask.LayerToName(hitGameObject.layer) == "Battery")
        //         {
        //             holdingbattery = true;
        //             handgrabbing.SetBool("grabbing", true);
        //         }
        //         if (hitGameObject.GetComponent<HandScanner>() == true)
        //         {
        //             handgrabbing.SetBool("grabbing", false);
        //         }
        //         if (hitGameObject.GetComponent<Rigidbody>() != null)
        //         {
        //             if (hitGameObject.GetComponent<Barricade>() != null)
        //             {
        //                 br = hitGameObject.GetComponent<Barricade>();
        //
        //             }
        //             if (!isPressureHand)
        //             {
        //                 handgrabbing.SetBool("grabbing", true);
        //
        //             }
        //
        //             Invoke("EnableDrag", 0.5f);
        //
        //         }
        //         if (LayerMask.LayerToName(hitGameObject.layer) == "Grabanimation" || LayerMask.LayerToName(hitGameObject.layer) == "Minecart" || LayerMask.LayerToName(hitGameObject.layer) == "KeyCard")
        //         {
        //             handgrabbing.SetBool("grabbing", true);
        //         }
        //
        //
        //         if (Hand == "Right")
        //         {
        //             Vector3 projectedForward = Vector3.ProjectOnPlane(transform.forward, -hit.normal);
        //             handTransform.rotation = Quaternion.LookRotation(projectedForward, -hit.normal);
        //         }
        //         if (Hand == "Left")
        //         {
        //             Vector3 projectedForward = Vector3.ProjectOnPlane(transform.forward, hit.normal);
        //             handTransform.rotation = Quaternion.LookRotation(projectedForward, hit.normal);
        //         }
        //
        //     }
        Vector3 impactPoint = hit.point;
        
        if (hit.collider != null)
            if (hit.collider.TryGetComponent(out HandInteractable interactable))
            {
                CanReturn = false; 
                this.interactable = interactable;
                interactable.Grab(this); 
                handgrabbing.SetBool("grabbing", interactable.GrabType == HandInteractable.GrabTypeEnum.Grip);
                HandleInteractable(interactable);
            }
        
        StartCoroutine(MoveHand(targetPoint, impactPoint));
    }

    void HandleInteractable(HandInteractable interactable)
    {
        if (interactable is Draggable draggable)
        {
            HandleDraggable(draggable);
            return; 
        }
        if (interactable is Pickupable pickupable)
        {
            HandlePickupable(pickupable);
            return;
        }
    }

    protected virtual void HandleDraggable(Draggable draggable)
    {
        
    }
    protected virtual void HandlePickupable(Pickupable pickupable)
    {
        
    }

    public void GiveItem(Pickupable pickupable)
    {
        this.pickupable ??= pickupable;
    }

    public void ReleaseItem()
    {
        if (pickupable == null) return;
        
        pickupable.Retract(this);
        pickupable = null; 
    }
    
    public void Return()
    {
        if (!gameObject.activeSelf) return;
        if (lockReturn || canDrag) return;

        CanReturn = true;
        
        interactable?.Retract(this);
        interactable = null; 
        StartCoroutine(ReturnHand());
    }

    public void SetParent(Transform parent, bool worldPositionStays = true)
    {
        handTransform.SetParent(parent, worldPositionStays); 
    }
    
    private IEnumerator MoveHand(Vector3 target, Vector3 impactPoint)
    {
        Vector3 start = handTransform.position;
        float distance = Vector3.Distance(start, target);
        float duration = distance / speed;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            handTransform.position = Vector3.Lerp(start, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        handTransform.position = target;
        if (hitGameObject != null)
        {
            handTransform.position = impactPoint;
            handTransform.SetParent(hitGameObject.transform, true);
            if (LayerMask.LayerToName(hitGameObject.layer) == "Battery")
            {

                if (hitGameObject.GetComponent<gear>() != null)
                {
                    if (hitGameObject.transform.childCount == 2)
                    {
                        battery = hitGameObject;
                    }
                }
                else
                {
                    if (hitGameObject.transform.childCount == 1)
                    {
                        battery = hitGameObject;
                    }
                }
            }
            
            lockReturn = true;
            StartCoroutine(UnlockReturn());
        }

        yield return new WaitForSeconds(0.1f);

        if (CanReturn)
        {
            StartCoroutine(ReturnHand());
        }
    }

    private IEnumerator UnlockReturn()
    {
        yield return new WaitForSeconds(0.1f);
        lockReturn = false;
    }


    private IEnumerator ReturnHand()
    {
        handTransform.parent = null;

        if (battery != null)
        {
            limon Limon = battery.GetComponent<limon>();
            if (Limon == null)
            {
                battery.transform.position = batterypos.position;
                battery.transform.rotation = batterypos.rotation;
            }
            else
            {
                battery.transform.position = limonpos.position;
                battery.transform.rotation = limonpos.rotation;
            }


            Rigidbody rb = battery.GetComponent<Rigidbody>();
            
            if (rb != null)
            {
                battery.transform.parent = null;

                rb.isKinematic = true;

                BoxCollider col1 = battery.GetComponent<BoxCollider>();
                col1.enabled = false;
                battery.transform.parent = handTransform;

            }
        }

        handgrabbing.SetBool("grabbing", true);

        //dragsounds.SetActive(false);
        pressure = 0;
        globalAudio.PlayOneShot(grabsfx, 0.7f);

        br = null;
        Transform batterychild = transform.Find("Battery");
        if (batterychild == null)
        {
            batterychild = transform.Find("Gear");

        }
        if (batterychild == null)
        {
            holdingbattery = false;
            battery = null;
        }


        isActive = true;
        canDrag = false;
        hitGameObject = null;
        Vector3 startPosition = handTransform.position;
        Quaternion startRotation = handTransform.rotation;

        float duration = Vector3.Distance(startPosition, handOrigin.position) / speed;
        float elapsedTime = 0f;

        while (CableSim.GetCablePoints().Count > 1)
        {
            List<Vector3> points = CableSim.GetCablePoints();

            if (points.Count <= 2)
                break;

            Vector3 targetPoint = points[points.Count - 2];

            while (Vector3.Distance(handTransform.position, targetPoint) > 0.05f)
            {
                handTransform.position = Vector3.MoveTowards(
                    handTransform.position,
                    targetPoint,
                    speed * Time.deltaTime
                );

                yield return null;
            }

            CableSim.RemoveLastWrapPoint();
        }

        while (Vector3.Distance(handTransform.position, handOrigin.position) > 0.1f)
        {
            handTransform.position = Vector3.MoveTowards(
                handTransform.position,
                handOrigin.position,
                speed * Time.deltaTime
            );

            yield return null;
        }

        if (batterychild == null)
        {
            handgrabbing.SetBool("grabbing", false);

        }

        handTransform.position = handOrigin.position;
        handTransform.rotation = handOrigin.rotation;
        handTransform.parent = originalParent;
        isActive = false;
        // playeranimations.SetTrigger("return");
        canDrag = false;
        // cableRenderer.enabled = false;
        CableSim.InitializeCable();
        CableSim.isActive = false;
        gameObject.transform.localScale = selftransform;


        if (Hand == "Right")
        {
            aimOverride.rightActive = false;
        }
        if (Hand == "Left")
        {
            aimOverride.leftActive = false;
        }
    }
    
    

    void LateUpdate()
    {
        if (CanReturn || !canDrag)
        {
            dragsounds.SetActive(false);
            return;
        }

        bool leftReleased = Hand == "Left" && Input.GetMouseButtonUp(0);
        bool rightReleased = Hand == "Right" && Input.GetMouseButtonUp(1);


        bool isHandActive =
            (Hand == "Left" && Input.GetMouseButton(0)) ||
            (Hand == "Right" && Input.GetMouseButton(1));

        if (!CanReturn && canDrag && isHandActive)
        {
            GameObject grabbed = handTransform.parent?.gameObject;
            if (grabbed == null) return;

            Rigidbody rb = grabbed.GetComponent<Rigidbody>();
            if (rb == null) return;

            Vector3 targetPos = handOrigin.position;
            string objectLayer = LayerMask.LayerToName(grabbed.layer);

            
            dragsounds.SetActive(true);

            Vector3 direction = targetPos - rb.position;
            float distance = direction.magnitude;

            Vector3 dirNormalized = direction.normalized;

            float constantPullForce = pullSpeed * 350; // increase this for stronger pull
            float damping = 8f;

            Vector3 force =
                dirNormalized * constantPullForce
                - rb.linearVelocity * damping;

            rb.AddForce(force, ForceMode.Force);
            //else
            //{
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
            //     if (breakable != null && breakable.SwitchMaterials)
            //     {
            //         if (pressure < 5f)
            //             breakable.renderer.material = breakable.pristine;
            //         else if (pressure <= 9f)
            //             breakable.renderer.material = breakable.damaged;
            //         else
            //             breakable.renderer.material = breakable.broken;
            //     }
            // }
            // }
            // else
            // {
            //     dragsounds.SetActive(false);
            // }
        }
    }

    public void ForceImmediateReturn()
    {
        pressure = 0;
        StopAllCoroutines();
        CableSim.isActive = false;
        isActive = false;
        canDrag = false;
        hitGameObject = null;
        handgrabbing.SetBool("grabbing", false);
        CanReturn = true;
        handTransform.parent = originalParent;
        handTransform.position = handOrigin.position;
        handTransform.rotation = handOrigin.rotation;

        //cableRenderer.enabled = false;
        CableSim.InitializeCable();

        if (Hand == "Right")
        {
            aimOverride.rightActive = false;
        }
        if (Hand == "Left")
        {
            aimOverride.leftActive = false;
        }
    }
}


public class PullBehaviour
{
    
}
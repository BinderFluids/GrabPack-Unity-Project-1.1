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
    ///public Transform transform;
    public Transform handOrigin;
    public float maxRange = 10f;
    public float speed = 20f;

    private Transform originalParent;
    private bool isActive;
    public bool IsActive => isActive;

    public Animator playeranimations;
    public float pullSpeed = 5f;

    public CablePhysics CableSim;
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

    public GameObject Crosshair;
    [SerializeField] private HandInteractable interactable;
    public HandInteractable Interactable => interactable;
    [SerializeField] private Pickupable pickupable;
    
    public bool MouseButtonHeld { get; private set; }
    
    void Start()
    {
        originalParent = transform.parent;
        selftransform = gameObject.transform.localScale;
    }

    // public void HandleInput(int mouseIndex, Ray ray, float maxRange, int handNormal)
    // {
    //     //Fire hand if not holding an item, drop the item otherwise
    //     if (Input.GetMouseButtonDown(mouseIndex))
    //         if (!IsActive && pickupable == null) FireHand(ray, maxRange, handNormal);
    //         else if (!IsActive && pickupable != null) ReleaseItem(); 
    //     
    //     MouseButtonHeld = Input.GetMouseButton(mouseIndex);
    //     
    //     //If holding onto an interactable, release it on mouse up
    //     if (Input.GetMouseButtonUp(mouseIndex))
    //     {
    //         if (interactable != null && isGrabbing)
    //             Return(); 
    //     }
    // }
    

    public void HandleInput(int mouseIndex, Ray ray, float maxRange, int handNormal)
    {
        // detect initial presses
        if (Input.GetMouseButtonDown(mouseIndex))
        {
            if (!IsActive && pickupable == null)
            {
                FireHand(ray, maxRange, handNormal);
            }
            else if (!IsActive && pickupable != null)
            {
                ReleaseItem();
            }
        }

        MouseButtonHeld = Input.GetMouseButton(mouseIndex);

        if (Input.GetMouseButtonUp(mouseIndex) && interactable != null)
        {
            Retract(); 
        }
    }
    
    
    public void FireHand(Ray ray, float range, int handNormal = 1)
    {
        if (isActive) return;
        if (!gameObject.activeSelf) return;

        globalAudio.PlayOneShot(firesfx, 0.7f);

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

        playeranimations.SetTrigger("shoot");
        transform.parent = null;
        isActive = true;

        if (Hand == "Right")
        {
            aimOverride.rightHitPoint = targetPoint;
        }
        if (Hand == "Left")
        {
            aimOverride.leftHitPoint = targetPoint;
        }
        
        
        Vector3 projectedForward = Vector3.ProjectOnPlane(transform.forward, hit.normal * handNormal);
        transform.rotation = Quaternion.LookRotation(projectedForward, hit.normal * handNormal);
        

        // capture the interactable (if any) and hit object, but DO NOT call Grab yet
        HandInteractable hitInteractable = null;
        GameObject hitObj = null;
        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent(out HandInteractable foundInteractable))
                hitInteractable = foundInteractable;

            // store the raw hit GameObject as well for your battery / parenting logic
            hitObj = hit.collider.gameObject;
        }

        Vector3 impactPoint = hit.point;

        // start movement and pass the captured objects along
        StartCoroutine(MoveHand(targetPoint, impactPoint, hitInteractable, hitObj));
    }

    public void GiveItem(Pickupable pickupable)
    {
        this.pickupable ??= pickupable;
    }
    public void ReleaseItem()
    {
        if (pickupable == null) return;
        
        pickupable.Drop(this);
        pickupable = null; 
    }
    
    public void Retract()
    {
        if (!gameObject.activeSelf) return;
        if (lockReturn) return;
        
        StartCoroutine(ReturnHand());
        interactable?.Retract(this);
        interactable = null; 
    }

    public void SetParent(Transform parent, bool worldPositionStays = true)
    {
        transform.SetParent(parent, worldPositionStays); 
    }
    
    private IEnumerator MoveHand(Vector3 target, Vector3 impactPoint, HandInteractable interactableParam, GameObject hitObj)
    {
        Vector3 start = transform.position;
        float distance = Vector3.Distance(start, target);
        float duration = distance / speed;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(start, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = target;

        if (interactableParam != null)
        {
            interactable = interactableParam;
            interactableParam.Grab(this);

            handgrabbing.SetBool("grabbing", interactableParam.GrabType == HandInteractable.GrabTypeEnum.Grip);
        }
        else
            Retract(); 
    }
    


    private IEnumerator ReturnHand()
    {
        transform.parent = null;

        //MOVED LOGIC TO PICKUPABLE class

        handgrabbing.SetBool("grabbing", true);

        //dragsounds.SetActive(false);
        //pressure = 0;
        globalAudio.PlayOneShot(grabsfx, 0.7f);

        br = null;
        // Transform batterychild = transform.Find("Battery");
        // if (batterychild == null)
        // {
        //     batterychild = transform.Find("Gear");
        //
        // }
        // if (batterychild == null)
        // {
        //     holdingbattery = false;
        //     battery = null;
        // }


        isActive = true;
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        float duration = Vector3.Distance(startPosition, handOrigin.position) / speed;
        float elapsedTime = 0f;

        while (CableSim.GetCablePoints().Count > 1)
        {
            List<Vector3> points = CableSim.GetCablePoints();

            if (points.Count <= 2)
                break;

            Vector3 targetPoint = points[points.Count - 2];

            while (Vector3.Distance(transform.position, targetPoint) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPoint,
                    speed * Time.deltaTime
                );

                yield return null;
            }

            CableSim.RemoveLastWrapPoint();
        }

        while (Vector3.Distance(transform.position, handOrigin.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                handOrigin.position,
                speed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = handOrigin.position;
        transform.rotation = handOrigin.rotation;
        transform.parent = originalParent;
        isActive = false;
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
    

    public void ForceImmediateReturn()
    {
        StopAllCoroutines();
        CableSim.isActive = false;
        isActive = false;
        handgrabbing.SetBool("grabbing", false);
        transform.parent = originalParent;
        transform.position = handOrigin.position;
        transform.rotation = handOrigin.rotation;

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
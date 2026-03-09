using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.LowLevelPhysics2D;
using UnityEngine.UI;


public class BaseHandBehaviour : MonoBehaviour
{
    public CableManager cableManager;

    public string Hand = "Left";
    public Transform _transform;
    
    public Transform handOrigin;
    public float maxRange = 10f;
    public float fireSpeed = 20f;
    public float pullSpeed = 5f;

    private Transform originalParent;
    private bool isActive;
    public bool IsActive => isActive;

    public Animator playeranimations;

    public CablePhysics CableSim;
    public Vector3 selftransform;


    public Animator handgrabbing;
    public Barricade br;

    public AudioSource globalAudio;
    public AudioClip firesfx;
    public AudioClip grabsfx;
    public AudioClip dragsfx;
    public GameObject dragsounds;

    [SerializeField] private bool lockRetract = false;

    public RotateArms aimOverride;
    [SerializeField] private HandInteractable interactable;
    public HandInteractable Interactable => interactable;
    [SerializeField] private Pickupable pickupable;
    
    public bool MouseButtonHeld { get; private set; }
    
    void Start()
    {
        originalParent = _transform.parent;
        selftransform = gameObject.transform.localScale;
    }

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
        _transform.SetParent(null, true); 
        isActive = true;

        if (Hand == "Right")
        {
            aimOverride.rightHitPoint = targetPoint;
        }
        if (Hand == "Left")
        {
            aimOverride.leftHitPoint = targetPoint;
        }
        
        
        Vector3 projectedForward = Vector3.ProjectOnPlane(_transform.forward, hit.normal * handNormal);
        _transform.rotation = Quaternion.LookRotation(projectedForward, hit.normal * handNormal);
        

        HandInteractable hitInteractable = null;
        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent(out HandInteractable foundInteractable))
                hitInteractable = foundInteractable;
        }

        Vector3 impactPoint = hit.point;

        // start movement and pass the captured objects along
        StartCoroutine(MoveHand(targetPoint, impactPoint, hitInteractable));
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
        if (lockRetract) return;
        
        StartCoroutine(ReturnHand());
        interactable?.Retract(this);
        interactable = null; 
    }

    public void SetParent(Transform parent, bool worldPositionStays = true)
    {
        _transform.SetParent(parent, worldPositionStays); 
    }

    private float lockRetractTime = .5f;
    private IEnumerator MoveHand(Vector3 target, Vector3 impactPoint, HandInteractable interactableParam)
    {
        Vector3 start = _transform.position;
        float distance = Vector3.Distance(start, target);
        float duration = distance / fireSpeed;
        float elapsedTime = 0f;

        //Move
        while (elapsedTime < duration)
        {
            _transform.position = Vector3.Lerp(start, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _transform.position = target;

        //Interactable logic
        if (interactableParam != null)
        {
            interactable = interactableParam;
            interactableParam.Grab(this);

            handgrabbing.SetBool("grabbing", interactableParam.GrabType == HandInteractable.GrabTypeEnum.Grip);

            lockRetract = true;
            yield return new WaitForSeconds(lockRetractTime);
            lockRetract = false;
        }
        else
            Retract(); 
    }
    


    private IEnumerator ReturnHand()
    {
        _transform.parent = null;

        //MOVED LOGIC TO PICKUPABLE class

        handgrabbing.SetBool("grabbing", true);

        //dragsounds.SetActive(false);
        globalAudio.PlayOneShot(grabsfx, 0.7f);

        br = null;


        isActive = true;
        Vector3 startPosition = _transform.position;
        Quaternion startRotation = _transform.rotation;

        float duration = Vector3.Distance(startPosition, handOrigin.position) / fireSpeed;
        float elapsedTime = 0f;

        while (CableSim.GetCablePoints().Count > 1)
        {
            List<Vector3> points = CableSim.GetCablePoints();

            if (points.Count <= 2)
                break;

            Vector3 targetPoint = points[points.Count - 2];

            while (Vector3.Distance(_transform.position, targetPoint) > 0.05f)
            {
                _transform.position = Vector3.MoveTowards(
                    _transform.position,
                    targetPoint,
                    fireSpeed * Time.deltaTime
                );

                yield return null;
            }

            CableSim.RemoveLastWrapPoint();
        }

        while (Vector3.Distance(_transform.position, handOrigin.position) > 0.1f)
        {
            _transform.position = Vector3.MoveTowards(
                _transform.position,
                handOrigin.position,
                fireSpeed * Time.deltaTime
            );

            yield return null;
        }

        _transform.position = handOrigin.position;
        _transform.rotation = handOrigin.rotation;
        _transform.parent = originalParent;
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
        _transform.parent = originalParent;
        _transform.position = handOrigin.position;
        _transform.rotation = handOrigin.rotation;

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
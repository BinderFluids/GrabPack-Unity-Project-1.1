using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.LowLevelPhysics2D;
using UnityEngine.UI;


public class BaseHandBehaviour : MonoBehaviour
{
    [SerializeField] private HandType handType;
    public HandType HandType => handType;

    private GrabPackManager grabPack;
    public GrabPackManager GrabPack => grabPack;
    [SerializeField] private Transform _transform;
    public Transform Transform => _transform;
    
    [SerializeField] private Transform handOrigin;
    public Transform Origin => handOrigin;
    private Transform originalParent;
    

    [Header("Animation")]
    [SerializeField] private Animator handgrabbing;
    
    [Header("Audio")]
    //public AudioSource globalAudio;
    public AudioClip firesfx;
    public AudioClip grabsfx;
    
    //Events
    public event Action<BaseHandBehaviour> onFire;
    [SerializeField] private UnityEvent onFireUnityEvent;
    
    public event Action<HandInteractable> onGrab;
    [SerializeField] private UnityEvent<GameObject> onGrabUnityEvent;
    
    public event Action<BaseHandBehaviour> onRetract;
    [SerializeField] private UnityEvent onRetractUnityEvent;
    
    
    private bool isActive;
    public bool IsActive => isActive;
    
    private Vector3 targetPoint;
    public Vector3 TargetPoint => targetPoint;
    
    public CablePhysics CableSim;
    private bool lockRetract = false;
    
    //Interaction
    [SerializeField] private HandInteractable interactable;
    public HandInteractable Interactable => interactable;
    [SerializeField] private Pickupable pickupable;
    public bool HasItem => pickupable != null;

    private void Awake()
    {
        if (_transform == null)
            _transform = transform;
    }

    void Start()
    {
        originalParent = _transform.parent;
        SubscribeEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    protected virtual void SubscribeEvents() {}
    protected virtual void UnsubscribeEvents() {}

    public void EnableHand(GrabPackManager grabPack, CablePhysics cableSim, Transform origin)
    {
        this.grabPack = grabPack;
        handOrigin = origin; 
        
        CableSim = cableSim;
        CableSim.endTransform = _transform;
        CableSim.baseHandBehaviour = this; 
        
        gameObject.SetActive(true);
        originalParent = _transform.parent;
    }
    public void DisableHand()
    {
        grabPack = null; 
        
        gameObject.SetActive(false);

        CableSim.endTransform = null;
        CableSim.baseHandBehaviour = null;
        CableSim = null; 
    }
    
    #region INPUT
    public void HandleInput(int mouseIndex, Ray ray, int handNormal)
    {
        // detect initial presses
        if (Input.GetMouseButtonDown(mouseIndex))
        {
            if (!IsActive && pickupable == null)
            {
                FireHand(ray, handNormal);
            }
            else if (!IsActive && pickupable != null)
            {
                ReleaseItem();
            }
            
            if (interactable != null)
                StartPull();
        }

        if (Input.GetMouseButton(mouseIndex))
            if (interactable != null)
                UpdatePull();

        if (Input.GetMouseButtonUp(mouseIndex) && interactable != null)
            Retract(); 
    }
    public void LateHandleInput(int mouseIndex, Ray ray,  int handNormal)
    {
        if (Input.GetMouseButtonDown(mouseIndex))
            interactable?.LateUpdatePull(this); 
    }
#endregion
    
    protected virtual void StartPull() {}
    protected virtual void UpdatePull()
    {
        interactable?.UpdatePull(this); 
    }

    
    public void FireHand(Ray ray,  int handNormal = 1)
    {
        if (isActive) return;
        if (!gameObject.activeSelf) return;

       // globalAudio.PlayOneShot(firesfx, 0.7f);

        CableSim.isActive = true;
        float remaining = grabPack.CableManager.GetRemainingLength();
        float maxRange = Mathf.Min(remaining, grabPack.MaxRange);

        Physics.Raycast(ray, out RaycastHit hit, maxRange);
        targetPoint = hit.collider ? hit.point : ray.origin + ray.direction * maxRange;
        
        grabPack.Animator.SetTrigger("shoot");
        _transform.SetParent(null, true); 
        isActive = true;
        
        Vector3 projectedForward = Vector3.ProjectOnPlane(_transform.forward, hit.normal * handNormal);
        _transform.rotation = Quaternion.LookRotation(projectedForward, hit.normal * handNormal);
        

        HandInteractable hitInteractable = null;
        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent(out HandInteractable foundInteractable))
                hitInteractable = foundInteractable;
        }

        onFire?.Invoke(this);
        onFireUnityEvent?.Invoke();
        
        StartCoroutine(MoveHand(targetPoint, hit.point, hitInteractable));
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
        OnRetract();
        onRetract?.Invoke(this); 
        onRetractUnityEvent?.Invoke();
        interactable?.Retract(this);
        interactable = null; 
    }
    protected virtual void OnRetract() {}

    public void SetParent(Transform parent, bool worldPositionStays = true)
    {
        _transform.SetParent(parent, worldPositionStays); 
    }

    #region MOVEMENT
    private float lockRetractTime = .2f;
    private IEnumerator MoveHand(Vector3 target, Vector3 impactPoint, HandInteractable interactableParam)
    {
        Vector3 start = _transform.position;
        float distance = Vector3.Distance(start, target);
        float duration = distance / grabPack.FireSpeed;
        float elapsedTime = 0f;

        handgrabbing.SetBool("grabbing", false);
        
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
            if (interactableParam.Grab(this))
            {
                handgrabbing.SetBool("grabbing", interactableParam.GrabType == HandInteractable.GrabTypeEnum.Grip);
                interactable = interactableParam;
                onGrab?.Invoke(interactable); 
                onGrabUnityEvent?.Invoke(interactable.gameObject);
                
                lockRetract = true;
                yield return new WaitForSeconds(lockRetractTime);
                lockRetract = false;
            }
            else
                //skips triggering any handinteractable specific events
                StartCoroutine(nameof(ReturnHand));
        }
        else
            Retract(); 
    }
    private IEnumerator ReturnHand()
    {
        _transform.parent = null;

        
        handgrabbing.SetBool("grabbing", pickupable != null);
        
        //globalAudio.PlayOneShot(grabsfx, 0.7f);

        Vector3 startPosition = _transform.position;
        Quaternion startRotation = _transform.rotation;

        float duration = Vector3.Distance(startPosition, handOrigin.position) / grabPack.FireSpeed;
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
                    grabPack.FireSpeed * Time.deltaTime
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
                grabPack.FireSpeed * Time.deltaTime
            );

            yield return null;
        }

        onRetract?.Invoke(this); 
        _transform.position = handOrigin.position;
        _transform.rotation = handOrigin.rotation;
        _transform.parent = originalParent;
        isActive = false;
        CableSim.InitializeCable();
        CableSim.isActive = false;
    }
    #endregion
}
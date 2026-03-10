using System;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class CollisionEventBroadcaster : MonoBehaviour
{
    [SerializeField] private Collider col; 
    
    [Header("Allowed Tags (empty = allow all)")]
    [SerializeField] private string[] _tags;

    // ---------- UnityEvent wrappers ----------
    [Serializable] public class CollisionEvent : UnityEvent<Collision> {}
    [Serializable] public class ColliderEvent : UnityEvent<Collider> {}
    [Serializable] public class Collision2DEvent : UnityEvent<Collision2D> {}
    [Serializable] public class Collider2DEvent : UnityEvent<Collider2D> {}

    // ---------- C# Actions ----------
    public event Action<Collision> OnCollisionEnterAction;
    public event Action<Collision> OnCollisionExitAction;
    public event Action<Collision> OnCollisionStayAction;

    public event Action<Collider> OnTriggerEnterAction;
    public event Action<Collider> OnTriggerExitAction;
    public event Action<Collider> OnTriggerStayAction;

    public event Action<Collision2D> OnCollisionEnter2DAction;
    public event Action<Collision2D> OnCollisionExit2DAction;
    public event Action<Collision2D> OnCollisionStay2DAction;

    public event Action<Collider2D> OnTriggerEnter2DAction;
    public event Action<Collider2D> OnTriggerExit2DAction;
    public event Action<Collider2D> OnTriggerStay2DAction;

    // ---------- UnityEvents ----------
    [Header("3D Collision")]
    public CollisionEvent OnCollisionEnterUnityEvent;
    public CollisionEvent OnCollisionExitUnityEvent;
    public CollisionEvent OnCollisionStayUnityEvent;

    [Header("3D Trigger")]
    public ColliderEvent OnTriggerEnterUnityEvent;
    public ColliderEvent OnTriggerExitUnityEvent;
    public ColliderEvent OnTriggerStayUnityEvent;

    [Header("2D Collision")]
    public Collision2DEvent OnCollisionEnter2DUnityEvent;
    public Collision2DEvent OnCollisionExit2DUnityEvent;
    public Collision2DEvent OnCollisionStay2DUnityEvent;

    [Header("2D Trigger")]
    public Collider2DEvent OnTriggerEnter2DUnityEvent;
    public Collider2DEvent OnTriggerExit2DUnityEvent;
    public Collider2DEvent OnTriggerStay2DUnityEvent;

    private void Start()
    {
        col ??= GetComponent<Collider>();
    }

    public void SetEnabled(bool active)
    {
        col.enabled = active;
    }
    
    // ---------- Tag Check ----------
    private bool IsAllowed(GameObject other)
    {
        if (_tags == null || _tags.Length == 0)
            return true;

        for (int i = 0; i < _tags.Length; i++)
        {
            if (other.CompareTag(_tags[i]))
                return true;
        }

        return false;
    }

    // ---------- 3D ----------
    private void OnCollisionEnter(Collision collision)
    {
        if (!IsAllowed(collision.gameObject)) return;

        OnCollisionEnterAction?.Invoke(collision);
        OnCollisionEnterUnityEvent?.Invoke(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!IsAllowed(collision.gameObject)) return;

        OnCollisionExitAction?.Invoke(collision);
        OnCollisionExitUnityEvent?.Invoke(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!IsAllowed(collision.gameObject)) return;

        OnCollisionStayAction?.Invoke(collision);
        OnCollisionStayUnityEvent?.Invoke(collision);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsAllowed(other.gameObject)) return;

        OnTriggerEnterAction?.Invoke(other);
        OnTriggerEnterUnityEvent?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsAllowed(other.gameObject)) return;

        OnTriggerExitAction?.Invoke(other);
        OnTriggerExitUnityEvent?.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!IsAllowed(other.gameObject)) return;

        OnTriggerStayAction?.Invoke(other);
        OnTriggerStayUnityEvent?.Invoke(other);
    }

    // ---------- 2D ----------
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsAllowed(collision.gameObject)) return;

        OnCollisionEnter2DAction?.Invoke(collision);
        OnCollisionEnter2DUnityEvent?.Invoke(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!IsAllowed(collision.gameObject)) return;

        OnCollisionExit2DAction?.Invoke(collision);
        OnCollisionExit2DUnityEvent?.Invoke(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!IsAllowed(collision.gameObject)) return;

        OnCollisionStay2DAction?.Invoke(collision);
        OnCollisionStay2DUnityEvent?.Invoke(collision);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsAllowed(other.gameObject)) return;

        OnTriggerEnter2DAction?.Invoke(other);
        OnTriggerEnter2DUnityEvent?.Invoke(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsAllowed(other.gameObject)) return;

        OnTriggerExit2DAction?.Invoke(other);
        OnTriggerExit2DUnityEvent?.Invoke(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!IsAllowed(other.gameObject)) return;

        OnTriggerStay2DAction?.Invoke(other);
        OnTriggerStay2DUnityEvent?.Invoke(other);
    }
}
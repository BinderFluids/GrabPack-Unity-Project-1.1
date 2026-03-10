using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorExtensionEvents : MonoBehaviour
{
    [SerializeField] Animator _animator;
    // Start is called before the first frame update
    void Awake()
    {
        _animator ??= GetComponent<Animator>();
    }

    public void SetBoolTrue(string name) => _animator.SetBool(name, true);

    public void SetBoolFalse(string name) => _animator.SetBool(name, false);

    public void ToggleBool(string name) => _animator.SetBool(name, !_animator.GetBool(name));


}
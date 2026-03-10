using UnityEngine;

public class RotateArm : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform bone;

    [Header("State")]
    [SerializeField] private Vector3 hitPoint;
    [SerializeField] private bool active = false;

    [Tooltip("Use -1 if the bone's forward is reversed relative to the target direction")]
    [SerializeField] private float directionMultiplier = 1f;

    [Header("Limits")]
    [SerializeField] private float maxPitch = 180f;
    [SerializeField] private float maxYaw = 180f;

    private Quaternion baseLocalRotation;
    private bool initialized = false;

    void LateUpdate()
    {
        if (!initialized)
        {
            if (bone != null)
                baseLocalRotation = bone.localRotation;

            initialized = true;
        }

        Rotate();
    }

    public void SetActive(bool active, Vector3 hitPoint)
    {
        this.active = active;
        this.hitPoint = hitPoint;
    }
    
    void Rotate()
    {
        if (!active) return;
        if (bone == null) return;
        if (hitPoint == Vector3.zero) return;

        Vector3 worldDir =
            (hitPoint - bone.position).normalized * directionMultiplier;

        Quaternion fromTo =
            Quaternion.FromToRotation(bone.forward, worldDir);

        Quaternion targetWorldRot =
            fromTo * bone.rotation;

        Quaternion targetLocalRot =
            Quaternion.Inverse(bone.parent.rotation) * targetWorldRot;

        Quaternion delta =
            Quaternion.Inverse(baseLocalRotation) * targetLocalRot;

        Vector3 deltaEuler = delta.eulerAngles;

        deltaEuler.x = NormalizeAngle(deltaEuler.x);
        deltaEuler.y = NormalizeAngle(deltaEuler.y);
        deltaEuler.z = 0f; // lock roll

        deltaEuler.x = Mathf.Clamp(deltaEuler.x, -maxPitch, maxPitch);
        deltaEuler.y = Mathf.Clamp(deltaEuler.y, -maxYaw, maxYaw);

        bone.localRotation =
            baseLocalRotation * Quaternion.Euler(deltaEuler);

        Debug.DrawLine(bone.position, hitPoint, Color.red);
    }

    float NormalizeAngle(float angle)
    {
        if (angle > 180f) angle -= 360f;
        return angle;
    }
}

using UnityEngine;

public class BulkMaterialSetter : MonoBehaviour
{
    [SerializeField] private Renderer[] renderers;

    public void SetMaterial(Material material)
    {
        foreach (var renderer in renderers)
            renderer.material = material;
    }
}
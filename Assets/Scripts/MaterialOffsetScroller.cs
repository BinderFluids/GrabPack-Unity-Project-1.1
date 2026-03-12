using System;
using UnityEngine;

public class MaterialOffsetScroller : MonoBehaviour
{
    [SerializeField] private Vector2 scrollVector; 
    [SerializeField] private Renderer targetRenderer;
    private Material material;
    private Vector2 currentOffset;
    private bool scrollEnabled = true;
    
    public void EnableScroll(bool value) => scrollEnabled = value;
    public void SetScrollXSpeed(float value) => scrollVector.x = value;
    public void SetScrollYSpeed(float value) => scrollVector.y = value;
    public void SetScrollSpeed(float x, float y) => scrollVector = new Vector2(x, y);
    
    private void Awake()
    {
        targetRenderer ??= GetComponent<Renderer>();
        material = targetRenderer.material;
        currentOffset = material.mainTextureOffset;
    }

    private void Update()
    {
        if (!scrollEnabled) return; 
        
        if (material != null)
        {
            currentOffset += scrollVector * Time.deltaTime;
            material.mainTextureOffset = currentOffset;
        }
    }
}

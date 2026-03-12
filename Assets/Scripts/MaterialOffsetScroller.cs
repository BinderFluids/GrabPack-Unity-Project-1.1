using System;
using UnityEngine;

public class MaterialOffsetScroller : MonoBehaviour
{
    [SerializeField] private Vector2 scrollVector; 
    [SerializeField] private Renderer targetRenderer;
    private bool scrollEnabled = true;
    
    public void EnableScroll(bool value) => scrollEnabled = value;
    public void SetScrollXSpeed(float value) => scrollVector.x = value;
    public void SetScrollYSpeed(float value) => scrollVector.y = value;
    public void SetScrollSpeed(float x, float y) => scrollVector = new Vector2(x, y);
    
    private void Awake()
    {
        targetRenderer ??= GetComponent<Renderer>();
    }

    private void Update()
    {
        if (!scrollEnabled) return; 
        
        Material material = targetRenderer.material;
        if (material != null) material.mainTextureOffset += scrollVector * Time.deltaTime;
    }
}

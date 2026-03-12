using System;
using UnityEngine;

public class MaterialColorSetter : MonoBehaviour
{
    private const string EmissionColor = "_EmissionColor";
    
    [SerializeField] private Color color;
    
    [ColorUsage(true, true)]
    [SerializeField] private Color emissionColor; 
    
    [SerializeField] private Renderer[] targetRenderers; 
    
    private void Start()
    {
        SetColor(color);
        SetEmissionColor(emissionColor);
    }

    public void SetColor(Color color) => this.color = color;
    public void SetEmissionColor(Color color) => emissionColor = color;

    private void Update()
    {
        ApplyColor();
    }

    public void ApplyColor()
    {
        foreach (Renderer renderer in targetRenderers)
        {
            renderer.material.SetColor("_Color", color);
            renderer.material.SetColor(EmissionColor, emissionColor);
        }
    }
}

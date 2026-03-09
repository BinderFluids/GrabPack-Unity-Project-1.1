
using System;
using Unity.VisualScripting;
using UnityEngine;

public class HandsController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float maxRange;

    [Header("Hands")] 
    [SerializeField] private ContainerData[] handContainersData;
    private void Start()
    {
        cam ??= Camera.main;
    }

    private void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        foreach (var data in handContainersData)
        {
            BaseHandBehaviour hand = data.hand;
            hand.HandleInput(data.mouseIndex, CastRay(), maxRange, data.handNormal);
        }
    }
    
    Ray CastRay()
    {
        Vector3 center = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
        return cam.ScreenPointToRay(center);
    }

    [Serializable]
    public struct ContainerData
    {
        public BaseHandBehaviour hand;
        public int handNormal; 
        public int mouseIndex;   
    }
}
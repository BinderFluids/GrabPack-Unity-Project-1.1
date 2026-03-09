
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

            hand.HandleInput(data.mouseIndex);
            
            if (Input.GetMouseButtonDown(data.mouseIndex))
                hand.Fire(CastRay(), maxRange);
            if (Input.GetMouseButtonUp(data.mouseIndex))
                hand.Return();
        }
    }
    
    Ray CastRay()
    {
        return cam.ScreenPointToRay(Input.mousePosition);
        // if (Physics.Raycast(ray, out RaycastHit hit, maxRange))
        //     return hit.point; 
        //
        // return ray.origin + ray.direction * maxRange;
    }

    [Serializable]
    public struct ContainerData
    {
        public BaseHandBehaviour hand;
        public int mouseIndex;   
    }
}
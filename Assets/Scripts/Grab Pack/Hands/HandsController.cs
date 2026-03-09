
using System;
using Unity.VisualScripting;
using UnityEngine;

public class HandsController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float maxRange;

    [Header("Hands")] 
    [SerializeField] private ContainerData[] handContainersData;

    [SerializeField] private Vector3 mousePos; 

    private void Start()
    {
        cam ??= Camera.main;
    }

    private void Update()
    {
        mousePos = Input.mousePosition;
        HandleInput();
    }

    void HandleInput()
    {
        foreach (var data in handContainersData)
        {
            BaseHandBehaviour hand = data.hand;

            hand.HandleInput(data.mouseIndex);

            if (Input.GetMouseButtonDown(data.mouseIndex))
                if (!hand.IsActive) hand.Fire(CastRay(), maxRange);
                else hand.Return();
        }
    }
    
    Ray CastRay()
    {
        Vector3 center = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
        return cam.ScreenPointToRay(center);
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
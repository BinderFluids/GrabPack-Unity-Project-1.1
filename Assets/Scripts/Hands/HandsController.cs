
using System;
using Unity.VisualScripting;
using UnityEngine;

public class HandsController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float maxRange;

    [Header("Hands")] 
    [SerializeField] private HandConfig[] handContainersData;
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
        foreach (HandConfig config in handContainersData)
        {
            BaseHandBehaviour hand = config.hand;
            RotateArm rotateArm = config.rotateArm;
            
            hand.HandleInput(config.mouseIndex, CastRay(), maxRange, config.handNormal);

            if (hand.IsActive)
                rotateArm.SetActive(true, hand.TargetPoint);
            else
                rotateArm.SetActive(false, Vector3.zero);
        }
    }
    
    Ray CastRay()
    {
        Vector3 center = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
        return cam.ScreenPointToRay(center);
    }

    [Serializable]
    public struct HandConfig
    {
        public BaseHandBehaviour hand;
        public RotateArm rotateArm;
        public int handNormal; 
        public int mouseIndex;   
    }
}
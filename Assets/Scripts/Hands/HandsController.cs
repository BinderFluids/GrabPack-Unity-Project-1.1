
using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class HandsController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float maxRange;

    [Header("Hands")] 
    [SerializeField] private HandsInventory inventory;
    [SerializeField] private HandConfig[] handConfigs;
    private void Start()
    {
        cam ??= Camera.main;
        EnableHand(0, handConfigs[0].hand);
        EnableHand(1, handConfigs[1].hand);
    }

    void SubscribeToHandEvents(BaseHandBehaviour hand)
    {
        hand.onFire += OnFire;
        hand.onRetract += OnRetract;
    }
    void UnsubscribeFromHandEvents(BaseHandBehaviour hand)
    {
        hand.onFire -= OnFire;
        hand.onRetract -= OnRetract;
    }

    private void Update()
    {
        HandleHandInput();
        HandleInventoryInput();
    }

    private void LateUpdate()
    {
        LateHandleInput(); 
    }

    void HandleInventoryInput()
    {
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i) ||
                Input.GetKeyDown(KeyCode.Keypad0 + i))
            {
                BaseHandBehaviour newHand = inventory.GetHand(i);
                if (newHand == null || newHand == handConfigs[1].hand) return;
                
                DisableHand(1);
                EnableHand(1, newHand); 
            }
        }
    }
    

    void HandleHandInput()
    {
        foreach (HandConfig config in handConfigs)
        {
            config.hand.HandleInput(config.mouseIndex, CastRay(), maxRange, config.handNormal);
        }
    }
    void LateHandleInput()
    {
        foreach (HandConfig config in handConfigs)
        {
            BaseHandBehaviour hand = config.hand;
            hand?.LateHandleInput(config.mouseIndex, CastRay(), maxRange, config.handNormal);
        }
    }

    void OnFire(BaseHandBehaviour hand)
    {
        RotateArm rotateArm = handConfigs.First(c => c.hand == hand).rotateArm;
        rotateArm.SetActive(true, hand.TargetPoint);
    }
    void OnRetract(BaseHandBehaviour hand)
    {
        RotateArm rotateArm = handConfigs.First(c => c.hand == hand).rotateArm;
        rotateArm.SetActive(false, Vector3.zero);
    }

    public void DisableHand(int index)
    {
        HandConfig config = handConfigs[index];
        UnsubscribeFromHandEvents(config.hand);
        config.hand.DisableHand();
        config.hand = null; 
    }
    public void EnableHand(int index, BaseHandBehaviour hand)
    {
        HandConfig config = handConfigs[index];
        SubscribeToHandEvents(hand); 
        config.hand = hand;
        config.hand.EnableHand(config.physics);
    }
    
    Ray CastRay()
    {
        Vector3 center = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
        return cam.ScreenPointToRay(center);
    }

    private void OnDestroy()
    {
        foreach (HandConfig config in handConfigs)
        {
            config.hand.onFire -= OnFire;
            config.hand.onRetract -= OnRetract;
        }
    }


    [Serializable]
    public class HandConfig
    {
        public BaseHandBehaviour hand;
        public Transform anchor;
        public CablePhysics physics; 
        public RotateArm rotateArm;
        public int handNormal; 
        public int mouseIndex;   
    }
}
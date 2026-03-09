using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public float interactionRange = 2f;
    public LayerMask buttonLayer; 
    public LayerMask CodeCheckerLayer;
    public LayerMask grabpackinteractable;
    public LayerMask handpickups;

    public Animator grabpack;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastInteraction();
        }
    }

    void RaycastInteraction()
    {
        Vector3 center = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
        Ray ray = Camera.main.ScreenPointToRay(center);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange, buttonLayer))
        {
            keyButton KeyButton = hit.transform.gameObject.GetComponent<keyButton>();
            KeyButton.pushed();
        }
        if (Physics.Raycast(ray, out hit, interactionRange, CodeCheckerLayer))
        {
            CheckCode checkcode = hit.transform.gameObject.GetComponent<CheckCode>();
            checkcode.Check();
        }
        if (Physics.Raycast(ray, out hit, interactionRange, grabpackinteractable))
        {
            PickupGrabpack pickup = hit.transform.gameObject.GetComponent<PickupGrabpack>();
            pickup.Pickup();
            grabpack.SetTrigger("pickup");
        }
        if (Physics.Raycast(ray, out hit, interactionRange, handpickups))
        {
            HandPickUp pickup = hit.transform.gameObject.GetComponent<HandPickUp>();
            pickup.PickupHand();
        }
    }
}
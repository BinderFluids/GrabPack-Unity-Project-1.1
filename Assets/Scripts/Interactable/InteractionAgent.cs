using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionAgent : MonoBehaviour
{

    [SerializeField] private Camera cam;
    [SerializeField] private float interactionRange = 2f;

    private void Start()
    {
        cam ??= Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            Interact();
    }

    void Interact()
    {
        Vector3 center = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
        Ray ray = cam.ScreenPointToRay(center);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, interactionRange))
            if (hit.collider.TryGetComponent(out IInteractable interactable))
                interactable.Interact();

    }
}
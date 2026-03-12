using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpForce = 10f;

    public Material poweredmatieral;
    public Material offMaterial;
    public MeshRenderer renderer;

    public GameObject light;

    public RigidboyPlayerController player;
    public AudioClip boostsfx;

    public GameObject rocketHand;

    public float maxBoostDistance = 10f;
    public float minBoostMultiplier = 0.2f;

    public float cooldownTime = 2f;
    private float cooldownTimer = 0f;

    public void LaunchPlayer(BaseHandBehaviour hand)
    {
        Rigidbody rb = hand.GrabPack.PlayerRigidbody;
        
        float baseForce = player.isGrounded ? jumpForce : jumpForce / 2f;

        float distance = Vector3.Distance(rb.transform.position, transform.position);

        float distanceMultiplier = Mathf.Clamp01(1 - (distance / maxBoostDistance));
        distanceMultiplier = Mathf.Lerp(minBoostMultiplier, 1f, distanceMultiplier);

        float finalForce = baseForce * distanceMultiplier;

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(transform.up * finalForce, ForceMode.Impulse);

        cooldownTimer = cooldownTime;

        GlobalAudio.Instance.PlayOneShot(boostsfx, 1.0f);
    }
}
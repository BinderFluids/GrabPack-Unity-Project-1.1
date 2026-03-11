using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalSource : MonoBehaviour
{
    public bool powering = false;

    public MeshRenderer cable;
    public MeshRenderer cable2;

    public Material glowingcable;

    public Material normalcable;

    public bool changed = false;

    public AudioSource GlobalAudio;
    public AudioClip grabcoilsfx;


    private bool wasPoweringLastFrame = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Transform child1 = transform.Find("Hand_Rocket");
        Transform child2 = transform.Find("Hand_Red");
        Transform child3 = transform.Find("Hand_Blue");
        Transform child4 = transform.Find("Hand_Pressure");
        Transform child5 = transform.Find("Hand_Conductive");

        if (child1 != null || child2 != null || child3 != null || child4 != null || child5 != null)
        {

            powering = true;

            cable.material = glowingcable;
            cable2.material = glowingcable;

            changed = false;

        }
        if (child1 == null && child2 == null && child3 == null && child4 == null && child5 == null)
        {

            powering = false;

            if (!changed)
            {
                cable.material = normalcable;
                cable2.material = normalcable;

                changed = true;
            }

        }
        if (powering && !wasPoweringLastFrame)
        {
            GlobalAudio.PlayOneShot(grabcoilsfx, 1.0f);
        }

        wasPoweringLastFrame = powering;

    }
}

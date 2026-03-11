using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uncrouch : MonoBehaviour
{
    public RigidboyPlayerController player;


    
    public void uncrouchplayer()
    {
        player.IsCrouched = false;


    }
    public void crouchplayer()
    {
        player.IsCrouched = true;


    }
}

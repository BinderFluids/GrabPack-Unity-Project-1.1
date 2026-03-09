using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCard : MonoBehaviour
{
    private MeshRenderer renderer;
    private BoxCollider collider;

    public BaseHandBehaviour redhand;
    public BaseHandBehaviour purpleHandBehaviour;
    public BaseHandBehaviour blueHandBehaviour;
    public BaseHandBehaviour pressureHandBehaviour;
    public BaseHandBehaviour conductiveHandBehaviour;


    public Transform child1;
    public Transform child2;
    public Transform child3;
    public Transform child4;
    public Transform child5;

    public bool PICKED = false;
    // Start is called before the first frame update
    void Start()
    {
        renderer = gameObject.GetComponent<MeshRenderer>();
        collider = gameObject.GetComponent<BoxCollider>();

    }

    // Update is called once per frame
    void Update()
    {
        child1 = transform.Find("Hand_Rocket");
        child2 = transform.Find("Hand_Red");
        child3 = transform.Find("Hand_Blue");
        child4 = transform.Find("Hand_Pressure");
        child5 = transform.Find("Hand_Conductive");

        if (child1 != null || child2 != null || child3 != null || child4 != null || child5 != null)
        {

            if (child1 != null && Input.GetMouseButtonDown(1))
            {
                PickUp();
                purpleHandBehaviour.Return();
            }

            if (child2 != null && Input.GetMouseButtonDown(1))
            {
                PickUp();
                redhand.Return();


            }
            if (child3 != null && Input.GetMouseButtonDown(0))
            {


                PickUp();
                blueHandBehaviour.Return();

            }

            if (child4 != null && Input.GetMouseButtonDown(1))
            {
                PickUp();
                pressureHandBehaviour.Return();


            }
            if (child5 != null && Input.GetMouseButtonDown(1))
            {
                PickUp();
                conductiveHandBehaviour.Return();


            }




        }
    }

    public void PickUp()
    {
        renderer.enabled = false;
        collider.enabled = false;
        PICKED = true;
        if (child1 != null)
        {
            purpleHandBehaviour.Return();
        }

        if (child2 != null)
        {
            redhand.Return();


        }
        if (child3 != null)
        {


            blueHandBehaviour.Return();

        }
        if (child4 != null)
        {
            pressureHandBehaviour.Return();
        }
        if (child5 != null)
        {
            conductiveHandBehaviour.Return();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{


    public int partAmount = 0;
    private int partMax = 3;

    public GameObject door1;
    
    public Animator door1Ani;
    




    void Start()
    {
        PlayerNew.Instance.OnDoorPartDelivered += Instance_OnDoorPartDelivered;
        door1Ani = door1.GetComponent<Animator>();
        
    }

    private void Instance_OnDoorPartDelivered(object sender, System.EventArgs e)
    {
        partAmount++;
    }

    
    void Update()
    {
        if (partAmount >= 1)
        {
            door1Ani.SetBool("IsOpen", true);
            Collider console1Collider = gameObject.GetComponent<Collider>();
            console1Collider.enabled = false;
            //Destroy(door1);
            //Debug.Log("FirstDoorOpen");
        }
        
    }
}

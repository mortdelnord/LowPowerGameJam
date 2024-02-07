using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DoorScript1 : MonoBehaviour
{

    public static DoorScript1 Instance { get; private set; }

    public event EventHandler OnLastDoorOpen;

    public int partAmount = 0;
    private int partMax = 3;

    
    public GameObject door2;
    
    public Animator door2Ani;

    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        PlayerNew.Instance.OnDoorPartDelivered += Instance_OnDoorPartDelivered;
        
        door2Ani = door2.GetComponent<Animator>();
    }

    private void Instance_OnDoorPartDelivered(object sender, System.EventArgs e)
    {
        partAmount++;
    }

    
    void Update()
    {
        
        
        if (partAmount >= partMax)
        {

            door2Ani.SetBool("IsOpen", true);
            Collider console1Collider = gameObject.GetComponent<Collider>();
            console1Collider.enabled = false;
            OnLastDoorOpen?.Invoke(this, EventArgs.Empty);
            //Destroy(door2);
            //Debug.Log("Last Door");

        }
    }
}

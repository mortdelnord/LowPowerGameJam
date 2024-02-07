using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Player : MonoBehaviour
{


    public static Player Instance { get; private set; }

    public event EventHandler OnDoorPartDelivered;



    public GameObject mouseWheel;
    public GameObject playerModel;
    public Animator playerAnimator;
    public GameObject mouseModel;
    public Animator mouseAnimator;


    public GameObject holdPoint;
    public Transform dropPoint;
    
    public GameObject itemCurrentlyHolding;


    public NavMeshAgent playerNavAgent;
    public Collider iteractCollider;




    public bool isDisabled = false;
    public bool isHolding;
    public bool hasMouse = false;
    public bool hasBattery = false;
    public bool hasPart = false;


    public float energyAmount;
    public float energyMax = 200f;
    public float energyMid = 100f;
    public float energyMin = 0f;


    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        playerNavAgent = GetComponent<NavMeshAgent>();
        energyAmount = energyMid;
        playerAnimator = playerModel.GetComponent<Animator>();
        mouseAnimator = mouseModel.GetComponent<Animator>();

    }

    public void Update()
    {
        
        energyAmount = Mathf.Clamp(energyAmount, energyMin, energyMax);

        if (hasMouse)
        {
            energyAmount = energyMid;

        }
        if (hasBattery)
        {
            energyAmount -= Time.deltaTime * 0.5f;
            
        } else
        {
            energyAmount -= Time.deltaTime;
        }

        if (energyAmount <= 0)
        {
            isDisabled = true;
        }




        if (!isDisabled)
        {

            if (playerNavAgent.velocity != Vector3.zero)
                {
                    playerAnimator.SetBool("IsMoving", true);
                }
                else
                {
                    playerAnimator.SetBool("IsMoving", false);

                }
            if (Input.GetMouseButtonDown(0))
            {
                Ray clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(clickRay, out RaycastHit hit, 1000, playerNavAgent.areaMask) && hit.transform != null)
                {
                    playerNavAgent.SetDestination(hit.point);
                    
                    

                    if (hit.transform.CompareTag("Item") && hit.transform != null || hit.transform.CompareTag("Mouse") && hit.transform != null || hit.transform.CompareTag("Battery") && hit.transform != null || hit.transform.CompareTag("Door") && hit.transform != null)
                    {
                        //if (hit.transform.CompareTag("Mouse"))
                        //{
                        //    OnMouseClicked?.Invoke(this, EventArgs.Empty);
                        //}
                        iteractCollider.enabled = true;
                    }else
                    {
                        iteractCollider.enabled = false;
                    }

                    
                }


            }
        }


        

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Untagged"))
        {
            return;
        }
        if (other.CompareTag("Door") && hasPart)
        {
            isHolding = false;
            Destroy(itemCurrentlyHolding);
            hasPart = false;
            OnDoorPartDelivered?.Invoke(this, EventArgs.Empty);

        }
        else
        {

            if (isHolding)
            {
                Drop();
            }
            itemCurrentlyHolding = other.gameObject;
            PickUp();
            iteractCollider.enabled = false;
        }
    }

    private void PickUp()
    {
        playerAnimator.SetBool("IsMoving", false);
        playerAnimator.SetBool("isPutting", false);
        playerAnimator.SetBool("isTaking", true);
        Invoke("TurnOffAnimations", 2f);
        if (itemCurrentlyHolding.CompareTag("Mouse"))
        {
            mouseWheel.SetActive(true);
            //Animator animator = itemCurrentlyHolding.GetComponent<Animator>();
            mouseAnimator.SetBool("IsWheel", true);
            NavMeshAgent navMeshAgent = itemCurrentlyHolding.GetComponent<NavMeshAgent>();
            navMeshAgent.enabled = false;
            hasMouse = true;
            Mouse mouseScript = itemCurrentlyHolding.GetComponent<Mouse>();
            mouseScript.enabled = false;
        }
        if (itemCurrentlyHolding.CompareTag("Battery"))
        {
            hasBattery = true;
        }
        if (itemCurrentlyHolding.CompareTag("Item"))
        {
            hasPart = true;
        }
        Rigidbody itemRb = itemCurrentlyHolding.GetComponent<Rigidbody>();
        Collider itemCollider = itemCurrentlyHolding.GetComponent<Collider>();
        
        itemRb.isKinematic = true;
        itemCollider.enabled = false;
        

        itemCurrentlyHolding.transform.SetParent(holdPoint.transform);
        itemCurrentlyHolding.transform.localPosition = Vector3.zero;
        //itemCurrentlyHolding.transform.localRotation = Quaternion.Euler(Vector3.zero);
        





        isHolding = true;
    }

    private void Drop()
    {

        playerAnimator.SetBool("IsMoving", false);
        playerAnimator.SetBool("isTaking", false);
        playerAnimator.SetBool("isPutting", true);
        Invoke("TurnOffAnimations", 2f);

        if (itemCurrentlyHolding.CompareTag("Mouse"))
        {
            mouseWheel.SetActive(false);
            //Animator animator = itemCurrentlyHolding.GetComponent<Animator>();
            //animator.SetBool("IsWheel", false);
            mouseAnimator.SetBool("IsWheel", true);

            NavMeshAgent navMeshAgent = itemCurrentlyHolding.GetComponent<NavMeshAgent>();
            navMeshAgent.enabled = true;
            hasMouse = false;
            Mouse mouseScript = itemCurrentlyHolding.GetComponent<Mouse>();
            mouseScript.enabled = true;
        }
        if (itemCurrentlyHolding.CompareTag("Battery"))
        {
            hasBattery = false;
            
        }
        if (itemCurrentlyHolding.CompareTag("Item"))
        {
            hasPart = false;
        }

        itemCurrentlyHolding.transform.parent = null;
        itemCurrentlyHolding.transform.position = dropPoint.transform.position;
        

        Rigidbody itemRb = itemCurrentlyHolding.GetComponent<Rigidbody>();
        Collider itemCollider = itemCurrentlyHolding.GetComponent<Collider>();
        itemRb.isKinematic = true;
        itemCollider.enabled = true;

        


        isHolding = false;
    }


    private void TurnOffAnimations()
    {
        playerAnimator.SetBool("IsMoving", false);
        playerAnimator.SetBool("isTaking", false);
        playerAnimator.SetBool("isPutting", false);
    }
}

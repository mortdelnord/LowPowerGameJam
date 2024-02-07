using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerNew : MonoBehaviour
{

    public static PlayerNew Instance { get; private set;}

    public event EventHandler OnDoorPartDelivered;
    
    [Header ("References")]
    public GameObject mouseWheel;
    public GameObject playerModel;
    public GameObject mouseModel;
    public GameObject holdPoint;
    public GameObject itemCurrentlyHolding;

    public Transform dropPoint;

    public Collider interactCollider;
    public NavMeshAgent playerNavAgent;
    public Animator playerAnimator;
    public Animator mouseAnimator;

    [Header ("Bools")]
    public bool isDisabled = false;
    public bool isHolding;
    public bool hasMouse = false;
    public bool hasBattery = false;

    public bool hasPart = false;

    [Header ("floats")]

    public float energyAmount;
    public float energyMax = 200f;
    public float energyMid = 100f;
    public float energyMin = 0f;

    public enum State
    {
        Walking,
        PickUp,
        PutDown,
        Idle
    }

    private State state;


    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        playerNavAgent = GetComponent<NavMeshAgent>();
        energyAmount = energyMid;
        playerAnimator = playerModel.GetComponent<Animator>();
        mouseAnimator = mouseModel.GetComponent<Animator>();
        state = State.Idle;
    }


    public void Update()
    {
        EnergyManager();


        if (Input.GetMouseButtonDown(0))
        {
            Ray clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(clickRay, out RaycastHit hit, 1000, playerNavAgent.areaMask) && hit.transform != null)
            {
                NavMeshPath path = new NavMeshPath();
                playerNavAgent.CalculatePath(hit.point, path);
                playerNavAgent.SetPath(path);
                //playerNavAgent.SetDestination(hit.point);

                if (playerNavAgent.remainingDistance >= 1)
                {
                    state = State.Walking;
                }else
                {
                    state = State.Idle;
                }

                if (hit.transform.CompareTag("Item") && hit.transform != null || hit.transform.CompareTag("Mouse") && hit.transform != null || hit.transform.CompareTag("Battery") && hit.transform != null || hit.transform.CompareTag("Door") && hit.transform != null)
                {
                    interactCollider.enabled = true;
                }else
                {
                    interactCollider.enabled = false;
                }

            }
        }

        switch(state)
        {
            case State.Idle:
                playerAnimator.SetBool("IsMoving", false);
                break;
            case State.Walking:
                playerAnimator.SetBool("IsMoving", true);
                break;
            case State.PickUp:
                playerAnimator.SetTrigger("PutIn");
                break;
            case State.PutDown:
                playerAnimator.SetTrigger("TakeOut");
                break;
        }



    }

    public void EnergyManager()
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
    }


    private void PickUp()
    {
        state = State.PickUp;

        if (itemCurrentlyHolding.CompareTag("Mouse"))
        {
            mouseWheel.SetActive(true);

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

        isHolding = true;
    }

    private void Drop()
    {
        state = State.PutDown;

        if (itemCurrentlyHolding.CompareTag("Mouse"))
        {
            mouseWheel.SetActive(false);

            mouseAnimator.SetBool("IsWheel", false);

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
        } else
        {
            if (isHolding)
            {
                Drop();
            }
            itemCurrentlyHolding = other.gameObject;
            PickUp();
            interactCollider.enabled = false;
        }


    }

}

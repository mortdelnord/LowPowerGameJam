using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Mouse : MonoBehaviour
{

    public float wanderRadius;
    public float wanderTimer;

    public bool isClicked = false;

    private Transform target;
    private NavMeshAgent mouseNav;
    private float timer;
    private bool canMove;

    //public void Start()
    //{
    //    Player.Instance.OnMouseClicked += Instance_OnMouseClicked;
    //}

    //private void Instance_OnMouseClicked(object sender, System.EventArgs e)
    //{
    //    isClicked = true;
    //}

    

    private void OnEnable()
    {
        mouseNav = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        isClicked = false;
    }

    public void Update()
    {

        if (isClicked)
        {
            wanderRadius = 0f;
        }else
        {
            wanderRadius = 20f;
        }

        timer += Time.deltaTime;



        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            mouseNav.SetDestination(newPos);
            timer = 0;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }



    public void StopMoving()
    {
        isClicked = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AILocomotion : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform[] waypoints;
    int waypointIndex;
    Vector3 target;
    

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        UpdateDestination();

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Vector3.Distance(this.transform.position, wayPoint1.position));
        //Debug.Log((Vector3.Distance(transform.position, target)));

        if (Vector3.Distance(transform.position,target) < 25)
        {
            IterateWayPointIndex();
            UpdateDestination();
        }
        
        //Debug.Log(target);
    }

    void UpdateDestination()
    {
        target = waypoints[waypointIndex].position;
        agent.SetDestination(target);

    }

    void IterateWayPointIndex()
    {
        waypointIndex++;
        if (waypointIndex == waypoints.Length)
        {
            waypointIndex = 0;
        }
    }

}

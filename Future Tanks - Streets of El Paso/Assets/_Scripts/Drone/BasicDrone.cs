using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicDrone : MonoBehaviour 
{
	public Rigidbody m_Rigidbody;
	public GameObject[] waypoints;
	[SerializeField]float m_Speed = 1f;
	int currentPoint = 0;
	bool bMoving;
	Vector3 CurrentVector;
	NavMeshAgent agent;   

	void Start()
	{
		//initialize color, etc.
		agent = GetComponent<NavMeshAgent>();
		if(waypoints[currentPoint]!=null)
		{
			CurrentVector = waypoints[currentPoint].transform.position;
			bMoving = true;
			agent.SetDestination(CurrentVector);
			agent.speed = 8;
		}
	
	}

	void Update()
	{
		if(bMoving && currentPoint<waypoints.Length-1)
		{
			if(Vector3.Distance(transform.position, CurrentVector)<=2)
			{
				currentPoint++;
				if(waypoints[currentPoint]!=null)
				{
					CurrentVector = waypoints[currentPoint].transform.position;
					agent.SetDestination(CurrentVector);
				}else {
				bMoving = false;
				}
			}
		}
	}
}

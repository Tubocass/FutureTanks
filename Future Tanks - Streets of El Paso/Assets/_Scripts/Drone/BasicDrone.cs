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
	Vector3 CurrentVector;
	NavMeshAgent agent;   

	void Start()
	{
		//initialize color, etc.
		agent = GetComponent<NavMeshAgent>();
		CurrentVector = waypoints[currentPoint].transform.position;
		agent.SetDestination(CurrentVector);
		agent.speed = 8;
	}

	void Update()
	{
		if(currentPoint<waypoints.Length-1)
		{
			if(Vector3.Distance(transform.position, CurrentVector)<=2)
			{
				currentPoint++;
				CurrentVector = waypoints[currentPoint].transform.position;
				agent.SetDestination(CurrentVector);
			}
		}
	}
}

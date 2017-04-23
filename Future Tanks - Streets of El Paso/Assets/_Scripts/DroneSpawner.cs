using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSpawner : MonoBehaviour 
{
	public GameObject DroneSpawn;
	public Transform spawnPoint;

	public GameObject[] wpsOuterLeft, wpsOuterRight, wpsInnerRight, wpsInnerLeft;
	int num;

	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			Quaternion q = Quaternion.LookRotation(spawnPoint.position - transform.position);
			GameObject spawn = (GameObject)Instantiate(DroneSpawn,spawnPoint.position,q);
			num = (int)Mathf.Repeat(num, 4);
			switch(num)
			{
				case 0: spawn.GetComponent<BasicDrone>().waypoints = wpsOuterLeft;
				break;
				case 1: spawn.GetComponent<BasicDrone>().waypoints = wpsOuterRight;
				break;
				case 2: spawn.GetComponent<BasicDrone>().waypoints = wpsInnerRight;
				break;
				case 3: spawn.GetComponent<BasicDrone>().waypoints = wpsInnerLeft;
				break;
			}
			num++;
		}
	}
}

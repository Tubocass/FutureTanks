using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSpawner : MonoBehaviour 
{
	public GameObject DroneSpawn;
	public GameObject[] wpsLeft, wpsRight;
	int num;

	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			GameObject spawn = (GameObject)Instantiate(DroneSpawn,transform.position+new Vector3(5,2,1),Quaternion.identity);
			num = (int)Mathf.Repeat(num, 2);
			spawn.GetComponent<BasicDrone>().waypoints = num == 1 ? wpsLeft:wpsRight;
			num++;
		}
	}
}

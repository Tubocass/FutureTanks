using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour 
{
	BasicDrone target;
	LayerMask mask;
	[SerializeField]Transform Muzzle;
	[SerializeField] GameObject bullet;
	[SerializeField]float sightRange;
	bool canFire = true;

	void Start()
	{
		mask = 1<<LayerMask.NameToLayer("Players");
		StartCoroutine(TargetNearest());
	}

	IEnumerator TargetNearest()
	{
		while(true)
		{
			if(target == null)
			{
				RaycastHit hitInfo = new RaycastHit();
				Physics.SphereCast(transform.position,sightRange,transform.forward,out hitInfo,sightRange,mask, QueryTriggerInteraction.Ignore);
				if(hitInfo.collider!=null && hitInfo.collider.CompareTag("Drone"))
				{
					target = hitInfo.collider.GetComponent<BasicDrone>();
				}
			}else{
				if(Vector3.Distance(transform.position,target.transform.position)>sightRange*2)
				{
					target = null;
				}
			}
			yield return new WaitForSeconds(.25f);
		}
	}
	IEnumerator Cooldown()
	{
		yield return new WaitForSeconds(1f);
		canFire = true;
	}
	void Update()
	{
		if(target!=null)
		{
			transform.LookAt(target.transform.position);
			if(canFire)
			{
				Quaternion q = Quaternion.LookRotation(target.transform.position-Muzzle.position);
				GameObject bull = (GameObject)Instantiate(bullet,Muzzle.position,q);
				canFire = false;
				StartCoroutine(Cooldown());
			}
		}
	}
}

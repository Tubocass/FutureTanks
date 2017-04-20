using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour 
{
	public float sightRange;
	public Transform Muzzle;
	public GameObject bulletFab;
	private bool canFire = true;
	private float sightSqrd;
	[SerializeField] string Enemy;
	GameObject target;
	[SerializeField]LayerMask enemyMask;
	LayerMask levelMask;

	void Start()
	{
		//enemyMask = 1<<LayerMask.NameToLayer("Players");
		levelMask = 1<<LayerMask.NameToLayer("LevelGeometry");
		sightSqrd = sightRange*sightRange;
		StartCoroutine(TargetNearest());
	}

	IEnumerator TargetNearest()
	{
		while(true)
		{
			if(target == null)
			{
//				RaycastHit hitInfo = new RaycastHit();
//				Physics.SphereCast(transform.position,sightRange,transform.forward,out hitInfo,1,mask, QueryTriggerInteraction.Ignore);
//				if(hitInfo.collider!=null && hitInfo.collider.CompareTag("Drone"))
//				{
//					target = hitInfo.collider.GetComponent<BasicDrone>();
//				}
				Collider[] cols = Physics.OverlapSphere(transform.position,sightRange,enemyMask,QueryTriggerInteraction.Ignore);
				float nearestDist, newDist;
				Collider temp = new Collider();

				if(cols.Length>0)
				{
					nearestDist = (cols[0].transform.position-transform.position).sqrMagnitude; //Vector3.Distance(Location,enemies[0].Location);
					foreach(Collider o in cols)
					{
						if(o.CompareTag(Enemy))
						{
							newDist = (o.transform.position-transform.position).sqrMagnitude;//Vector3.Distance(Location,unit.Location);
							if(newDist <= nearestDist)
							{
								nearestDist = newDist;
								temp = o;
							}
						}
					}
					if(temp!=null) //&& !Physics.Raycast(Muzzle.position,temp.transform.position-Muzzle.position,sightRange,levelMask))
					target = temp.gameObject;
				}

			}else{
				if((transform.position-target.transform.position).sqrMagnitude>sightSqrd)
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
		if(target!=null && !Physics.Raycast(Muzzle.position,target.transform.position-Muzzle.position,sightRange,levelMask))
		{
			Debug.DrawRay(Muzzle.position, target.transform.position-Muzzle.position);
			transform.LookAt(target.transform.position);
			if(canFire)
			{
				Quaternion q = Quaternion.LookRotation(target.transform.position-Muzzle.position);
				GameObject bull = (GameObject)Instantiate(bulletFab,Muzzle.position,q);
				canFire = false;
				StartCoroutine(Cooldown());
			}
		}
	}
}

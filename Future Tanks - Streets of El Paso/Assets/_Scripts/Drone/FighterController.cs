﻿using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class FighterController : DroneController 
{
	[SerializeField] float attackStrength , selfAttack, refractoryPeriod;
	Unit_Base targetEnemy;
	List<Unit_Base> enemies;
	List<Unit_Base> enemiesCopy;
	ParticleSystem spark;
	LayerMask mask;
	bool canAttack=true, bReturning;

	protected override void OnEnable()
	{
		spark = GetComponentInChildren<ParticleSystem>();
		enemies = new List<Unit_Base>();
		mask = 1<<LayerMask.NameToLayer("Units");
		canAttack=true;
		base.OnEnable();
		//UnityEventManager.StartListeningInt("PlaceFightFlag", UpdateFlagLocation);
	}
//	protected override void OnDisable()
//	{
//		base.OnDisable();
//		UnityEventManager.StopListeningInt("PlaceFightFlag", UpdateFlagLocation);
//	}
//	protected override void UpdateFlagLocation(int mom)
//	{
//		if(isServer)
//		{
//			if(myMoM.unitID == mom  && Vector3.Distance(Location, myMoM.FightAnchor)>orbit)
//			{
//				targetEnemy = null;
//				MoveTo(myMoM.FightAnchor);
//			}
//		}
//	}

	protected override void TargetLost(int id)
	{
		if(targetEnemy!=null && id == targetEnemy.unitID)
		{
			enemies.Remove(targetEnemy);
			targetEnemy = null;
			ArrivedAtTargetLocation();
		}
	}

//	protected override void Death()
//	{
//		base.Death();
//		myMoM.fighters-=1;
//	}

//	protected override void ArrivedAtTargetLocation()
//	{
//		//base.ArrivedAtTargetLocation();
//		if(isServer)
//		{
//			if(IsTargetingEnemy())
//			{
//				if(canAttack && Vector3.Distance(Location,targetEnemy.Location)<1f)
//				{
//					Attack(targetEnemy);
//				}else{
//			 	MoveTo(targetEnemy.Location);
//			 	}
//			}else {
//				targetEnemy = TargetNearest();
//				if(targetEnemy!=null)
//				{
//					MoveTo(targetEnemy.Location);
//				}else MoveRandomly(myMoM.FightAnchor, orbit);
//			}
//		}
//	}

	protected override IEnumerator MovingTo()
	{
		while(bMoving)
		{
			if(agent.remainingDistance<1)
			{
				if(currntPoint<points-1)
				{
					currntPoint +=1;
					currentVector = Path[currntPoint];
					agent.SetDestination(currentVector);
				}else bMoving = false;
			}else
			{
				yield return new WaitForSeconds(0.5f);
				if(IsTargetingEnemy()) MoveTo(targetEnemy.Location);
			}
		}
	}

	Unit_Base TargetNearest()
	{
		float nearestEnemyDist, newDist;
		Unit_Base enemy = null;
		//enemies.RemoveAll(e=> !e.isActive);
		enemies.Clear();
		//enemiesCopy = enemies.FindAll(e=> e.isActive && e.teamID!=teamID && (e.Location-Location).sqrMagnitude<sqrDist);

		RaycastHit[] hits = Physics.SphereCastAll(Location,sightRange,tran.forward,1,mask, QueryTriggerInteraction.Ignore);
		if(hits.Length>0)
		{
			foreach(RaycastHit f in hits)
			{
//				if(f.collider.CompareTag("Sarlac"))
//				{
//					enemy = f.collider.GetComponent<SarlacController>();
//					if(enemy!=null )
//					{
//						return enemy;
//					}
//				}
//				if(f.collider.CompareTag("MoM"))
//				{
//					Unit_Base ot = f.collider.GetComponent<MoMController>();
//					if(ot!=null && ot.teamID!=teamID && !enemies.Contains(ot))
//					{
//						enemies.Add(ot);
//					}
//				}
				if(f.collider.CompareTag("Drone"))
				{
					Unit_Base ot = f.collider.GetComponent<Unit_Base>();
					if(ot!=null && !ot.teamID.Equals(teamID) && !enemies.Contains(ot))
					{
						enemies.Add(ot);
					}
				}
			}
		}

		enemiesCopy = enemies.FindAll(e=> e.isActive && e.teamID!=teamID && (e.Location-Location).sqrMagnitude<sqrDist);
		if(enemiesCopy.Count>0)
		{
			nearestEnemyDist = (enemiesCopy[0].Location-Location).sqrMagnitude; //Vector3.Distance(Location,enemies[0].Location);
			foreach(Unit_Base unit in enemiesCopy)
			{
				if(unit.isActive)
				{
					newDist = (unit.Location-Location).sqrMagnitude;//Vector3.Distance(Location,unit.Location);
					if(newDist <= nearestEnemyDist)
					{
						nearestEnemyDist = newDist;
						enemy = unit;
					}
				}else enemies.Remove(unit);
			}
		}
		return enemy;
	}

	void Attack(Unit_Base target)
	{
		spark.Play();
		target.TakeDamage(attackStrength);
		TakeDamage(selfAttack);
		canAttack = false;
		if(this.isActive)
		StartCoroutine(AttackCooldown());
	}

	IEnumerator AttackCooldown()
	{
		yield return new WaitForSeconds(refractoryPeriod);
		canAttack = true;
	}

	bool IsTargetingEnemy()
	{
		if(targetEnemy!=null && targetEnemy.isActive)
		return true;
		else return false;
	}

//	bool CanTargetEnemy()
//	{
//		if(!IsTargetingEnemy())
//		return true;
//		else return false;
//	}

	public override void OnCollisionEnter(Collision bang)
	{
		if(!isServer)
		return;
		if(bang.collider.CompareTag("Drone")||bang.collider.CompareTag("Sarlac")||bang.collider.CompareTag("MoM"))
		{
			Unit_Base ot = bang.gameObject.GetComponent<Unit_Base>();
			if(ot!=null && !ot.teamID.Equals(teamID) && canAttack)
			{
				Attack(ot);
			}
		}
	}
}

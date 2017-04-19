using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{
	[SerializeField] float speed = 4f, timer = 4f;
	Transform tran;
	void OnEnable () 
	{
		tran = transform;
		StartCoroutine(Death());
	}
	void Update () 
	{
		//tran.Translate(Vector3.up*speed*Time.deltaTime);
		Vector3 targetDir  = tran.position + tran.forward*speed; 

		tran.position = Vector3.MoveTowards(tran.position,targetDir, speed);

	}
	IEnumerator Death()
	{
		yield return new WaitForSeconds(timer);
		gameObject.SetActive(false);
	}
	void OnCollisionEnter(Collision bam)
	{
		if(!bam.collider.CompareTag("Turret"))
		{
			StopCoroutine(Death());
			gameObject.SetActive(false);
		}
	}

}

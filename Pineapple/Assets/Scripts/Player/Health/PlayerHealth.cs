﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHealth : HealthGeneric {

	public GameObject deathEffect;
	public GameObject deathEffectHairCut;
	public bool invincible;
	public Vector2 impulseForce;
	public TakeScreenShot takeScreenShot;
	[Header("Shield")]
	public GameObject hitEffect;
	public AudioClip shieldHitSound;
	public bool shieldActive;
	public List<GameObject> healthBars = new List<GameObject>();
	private PlayerController playerController;
	
	void Awake()
	{
		playerController = GetComponent<PlayerController>();
		
	}

	void Start()
	{
		deathEffect = CharacterManager.activeVisual.deathEffect;
	}

	override public void TakeDamage(float damage)
	{
		if(!invincible)
		{
			if(health == 1)
			{
				shieldActive = false;
				Invulnerable(1.5f);
			}
			else if(shieldActive)
					DamageShield();
			base.TakeDamage(damage);
		}
		StartCoroutine(killPlayer());
	}

	public void AddHealth(float amount)
	{
		health += amount;
	}
	
	public void DamageShield()
	{
		if(hitEffect) hitEffect.SetActive(true);
		if(shieldHitSound) GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(shieldHitSound);
		for (int i = healthBars.Count - 1; i > 0; i--)
		{
			if(healthBars[i].activeInHierarchy)
			{
				healthBars[i].SetActive(false);
				break;
			}
		}
	}

	public void AddShield(float amount, List<GameObject> shieldHealthBars)
	{
		health += amount;
		healthBars = shieldHealthBars;
		shieldActive = true;
	}

	public void Invulnerable(float duration)
	{
		invincible = true;
		playerController._anim.SetBool("Invincible", true);
		Invoke("resetInvulnerablilty", duration);
	}
	void resetInvulnerablilty()
	{
		invincible = false;
		playerController._anim.SetBool("Invincible", false);
	}

	IEnumerator killPlayer()
	{
		if(health <= 0 && !dead)
		{
			playerController._anim.SetBool("Sad", true);
			if(takeScreenShot)
				yield return (StartCoroutine(takeScreenShot.ScreenShot()));
			//player is dead
			dead = true;
			//Instantiate(playerController._haircut ? deathEffectHairCut : deathEffect, transform.position + Vector3.up * 2.5f, transform.rotation);
			deathEffect.SetActive(true);
			deathEffect.transform.parent = null;
			foreach(Rigidbody2D r in deathEffect.GetComponentsInChildren<Rigidbody2D>())
			{
				r.AddForce(new Vector2(impulseForce.x + Random.Range(-2,2), impulseForce.y + Random.Range(-2,2)), ForceMode2D.Impulse);
			}
			playerController.pausePlayer = true;
			gameObject.SetActive(false);
		}
	}

	public Transform FindFurthestBodyPart()
	{
		Transform g = null;
		foreach(Transform child in deathEffect.transform)
		{
			if(g == null)
				g = child;
			if(child.position.x > g.transform.position.x)
				g = child;
		}
		return g;
	}

	public bool BodyPartsStopMoving()
	{
		List<bool> allStopped = new List<bool>();
		foreach(Rigidbody2D r in deathEffect.GetComponentsInChildren<Rigidbody2D>())
			allStopped.Add(r.velocity == Vector2.zero);
			
		return !allStopped.Contains(false) ? true : false;
	}
}
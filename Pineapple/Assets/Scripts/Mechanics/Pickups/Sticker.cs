﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Sticker : MonoBehaviour
{
    [Header("Sticker")]
    public int triggerAmount = 1;
    public int value;
    public AudioClip pickUpSound;
    public GameObject deathEffect;

    [Header("Sticker Move To Player")]
    public float moveSpeed = 30f;
    public AnimationCurve animationCurve;
    [HideInInspector] public GameObject moveToTarget;
    [HideInInspector] public bool move;
  
    void Start()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Random.Range(-90f,90f));
    }

    void OnEnable()
    {
        triggerAmount = 1;
        deathEffect.transform.position = this.gameObject.transform.position;
        gameObject.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(triggerAmount > 0 && (other.CompareTag("Player") || other.CompareTag("Hair")))
            DoAction(other.gameObject);
    }

    void Update()
    {
        if(move)
        {
            moveStickerToTarget(CharacterManager.activeVisual.gameObject, moveSpeed);
        }
    }

    public void DoAction(GameObject player)
    {
        triggerAmount--;
        AudioSource a = StatsManager.Instance.gameObject.GetComponent<AudioSource>();
        a.Stop();
        a.PlayOneShot(pickUpSound);
        deathEffect.SetActive(true);
        deathEffect.transform.parent = null;
        StatsManager.Instance.stickerCollectedThisRound += value;
        gameObject.SetActive(false);
        move = false;
        //base.DoAction(player);
    }

    public void moveStickerToTarget(GameObject target, float speed)
    {
       transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * (speed * animationCurve.Evaluate(Time.time)));
       //transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * (speed)); //+ target.GetComponent<Rigidbody2D>().velocity.x));
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public bool destroyHair;
    public float damageAmount;
    public bool destroyAfterDamage;
    public AudioClip hitSoundEffect;
    public GameObject hitEffect;

    private Collision2D collision;

    void OnTriggerEnter2D(Collider2D other)
    {
       DoDamage(other.gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
            collision = other;
        DoDamage(other.gameObject);
    }

    void DoDamage(GameObject other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<HealthGeneric>().TakeDamage(damageAmount);
            if(hitSoundEffect != null)
                GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>().PlayOneShot(hitSoundEffect);
            if(destroyAfterDamage)
            {
                Debug.Log(collision);
                var newRot = Quaternion.Euler(0, 0, -Vector3.Angle(GetComponent<Rigidbody2D>().velocity, -collision.GetContact(0).normal));
                Instantiate(hitEffect, collision.GetContact(0).point,newRot);
                gameObject.GetComponent<ObjectID>().Disable();
            }
        }
        if(other.CompareTag("Hair") && destroyHair)
        {
            PlayerController p =  other.GetComponentInParent<PlayerController>();
            if(!p.hairMask.activeInHierarchy)
            {
                p.hairMask.SetActive(true);
                DialogueSequence dialogue = p.GetComponent<DialogueSequence>();
                dialogue.dialogues[0].text = "MY HAIR!";
                dialogue.dialogues[0].diallogueInterval = 2;
                dialogue.startDialogue();
                p._anim.SetTrigger("Sad");
                p.GetComponent<AudioSource>().PlayOneShot(p.hairSlicedAudio);
                StatsManager.AddToAStat(1,"HaircutsTaken");
                GameObject tempObj = Instantiate(p.slicedHair, other.transform.position, p.slicedHair.transform.rotation);
                Destroy(tempObj, 1f);
            }
        }
    }
}
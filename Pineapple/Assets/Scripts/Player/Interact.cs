﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{
    public float distance;
    public InteractButton interactButton;

    void Update()
    {
        RayCast();
    }
    void RayCast()
    {
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3.up * 0.5f), Vector2.right * transform.localScale.x, distance);
        Debug.DrawRay(transform.position + (Vector3.up * 0.5f), Vector2.right * transform.localScale.x * distance, Color.red);

        if(hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable"))
            interactButton.InteractableObject = hit.collider.gameObject.GetComponent<Interactable>();
        else
            interactButton.InteractableObject = null;
           
    }
}


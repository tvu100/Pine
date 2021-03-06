﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    
    public bool shouldExpand;
    public List<GameObject> activeObjects = new List<GameObject>();
    private float _timer;
    [HideInInspector] public  GameObject _previousSpawn;

    public Vector2 GetNextSpawnPosHorizontal(SpriteRenderer previousSpawnSPos, SpriteRenderer nextSpawn)
    {
        //get the half size of the previous sprite
        float previousX = (previousSpawnSPos.size.x/2 * previousSpawnSPos.gameObject.transform.localScale.x);
        //get the half size of the next sprite
        float nextX = (nextSpawn.size.x/2 * nextSpawn.transform.localScale.x);
        //translate the new sprite to the right by previous X
        Vector2 newSpawnPos = new Vector2((previousSpawnSPos.transform.position.x + previousX) + nextX, previousSpawnSPos.transform.parent.position.y);
        return newSpawnPos;
    }

    public Vector2 GetNextSpawnPosVertical(SpriteRenderer previousSpawnSPos, SpriteRenderer nextSpawn)
    {
        Vector2 previousCornerPos = new Vector2(previousSpawnSPos.size.x/2 * previousSpawnSPos.gameObject.transform.localScale.x, previousSpawnSPos.size.y/2 * previousSpawnSPos.gameObject.transform.localScale.y);
        Vector2 newCornerPos = new Vector2(nextSpawn.size.x/2, nextSpawn.size.y * nextSpawn.gameObject.transform.localScale.y);
        Vector2 newSpawnPos = new Vector2((previousSpawnSPos.transform.position.x - previousCornerPos.x) + newCornerPos.x, (previousSpawnSPos.transform.position.y - previousCornerPos.y) - newCornerPos.y/2);
        //Vector2 newSpawnPos = new Vector2((previousSpawnSPos.transform.position.x - previousCornerPos.x) + newCornerPos.x, previousSpawnSPos.transform.position.y - newCornerPos.y);
        return newSpawnPos;
    }

    /*public bool SpawnTimer()
    {
        _timer += Time.deltaTime;
        if(_timer > spawnInterval)
        {
            _timer = 0f;
            return true;
        }
        return false;
    }*/
    public void ClearActiveObjects()
    {
        if(activeObjects.Count > 0)
        {
            for (int i = 0; i < activeObjects.Count; i++)
            {
                activeObjects[0].SetActive(false);
                activeObjects[0].transform.parent = RegionPoolManager.Instance.transform;
                activeObjects.Remove(activeObjects[0]);
            }
        }
    }
    public abstract void DoSpawn();  
}

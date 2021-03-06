﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum SpawnType
    {
        wave,
        line
    }
    
public class ProjectileSpawner : Spawner
{       
    public float spawnIntervalTime = 1;

    [Header("Spawner Details")]
    public SpawnType spawnType;
    public float yAmplitude; //how high and low the spawnermoves
    public float moveSpeed;
    [MinMaxSlider(1,5)] public Vector2 spawnAmount = new Vector2(1, 3);
    public AudioClip warningSound;
    public ObjectPools warningBubble;
    
    [HideInInspector]public float disableOSpawnerTimer = 3;
    [HideInInspector]public float warningTimer = 1.5f;
    private float _halfHeight;
    private float _halfWidth;
    private float _newY;
    private Camera _camera;
    public ObjectPools projectilePool;
    void Awake()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        //bounce this obj up and down
        _newY = _camera.transform.position.y + Mathf.Sin(moveSpeed * Time.time) * yAmplitude;
        transform.position = new Vector3(transform.position.x, _newY, transform.position.z);
    }
    
    public override void DoSpawn()
    {
        StartCoroutine(RandomSpawnType());
    }

    public IEnumerator RandomSpawnType()
    {
        
        spawnType = (SpawnType)Random.Range(0, System.Enum.GetValues(typeof(SpawnType)).Length);
        if(MasterSpawner.Instance != null) MasterSpawner.Instance.enabled = false;
        switch(spawnType)
        {
            case SpawnType.line:
                SpawnLine();
            break;
            case SpawnType.wave:
                StartCoroutine(SpawnWaveHoming());
            break;
        }
        yield return new WaitForSeconds(disableOSpawnerTimer);
        if(MasterSpawner.Instance != null) MasterSpawner.Instance.enabled = true;
    }

    public void SpawnLine()
    {
        int r = 2; //(int)Random.Range(spawnAmount.x,spawnAmount.y);
        float yPos = _newY;
        for(int i = 0; i < r; i++)
        {
            StartCoroutine(SpawnObject(yPos));
            //check the distance between the yPos
            if(yPos + 1 < _camera.transform.position.y + yAmplitude)
                yPos += Random.Range(2f, 3f);
            else
                 yPos -= Random.Range(4f,6f);
        }
    }

    public IEnumerator SpawnWave()
    {
        int r = (int)Random.Range(spawnAmount.x,spawnAmount.y);
        for(int i = 0; i < r; i++)
        { 
           StartCoroutine(SpawnObject(_newY));
           yield return new WaitForSeconds(spawnIntervalTime);
        }
    }

    public IEnumerator SpawnWaveHoming()
    {
        int r = (int)Random.Range(spawnAmount.x,spawnAmount.y);
        for(int i = 0; i < r; i++)
        { 
           StartCoroutine(SpawnObject(CharacterManager.activeCharacter.transform.position.y));
           yield return new WaitForSeconds(spawnIntervalTime);
        }
    }

    IEnumerator SpawnObject(float yPos)
    {
        _halfHeight = _camera.orthographicSize;
        _halfWidth  = _camera.aspect * _halfHeight; 
        GameObject nextSpawn = projectilePool.GetNextItem();
        //wait for this Warning to finish then continue
        yield return StartCoroutine(Warning(nextSpawn,yPos));
        //reset trailrenderer
        nextSpawn.GetComponentInChildren<TrailRenderer>().Clear();
        nextSpawn.SetActive(true);
        nextSpawn.transform.position = new Vector3(Camera.main.transform.position.x + _halfWidth, yPos, transform.position.z);
    }

    IEnumerator Warning(GameObject obj, float yPos)
    {
        GameObject w = warningBubble.GetNextItem();
        if(!w.activeInHierarchy)
        {
            w.SetActive(true);
            GetComponent<AudioSource>().PlayOneShot(warningSound);
            w.transform.position = new Vector3(Camera.main.transform.position.x + _halfWidth, yPos, transform.position.z);
            w.transform.parent = _camera.transform;
            yield return new WaitForSeconds(warningTimer);
            w.SetActive(false);
        }
    }
}

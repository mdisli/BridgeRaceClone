using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StickCreater : MonoBehaviour
{
    [Header("STICK CREATING INFOS")]
    [SerializeField] List<GameObject> prefabs;
    [SerializeField] Transform startingPos;
    [SerializeField] int xCount, zCount;
    [SerializeField] Vector3 xPosAdd, zPosAdd;

    public static StickCreater instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        CreateStickOnStart();
    }
    void CreateStickOnStart()
    {
        Vector3 startPos = startingPos.position;
        for (int z = 0; z < zCount; z++)
        {
            for (int x = 0; x < xCount; x++)
            {
                int randomNum = Random.Range(0, 11) % 3;
                Instantiate(prefabs[randomNum], startPos, Quaternion.identity, GameObject.Find("Stick Holder").transform);
                startPos += xPosAdd;
            }

            startPos = startingPos.position;
            startPos += zPosAdd * (z + 1);
        }
    }

    public IEnumerator CreateStickAfterCollect(Vector3 position,float delay)
    {
        yield return new WaitForSeconds(delay);
        int num = Random.Range(0, prefabs.Count);
        Instantiate(prefabs[num], position, Quaternion.identity, GameObject.Find("Stick Holder").transform);
    }
    
}
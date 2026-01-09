using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPooling : MonoBehaviour
{
    public static BulletPooling instance;
    private void OnEnable()
    {
        instance = this;
    }
    private void OnDisable()
    {
        instance = null;
    }
    [SerializeField] List<GameObject> pooledObjects;
    [SerializeField] GameObject objectToPool;
    [SerializeField] int amountPool;

    private void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject pooledObj;
        for (int i = 0; i < amountPool; i++)
        {
            pooledObj = Instantiate(objectToPool);
            pooledObj.SetActive(false);
            pooledObjects.Add(pooledObj);
        }
    }
    public GameObject GetPooledObject()
    {
        foreach (GameObject pooledObj in pooledObjects)
        {
            if (!pooledObj.activeInHierarchy)
            {
                return pooledObj;
            }
        }
        return null;
    }
}

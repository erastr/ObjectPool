using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    [NonReorderable][SerializeField] private List<ObjectWithCount> pooledObjects;

    private Dictionary<PooledGameObjectType, List<GameObject>> pooledGameObjectDic = new Dictionary<PooledGameObjectType, List<GameObject>>();

    [SerializeField] private Transform objectsParent;

    //We create the pool with the specified parameters.
    private void Awake()
    {
        GameObject go;

        for (int i = 0; i < pooledObjects.Count; i++)
        {
            pooledGameObjectDic[pooledObjects[i].type] = new List<GameObject>();

            for (int j = 0; j < pooledObjects[i].count; j++)
            {
                go = Instantiate(pooledObjects[i].go, objectsParent);
                go.SetActive(false);

                pooledGameObjectDic[pooledObjects[i].type].Add(go);
            }
        }


    }

    //We pull the usable object from the pool.
    public GameObject GetPooledGameObject(PooledGameObjectType pooledGoType)
    {

        for (int i = 0; i < pooledGameObjectDic[pooledGoType].Count; i++)
        {
            if (pooledGameObjectDic[pooledGoType][i].activeInHierarchy == false)
                return pooledGameObjectDic[pooledGoType][i];
        }

        //If there is no usable object in the pool, we add 5 more objects to the pool.

        GameObject go = null;

        for (int i = 0; i < 5; i++)
        {
            go = Instantiate(pooledGameObjectDic[pooledGoType][0], objectsParent);
            go.name = pooledGameObjectDic[pooledGoType][0].name;
            go.SetActive(false);
            pooledGameObjectDic[pooledGoType].Add(go);
        }

        return go;


    }


    [System.Serializable]
    private class ObjectWithCount
    {
        public PooledGameObjectType type;
        public GameObject go;
        public int count;
    }


}

public enum PooledGameObjectType
{
    Bullet = 0
}

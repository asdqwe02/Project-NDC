using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum TagName
{
    FireBullet,
    IceBullet,
    LightningBullet,
    Slime,
    Archer,
    Bomber,
    ShieldEnemy,
    IceArrow,

}
namespace DesignPattern
{
    [System.Serializable] 
    public class ObjectPoolItem
    {
        [SerializeField] private string name;
        public GameObject objectToPool;
        public int amountToPool;
        public bool shouldExpand = false;
        public float expirationTime;
        public TagName nameTag;

    }
    [System.Serializable]
    public class PoolObject
    {
        public float lastActiveTime = 0;
        public TagName nameTag;
        public GameObject gameObject;
        public PoolObject(GameObject obj, TagName tag, float time = 0f)
        {
            gameObject = obj;
            nameTag = tag;
            lastActiveTime = time;
        }
    }
    public class ObjectPooler : UnitySingleton<ObjectPooler>
    {
        [SerializeField] private float _cleanUpInterval = 30f; //default 30 second
        public List<PoolObject> allPooledObjects;
        public List<ObjectPoolItem> itemsToPool;
        private void Awake()
        {
            base.Awake();
            allPooledObjects = new List<PoolObject>();
            foreach (ObjectPoolItem item in itemsToPool)
            {
                for (int i = 0; i < item.amountToPool; i++)
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool, transform);
                    obj.SetActive(false);
                    allPooledObjects.Add(new PoolObject(obj, item.nameTag));
                }
            }
            StartCoroutine(CleanUp());
        }
        /// <summary>
        /// Get Pooled Object by tag given from the Unity engine
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public GameObject GetPooledObject(string tag)
        {
            foreach (PoolObject pObject in allPooledObjects)
            {
                if (pObject.gameObject.activeInHierarchy == false && pObject.gameObject.tag == tag)
                {
                    pObject.lastActiveTime = Time.realtimeSinceStartup;
                    return pObject.gameObject;
                }
            }
            foreach (ObjectPoolItem item in itemsToPool)
            {
                if (item.objectToPool.tag == tag)
                {
                    if (item.shouldExpand)
                    {

                        return CreateObject(item.objectToPool, item.nameTag);
                    }
                }
            }
            return null;
        }
        public GameObject GetPooledObject(TagName tag)
        {
            foreach (PoolObject pObject in allPooledObjects)
            {
                if (pObject.gameObject.activeInHierarchy == false && pObject.nameTag == tag)
                {
                    // pObject.lastActiveTime = Time.realtimeSinceStartup;
                    return pObject.gameObject;
                }
            }
            foreach (ObjectPoolItem item in itemsToPool)
            {
                if (item.nameTag == tag)
                {
                    if (item.shouldExpand)
                    {
                        return CreateObject(item.objectToPool, item.nameTag);
                    }
                }
            }
            return null;
        }
        public int ActivePooledObjectCount(string tag)
        {
            int c = 0;
            for (int i = 0; i < allPooledObjects.Count; i++)
            {
                if (allPooledObjects[i].gameObject.activeInHierarchy && allPooledObjects[i].gameObject.tag == tag)
                    c++;
            }
            return c;
        }
        public List<GameObject> GetActivePoolObjects(string tag)
        {
            List<GameObject> activeObjectts = new List<GameObject>();
            for (int i = 0; i < allPooledObjects.Count; i++)
            {
                if (allPooledObjects[i].gameObject.activeInHierarchy && allPooledObjects[i].gameObject.tag == tag)
                    activeObjectts.Add(allPooledObjects[i].gameObject);
            }
            return activeObjectts; // if null => error
        }
        public bool Reclaim(GameObject obj) // is this necessary ? doesn't seem like it
        {
            PoolObject poolObject = allPooledObjects.Find(pObject => pObject.gameObject == obj && pObject.gameObject.activeInHierarchy);
            // if (pooledObjects.Any(pObject => pObject.gameObject == obj))
            if (poolObject != null)
            {
                poolObject.gameObject.transform.parent = transform;
                poolObject.gameObject.transform.localPosition = Vector3.zero;
                poolObject.gameObject.SetActive(false);
                poolObject.lastActiveTime = Time.realtimeSinceStartup;
                return true; // deactivate successful
            }
            return false; // deactivate not successful either not exist in list or some error
        }

        public void RemoveAllObjectWithTag(string tag)
        {
            foreach (var pObj in allPooledObjects)
            {
                if (pObj.gameObject.tag == tag)
                {
                    Vector3 pos = pObj.gameObject.transform.localPosition;
                    pos.x = 0; pos.z = 0; // mostly keep the y pos maybe should make another function for this
                    pObj.gameObject.transform.parent = transform;
                    pObj.gameObject.transform.localPosition = pos;
                    pObj.gameObject.SetActive(false);
                    pObj.lastActiveTime = Time.realtimeSinceStartup;
                }
            }
        }
        public GameObject CreateObject(GameObject objectToPool, TagName tag)
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            obj.transform.parent = transform;
            obj.SetActive(false);
            allPooledObjects.Add(new PoolObject(obj, tag, Time.realtimeSinceStartup));
            return obj;
        }
        public IEnumerator CleanUp() // clean up the unused disable object in pool after certain interval
        {
            while (Instance != null)
            {
                yield return new WaitForSeconds(_cleanUpInterval);
                // Debug.Log("object pool clean up start!!!");
                foreach (ObjectPoolItem objectPoolItem in itemsToPool)
                {
                    List<PoolObject> poolObjects = allPooledObjects.FindAll(pObject =>
                        // pObject.gameObject.name.Contains(objectPoolItem.objectToPool.name)
                        pObject.nameTag == objectPoolItem.nameTag
                    );

                    // Debug.Log($"founded {poolObjects.Count} object of {objectPoolItem.objectToPool.name}");
                    if (poolObjects.Count > objectPoolItem.amountToPool)
                    {
                        // Debug.Log($"pooled amount for {objectPoolItem.objectToPool.name} is greater than the max limit!");
                        List<PoolObject> expiredPoolObjects = poolObjects.FindAll(pObject =>
                            !pObject.gameObject.activeInHierarchy
                            && ((Time.realtimeSinceStartup - pObject.lastActiveTime) > objectPoolItem.expirationTime)
                        );
                        // Debug.Log($"number of expired objects: {expiredPoolObjects.Count}");
                        foreach (var expiredObj in expiredPoolObjects)
                        {
                            Destroy(expiredObj.gameObject);
                            allPooledObjects.Remove(expiredObj);
                            poolObjects.Remove(expiredObj);
                            if (poolObjects.Count <= objectPoolItem.amountToPool)
                                break;
                        }
                    }
                }
            }
        }
    }

}

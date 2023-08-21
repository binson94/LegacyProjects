using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : MonoBehaviour
{
    T prefab;
    Queue<T> queue = new Queue<T>();

    public Pool(T prefab)
    {
        this.prefab = prefab;
    }

    public T Create()
    {
        if(queue.Count <= 0)
            return Object.Instantiate(prefab);

        return queue.Dequeue();
    }

    public void Remove(T obj)
    {
        queue.Enqueue(obj);
    }
}

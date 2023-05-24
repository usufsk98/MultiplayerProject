using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton_IndependentObject<T> : MonoBehaviour
    where T : Component
{
    public static T instance;
    public virtual void Awake() => Instantiate();
    public void Instantiate()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }
}

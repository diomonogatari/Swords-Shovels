using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T> //prevents singletons being declared of any type, only properly extended Singleton classes are allowed
{
    private static T instance;
    public static T Instance
    {
        get { return instance; }//only get because we want to protect from external changes
    }

    public static bool IsInitialized
    {
        get { return instance != null; }
    }
    protected virtual void Awake()//The 1st methods called in Unity lifecycle
    {
        if (instance != null)
        {
            Debug.LogError("[Singleton] Trying to instantiate a second instance of a singleton class. BAD!");
        }
        else
        {
            instance = (T)this; //make sure it's the same type
        }
    }

    protected virtual void OnDestroy() //when things are called to being destroyd
    {
        if (instance == this)
        {
            instance = null; //"Destroy" the singleton, it makes it instantiatable again
        }
    }
}

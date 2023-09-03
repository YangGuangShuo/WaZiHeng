using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
{
    private static T instance;
    public static T Instance { get { return instance; } }
    protected virtual void Awake() { instance = (T)this; }
    public virtual void Init() { }
}

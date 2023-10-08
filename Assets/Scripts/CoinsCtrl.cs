using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsCtrl : MonoBehaviour
{
    private Transform[] mChildrens;

    void Start()
    {
        mChildrens = transform.GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform child in mChildrens)
        {
            if (child.GetInstanceID() != transform.GetInstanceID())
            {
                child.eulerAngles += Time.deltaTime * Vector3.up * 20.0f;
            }
        }
    }
}

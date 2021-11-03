using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class DontDestroyOnLoad : MonoBehaviour
{
    void Awake()
    {
        transform.SetParent(null);
        Object.DontDestroyOnLoad(gameObject);
    }
}

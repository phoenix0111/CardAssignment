using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInstance : MonoBehaviour
{
    public static AudioInstance Instance;

    public AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

using UnityEngine.Audio;
using UnityEngine;



//OBSOLETE DELETE THIS
[System.Serializable]
public class SoundDEPRECATED
{
    public string name;
    public AudioClip audioClip;

    [Range(0f,1f)]
    public float volume;
    [Range(.1f,3f)]
    public float pitch;
    public bool loop=false;

    [HideInInspector]
    public AudioSource audioSource;
    
}

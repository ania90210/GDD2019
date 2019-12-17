using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public SoundAudioClip[] soundAudioClips;

    private static GameAssets _instance;
    public static GameAssets instance {
        get {
            return _instance;
        }
    }

    void Awake() {
        if (_instance == null) _instance = this;

        DontDestroyOnLoad(this);
    }

    [System.Serializable]
    public class SoundAudioClip {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }


}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager 
{
    public enum Sound {
        PlayerMove,
        PlayerAttack,
        EnemyHit,
        EnemyDie,
        TreasureFound,
        TreasureCollected,
        TrashPickedUp,
        TrashThrownAway,
        TrashFilled,
        TrashUpgrade1,
        TrashUpdrage2
    }

    public static void PlaySound(Sound sound, float timer = 1.0f) {
        GameObject soundGameObject = new GameObject("sound");
        AudioSource audiosource = soundGameObject.AddComponent<AudioSource>();
        SelfDestruction selfDestruction = soundGameObject.AddComponent<SelfDestruction>();
        selfDestruction.SelfDestroyIn(timer);

        audiosource.PlayOneShot(GetAudioClip(sound));  
    }

    public static AudioClip GetAudioClip(Sound sound) {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.instance.soundAudioClips) {
            if (sound == soundAudioClip.sound) {
                return soundAudioClip.audioClip;
            }
        }

        Debug.LogError("Sound" + sound + " not found");
        return null;
    }
 }

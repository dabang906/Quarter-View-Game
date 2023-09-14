using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource[] audioSources; // 모든 AudioSource 컴포넌트를 저장하는 배열

    public void SoundControll(float _volume)
    {
        for(int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].volume = _volume;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource[] audioSources; // ��� AudioSource ������Ʈ�� �����ϴ� �迭

    public void SoundControll(float _volume)
    {
        for(int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].volume = _volume;
        }
    }
}

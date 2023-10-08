using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Wazi;

public class SoundManager : SingletonMono<SoundManager>
{
    [SerializeField] private AudioSource m_AudioSourceBGM;
    [SerializeField] private AudioSource m_AudioSourceEffect;
    [SerializeField] private AudioClip m_EatCoinClip;

    public void EatCoin()
    {
        m_AudioSourceEffect.clip = m_EatCoinClip;
        m_AudioSourceEffect.Play();
    }

    public void BGM(bool isPlay = true)
    {
        if (isPlay)
        {
            m_AudioSourceBGM.Play();
        }
        else
        {
            m_AudioSourceBGM.Stop();
        }
    }
}

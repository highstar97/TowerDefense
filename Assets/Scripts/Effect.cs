using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    #region Variables
    private ParticleSystem particle;    // 파티클

    private AudioSource audioSource;    // 오디오 소스
    #endregion

    #region Properties
    public ParticleSystem Particle 
    { 
        get { return particle; }
    }

    public AudioSource AudioSource 
    { 
        get { return audioSource; }
    }
    #endregion

    #region Unity Functions
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }
    #endregion

    #region User Functions
    public void Play()
    {
        // 파티클과 오디오에 내장된 파일 실행
        if (particle != null)
        {
            particle.Play();
        }

        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
    #endregion
}
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
    // 작성자 : 박규탁
    // 기  능 : 이펙트 프리팹에 기본적으로 파티클과 오디오가 내장되어 있음. 따라서 GetComponent를 통해서 변수로 저장
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }
    #endregion

    #region User Functions
    // 작성자 : 박규탁
    // 기  능 : 파티클과 오디오가 있다면 내장된 파일을 실행
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
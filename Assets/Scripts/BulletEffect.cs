using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffect : MonoBehaviour
{
    #region Variables
    public ParticleSystem particle;     // 파티클

    public AudioSource audioSource;     // 오디오 소스
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

    public IEnumerator DestroyAfterPlay()
    {
        // 파티클과 오디오 둘 중 하나라도 플레이되고 있으면 대기
        while(particle.isPlaying || audioSource.isPlaying)
        {
            yield return new WaitForSeconds(0.1f);
        }
        // 파티클, 오디오 모두 플레이 완료 시 오브젝트 삭제
        // TODO : 오브젝트 풀 이용하여서 더욱 최적화 가능할 듯
        Destroy(this.gameObject);
    }
    #endregion
}
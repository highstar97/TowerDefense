using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangedAttackComponent : MonoBehaviour
{
    #region Variables
    public LayerMask allianceLayerMasks;            // alliance Layer Masks;

    public LayerMask targetLayerMasks;              // Target Layer Masks;

    [SerializeField]
    private GameObject bulletEffectPrefabs;

    private BulletEffect bulletEffectInstance;

    [SerializeField]
    private GameObject crosshairPrefabs;

    private GameObject crosshairInstance;
    #endregion

    #region Unity Functions;
    private void Start()
    {
        crosshairInstance = Instantiate(crosshairPrefabs);
    }

    private void Update()
    {
        ARAVRInput.DrawCrosshair(crosshairInstance.transform);

        if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.RTouch))
        {
            ARAVRInput.PlayVibration(ARAVRInput.Controller.RTouch); // 컨트롤러 진동

            Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);

            RaycastHit hitResult;
            if (Physics.Raycast(ray, out hitResult, 200, ~allianceLayerMasks))
            {
                // ray hit on targets
                if ((targetLayerMasks & (1 << hitResult.collider.gameObject.layer)) != 0)
                {
                    Enemy enemy = hitResult.transform.GetComponent<Enemy>();
                    if (enemy)
                    {
                        enemy.TakeDamage();
                    }
                }

                // BulletEffect 프리팹 생성
                bulletEffectInstance = Instantiate(bulletEffectPrefabs).GetComponent<BulletEffect>();
                bulletEffectInstance.transform.forward = hitResult.normal;
                bulletEffectInstance.transform.position = hitResult.point;
                
                // BulletEffect 내장된 파티클, 오디오 실행
                bulletEffectInstance.Play();
                // 파티클, 오디오 모두 실행 시 자동 삭제 될 수 있도록 코루틴 실행
                bulletEffectInstance.StartCoroutine(bulletEffectInstance.DestroyAfterPlay());
            }
        }
        #endregion
    }
}
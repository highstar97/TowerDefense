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

                // BulletEffect Spawner에서 생성
                BulletEffectSpawner.Instance.SpawnBulletEffect(hitResult.point, hitResult.normal);
            }
        }
        #endregion
    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangedAttackComponent : MonoBehaviour
{
    #region Variables
    public Transform fireTransform;

    public LayerMask allianceLayerMasks;            // alliance Layer Masks;

    public LayerMask targetLayerMasks;              // Target Layer Masks;

    [SerializeField]
    private GameObject crosshairPrefabs;

    private GameObject crosshairInstance;

    private LineRenderer lineRenderer;              // 공격 궤적을 그리기 위한 line Renderer

    private EffectSpawner effectSpawner;            // Bullet Effect Spawner
    #endregion

    #region Unity Functions;
    private void Awake()
    {
        fireTransform = Camera.main.transform;      // 우선적으로 궤적의 시작은 카메라

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;             // 사용할 점을 2개로 변경
        lineRenderer.enabled = false;               // line Renderer 비활성화
        lineRenderer.material.color = Color.yellow;

        effectSpawner = GameObject.Find("Bullet Effect Spawner").GetComponent<EffectSpawner>();
    }

    private void Start()
    {
        crosshairInstance = Instantiate(crosshairPrefabs);
    }

    // 작성자 : 강사님, 박규탁
    // 기  능 : 강사님이 작성하신 기본 코드에 LayerMask를 통해서 적을 범용적으로(쉽게 에디터에서 수정가능하게) 공격하는 함수
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

                // 총알 궤적 그리기
                StartCoroutine(DrawLineEffect(fireTransform.position, hitResult.point, 0.03f));

                // Effect Spawner에서 생성
                effectSpawner.SpawnEffect(hitResult.point, hitResult.normal);
            }
        }
    }

    // 작성자 : 박규탁
    // 기  능 : 총알 궤적을 time 만큼 인게임에서 그려냄
    private IEnumerator DrawLineEffect(Vector3 startPosition, Vector3 endPosition, float time)
    {
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);

        lineRenderer.enabled = true;        // 라인 렌더러를 활성화하여 탄알 궤적을 그림

        yield return new WaitForSeconds(time);
       
        lineRenderer.enabled = false;       // 라인 렌더러를 비활성화하여 탄알 궤적을 지움
    }
    #endregion
}
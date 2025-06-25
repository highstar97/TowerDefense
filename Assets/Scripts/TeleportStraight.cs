using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TeleportStraight : MonoBehaviour
{
    public Transform teleportCircleUI;
    Vector3 originScale = Vector3.one * 0.02f;
    LineRenderer lineRenderer;
    public bool isWarp = false;
    public float warpTime = 0.1f;
    public PostProcessVolume post;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if(ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            lineRenderer.enabled = true;
        }
        else if(ARAVRInput.GetUp(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            lineRenderer.enabled = false;
            if(teleportCircleUI.gameObject.activeSelf)
            {
                if(isWarp == false)
                {
                    GetComponent<CharacterController>().enabled = false;
                    transform.position = teleportCircleUI.position + Vector3.up;
                    GetComponent<CharacterController>().enabled = true;
                }
                else
                {
                    StartCoroutine(Warp());
                }                
            }
            teleportCircleUI.gameObject.SetActive(false);
        }

        if (ARAVRInput.Get(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            Ray ray = new Ray(ARAVRInput.LHandPosition, ARAVRInput.LHandDirection);
            RaycastHit hitResult;
            int layer = 1 << LayerMask.NameToLayer("Terrain");
            if (Physics.Raycast(ray, out hitResult, 200, layer))
            {
                lineRenderer.SetPosition(0, ray.origin);
                lineRenderer.SetPosition(1, hitResult.point);
                teleportCircleUI.gameObject.SetActive(true);
                teleportCircleUI.position = hitResult.point;
                teleportCircleUI.forward = hitResult.normal;
                teleportCircleUI.localScale = originScale * Mathf.Max(1, hitResult.distance);
            }
            else
            {
                lineRenderer.SetPosition(0, ray.origin);
                lineRenderer.SetPosition(1, ray.origin + ARAVRInput.LHandDirection * 200);
                teleportCircleUI.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator Warp()
    {
        MotionBlur blur;
        Vector3 pos = transform.position;
        Vector3 targetPos = teleportCircleUI.transform.position + Vector3.up;
        float currentTime = 0;
        post.profile.TryGetSettings<MotionBlur>(out blur);
        blur.active = true;
        GetComponent<CharacterController>().enabled = false;
        while(currentTime < warpTime)
        {
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(pos, targetPos, currentTime / warpTime);
            yield return null;
        }
        transform.position = teleportCircleUI.position + Vector3.up;
        GetComponent<CharacterController>().enabled = true;
        blur.active = false;
    }
}
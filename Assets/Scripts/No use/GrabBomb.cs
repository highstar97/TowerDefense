using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabBomb : MonoBehaviour
{
    public bool isRemoteGrab = true;  // 원거리에서 물체를 잡는 기능 활성화 여부
    public float grabRange = 0.2f;
    public LayerMask grabbedLayer;

    bool isGrabbing = false;
    float ThrowPower = 10;
    float remoteGrabDistance = 20; // 원거리에서 물체를 잡을 수 있는 거리
    Vector3 prevPosition;
    Quaternion prevRotation;
    GameObject grabbedObject;

    private void Start()
    {
       
    }

    private void Update()
    {
        if (isGrabbing) TryUnGrab();
        else TryGrab();
    }

    void TryGrab()
    {
        if(ARAVRInput.GetDown(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch))
        {
            if(isRemoteGrab)
            {
                Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);
                RaycastHit hitResult;
                if(Physics.SphereCast(ray, 0.5f, out hitResult, remoteGrabDistance, grabbedLayer))
                {
                    isGrabbing = true;
                    grabbedObject = hitResult.transform.gameObject;
                    StartCoroutine(GrabbingAnimator());
                }
                return;
            }

            Collider[] hitObjects = Physics.OverlapSphere(ARAVRInput.RHandPosition, grabRange, grabbedLayer);

            int closest = 0;
            Vector3 closestPos;
            float closetDistance = float.MaxValue;

            for (int i = 0; i < hitObjects.Length; ++i)
            {
                Vector3 nextPos = hitObjects[i].transform.position;
                float nextDistance = Vector3.Distance(nextPos, ARAVRInput.RHandPosition);

                if(nextDistance < closetDistance)
                {
                    closest = i;
                    closestPos = hitObjects[closest].transform.position;
                    closetDistance = Vector3.Distance(closestPos, ARAVRInput.RHandPosition);
                }
            }

            if (hitObjects.Length > 0)
            {
                isGrabbing = true;
                grabbedObject = hitObjects[closest].gameObject;
                grabbedObject.transform.parent = ARAVRInput.RHand.transform;
                grabbedObject.GetComponent<Rigidbody>().isKinematic = true;

                prevPosition = ARAVRInput.RHandPosition;
            }
        }
    }

    void TryUnGrab()
    {
        Vector3 throwDirection = ARAVRInput.RHandPosition - prevPosition;
        // prevPos = ARAVRInput.RHandPosition;

        if(ARAVRInput.GetUp(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch))
        {
            isGrabbing = false;
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            grabbedObject.transform.parent = null;
            // grabbedObject.GetComponent<Rigidbody>().velocity = throwDirection * ThrowPower;
            grabbedObject.GetComponent<Rigidbody>().velocity = ARAVRInput.RHandDirection * ThrowPower;
            grabbedObject = null;
        }
    }

    IEnumerator GrabbingAnimator()
    {
        grabbedObject.GetComponent<Rigidbody>().isKinematic = true; // 물리 기능 정지
        prevPosition = ARAVRInput.RHandPosition;
        prevRotation = ARAVRInput.RHand.rotation;
        Vector3 startLocation = grabbedObject.transform.position;
        Vector3 targetLocation = ARAVRInput.RHandPosition + ARAVRInput.RHandDirection * 0.1f;

        float currentTime = 0f;
        float finishTime = 0.2f;
        float elapsedRate = currentTime / finishTime;
        while (elapsedRate < 1)
        {
            currentTime += Time.deltaTime;
            elapsedRate = currentTime / finishTime;
            grabbedObject.transform.position = Vector3.Lerp(startLocation, targetLocation, elapsedRate);
            yield return null;
        }

        grabbedObject.transform.position = targetLocation;
        grabbedObject.transform.parent = ARAVRInput.RHand;
    }
}
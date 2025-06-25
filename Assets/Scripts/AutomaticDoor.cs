using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    #region Variables
    private bool isOpening = false;

    [SerializeField]
    private int numOfTriggeredObjects = 0;

    private float elapsedTime = 0.0f;       // elapsed time to open/close door (for use Coroutine)

    private float duration = 1.0f;          // duration time of open/close door

    private float distanceOfMove = 3.5f;            // distance of moving door

    private float timePerLoop = 0.05f;               // time per loop (for use Coroutine)

    private Vector3 prevLocalPositionOfLeftDoor;

    private Vector3 targetLocalPositionOfLeftDoor;

    private Vector3 prevLocalPositionOfRightDoor;

    private Vector3 targetLocalPositionOfRightDoor;

    [SerializeField]
    private LayerMask triggerLayerMasks;

    private GameObject leftDoor;

    private GameObject rightDoor;

    private Coroutine currentDoorCoroutine;
    #endregion

    #region Unity Functions
    private void Start()
    {
        leftDoor = transform.Find("Left Door").gameObject;
        prevLocalPositionOfLeftDoor = leftDoor.transform.localPosition;
        targetLocalPositionOfLeftDoor = prevLocalPositionOfLeftDoor - new Vector3(distanceOfMove, 0, 0);

        rightDoor = transform.Find("Right Door").gameObject;
        prevLocalPositionOfRightDoor = rightDoor.transform.localPosition;
        targetLocalPositionOfRightDoor = prevLocalPositionOfRightDoor + new Vector3(distanceOfMove, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((triggerLayerMasks & (1 << other.gameObject.layer)) != 0)
        {
            ++numOfTriggeredObjects;

            if (!isOpening)
            {
                if (currentDoorCoroutine != null)
                {
                    StopCoroutine(currentDoorCoroutine);
                }
                currentDoorCoroutine = StartCoroutine(OpenDoor());
                isOpening = true;
            }            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((triggerLayerMasks & (1 << other.gameObject.layer)) != 0)
        {
            --numOfTriggeredObjects;
            if (isOpening && numOfTriggeredObjects == 0)
            {
                if (currentDoorCoroutine != null)
                {
                    StopCoroutine(currentDoorCoroutine);
                }
                currentDoorCoroutine = StartCoroutine(CloseDoor());
                isOpening = false;
            }
        }
    }
    #endregion

    #region User Functions
    private IEnumerator OpenDoor()
    {
        elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            leftDoor.transform.localPosition = Vector3.Lerp(leftDoor.transform.localPosition, targetLocalPositionOfLeftDoor, elapsedTime / duration);
            rightDoor.transform.localPosition = Vector3.Lerp(rightDoor.transform.localPosition, targetLocalPositionOfRightDoor, elapsedTime / duration);
            yield return new WaitForSeconds(timePerLoop);
            elapsedTime += timePerLoop;
        }
    }

    private IEnumerator CloseDoor()
    {
        elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            leftDoor.transform.localPosition = Vector3.Lerp(leftDoor.transform.localPosition, prevLocalPositionOfLeftDoor, elapsedTime / duration);
            rightDoor.transform.localPosition = Vector3.Lerp(rightDoor.transform.localPosition, prevLocalPositionOfRightDoor, elapsedTime / duration);
            yield return new WaitForSeconds(timePerLoop);
            elapsedTime += timePerLoop;
        }
    }
    #endregion
}
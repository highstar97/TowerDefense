#define PC
//#define Oculus
//#define Vive

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if Vive
using Valve.VR;
using UnityEngine.XR;
#endif

public static class ARAVRInput
{
#if PC
    public enum ButtonTarget
    {
        Fire1,
        Fire2,
        Fire3,
        Jump,
    }
#elif Vive
    public enum ButtonTarget
    {
        Teleport,
        InteractUI,
        GrabGrip,
        Jump,
    }
#endif

    public enum Button
    {
#if PC
        One = ButtonTarget.Fire1,
        Two = ButtonTarget.Jump,
        Thumbstick = ButtonTarget.Fire1,
        IndexTrigger = ButtonTarget.Fire3,
        HandTrigger = ButtonTarget.Fire2
#elif Oculus
        One = OVRInput.Button.One,
        Two = OVRInput.Button.Two,
        Thumbstick = OVRInput.Button.PrimaryThumbstick,
        IndexTrigger = OVRInput.Button.PrimaryIndexTrigger,
        HandTrigger = OVRInput.Button.PrimaryHandTrigger
#elif Vive
        One = ButtonTarget.InteractUI,
        Two = ButtonTarget.Jump,
        Thumbstick = ButtonTarget.Teleport,
        IndexTrigger = ButtonTarget.InteractUI,
        HandTrigger = ButtonTarget.GrabGrip,
#endif
    }

    public enum Controller
    {
#if PC
        LTouch,
        RTouch
#elif Oculus
        LTouch = OVRInput.Controller.LTouch,
        RTouch = OVRInput.Controller.RTouch
#elif Vive
        LTouch = SteamVR_Input_Sources.LeftHand,
        RTouch = SteamVR_Input_Sources.RightHand,
#endif
    }

    static Transform lHand;
    public static Transform LHand
    {
        get
        {
            if (lHand == null)
            {
#if PC
                GameObject handObj = new GameObject("LHand");

                lHand = handObj.transform;
                lHand.parent = Camera.main.transform;
#elif Oculus
                lHand = GameObject.Find("LeftControllerAnchor").transform;
#elif Vive
                lHand = GameObject.Find("Controller(left)").transform;
#endif
            }
            return lHand;
        }
    }

    static Transform rHand;
    public static Transform RHand
    {
        get
        {
            if (rHand == null)
            {
#if PC
                GameObject handObj = new GameObject("RHand");
                rHand = handObj.transform;
                rHand.parent = Camera.main.transform;
#elif Oculus
                rHand = GameObject.Find("RightControllerAnchor").transform;
#endif
            }
            return rHand;
        }
    }

    public static Vector3 RHandPosition
    {
        get
        {
#if PC
            Vector3 pos = Input.mousePosition;
            pos.z = 0.7f;
            pos = Camera.main.ScreenToWorldPoint(pos);

            RHand.position = pos;
            return pos;
#elif Oculus
            Vector3 pos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            pos = GetTransform().TransformPoint(pos);
            return pos;
#elif Vive
            Vector3 pos = RHand.position;
            return pos;
#endif
        }
    }

    public static Vector3 RHandDirection
    {
        get
        {
#if PC
            Vector3 direction = RHandPosition - Camera.main.transform.position;

            RHand.forward = direction;
            return direction;
#elif Oculus

            Vector3 direction = OVRInput.GetLocalControllerRotation(OVRInput.Controller.
            RTouch) * Vector3.forward;
            direction = GetTransform().TransformDirection(direction);

            return direction;
#elif Vive
            Vector3 direction = RHand.forward;
            return direction;
#endif
        }
    }

    public static Vector3 LHandPosition
    {
        get
        {
#if PC
            return RHandPosition;
#elif Oculus
            Vector3 pos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            pos = GetTransform().TransformPoint(pos);
            return pos;
#elif Vive
            Vector3 pos = LHand.position;
            return pos;
#endif
        }
    }

    public static Vector3 LHandDirection
    {
        get
        {
#if PC
            return RHandDirection;
#elif Oculus
            Vector3 direction = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch) * Vector3.forward;
            direction = GetTransform().TransformDirection(direction);

            return direction;
#elif Vive
            Vector3 direction = LHand.forward;
            return direction;
#endif
        }
    }

#if Oculus || Vive
    static Transform rootTransform;
#endif

#if Oculus
    static Transform GetTransform()
    {
        if (rootTransform == null)
        {
            rootTransform = GameObject.Find("TrackingSpace").transform;
        }
        return rootTransform;
    }
#elif Vive
    static Transform GetTransform()
    {
        if (rootTransform == null)
        {

            rootTransform = GameObject.Find("[CameraRig]").transform;
        }
        return rootTransform;
    }
#endif

    public static bool Get(Button virtualMask, Controller hand = Controller.RTouch)
    {
#if PC
        return Input.GetButton(((ButtonTarget)virtualMask).ToString());
#elif Oculus
        return OVRInput.Get((OVRInput.Button)virtualMask, (OVRInput.Controller)hand);
#elif Vive
        string button = ((ButtonTarget)virtualMask).ToString();
        return SteamVR_Input.GetState(button, (SteamVR_Input_Sources)(hand));
#endif
    }

    public static bool GetDown(Button virtualMask, Controller hand = Controller.RTouch)
    {
#if PC
        return Input.GetButtonDown(((ButtonTarget)virtualMask).ToString());
#elif Oculus
        return OVRInput.GetDown((OVRInput.Button)virtualMask, (OVRInput.Controller)hand);
#elif Vive
        string button = ((ButtonTarget)virtualMask).ToString();
        return SteamVR_Input.GetStateDown(button, (SteamVR_Input_Sources)(hand));
#endif
    }

    public static bool GetUp(Button virtualMask, Controller hand = Controller.RTouch)
    {
#if PC
        return Input.GetButtonUp(((ButtonTarget)virtualMask).ToString());
#elif Oculus
        return OVRInput.GetUp((OVRInput.Button)virtualMask, (OVRInput.Controller)hand);
#elif Vive
        string button = ((ButtonTarget)virtualMask).ToString();
        return SteamVR_Input.GetStateUp(button, (SteamVR_Input_Sources)(hand));
#endif
    }

    public static float GetAxis(string axis, Controller hand = Controller.LTouch)
    {
#if PC
        return Input.GetAxis(axis);
#elif Oculus
        if (axis == "Horizontal")
        {
            return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, (OVRInput.Controller)hand).x;
        }
        else
        {
            return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, (OVRInput.Controller)hand).y;
        }
#elif Vive
        if (axis == "Horizontal")
        {
            return SteamVR_Input.GetVector2("TouchPad", (SteamVR_Input_Sources)(hand)).x;
        }
else
        {
            return SteamVR_Input.GetVector2("TouchPad", (SteamVR_Input_Sources)(hand)).y;
        }
#endif
    }


    public static void PlayVibration(Controller hand)
    {
#if Oculus
        PlayVibration(0.06f, 1, 1, hand);
#elif Vive
        PlayVibration(0.06f, 160, 0.5f, hand);
#endif
    }

    public static void PlayVibration(float duration, float frequency, float amplitude, Controller hand)
    {
#if Oculus
        if (CoroutineInstance.coroutineInstance == null)
        {
            GameObject coroutineObj = new GameObject("CoroutineInstance");
            coroutineObj.AddComponent<CoroutineInstance>();
        }

        // �̹� �÷������� ���� �ڷ�ƾ�� ����
        CoroutineInstance.coroutineInstance.StopAllCoroutines();
        CoroutineInstance.coroutineInstance.StartCoroutine(VibrationCoroutine(duration, frequency, amplitude, hand));
#elif Vive
        SteamVR_Actions._default.Haptic.Execute(0, duration, frequency, amplitude, (SteamVR_Input_Sources)hand);
#endif
    }


    public static void Recenter()
    {
#if Oculus
        OVRManager.display.RecenterPose();
#elif Vive
        List<XRInputSubsystem> subsystems = new List<XRInputSubsystem>();
        SubsystemManager.GetInstances<XRInputSubsystem>(subsystems);
        for (int i = 0; i < subsystems.Count; i++)
        {
            subsystems[i].TrySetTrackingOriginMode(TrackingOriginModeFlags.
            TrackingReference);
            subsystems[i].TryRecenter();
        }
#endif
    }

    public static void Recenter(Transform target, Vector3 direction)
    {
        target.forward = target.rotation * direction;
    }


#if PC
    static Vector3 originScale = Vector3.one * 0.02f;
#else
    static Vector3 originScale = Vector3.one * 0.005f;
#endif

    public static void DrawCrosshair(Transform crosshair, bool isHand = true, Controller hand = Controller.RTouch)
    {

        Ray ray;
        if (isHand)
        {
#if PC
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#else
            if (hand == Controller.RTouch)
            {
                ray = new Ray(RHandPosition, RHandDirection);
            }
            else
            {
                ray = new Ray(LHandPosition, LHandDirection);
            }
#endif
        }
        else
        {
            ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        }

        Plane plane = new Plane(Vector3.up, 0);
        float distance = 0;
        if (plane.Raycast(ray, out distance))
        {
            crosshair.position = ray.GetPoint(distance);
            crosshair.forward = -Camera.main.transform.forward;
            crosshair.localScale = originScale * Mathf.Max(1, distance);
        }
        else
        {
            crosshair.position = ray.origin + ray.direction * 100;
            crosshair.forward = -Camera.main.transform.forward;
            distance = (crosshair.position - ray.origin).magnitude;
            crosshair.localScale = originScale * Mathf.Max(1, distance);
        }
    }


#if Oculus
    static IEnumerator VibrationCoroutine(float duration, float frequency, float amplitude, Controller hand)
    {
        float currentTime = 0;

        while (currentTime < duration)
        {

            currentTime += Time.deltaTime;

            OVRInput.SetControllerVibration(frequency, amplitude, (OVRInput.Controller)
            hand);
            yield return null;
        }
        OVRInput.SetControllerVibration(0, 0, (OVRInput.Controller)hand);
    }
#endif
}

class CoroutineInstance : MonoBehaviour
{
    public static CoroutineInstance coroutineInstance = null;
    private void Awake()
    {
        if (coroutineInstance == null)
        {
            coroutineInstance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
}


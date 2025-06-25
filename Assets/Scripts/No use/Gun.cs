using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Gun : MonoBehaviour
{
    #region Variables
    public Transform bulletImpact;
    public Transform crosshair;
    
    ParticleSystem bulletEffetct;
    AudioSource bulletAudio;

    #endregion
    
    #region Unity Functions;

    #endregion

    private void Start()
    {
        bulletEffetct = bulletImpact.GetComponent<ParticleSystem>();
        bulletAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        ARAVRInput.DrawCrosshair(crosshair);

        if(ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.RTouch))
        {
            ARAVRInput.PlayVibration(ARAVRInput.Controller.RTouch); // 컨트롤러 진동

            Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);
            RaycastHit hitResult;
            int playerLayer = 1 << LayerMask.NameToLayer("Player");
            int towerLayer = 1 << LayerMask.NameToLayer("Tower");
            int layerMask = playerLayer | towerLayer;
            if (Physics.Raycast(ray, out hitResult, 200, ~layerMask))
            {
                if(hitResult.transform.name.Contains("Drone"))
                {
                    DroneAI drone = hitResult.transform.GetComponent<DroneAI>();
                    if (drone)
                    {
                        drone.OnDamageProcess();
                    }
                }
                bulletEffetct.Stop();
                bulletEffetct.Play();
                bulletImpact.forward = hitResult.normal;
                bulletImpact.position = hitResult.point;
            }
        }
    }
}
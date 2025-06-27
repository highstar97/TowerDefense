using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

//총을 안정적으로 잡게 하는 scripts
public class IKScripts : MonoBehaviour
{
    Animator anim;
    public Transform rightHand;
    public Transform leftHand;
    public Transform gunPivot;
    private void OnAnimatorIK(int layerIndex)
    {
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        anim.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);
        anim.SetIKRotation(AvatarIKGoal.RightHand, rightHand.rotation);

        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position);
        anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHand.rotation);

    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

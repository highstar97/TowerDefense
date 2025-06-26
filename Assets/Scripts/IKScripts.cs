using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

//총과 수류탄을 더 자연스럽게 잡기위해 IKscripts 작성
public class IKScripts : MonoBehaviour
{
    Animator anim;
    //public Transform leftHand; // 왼손의 위치
    public Transform rightHand; // 오른손의 위치
    public Transform gunPivot;//총의 위치
    private void OnAnimatorIK(int layerIndex)
    {
        
        // gunPivot.position = anim.GetIKHintPosition(AvatarIKHint.RightElbow);
       // print("레이어 인덱스: " + layerIndex);
        //anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        //anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        //anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position);
        //anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHand.rotation);

        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        anim.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);
        anim.SetIKRotation(AvatarIKGoal.RightHand, rightHand.rotation);

        
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

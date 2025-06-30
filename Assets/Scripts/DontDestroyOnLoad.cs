using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    // 작성자 : 박규탁
    // 기  능 : 해당 스크립트가 장착된 Object들은 씬 이동간에 제거되지 않게 함.
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
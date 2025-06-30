using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 작성자 : 박규탁
// 기  능 : 데미지 받을 수 있는 오브젝트들은 이 인터페이스 상속
public interface ITakeDamageable
{
    public void TakeDamage(int damageAmount = 1);
}
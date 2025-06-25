using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Player : MonoBehaviour, ITakeDamageable
{
    #region Variables
    public static Player Instance;

    [SerializeField]
    private int maxHp = 10;                      // max hp

    private int currentHp;                      // current hp

    private float durationOfDamageUI = 0.5f;

    [SerializeField]
    private GameObject damageUI;

    [SerializeField]
    private Image damageImage;

    [SerializeField]
    private Text textHealthPoints;
    #endregion

    #region Unity Functions
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; //½Ì±ÛÅæ °´Ã¼ °ª ÇÒ´ç
        }
    }

    private void Start()
    {
        currentHp = maxHp;
        float z = Camera.main.nearClipPlane + 0.5f;

        damageUI.transform.SetParent(Camera.main.transform);
        damageUI.transform.localPosition = new Vector3(0, 0, z);
        damageImage.enabled = false;

        textHealthPoints.text = currentHp.ToString();
    }
    #endregion

    #region User Functions
    public void TakeDamage(int damageAmount = 1)
    {
        int availableDamage = Mathf.Min(currentHp, damageAmount);

        currentHp -= availableDamage;
        textHealthPoints.text = currentHp.ToString();

        StopAllCoroutines();
        StartCoroutine(DamageEvent());

        if (currentHp <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator DamageEvent()
    {
        damageImage.enabled = true;
        yield return new WaitForSeconds(durationOfDamageUI);
        damageImage.enabled = false;
    }
    #endregion
}
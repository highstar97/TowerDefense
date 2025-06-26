using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 10;

    public float moveSpeed = 5f;
    private bool isCollected = false; //코인 수집

    private Transform player; 

  
    private void Update()
    {
        if (isCollected && player != null)
        {
            Debug.Log(" // ");
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, player.position) < 0.6f)
            {
                CoinManager.Instance.AddCoin(coinValue);
                Destroy(gameObject);
            }
        }

    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            isCollected = true;
        }
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    #region Variables
    public int damage = 3;                      // Explosion Damage;
    public float range = 3;                     // Explosion Range
    public LayerMask targetLayerMasks;          // Target Layer Masks;

    [SerializeField]
    private GameObject explosionEffectPrefab;   // Explosion Effect Prefab
    #endregion

    #region Unity Functions
    private void OnCollisionEnter(Collision collision)
    {
        Collider[] targets = Physics.OverlapSphere(this.transform.position, this.range, targetLayerMasks);
        foreach(Collider target in targets)
        {
            float distance = Vector3.Distance(this.transform.position, target.transform.position);

            ITakeDamageable damageable = target.GetComponent<ITakeDamageable>();
            if(damageable != null)
            {
                damageable.TakeDamage(Mathf.Min((int)(damage / distance), damage));
            }
        }

        GameObject explosionEffect = Instantiate(explosionEffectPrefab);
        explosionEffect.transform.position = this.transform.position;
        explosionEffect.GetComponent<ParticleSystem>().Play();
        explosionEffect.GetComponent<AudioSource>().Play();
        
        Destroy(this.gameObject);
    }
    #endregion
}
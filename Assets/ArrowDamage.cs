using UnityEngine;

public class ArrowDamage : MonoBehaviour
{
    public int damage = 10;
    public float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit object: " + other.name);

        EnemyController enemy = other.GetComponentInParent<EnemyController>();

        if (enemy != null)
        {
            Debug.Log("Enemy found!");
            enemy.TakeDamage(damage);
        }
        else
        {
            Debug.Log("Enemy NOT found");
        }

        Destroy(gameObject);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    EnemyController enemy = other.GetComponentInParent<EnemyController>();

    //    if (enemy != null)
    //    {
    //        enemy.TakeDamage(damage);
    //    }

    //    Destroy(gameObject);
    //}
}
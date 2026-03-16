using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    public PlayerController player;
    public Animator animator;

    public int damage = 20;

    private bool canHit = false;

    void Start()
    {
        player = GetComponentInParent<PlayerController>();
        animator = player.animator;
    }

    void Update()
    {
        //Œ•‘•”õ‚Ì‚İ“®ì
        if (player.currentWeapon != WeaponType.Sword)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    public void Attack()
    {
        animator.SetTrigger("SwAt");
    }

    public void EnableHit()
    {
        canHit = true;
    }

    public void DisableHit()
    {
        canHit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canHit) return;

        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponentInParent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
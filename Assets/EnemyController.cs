using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class EnemyController : MonoBehaviour
{
    public Transform player;

    public float detectDistance = 10f;
    public float stopDistance = 2f;
    public float moveSpeed = 2.5f;
    public float rotateSpeed = 7f;

    public float maxhp_ = 100;
    private float hp_;

    public Image hp_fillImage;

    public float attackDistance = 1.8f;
    public float attackCooldown = 2f;
    public float attackDamage = 10f;
    public Collider attackCollider;

    float attackTimer = 0f;
    bool isDead = false;

    Rigidbody rb;
    Animator animator;

    bool isMoving;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        hp_ = maxhp_;
        UpdateHPBar();


        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        if (!player)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p) player = p.transform;
        }
    }
    public void AttackStart()
    {
        attackCollider.enabled = true;
    }

    public void AttackEnd()
    {
        attackCollider.enabled = false;
    }
    public void TakeDamage(float damage)
    {
        hp_ -= damage;

        if (hp_ < 0)
            hp_ = 0;

        UpdateHPBar();

        if (hp_ <= 0)
        {
            Die();
        }
    }

    void UpdateHPBar()
    {
        if (hp_fillImage != null)
        {
            hp_fillImage.fillAmount = hp_ / maxhp_;
        }
    }

    void Die()
    {
        // 死亡アニメーション再生
        animator.SetTrigger("Die");
        // コライダーとRigidbodyを無効化して、物理的な干渉を防止
        Collider col = GetComponent<Collider>();
        if (col) col.enabled = false;
        rb.isKinematic = true;
        isDead = true;
        // 一定時間後にオブジェクトを破壊
        Destroy(gameObject, 3f);
        //少し待ってからリザルトに遷移
        SceneManager.LoadScene("ResultScene");
    }
    void FixedUpdate()
    {
        if (isDead) return;

        if (!player)
        {
            Stop();
            return;
        }

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist > detectDistance)
        {
            Stop();
            return;
        }

        RotateToPlayer();

        if (dist > attackDistance)
        {
            MoveForward();
        }
        else
        {
            Stop();
            TryAttack();
        }
    }

    void Update()
    {
        animator.SetFloat("Speed", isMoving ? 1f : 0f);
    }

    void TryAttack()
    {
        attackTimer += Time.fixedDeltaTime;

        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0f;

            animator.SetTrigger("Attack");

            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.TakeDamage(attackDamage);
            }
        }
    }

    void RotateToPlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0;

        if (dir.sqrMagnitude < 0.001f) return;

        Quaternion target = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            target,
            rotateSpeed * Time.fixedDeltaTime
        );
    }

    void MoveForward()
    {
        isMoving = true;

        Vector3 move = transform.forward * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }

    void Stop()
    {
        isMoving = false;
        rb.linearVelocity = Vector3.zero;
    }
}
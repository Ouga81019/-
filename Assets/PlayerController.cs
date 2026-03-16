using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Animator animator;

    private Rigidbody rb;
    private Transform cam;

    public float rayDistance = 0.6f;

    private Vector3 inputMove;
    private Vector3 lastMoveDir = Vector3.forward;

    public float maxhp_ = 100;
    private float hp_;

    public Image hpFill;

    public bool isAiming = false;

    public GameObject WoodenBow;
    public GameObject LongSword;

    public PlayerSword swordHit;

    private bool isAttacking = false;

    public WeaponType currentWeapon = WeaponType.Bow;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        hp_ = maxhp_;

        cam = Camera.main.transform;

        SwitchWeapon(currentWeapon);

        UpdateHPBar();
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

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        inputMove = Vector3.zero;

        //武器切り替え 
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SwitchWeapon(WeaponType.Bow);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SwitchWeapon(WeaponType.Sword);

        if (Input.GetMouseButtonDown(1))
            animator.SetTrigger("Rolling");

        Vector3 camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 camRight = Vector3.Scale(cam.right, new Vector3(1, 0, 1)).normalized;

        inputMove = (camForward * v + camRight * h);

        animator.SetFloat("Speed", inputMove.magnitude);

        if (!isAiming && inputMove.sqrMagnitude > 0.001f)
        {
            lastMoveDir = inputMove.normalized;

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(lastMoveDir),
                Time.deltaTime * 7f
            );
        }

        if (currentWeapon == WeaponType.Bow)
        {
            isAiming = Input.GetMouseButton(1);
        }
        else
        {
            isAiming = false;
        }
    }

    void UpdateHPBar()
    {
        if (hpFill != null)
        {
            float ratio = hp_ / maxhp_;
            hpFill.transform.localScale = new Vector3(ratio, 1f, 1f);
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
        // 一定時間後にオブジェクトを破壊
        Destroy(gameObject, 3f);
        //少し待ってからリザルトに遷移
        SceneManager.LoadScene("ResultScene");
    }
    void FixedUpdate()
    {
        if (isAiming || isAttacking) return;
        if (inputMove.sqrMagnitude < 0.001f) return;

        bool wallInFront = Physics.Raycast(
            transform.position + Vector3.up * 0.5f,
            transform.forward,
            rayDistance
        );

        if (!wallInFront)
        {
            Vector3 newPos = rb.position + lastMoveDir * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);
        }
    }
    public void EndAttack()
    {
        isAttacking = false;
    }

    public void RotatePlayerToAim(Vector3 aimPoint)
    {
        if (currentWeapon != WeaponType.Bow) return;

        Vector3 dir = aimPoint - transform.position;
        dir.y = 0;

        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    void SwitchWeapon(WeaponType type)
    {
        currentWeapon = type;

        WoodenBow.SetActive(type == WeaponType.Bow);
        LongSword.SetActive(type == WeaponType.Sword);

        animator.SetInteger("WeaponType", (int)type);
    }
}

public enum WeaponType
{
    Bow,
    Sword
}

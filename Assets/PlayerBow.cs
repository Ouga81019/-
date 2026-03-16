using UnityEngine;
using Unity.Cinemachine;

public class PlayerBow : MonoBehaviour
{
    public int damage = 10;

    private bool canHit = false;

    public PlayerController player;
    public Animator animator;

    [Header("Arrow")]
    public GameObject arrowPrefab;
    public Transform shootPoint;
    public float arrowSpeed = 30f;

    [Header("Camera")]
    public CinemachineCamera normalCam;
    public CinemachineCamera aimCam;

    bool canShoot = true;

    void Start()
    {
        player = GetComponentInParent<PlayerController>();

        //プレイヤーのAnimatorを取得する
        animator = player.animator;

        normalCam.Priority = 10;
        aimCam.Priority = 0;
    }

    void Update()
    {
        //弓装備時のみ動作
        if (player.currentWeapon != WeaponType.Bow)
            return;

        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            StartAiming();
        }
    }

    void StartAiming()
    {
        canShoot = false;

        player.isAiming = true;

        // カメラ切り替え
        aimCam.Priority = 20;
        normalCam.Priority = 10;

        animator.SetTrigger("ArSh");
    }

    public void EndShoot()
    {
        player.isAiming = false;
        canShoot = true;

        // カメラを戻す
        aimCam.Priority = 0;
        normalCam.Priority = 10;
    }

    Vector3 GetAimPoint()
    {
        Camera cam = Camera.main;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
            return hit.point;

        return ray.origin + ray.direction * 100f;
    }

    public void ShootArrow()
    {
        Vector3 aimPoint = GetAimPoint();

        player.RotatePlayerToAim(aimPoint);

        Vector3 dir = (aimPoint - shootPoint.position).normalized;

        GameObject arrow = Instantiate(
            arrowPrefab,
            shootPoint.position,
            Quaternion.LookRotation(dir)
        );

        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.linearVelocity = dir * arrowSpeed;

        //ダメージを渡す
        ArrowDamage arrowDamage = arrow.GetComponent<ArrowDamage>();
        if (arrowDamage != null)
        {
            arrowDamage.damage = damage;
        }
    }

}

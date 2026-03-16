using UnityEngine;

public class LockOnCamera : MonoBehaviour
{
    public Transform cameraPivot;      // カメラ回転の中心（Inspectorでセット）
    public float lockOnDistance = 15f;
    public float lockOnAngle = 45f;
    public Transform currentTarget;

    void Update()
    {
        if (currentTarget == null)
            SearchTarget();
        else
            FocusOnTarget();
    }

    void SearchTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject e in enemies)
        {
            Vector3 dir = e.transform.position - transform.position;
            float distance = dir.magnitude;
            if (distance > lockOnDistance) continue;

            float angle = Vector3.Angle(transform.forward, dir);
            if (angle < lockOnAngle)
            {
                currentTarget = e.transform;
                return;
            }
        }
    }

    void FocusOnTarget()
    {
        if (currentTarget == null) return;

        Vector3 lookDir = currentTarget.position - cameraPivot.position;
        Quaternion targetRot = Quaternion.LookRotation(lookDir);
        cameraPivot.rotation = Quaternion.Slerp(cameraPivot.rotation, targetRot, Time.deltaTime * 6f);

        // ターゲットが視界外になったら解除
        Vector3 toTarget = currentTarget.position - transform.position; // Player基準
        if (toTarget.magnitude > lockOnDistance || Vector3.Angle(transform.forward, toTarget) > lockOnAngle * 1.5f)
        {
            currentTarget = null;
        }
    }
}

using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Hedef Ayarları")]
    public Transform target;
    public Vector3 offset = new Vector3(0f, 0f, -10f);
    
    [Header("Hız Ayarları")]
    public float smoothTime = 0.25f;

    [Header("Sınır Ayarları")]
    public bool lockY = true;
    public bool useLimits = true;
    public float minX = -10f;
    public float maxX = 50f;
    
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;
        float targetX = target.position.x + offset.x;
        float targetY;
        if (lockY)
        {
            targetY = transform.position.y;
        }
        else
        {
            targetY = target.position.y + offset.y;
        }
        Vector3 targetPosition = new Vector3(targetX, targetY, target.position.z + offset.z);
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        if (useLimits)
        {
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minX, maxX);
        }
        transform.position = smoothedPosition;
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }
    void OnDrawGizmos()
    {
        if (useLimits)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(minX, -100, 0), new Vector3(minX, 100, 0));
            Gizmos.DrawLine(new Vector3(maxX, -100, 0), new Vector3(maxX, 100, 0));
        }
    }
}
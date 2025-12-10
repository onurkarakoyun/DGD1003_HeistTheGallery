using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Hedef Ayarları")]
    public Transform target;
    public Vector3 offset = new Vector3(0f, 0f, -10f);
    
    [Header("Hız Ayarları")]
    public float smoothTime = 0.25f;
    
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }
}

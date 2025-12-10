using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float rotateSpeed = 10f;
    public float leftAngle = -45f;
    public float rightAngle = 45f;
    private bool rotatingRight = true;

    void Update()
    {
        if(rotatingRight)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, rightAngle), rotateSpeed * Time.deltaTime);
            if (Mathf.Abs(transform.eulerAngles.z - (rightAngle < 0 ? 360 + rightAngle : rightAngle)) < 1f)
            {
                rotatingRight = false;
            }
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, leftAngle), rotateSpeed * Time.deltaTime);
            if (Mathf.Abs(transform.eulerAngles.z - (leftAngle < 0 ? 360 + leftAngle : leftAngle)) < 1f)
            {
                rotatingRight = true;
            }
        }
    }
}

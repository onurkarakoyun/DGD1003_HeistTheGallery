using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private SliderJoint2D joint;
    private JointMotor2D motor;

    void Start()
    {
        joint = GetComponent<SliderJoint2D>();
    }

    void Update()
    {
        if (!joint.useMotor || !joint.useLimits) return;
        if (joint.limitState == JointLimitState2D.UpperLimit && joint.motor.motorSpeed > 0)
        {
            ReverseMotor();
        }
        else if (joint.limitState == JointLimitState2D.LowerLimit && joint.motor.motorSpeed < 0)
        {
            ReverseMotor();
        }
    }

    void ReverseMotor()
    {
        motor = joint.motor;
        motor.motorSpeed = motor.motorSpeed * -1;
        joint.motor = motor;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            collision.transform.SetParent(null);
        }
    }
}

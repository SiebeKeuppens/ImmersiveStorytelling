using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelchairController : MonoBehaviour
{
    public WheelCollider leftWheel;   // Reference to the left wheel collider
    public WheelCollider rightWheel;  // Reference to the right wheel collider
    public float motorTorque = 500f;  // Speed of the wheels
    public float maxSteerAngle = 30f; // Turning angle

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Adjust Wheel Collider settings
        SetUpWheelColliders(leftWheel);
        SetUpWheelColliders(rightWheel);
    }

    void Update()
    {
        // Get player input for forward/backward movement
        float moveInput = Input.GetAxis("Vertical");
        // Get player input for turning (left/right)
        float turnInput = Input.GetAxis("Horizontal");

        // Apply forward force to both wheels
        leftWheel.motorTorque = moveInput * motorTorque;
        rightWheel.motorTorque = moveInput * motorTorque;

        // Steer by adjusting the torque difference between wheels
        leftWheel.motorTorque += turnInput * motorTorque;
        rightWheel.motorTorque -= turnInput * motorTorque;
    }

    void SetUpWheelColliders(WheelCollider wheel)
    {
        // Set wheel radius to match real-world size
        wheel.radius = 0.3f;  // Adjust based on your wheelchair model

        // Suspension settings
        JointSpring suspensionSpring = wheel.suspensionSpring;
        suspensionSpring.spring = 10000f;    // Stiff suspension for wheelchair
        suspensionSpring.damper = 2000f;     // Damping to prevent bouncing
        suspensionSpring.targetPosition = 0.5f;  // Mid-point of the suspension travel
        wheel.suspensionSpring = suspensionSpring;
        
        // Small suspension distance since wheelchairs don't have much travel
        wheel.suspensionDistance = 0.05f;

        // Wheel mass
        wheel.mass = 20f;  // Adjust this to reflect the weight of the wheel

        // Friction settings
        WheelFrictionCurve forwardFriction = wheel.forwardFriction;
        forwardFriction.extremumSlip = 0.4f;  // Slip value at which friction starts
        forwardFriction.extremumValue = 1f;   // Maximum friction force
        forwardFriction.asymptoteSlip = 0.8f; // Slip value at which friction ends
        forwardFriction.asymptoteValue = 0.5f; // Friction force when sliding
        forwardFriction.stiffness = 1.5f;     // Overall grip strength
        wheel.forwardFriction = forwardFriction;

        WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;
        sidewaysFriction.extremumSlip = 0.2f;
        sidewaysFriction.extremumValue = 1f;
        sidewaysFriction.asymptoteSlip = 0.5f;
        sidewaysFriction.asymptoteValue = 0.75f;
        sidewaysFriction.stiffness = 2f;      // Stronger grip when turning
        wheel.sidewaysFriction = sidewaysFriction;
    }
}


using System;
using UnityEngine;
using UnityEngine.UI;

public class ThrowController : MonoBehaviour {

    public Disc disc;
    public Transform heldTransform;
    //public AccelerationVisualizer accelerationVisualizer;
    //public Text debugText;
    
    public float minThrowVelocity = 2f;
    public float maxThrowVelocity = 35f;
    public float accelLimit = 80f;
    //public AnimationCurve throwCurve;

    [Range(0.0f, 1.0f)]
    public float velocitySmoothingFactor = 0.5f;
    [Range(0.0f, 1.0f)]
    public float forwardWeight = 0.5f;
    [Range(0.0f, 1.0f)]
    public float velocityLimitingFactor = 0.75f;

    //[Range(0.0f, 1.0f)]
    //public float forwardWeight = 0.5f;

    //public float spinImpulse = 2000f;

    private Vector3 lastDiscPosition;
    private Quaternion lastDiscRotation;
    private Quaternion rotationDifference;
    private Vector3 discVelocity;
    private float[] accelMag = new float[6];
    private Vector3 direction;
    private Vector3 targetPosition;

    public void StartThrow (GvrControllerInputDevice controller)
    {
        discVelocity = Vector3.zero;
        disc.Grab(heldTransform);
        Zeros(accelMag);
    }

    public void UpdateDiscVelocity(GvrControllerInputDevice controller)
    {

        float newAccelMag = Math.Abs(controller.Accel.magnitude - 9.8f);
        //debugText.text = "accel = " + newAccelMag;
        UpdateValues(accelMag, newAccelMag);

        Vector3 pos = disc.transform.position;
        Quaternion rot = disc.transform.rotation;

        Vector3 frameVelocity = (pos - lastDiscPosition) / Time.deltaTime;
        discVelocity = Vector3.Lerp(frameVelocity, discVelocity, velocitySmoothingFactor);

        rotationDifference = Quaternion.Inverse(lastDiscRotation) * rot;

        lastDiscPosition = pos;
        lastDiscRotation = rot;
    }

    public bool ReleaseDisc(GvrControllerInputDevice controller)
    {
        float max = Max(accelMag);
        //debugText.text = "accel = " + max;

        float forceStrength = Mathf.Lerp(0, maxThrowVelocity, max / accelLimit);
        Debug.Log("controllerAccel = " + accelMag + ", velocity = " + forceStrength);

        float distanceToTarget = Vector3.Distance(targetPosition, disc.transform.position);

        Vector3 lookDirection = Vector3.Scale(Camera.main.transform.forward, new Vector3(1f, 0f, 1f)).normalized;

        Vector3 targetDirection = (targetPosition - disc.transform.position).normalized;
        Vector3 targetDirectionInPlane = Vector3.Scale(targetDirection, new Vector3(1f, 0f, 1f)).normalized;

        float lookAngle = Vector3.Angle(targetDirectionInPlane, lookDirection);

        Vector3 forceDirection;

        float a;

        if(distanceToTarget < 20f)
        {
            a = distanceToTarget * 0.5f;
            forceDirection = lookAngle < 30f ? targetDirection : lookDirection;
            forceStrength = Mathf.Min(forceStrength, maxThrowVelocity * velocityLimitingFactor);
        } 
        else
        {
            if (distanceToTarget > 100f) a = 20f;
            else a = 15f;
            forceDirection = lookAngle < 30f ? targetDirectionInPlane : lookDirection;
        }

        forceDirection = Vector3.RotateTowards(forceDirection, Vector3.up, a * Mathf.Deg2Rad, 0f).normalized;


        if(forceStrength < minThrowVelocity) return false;

        //Vector3 forward = direction;
        //float angle = Vector3.Angle(forceDirection, forward);
        //float radiansChanged = forwardWeight * angle * Mathf.Deg2Rad;
        //forceDirection = Vector3.RotateTowards(forceDirection, forward, radiansChanged, float.MaxValue);

        Rigidbody rb = disc.GetComponent<Rigidbody>();

        Vector3 aVel = new Vector3(0, Mathf.Sign(rotationDifference.eulerAngles.y), 0);
        disc.ReleaseThrow(forceDirection * forceStrength, ForceMode.VelocityChange, aVel * 2000f, ForceMode.Impulse);
        //disc.ReleaseThrow(forceDirection * max, ForceMode.Acceleration, aVel * 2000f, ForceMode.Impulse);

        return true;
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private static float Max(float[] vals)
    {
        float max = float.MinValue;
        for (int i = 0; i < vals.Length; i++)
        {
            if (vals[i] > max) max = vals[i];
        }
        return max;
    }

    private static void Zeros(float[] vals)
    {
        for (int i = 0; i < vals.Length; i++)
        {
            vals[i] = 0f;
        }
    }

    private static void UpdateValues(float[] vals, float newValue)
    {
        for (int i = 0; i < vals.Length - 1; i++)
        {
            vals[i] = vals[i + 1];
        }
        vals[vals.Length - 1] = newValue;
    }
}

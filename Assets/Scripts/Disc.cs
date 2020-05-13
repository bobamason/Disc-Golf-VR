using System;
using UnityEngine;

public class Disc : MonoBehaviour
{

    public float stallVelocity = 0.2f;

    //density of air
    public float RHO = 1.23f;

    //lift coefficient at alpha = 0
    public float CL0 = 0.1f;

    //lift coefficient
    public float CLA = 1.2f;

    //drag coefficient at alpha = 0
    //public float CD0 = 0.08f;
    //drag coefficient
    //public float CDA = 2.72f;
    //public float ALPHA0 = -4f * Mathf.Deg2Rad;

    public float radius = 0.14f;

    private float area;

    [Range(0.0f, 10.0f)]
    public float liftMultiplier = 1.0f;
    
    private Vector3 throwStartPosition = new Vector3();
    private TrailRenderer trailRenderer;
    private Rigidbody rb;
    private float scalarVelocity;

    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        rb = GetComponent<Rigidbody>();
        rb.drag = 0.09f;
        rb.angularDrag = 0;

        area = Mathf.PI * (radius * radius);
    }

    void FixedUpdate()
    {
        trailRenderer.enabled = !rb.isKinematic;

        if (rb.isKinematic) return;

        Vector3 velocity = rb.velocity;
        scalarVelocity = velocity.magnitude;

        Vector3 inverseVelocity = rb.transform.InverseTransformDirection(velocity);

        float vY = Math.Abs(inverseVelocity.y);
        float d = 0.5f * RHO * area * vY;
        rb.drag = Mathf.Clamp(d, 0.06f, 2.0f);

        if (Vector3.Dot(rb.transform.up, Vector3.up) < 0.05f && scalarVelocity > stallVelocity) return;

        Vector3 movementDirection = velocity / scalarVelocity;
        Vector3 up = rb.transform.up;
        Vector3 sideDirection = Vector3.Cross(up, movementDirection).normalized;
        Vector3 forwardDirection = Vector3.Cross(sideDirection, up).normalized;

        //float length = 0.25f;
        //Vector3 p = rb.transform.position;
        //Debug.DrawLine(p, p + movementDirection, Color.white);
        //Debug.DrawLine(p, p + up * length, Color.green);
        //Debug.DrawLine(p, p + sideDirection * length, Color.red);
        //Debug.DrawLine(p, p + forwardDirection * length, Color.blue);

        float alpha = Mathf.Sin(forwardDirection.y);
        //Debug.Log("Angle Of Attack = " + alpha * Mathf.Rad2Deg);
        //float tilt = Mathf.Sin(sideDirection.y);
        //Debug.Log("Tilt = " + tilt * Mathf.Rad2Deg);

        float cl = CL0 + CLA * alpha;

        float v = Vector3.Scale(velocity, new Vector3(1, 0, 1)).magnitude;
        float fY = 0.5f * RHO * v * v * area * cl;
        //string log = "Lift = " + fY + "N, ";
        //log += "Angle = " + (alpha * Mathf.Rad2Deg) + "deg, ";
        //log += "Speed = " + scalarVelocity + "m/s, ";
        //log += "Position = " + rb.position + ", ";
        //log += "AirSpeed = " + Vector3.Project(velocity, forwardDirection).magnitude + "m/s, ";
        //log += "DistanceTraveled = " + Vector3.Distance(throwStartPosition, rb.position) + "m";
        //Debug.Log(log);
        rb.AddRelativeForce(Vector3.up * fY * liftMultiplier, ForceMode.Force);
    }

    public float GetScalarVelocity()
    {
        return scalarVelocity;
    }

    public void Grab(Transform heldTransform)
    {
        gameObject.SetActive(true);
        GetComponent<TrailRenderer>().Clear();
        GetComponent<Rigidbody>().isKinematic = true;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.SetParent(heldTransform, false);
    }

    public void ReleaseThrow(Vector3 force, ForceMode forceMode, Vector3 relativeTorque, ForceMode torqueForceMode)
    {
        transform.SetParent(null, true);
        rb.isKinematic = false;

        throwStartPosition = transform.position;

        rb.AddForce(force, forceMode);
        rb.AddRelativeTorque(relativeTorque, torqueForceMode);
    }

    public void SetColliderEnabled(bool enabled)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        if(colliders != null)
        {
            foreach (Collider c in colliders)
            {
                c.enabled = enabled;
            }
        }
        else if (GetComponent<Collider>() != null)
        {
            GetComponent<Collider>().enabled = enabled;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerationVisualizer : MonoBehaviour {

    private Vector3 EARTH_GRAVITY = new Vector3(0f, -9.81f, 0f);
    private LineRenderer lineRenderer;
    private GvrControllerInputDevice controller;
    private Vector3 gravity;
    private float value;

    // Use this for initialization
    void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        controller = GvrControllerInput.GetDevice(GvrControllerHand.Dominant);
	}
	
	void Update () {
        if (controller == null) return;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, Vector3.zero);
        float alpha = 0.9f;
        gravity = gravity * alpha + controller.Accel * (1f - alpha);
        Vector3 accel = controller.Accel - gravity;
        //lineRenderer.SetPosition(1, controller.Orientation * accel * -0.1f);
        lineRenderer.SetPosition(1, Vector3.forward * value);
        Color c = Color.Lerp(Color.red, Color.green, value * 0.1f);
        lineRenderer.startColor = c;
        lineRenderer.endColor = c;
    }

    public void SetValue(float value)
    {
        this.value = value;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CanvasBackground3D : MonoBehaviour {

    public RectTransform canvasTransform;

    void Start () {
        Rect rect = canvasTransform.rect;
        Vector3 s = canvasTransform.localScale;
        transform.localScale = new Vector3(rect.width * s.x, rect.height * s.y, 0.25f);
        transform.localRotation = canvasTransform.rotation;
        Vector3 offset = canvasTransform.rotation * new Vector3(0, 0, 0.1255f);
        transform.localPosition = canvasTransform.position + offset;
    }
}

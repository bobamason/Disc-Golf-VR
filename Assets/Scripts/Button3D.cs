using System.Collections;
using UnityEngine;

public class Button3D : MonoBehaviour
{

    public Color colorOver;
    public AnimationCurve curve;
    public Vector3 animationOffset;

    private Color colorDefault;
    private Vector3 positionDefault;
    private Vector3 positionStartAnimation;
    private bool initialized = false;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        if (!initialized)
        {
            colorDefault = GetComponent<Renderer>().material.color;
            positionDefault = transform.localPosition;
            positionStartAnimation = positionDefault + animationOffset;
            initialized = true;
        }
    }

    public void PointerOver()
    {
        Init();
        GetComponent<Renderer>().material.color = colorOver;
    }

    public void PointerExit()
    {
        Init();
        GetComponent<Renderer>().material.color = colorDefault;
    }

    public void PointerDown()
    {
    }

    public void PointerUp()
    {
    }

    public void ResetAnimation()
    {
        Init();
        transform.localPosition = positionStartAnimation;
    }

    public void Animate()
    {
        Init();
        transform.localPosition = positionStartAnimation;
        StopCoroutine("AnimateCoroutine");
        StartCoroutine("AnimateCoroutine");
    }

    private IEnumerator AnimateCoroutine()
    {
        for (int i = 0; i < 20; i++)
        {
            transform.localPosition = Vector3.Lerp(positionStartAnimation, positionDefault, curve.Evaluate(i / 20f));
            yield return new WaitForFixedUpdate();
        }
        transform.localPosition = positionDefault;
        yield return null;
    }
}

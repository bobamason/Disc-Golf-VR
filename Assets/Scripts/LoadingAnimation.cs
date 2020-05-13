using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingAnimation : MonoBehaviour {

    public GameObject[] cubes;
    public float delay = 0.5f;

    private float startScale;
    private int offset = 0;
    private float lastTime;

    void Start () {
        startScale = cubes[0].transform.localScale.x;
        lastTime = Time.time;
	}
	
	void Update () {
        int length = cubes.Length;
        float tStep = 1f / (float)length;

        for (int i = 0; i < length; i++)
        {
            float s = i == offset ? startScale * 2f : startScale;
            cubes[i].transform.localScale = new Vector3(s, s, s);
        }

        if(Time.time - lastTime > delay)
        {
            offset = (offset + 1) % length;
            lastTime = Time.time;
        }
	}
}

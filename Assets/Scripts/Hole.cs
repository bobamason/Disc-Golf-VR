using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour {

    public int par = 2;

    private HoleStart holeStart = null;

    public HoleStart HoleStart {
        get
        {
            if(holeStart == null)
                holeStart = transform.Find("HoleStart").gameObject.GetComponent<HoleStart>();
            return holeStart;
        }
    }

    private HoleTrigger holeTrigger = null;

    public HoleTrigger HoleTrigger
    {
        get
        {
            if(holeTrigger == null)
                holeTrigger = transform.Find("HoleTrigger").gameObject.GetComponent<HoleTrigger>();
            return holeTrigger;
        }
    }
}

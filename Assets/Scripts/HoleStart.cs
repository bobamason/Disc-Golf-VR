using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoleStart : MonoBehaviour {

    public Text holeNumberText;
    public Text rangeText;

    public void SetHoleNumber(int n)
    {
        holeNumberText.text = "" + n;
    }

    public void SetRange(int range, string units)
    {
        rangeText.text = "Range: " + range + " " + units;
    }
}

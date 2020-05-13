using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour {

    public Text holeText;
    public Text parText;
    public Text largeText;
    public Text extraLargeText;
    public Text bottomText;

    public void SetHole(int hole, int par)
    {
        holeText.text = "Hole " + (hole + 1);
        parText.text = "Par " + par;
    }

    public void SetLargeText(string text)
    {
        if(text == null)
        {
            largeText.enabled = false;
        } else
        {
            largeText.enabled = true;
            largeText.text = text;
        }
    }

    public void SetExtraLargeText(string text)
    {
        if(text == null)
        {
            extraLargeText.enabled = false;
        } else
        {
            extraLargeText.enabled = true;
            extraLargeText.text = text;
        }
    }

    public void SetBottomText(string text)
    {
        if(text == null)
        {
            bottomText.enabled = false;
        } else
        {
            bottomText.enabled = true;
            bottomText.text = text;
        }
    }
}

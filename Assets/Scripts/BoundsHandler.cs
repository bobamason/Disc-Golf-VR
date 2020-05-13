using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsHandler : MonoBehaviour
{

    public delegate void OutOfBoundsEventListener();
    public event OutOfBoundsEventListener OutOfBoundsListener;

    public void SetTriggerEnabled(bool enabled)
    {
        GetComponent<Collider>().enabled = enabled;
    }

    private void OnTriggerExit(Collider other)
    {

        Debug.Log("trigger enter " + other.tag);

        if (other.tag == "DiscCollider" || other.tag == "Disc")
        {
            if (OutOfBoundsListener != null)
            {
                OutOfBoundsListener();
            }
            else
                Debug.Log("There is no OutOfBoundsListener attached!");
        }
    }
}

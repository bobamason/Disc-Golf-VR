using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleTrigger : MonoBehaviour {

    public delegate void OnHoleComplete();
    public event OnHoleComplete OnHoleCompleteListener;

    public void SetTriggerEnabled(bool enabled)
    {
        GetComponent<Collider>().enabled = enabled;
    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("trigger enter " + other.tag);
        
        if(other.tag == "DiscCollider" || other.tag == "Disc")
        {
            if (OnHoleCompleteListener != null)
            {
                AudioSource audio = GetComponent<AudioSource>();
                if (audio != null) audio.Play();
                OnHoleCompleteListener();
                SetTriggerEnabled(false);
            }
            else
                Debug.Log("There is no OnHoleCompleteListener attached!");
        }
    }
}

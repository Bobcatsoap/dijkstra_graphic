using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTrigger : MonoBehaviour
{
    public bool visiable;

    private void Awake()
    {
        visiable = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("barrier"))
            visiable = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("barrier") && visiable)
            visiable = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("barrier"))
            visiable = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTrigger : MonoBehaviour
{
    private MeshRenderer mesh;

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("barrier"))
            mesh.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("barrier") && mesh.enabled)
            mesh.enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("barrier"))
            mesh.enabled = true;
    }
}

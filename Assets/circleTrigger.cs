using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circleTrigger : MonoBehaviour
{
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("barrier"))
        {
            _renderer.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("barrier") && _renderer.enabled)
        {
            _renderer.enabled = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("barrier"))
        {
            _renderer.enabled = true;
        }
    }
}
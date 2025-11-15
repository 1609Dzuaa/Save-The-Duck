using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField]
    private float forceAmount;
    void Start()
    {
        rb2d.velocity = Vector3.down * forceAmount;
    }

    void Update() { 

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetsugaController : MonoBehaviour
{
    public Vector2 _targetv;
    Rigidbody2D _rb2d;
    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        _rb2d.AddForce(_targetv, ForceMode2D.Impulse);
    }
}

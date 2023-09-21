using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetsugaController : MonoBehaviour
{
    public Vector2 _targetv;
    Rigidbody2D _rb2d;
    SpriteRenderer _sr;
    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
    }
    private void FixedUpdate()
    {
        _sr.flipX = _targetv.x < 0;
        _rb2d.AddForce(_targetv, ForceMode2D.Impulse);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CatAI : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] float _moveSpd;
    [SerializeField] float _followDis;
    Animator _anim;
    Rigidbody2D _rb2d;
    SpriteRenderer _sr;
    private void Start()
    {
        _anim = GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        //�V�[���J�n�r�w�C�r�A
        _anim.SetTrigger("actStretch");
    }
    private void FixedUpdate()
    {
        //�ڕW�x�N�g���Z�o
        var tv = _player.transform.position - this.transform.position;
        var d = Vector2.Distance(_player.transform.position, this.transform.position);
        //��苗�����ꂽ��
        if(d > _followDis)
        {
            _rb2d.AddForce(tv.normalized * _moveSpd, ForceMode2D.Force);
        }
        //�摜�t���b�v����
        _sr.flipX = tv.x < 0;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, _followDis);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>�{�X��AI</summary>
public class BossAI_alpha : MonoBehaviour
{
    //Unity�R���|�[�l���g
    Animator _anim;
    Rigidbody2D _rb2d;
    SpriteRenderer _sr;
    AudioSource _as;
    //���J�t�B�[���h
    [SerializeField] float _health;
    [SerializeField] float _moveSpeed;
    [SerializeField] float _playerCapturedDistance;
    [SerializeField] float _attackRange;
    [SerializeField] float _spellRange;
    [SerializeField] float _damageFromPlayer;
    //����J�t�B�[���h
    GameObject _player;
    public bool _captured;
    public bool _insideRange;
    //�v���C���[�ߑ��A�ҋ@�X�e�[�g
    //�ʏ�U���A���@�U��
    //��U���A���S
    private void Start()
    {
        //Unity�R���|�[�l���g�̎擾
        _anim = GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();   
        _as = GetComponent<AudioSource>();
        //�v���C���[�̌���
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    private void FixedUpdate()
    {
        #region �ߑ�����
        Debug.Log($"{this.gameObject.name} : �v���C���[�ߑ�");
        //�ߑ���ԍX�V
        _captured = Vector2.Distance(_player.transform.position,
        this.transform.position) < _playerCapturedDistance;
        //�U���͈͓��ɋ��邩�̔���
        _insideRange = Vector2.Distance(_player.transform.position,
        this.transform.position) < _attackRange && _captured;
        if (_captured)
        {
            //�ڕW�ւ̃x�N�g���̎Z�o
            var tv = (_player.transform.position - this.transform.position).normalized;
            //���̕����֌�����
            _rb2d.AddForce(tv * _moveSpeed, ForceMode2D.Force);
        }
        #endregion
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�_���[�W����
        if (collision.CompareTag("PlayerWeapon"))
        {
            _health -= _damageFromPlayer;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.gameObject.transform.position, _playerCapturedDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.gameObject.transform.position, _attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.gameObject.transform.position, _spellRange);
    }
}

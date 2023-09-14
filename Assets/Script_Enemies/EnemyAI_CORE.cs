using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(Rigidbody2D))]
/// <summary> �G�L������AI�̃R�A </summary>
public class EnemyAI_CORE : MonoBehaviour
{
    Animator _anim;
    Rigidbody2D _rb2d;
    SpriteRenderer _sr;
    //���J�t�B�[���h
    /// <summary>�̗�</summary>
    [SerializeField] float _hp;
    /// <summary>�ړ����x</summary>
    [SerializeField] float _ms;
    //AI�t�B�[���h
    /// <summary>����̔��a</summary>
    [SerializeField] float _patrollRadius;
    /// <summary>�v���C���[�ߑ�����</summary>
    [SerializeField] float _playerCaptureDistance;
    /// <summary>�U���͈�</summary>
    [SerializeField] float _attackDistance;
    /// <summary>�Ԃ������Ƃ��Ƀ��p�X����OBJ�̃��C���[</summary>
    [SerializeField] int _repathLayer;
    /// <summary>�GAI�̈ړ����[�h</summary>
    [SerializeField] MoveMode _moveMode;
    /// <summary>�ڕW�̍��W�ւ́h�x�N�g���h</summary>
    Vector3 _targetPath = Vector3.zero;
    /// <summary>����̃v���C���[�ߑ��t���O</summary>
    bool _isPlayerFound = false;
    /// <summary>�ȑO�̃v���C���[�ߑ��t���O</summary>
    bool _pisPlayerFound = false;
    public bool _attacked = false;
    //�f���Q�[�g
    /// <summary>�_���[�W������������ɌĂяo�����f���Q�[�g</summary>
    public Action<Animator> damagedEvent;
    /// <summary>���񂾂Ƃ��ɌĂяo�����f���Q�[�g</summary>
    public Action<Animator> deathEvent;
    /// <summary>�v���C���[�ߑ����ɌĂяo�����f���Q�[�g</summary>
    public Action<Animator> playerCapturedEvent;
    /// <summary>�v���C���[�������������ɔ�яo�����f���Q�[�g</summary>
    public Action<Animator> playerMissedEvent;
    /// <summary>�v���C���[�U�����ɌĂяo�����f���Q�[�g</summary>
    public Action<Animator> attackingEvent;
    /// <summary>�ړ����[�h</summary>
    public enum MoveMode
    {
        Walk,
        Fly
    }
    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _rb2d = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        //�ړ����[�h�ɂ���ďd�̓X�P�[�����ϓ�
        _rb2d.gravityScale = (_moveMode == MoveMode.Fly) ? 0 : 1;
        _rb2d.freezeRotation = true;
        //�ڕW���W�̐ݒ�[����]
        RePath();
        //RayCast�̉e�����󂯂Ȃ��悤�ɂ���
        this.gameObject.layer = 2;
    }
    private void FixedUpdate()
    {
        Debug.Log($"�ڕW�⑫�t���O{_pisPlayerFound}");
        //�t���b�v����
        _sr.flipX = _targetPath.x < 0;
        //���S����
        if (_hp <= 0)
        {
            //�f���Q�[�g�Ăяo��
            deathEvent(_anim);
        }
        //�p�g���[�����������ǐՁ��U��
        //�p�g���[����
        //�P�D�i�ސ�̈ʒu�̊m��
        //�Q�D���̈ʒu�ɂ�����܂��ʂ̈ʒu�ɍs��
        //�R�D�p�g���[�����Ƀv���C���[��ߑ�����΂����ɍs��
        //�S�D�ߑ����āA�U�������Ƀv���C���[������΍U��
        /* = �����{�� = */
        //�p�g���[������
        #region �v���C���[���ߑ���
        //�v���C���[���ߑ���
        if (!_isPlayerFound)
        {
            Debug.Log($"�ڕW���W{_targetPath}");
            //�����̃L���X�g
            Ray2D ray = new Ray2D(this.gameObject.transform.position, _targetPath.normalized);
            RaycastHit2D hit2d = Physics2D.Raycast(ray.origin, ray.direction, 1);
            Debug.DrawRay(ray.origin, ray.direction, Color.green);
            if (hit2d.collider)
            {
                Debug.Log($"���C���[�F{hit2d.collider.gameObject.layer}");
                if (hit2d.collider.gameObject.layer == _repathLayer)
                {
                    RePath();
                    Debug.Log($"�ڕW���W�X�V{hit2d.collider.gameObject.name}");
                }
            }
            _rb2d.AddForce(_targetPath.normalized * _ms, ForceMode2D.Force);
        }
        else
        {
            _rb2d.AddForce(_targetPath.normalized * _ms * 2, ForceMode2D.Force);
        }
        #endregion
        #region �v���C���[��������
        //�v���C���[��������
        if (Vector2.Distance(this.gameObject.transform.position,
            GameObject.FindGameObjectWithTag("Player").transform.position) < _playerCaptureDistance)
        {
            _isPlayerFound = true;
            //�f���Q�[�g�Ăяo��
            if (_isPlayerFound && !_pisPlayerFound)
            {
                playerCapturedEvent(_anim);
                _pisPlayerFound = true;
            }
            var this2player = GameObject.FindGameObjectWithTag("Player").transform.position - this.gameObject.transform.position;
            //�ړ����[�h�ɂ���ăx�N�g����ύX
            switch (_moveMode)
            {
                case MoveMode.Walk:
                    {
                        _targetPath = new Vector2(this2player.x, 0);
                        break;
                    }
                case MoveMode.Fly:
                    {
                        _targetPath = this2player;
                        break;
                    }
            }
        }
        else
        {
            _isPlayerFound = false;
            //�f���Q�[�g�Ăяo��
            if (!_isPlayerFound && _pisPlayerFound)
            {
                playerMissedEvent(_anim);
                _pisPlayerFound = false;
            }
        }
        //�U������
        if (_isPlayerFound && Vector2.Distance(this.gameObject.transform.position,
            GameObject.FindGameObjectWithTag("Player").transform.position) < _attackDistance && !_attacked)
        {
            //�f���Q�[�g�Ăяo��
            attackingEvent(_anim);
            _attacked = true;
            StartCoroutine(WaitForEndOfAttack(3));
        }
        #endregion
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�����s�ړ����[�h�I�����ɂ͌����������������̃��p�X�ŏ\���B����͔�s�ړ����[�h���ǂɓ����������̃��p�X����
        if (collision.gameObject.layer == _repathLayer && !_isPlayerFound && _moveMode == MoveMode.Fly)
        {
            RePath();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�_���[�W����
        if (collision.gameObject.CompareTag("PlayerWeapon"))
        {
            //�f���Q�[�g�Ăяo��
            damagedEvent(_anim);
            _hp -= 10f;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.gameObject.transform.position, _playerCaptureDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.gameObject.transform.position, _attackDistance);
    }
    /// <summary>�ڕW���W�̍X�V[�����łƂ�]</summary>
    private void RePath()
    {
        Debug.Log("�ڕW�X�V");
        //�ړ����[�h�ɂ���ĖڕW���W�X�V�����𕪂���
        switch (_moveMode)
        {
            case MoveMode.Fly:
                {
                    _targetPath = new Vector2(
                                UnityEngine.Random.Range(-_patrollRadius, _patrollRadius),
                                UnityEngine.Random.Range(-_patrollRadius, _patrollRadius)
                                );
                    break;
                }
            case MoveMode.Walk:
                {
                    _targetPath = new Vector2(
                                    UnityEngine.Random.Range(-_patrollRadius, _patrollRadius),
                                    0
                                    );
                    break;
                }
        }
    }
    /// <summary>�U���C���^�[�o���R���[�`���B�U���t���O��false�ɂ��ăt���O��|��</summary>
    /// <param name="s"></param>
    /// <returns></returns>
    IEnumerator WaitForEndOfAttack(float s)
    {
        yield return new WaitForSeconds(s);
        _attacked = false;
    }
}

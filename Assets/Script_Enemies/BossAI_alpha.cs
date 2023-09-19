using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
/// <summary>�{�X��AI</summary>
public class BossAI_alpha : MonoBehaviour
{
    //Unity�R���|�[�l���g
    Animator _anim;
    Rigidbody2D _rb2d;
    SpriteRenderer _sr;
    AudioSource _as;
    //���J�t�B�[���h
    /// <summary>�̗�</summary>
    [SerializeField] float _health;
    /// <summary>�ړ����x</summary>
    [SerializeField] float _moveSpeed;
    /// <summary>�v���C���[�ߑ�����</summary>
    [SerializeField] float _playerCapturedDistance;
    /// <summary>�v���C���[�U���͈�</summary>
    [SerializeField] float _attackRange;
    /// <summary>�����U���͈�</summary>
    [SerializeField] float _spellRange;
    /// <summary>�v���C���[����̃_���[�W��</summary>
    [SerializeField] float _damageFromPlayer;
    /// <summary>�����U�����</summary>
    [SerializeField] int _spellLim;
    /// <summary>�����U���̃I�u�W�F�N�g</summary>
    [SerializeField] GameObject _spellObj;
    /// <summary>���S���I�u�W�F�N�g</summary>
    [SerializeField] GameObject _deathObj;
    //����J�t�B�[���h
    /// <summary>�v���C���[�I�u�W�F�N�g</summary>
    GameObject _player;
    /// <summary>�ߑ��t���O</summary>
    bool _captured = false;
    /// <summary>�U���͈͓��ߑ��t���O</summary>
    bool _insideRange = false;
    /// <summary>�U���t���O</summary>
    bool _attacked = false;
    /// <summary>�����U���t���O</summary>
    bool _spelled = false;
    /// <summary>�����U���J�E���g</summary>
    int _spellCnt = 0;
    /// <summary>
    /// 
    /// </summary>
    float _totalDamages = 0;
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
    private void Update()
    {
        if (_health < 0)
        {
            DeathBehaviour();
        }
    }
    /// <summary>���S������</summary>
    private void DeathBehaviour()
    {
        //���S����
        var obj = Instantiate(_deathObj);
        obj.transform.position = this.transform.position;
        this.gameObject.SetActive(false);
        Destroy(this);
        Destroy(obj, 2);
    }

    private void FixedUpdate()
    {
        Debug.Log($"{this.gameObject.name} : �v���C���[�ߑ�");
        //�ߑ���ԍX�V
        _captured = Vector2.Distance(_player.transform.position,
        this.transform.position) < _playerCapturedDistance;
        //�U���͈͓��ɋ��邩�̔���
        _insideRange = Vector2.Distance(_player.transform.position,
        this.transform.position) < _attackRange && _captured;
        //�A�j���[�^�[
        _anim.SetBool("Moving", _captured);
        if (_captured)
        {
            //�ڕW�ւ̃x�N�g���̎Z�o
            var tv = (_player.transform.position - this.transform.position).normalized;
            //�摜�t���b�v����
            _sr.flipX = tv.x > 0;
            //���̕����֌�����
            _rb2d.AddForce(tv * _moveSpeed, ForceMode2D.Force);
        }
        if (_insideRange && !_attacked)
        {
            //�U������
            StartCoroutine(AttackRoutine(3));
        }//�U����������O��A���@�̎˒����ɂ���Ƃ�
        else if (_captured && Vector2.Distance(_player.transform.position,
            this.transform.position) < _spellRange && !_spelled && _spellCnt < _spellLim)
        {
            //��������
            StartCoroutine(SpellRoutine(3));
        }
    }
    /// <summary>�ʏ�U�����[�`��</summary>
    /// <param name="t"></param>
    /// <returns></returns>
    IEnumerator AttackRoutine(float t)
    {
        //�A�j���[�^�[
        _anim.SetTrigger("actAtk");
        Debug.Log($"{this.gameObject.name}�U��");
        _attacked = true;
        yield return new WaitForSeconds(t);
        _attacked = false;
    }
    /// <summary>�����U�����[�`��</summary>
    /// <param name="t"></param>
    /// <returns></returns>
    IEnumerator SpellRoutine(float t)
    {
        //�A�j���[�^�[
        _anim.SetTrigger("actSpl");
        Debug.Log($"{this.gameObject.name}����");
        _spelled = true;
        //�I�u�W�F�N�g����
        var so = Instantiate(_spellObj);
        _spellCnt++;
        //�j��
        Destroy(so, 1);
        //���W�X�V
        so.transform.position = new Vector2(_player.transform.position.x, _player.transform.position.y + 1);
        yield return new WaitForSeconds(t);
        _spelled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�_���[�W����
        if (collision.CompareTag("PlayerWeapon"))
        {
            //�A�j���[�^�[
            _anim.SetTrigger("actHrt");
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
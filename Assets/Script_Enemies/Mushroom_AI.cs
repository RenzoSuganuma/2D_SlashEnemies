using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Mushroom_AI : EnemyAI_CORE
{
    /// <summary>�v���C���[�ߑ����̃}�e���A��</summary>
    [SerializeField] Material _playerCapturedMat;
    /// <summary>�v���C���[���ߑ����̃}�e���A��</summary>
    [SerializeField] Material _defaultMat;
    /// <summary>���S���̃h���b�v�A�C�e��</summary>
    [SerializeField] GameObject _dropObj;
    /// <summary>�v���C���[�ߑ����s��</summary>
    void PlayerCapturedEvent(Animator anim)
    {
        this.gameObject.GetComponent<Renderer>().material = _playerCapturedMat;
    }
    /// <summary>�v���C���[�r�����s��</summary>
    void PlayerMissedEvent(Animator anim)
    {
        this.gameObject.GetComponent<Renderer>().material = _defaultMat;
    }
    /// <summary>�U���s�����\�b�h</summary>
    /// <param name="anim"></param>
    void AttackingEvent(Animator anim)
    {
        //�U���p�^�[���̋[�������ɂ��I��
        switch ((UnityEngine.Random.Range(0, 100) % 100) / 10)
        {
            //�������~�̖���ג��o�H
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
                anim.SetTrigger("actAttack1");
                break;
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
                anim.SetTrigger("actAttack2");
                break;
        }
    }
    /// <summary>��U���s��</summary>
    /// <param name="animator"></param>
    void DamagedEvent(Animator anim)
    {
        Debug.Log($"{this.gameObject.name} IS1 Damaged");
        anim.SetTrigger("actTakeHit");
    }
    /// <summary>���S�s�����\�b�h</summary>
    void DeathEvent(Animator anim)
    {
        anim.Play("Mushroom_Death");
    }
    /// <summary>�A�j���[�V�����C�x���g����Ăяo��</summary>
    void DestroySelf()
    {
        //�h���b�v�A�C�e���̐���
        var go = GameObject.Instantiate(_dropObj);
        go.transform.position = this.transform.position;
        Destroy(this.GetComponent<Mushroom_AI>());
        Destroy(this.gameObject, 5f);
    }
    private void OnEnable()
    {
        //�f���Q�[�g�o�^
        base.playerCapturedEvent += PlayerCapturedEvent;
        base.playerMissedEvent += PlayerMissedEvent;
        base.attackingEvent += AttackingEvent;
        base.deathEvent += DeathEvent;
        base.damagedEvent += DamagedEvent;
    }
    private void OnDisable()
    {
        //�f���Q�[�g�o�^����
        base.playerCapturedEvent += PlayerCapturedEvent;
        base.playerMissedEvent -= PlayerMissedEvent;
        base.attackingEvent -= AttackingEvent;
        base.deathEvent -= DeathEvent;
        base.damagedEvent -= DamagedEvent;
    }
}

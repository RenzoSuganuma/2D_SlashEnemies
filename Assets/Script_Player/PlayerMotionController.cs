using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// .1-�v���C���[�̃A�j���[�V�����̑���N���X
/// .2-Animator�Ɠ����K�w[�I�u�W�F�N�g]�ɃA�^�b�`
/// .3-�A�j���[�V��������ɕK�v�ȃ��\�b�h���T�|�[�g
/// </summary>
public class PlayerMotionController
{
    /// <summary>�v���C���[�̃A�j���[�^�[</summary>
    Animator _anim = null;
    public PlayerMotionController(Animator anim)
    {
        //Animator�̎擾���ł�������
        _anim = anim;
    }
    /// <summary>�ړ����Ă��邩�̏�Ԃ�ݒ� �ړ���:condition = true</summary>
    public void SetMoveCondition(bool condition)
    {
        _anim.SetBool("isMoving", condition);
    }
    /// <summary>�ڒn���Ă��邩�̏�Ԃ�ݒ� �ڒn��:condition = true</summary>
    public void SetGroundedCondition(bool condition)
    {
        _anim.SetBool("isGrounded", condition);
    }
    /// <summary>�ǂɒ���t���Ă��邩�̏�Ԃ�ݒ� ����t����:condition = true</summary>
    public void SetGrabingWallCondition(bool condition)
    {
        _anim.SetBool("isGrabingWall", condition);
    }
    /// <summary>���S���Ă��邩�̏�Ԃ�ݒ� ���S��:condition = true</summary>
    public void SetDeathCondiotion(bool condition)
    {
        _anim.SetBool("isDeath", condition);
    }
    /// <summary>�ʏ�U�����ɌĂяo����郁�\�b�h[��f���Q�[�g�o�^]</summary>
    public void ActionAttack()
    {
        _anim.SetTrigger("actAttack");
    }
    /// <summary>�W�����v���ɌĂяo����郁�\�b�h[��f���Q�[�g�o�^]</summary>
    public void ActionJump()
    {
        _anim.SetTrigger("actJump");

    }
    /// <summary>�ǃW�����v���ɌĂяo����郁�\�b�h[��f���Q�[�g�o�^]</summary>
    public void ActionWallJump()
    {
        _anim.SetTrigger("actWallJump");

    }
    /// <summary>��U�����ɌĂяo����郁�\�b�h[��f���Q�[�g�o�^]</summary>
    public void ActionHurt()
    {
        _anim.SetTrigger("actHurt");
    }
    /// <summary>�_�b�V�����ɌĂяo����郁�\�b�h[��f���Q�[�g�o�^]</summary>
    public void ActionDash()
    {
        _anim.SetTrigger("actDash");
    }
    /// <summary>�W�����v�U�����ɌĂяo����郁�\�b�h[��f���Q�[�g�o�^]</summary>
    public void ActionJumpAttack()
    {
        _anim.SetTrigger("actJumpingAttack");
    }
    /// <summary>�U�����[�V�������̃A�j���[�V�����C�x���g</summary>
    void SetAttackCount(int count)
    {
        _anim.SetInteger("attackCount", count);
    }
}

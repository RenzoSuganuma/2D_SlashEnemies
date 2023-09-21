using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// .1-プレイヤーのアニメーションの操作クラス
/// .2-Animatorと同じ階層[オブジェクト]にアタッチ
/// .3-アニメーション操作に必要なメソッドをサポート
/// </summary>
public class PlayerMotionController
{
    /// <summary>プレイヤーのアニメーター</summary>
    Animator _anim = null;
    public PlayerMotionController(Animator anim)
    {
        //Animatorの取得ができたら代入
        _anim = anim;
    }
    /// <summary>移動しているかの状態を設定 移動時:condition = true</summary>
    public void SetMoveCondition(bool condition)
    {
        _anim.SetBool("isMoving", condition);
    }
    /// <summary>接地しているかの状態を設定 接地時:condition = true</summary>
    public void SetGroundedCondition(bool condition)
    {
        _anim.SetBool("isGrounded", condition);
    }
    /// <summary>壁に張り付いているかの状態を設定 張り付き時:condition = true</summary>
    public void SetGrabingWallCondition(bool condition)
    {
        _anim.SetBool("isGrabingWall", condition);
    }
    /// <summary>死亡しているかの状態を設定 死亡時:condition = true</summary>
    public void SetDeathCondiotion(bool condition)
    {
        _anim.SetBool("isDeath", condition);
    }
    /// <summary>通常攻撃時に呼び出されるメソッド[被デリゲート登録]</summary>
    public void ActionAttack()
    {
        _anim.SetTrigger("actAttack");
    }
    /// <summary>ジャンプ時に呼び出されるメソッド[被デリゲート登録]</summary>
    public void ActionJump()
    {
        _anim.SetTrigger("actJump");

    }
    /// <summary>壁ジャンプ時に呼び出されるメソッド[被デリゲート登録]</summary>
    public void ActionWallJump()
    {
        _anim.SetTrigger("actWallJump");

    }
    /// <summary>被攻撃時に呼び出されるメソッド[被デリゲート登録]</summary>
    public void ActionHurt()
    {
        _anim.SetTrigger("actHurt");
    }
    /// <summary>ダッシュ時に呼び出されるメソッド[被デリゲート登録]</summary>
    public void ActionDash()
    {
        _anim.SetTrigger("actDash");
    }
    /// <summary>ジャンプ攻撃時に呼び出されるメソッド[被デリゲート登録]</summary>
    public void ActionJumpAttack()
    {
        _anim.SetTrigger("actJumpingAttack");
    }
    /// <summary>シフト攻撃時に呼び出されるメソッド[被デリゲート登録]</summary>
    public void ActionShiftAttack()
    {
        _anim.SetTrigger("actShiftAttack");
    }
    /// <summary>攻撃モーション中のアニメーションイベント</summary>
    void SetAttackCount(int count)
    {
        _anim.SetInteger("attackCount", count);
    }
}

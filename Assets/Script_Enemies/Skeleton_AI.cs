using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Skeleton_AI : EnemyAI_CORE
{
    /// <summary>プレイヤー捕捉時のマテリアル</summary>
    [SerializeField] Material _playerCapturedMat;
    /// <summary>プレイヤー未捕捉時のマテリアル</summary>
    [SerializeField] Material _defaultMat;
    /// <summary>死亡時のドロップアイテム</summary>
    [SerializeField] GameObject _dropObj;
    /// <summary>プレイヤー捕捉時行動</summary>
    void PlayerCapturedEvent(Animator anim)
    {
        this.gameObject.GetComponent<Renderer>().material = _playerCapturedMat;
    }
    /// <summary>プレイヤー喪失時行動</summary>
    void PlayerMissedEvent(Animator anim)
    {
        this.gameObject.GetComponent<Renderer>().material = _defaultMat;
    }
    /// <summary>攻撃行動メソッド</summary>
    /// <param name="anim"></param>
    void AttackingEvent(Animator anim)
    {
        //攻撃パターンの擬似乱数による選択
        switch ((UnityEngine.Random.Range(0, 100) % 100) / 10)
        {
            //いい塩梅の無作為抽出？
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
    /// <summary>被攻撃行動</summary>
    /// <param name="animator"></param>
    void DamagedEvent(Animator animator)
    {
        Debug.Log($"{this.gameObject.name} IS1 Damaged");
    }
    /// <summary>死亡行動メソッド</summary>
    void DeathEvent(Animator anim)
    {
        anim.Play("Mushroom_Death");
    }
    /// <summary>アニメーションイベントから呼び出す</summary>
    void DestroySelf()
    {
        //ドロップアイテムの生成
        var go = GameObject.Instantiate(_dropObj);
        go.transform.position = this.transform.position;
        Destroy(this.gameObject);
    }
    private void OnEnable()
    {
        //デリゲート登録
        base.playerCapturedEvent += PlayerCapturedEvent;
        base.playerMissedEvent += PlayerMissedEvent;
        base.attackingEvent += AttackingEvent;
        base.deathEvent += DeathEvent;
        base.damagedEvent += DamagedEvent;
    }
    private void OnDisable()
    {
        //デリゲート登録解除
        base.playerCapturedEvent += PlayerCapturedEvent;
        base.playerMissedEvent -= PlayerMissedEvent;
        base.attackingEvent -= AttackingEvent;
        base.deathEvent -= DeathEvent;
        base.damagedEvent -= DamagedEvent;
    }
}

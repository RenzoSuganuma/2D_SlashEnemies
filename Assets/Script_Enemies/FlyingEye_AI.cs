using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FlyingEye_AI : EnemyAI_CORE
{
    /// <summary>プレイヤー捕捉時のマテリアル</summary>
    [SerializeField] Material _playerCapturedMat;
    /// <summary>プレイヤー未捕捉時のマテリアル</summary>
    [SerializeField] Material _defaultMat;
    /// <summary>死亡時のドロップアイテム</summary>
    [SerializeField] GameObject _dropObj;
    void PlayerCapturedEvent()
    {
        this.gameObject.GetComponent<Renderer>().material = _playerCapturedMat;
    }
    void PlayerMissedEvent()
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
    /// <summary>死亡行動メソッド</summary>
    void DeathEvent(Animator anim)
    {
        anim.Play("FlyingEye_Death");
        //ドロップアイテムの生成
        var go = GameObject.Instantiate(_dropObj);
        go.transform.position = this.transform.position;
        //破棄
        Destroy(this.gameObject);
    }
    private void OnEnable()
    {
        base.playerCapturedEvent += PlayerCapturedEvent;
        base.playerMissedEvent += PlayerMissedEvent;
        base.attackingEvent += AttackingEvent;
        base.deathEvent += DeathEvent;
    }
    private void OnDisable()
    {
        base.playerCapturedEvent += PlayerCapturedEvent;
        base.playerMissedEvent -= PlayerMissedEvent;
        base.attackingEvent -= AttackingEvent;
        base.deathEvent -= DeathEvent;
    }
}

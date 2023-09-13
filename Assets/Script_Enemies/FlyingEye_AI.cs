using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FlyingEye_AI : EnemyAI_CORE
{
    /// <summary>プレイヤー捕捉時のマテリアル</summary>
    [SerializeField] Material _playerCapturedMat;
    /// <summary>プレイヤー未捕捉時のマテリアル</summary>
    [SerializeField] Material _defaultMat;
    void PlayerCapturedEvent()
    {
        this.gameObject.GetComponent<Renderer>().material = _playerCapturedMat;
    }
    void PlayerMissedEvent()
    {
        this.gameObject.GetComponent<Renderer>().material = _defaultMat;
    }
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
    private void OnEnable()
    {
        base.playerCapturedEvent += PlayerCapturedEvent;
        base.playerMissedEvent += PlayerMissedEvent;
        base.attackingEvent += AttackingEvent;
    }
    private void OnDisable()
    {
        base.playerCapturedEvent += PlayerCapturedEvent;
        base.playerMissedEvent -= PlayerMissedEvent;
        base.attackingEvent -= AttackingEvent;
    }
}

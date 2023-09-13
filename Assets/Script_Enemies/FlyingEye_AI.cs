using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FlyingEye_AI : EnemyAI_CORE
{
    Animator _anim;
    /// <summary>プレイヤー捕捉時のマテリアル</summary>
    [SerializeField] Material _playerCapturedMat;
    private void Start()
    {
        _anim = GetComponent<Animator>();
    }
    void PlayerCapturedEvent()
    {
        this.gameObject.GetComponent<Renderer>().material = _playerCapturedMat;
    }
    private void OnEnable()
    {
        base.playerCapturedEvent += PlayerCapturedEvent;
    }
    private void OnDisable()
    {
        base.playerCapturedEvent += PlayerCapturedEvent;
    }
}

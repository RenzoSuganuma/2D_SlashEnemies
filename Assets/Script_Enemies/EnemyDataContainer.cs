using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyAI_CORE;
[CreateAssetMenu(fileName ="eData",menuName ="ScriptableObjects/CreateEnemyData")]
public class EnemyDataContainer : ScriptableObject
{
    /// <summary>体力</summary>
    public float _hp;
    /// <summary>移動速度</summary>
    public float _ms;
    /// <summary>巡回の半径</summary>
    public float _patrollRadius;
    /// <summary>プレイヤー捕捉距離</summary>
    public float _playerCaptureDistance;
    /// <summary>攻撃範囲</summary>
    public float _attackDistance;
    /// <summary>ダメージ量</summary>
    public float _dp;
    /// <summary>飛ばす光線の長さ</summary>
    public int _rayLength;
    /// <summary>ぶつかったときにリパスするOBJのレイヤー</summary>
    public int _repathLayer;
    /// <summary>敵AIの移動モード</summary>
    public MoveMode _moveMode;
}

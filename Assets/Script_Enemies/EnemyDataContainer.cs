using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyAI_CORE;
[CreateAssetMenu(fileName ="eData",menuName ="ScriptableObjects/CreateEnemyData")]
public class EnemyDataContainer : ScriptableObject
{
    /// <summary>�̗�</summary>
    public float _hp;
    /// <summary>�ړ����x</summary>
    public float _ms;
    /// <summary>����̔��a</summary>
    public float _patrollRadius;
    /// <summary>�v���C���[�ߑ�����</summary>
    public float _playerCaptureDistance;
    /// <summary>�U���͈�</summary>
    public float _attackDistance;
    /// <summary>�_���[�W��</summary>
    public float _dp;
    /// <summary>�Ԃ������Ƃ��Ƀ��p�X����OBJ�̃��C���[</summary>
    public int _repathLayer;
    /// <summary>�GAI�̈ړ����[�h</summary>
    public MoveMode _moveMode;
}

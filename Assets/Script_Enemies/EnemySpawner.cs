using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    /// <summary>�C���X�^���X������G�v���n�u</summary>
    [SerializeField] GameObject[] _enemies;
    /// <summary>�����C���^�[�o��</summary>
    [SerializeField] float _interval;
    /// <summary>�̐����E</summary>
    [SerializeField] int _limit;
    /// <summary>�o�ߎ���</summary>
    float _elapsedT = 0;
    private void FixedUpdate()
    {
        Debug.Log("�G�X�|�i�[���ԃJ�E���g");
        _elapsedT += Time.deltaTime;
        if (_elapsedT > _interval && GameObject.FindGameObjectsWithTag("Monster").Length < _limit)
        {
            var go = Instantiate(_enemies[UnityEngine.Random.Range(0, _enemies.Length)]);
            go.transform.position = this.transform.position;
            _elapsedT = 0;
        }
    }
}

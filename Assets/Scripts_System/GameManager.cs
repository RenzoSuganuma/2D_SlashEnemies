using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �v���C���[�̃X�e�[�^�X�ƃQ�[���̃X�e�[�^�X���Ǘ�����N���X
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] float _playerHealth;
    [SerializeField] float _playerStamina;
    [SerializeField] Material _pDefaultMaterial;
    [SerializeField] Material _pSpawnMaterial;
    private void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Renderer>().material = _pSpawnMaterial;
    }
    /// <summary>�̗͂��X�V���郁�\�b�h�B�����ɉ��Z����l����</summary>
    /// <param name="health"></param>
    public void ModifyHealth(float health)
    {
        _playerHealth += health;
    }
    /// <summary>�X�^�~�i���X�V���郁�\�b�h�B�����ɉ��Z����l����</summary>
    /// <param name="stamina"></param>
    public void ModifyStamina(float stamina)
    {
        _playerStamina += stamina;
    }
}
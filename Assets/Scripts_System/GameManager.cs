using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// �v���C���[�̃X�e�[�^�X�ƃQ�[���̃X�e�[�^�X���Ǘ�����N���X
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] float _playerHealth;
    [SerializeField] Material _pDefaultMaterial;
    [SerializeField] Material _pSpawnMaterial;
    [SerializeField] Text _elapsedTimeText;
    [SerializeField] Slider _hpSlider;
    float _elapsedTime = 0;
    private void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Renderer>().material = _pSpawnMaterial;
        _elapsedTime = 0;
    }
    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        _elapsedTimeText.text = _elapsedTime.ToString("F3");
        _hpSlider.value = _playerHealth / 100f;
    }
    /// <summary>�̗͂��X�V���郁�\�b�h�B�����ɉ��Z����l����</summary>
    /// <param name="health"></param>
    public void ModifyHealth(float health)
    {
        _playerHealth += health;
    }
}
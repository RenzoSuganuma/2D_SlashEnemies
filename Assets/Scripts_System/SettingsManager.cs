using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] AudioMixer _aMixer;
    [SerializeField] Slider _masterSlider;
    [SerializeField] Slider _bgmSlider;
    [SerializeField] Slider _voiceSlider;
    /// <summary>���O���̓t�B�[���h</summary>
    [SerializeField] InputField _inputFeild;
    /// <summary>�v���C���[��</summary>
    [SerializeField] Text _playerName;
    string jsonData = string.Empty;
    private void Start()
    {
        if (JsonUtility.FromJson<PlayerSaveDataContainer>(PlayerPrefs.GetString("userDatas")) != null){
            PlayerSaveDataContainer psdc =
                JsonUtility.FromJson<PlayerSaveDataContainer>(PlayerPrefs.GetString("userDatas"));
            //�I�[�f�B�I�~�L�T�[���ʂ̐ݒ�
            _aMixer.SetFloat("MasterVol", psdc._masterVol);
            _aMixer.SetFloat("BGMVol", psdc._bgmVol);
            _aMixer.SetFloat("VoiceVol", psdc._voiceVol);
            //�v���C���[���ݒ�
            _playerName.text = psdc._playerName;
        }
    }
    public void SetDatas()
    {
        PlayerSaveDataContainer psdc = new PlayerSaveDataContainer();
        psdc._playerName = _inputFeild.text;
        psdc._masterVol = _masterSlider.value;
        psdc._bgmVol = _bgmSlider.value;
        psdc._voiceVol = _voiceSlider.value;
        //JSON��
        jsonData = JsonUtility.ToJson(psdc);
        //�f�[�^�̃Z�[�u
        PlayerPrefs.SetString("userDatas", jsonData);
        Debug.Log($"�v���C���[�f�[�^{PlayerPrefs.GetString("userDatas")}");
    }
    public void SetMasterVolume()
    {
        _aMixer.SetFloat("MasterVol",_masterSlider.value);
    }
    public void SetBGMVolume()
    {
        _aMixer.SetFloat("BGMVol", _bgmSlider.value);
    }
    public void SetVoiceVolume()
    {
        _aMixer.SetFloat("VoiceVol", _voiceSlider.value);
    }
}
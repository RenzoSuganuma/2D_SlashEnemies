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
    /// <summary>名前入力フィールド</summary>
    [SerializeField] InputField _inputFeild;
    /// <summary>プレイヤー名</summary>
    [SerializeField] Text _playerName;
    string jsonData = string.Empty;
    private void Start()
    {
        if (JsonUtility.FromJson<PlayerSaveDataContainer>(PlayerPrefs.GetString("userDatas")) != null){
            PlayerSaveDataContainer psdc =
                JsonUtility.FromJson<PlayerSaveDataContainer>(PlayerPrefs.GetString("userDatas"));
            //オーディオミキサー音量の設定
            _aMixer.SetFloat("MasterVol", psdc._masterVol);
            _aMixer.SetFloat("BGMVol", psdc._bgmVol);
            _aMixer.SetFloat("VoiceVol", psdc._voiceVol);
            //プレイヤー名設定
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
        //JSON化
        jsonData = JsonUtility.ToJson(psdc);
        //データのセーブ
        PlayerPrefs.SetString("userDatas", jsonData);
        Debug.Log($"プレイヤーデータ{PlayerPrefs.GetString("userDatas")}");
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

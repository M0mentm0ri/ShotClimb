using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public AudioSource audio_SE;
    public AudioSource audio_battleBGM;
    public AudioSource audio_themeBGM;
    public AudioClip[] audioClip_SE;

    public Slider SE_Slider;
    public Slider BGM_Slider;

    public float SE_Volume;
    public float BGM_Volume;

    private bool IsButton = false;

    public bool IsTimeScale = true;

    // Start is called before the first frame update
    void Start()
    {
        audio_SE.volume = PlayerPrefs.GetFloat("SE_Volume", 1f);
        audio_battleBGM.volume = PlayerPrefs.GetFloat("BGM_Volume", 0.5f);
        audio_themeBGM.volume = PlayerPrefs.GetFloat("BGM_Volume", 0.5f);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(IsTimeScale)
        {
            audio_SE.pitch = Time.timeScale;
            audio_battleBGM.pitch = Time.timeScale;
            audio_themeBGM.pitch = Time.timeScale;
        }
    }


    public void PlayOnSound(int Index)
    {
        audio_SE.PlayOneShot(audioClip_SE[Index]); // �I�[�f�B�I���Đ�
        //UnityEngine.Debug.Log("�����N���b�v���Đ����܂��I: " + Index);
    }
    public void PlayBattleBGM(int play)
    {
        switch (play)
        {
            case 0:
                {
                    audio_battleBGM.Stop();
                    //UnityEngine.Debug.Log("BGM���~�߂܂��I");
                    break;
                }
            case 1:
                {
                    audio_battleBGM.Play(); // �I�[�f�B�I���Đ�
                    //UnityEngine.Debug.Log("BGM�𗬂��܂��I");
                    break;
                }
            case 2:
                {
                    audio_battleBGM.Pause(); // �I�[�f�B�I���Đ�
                    //UnityEngine.Debug.Log("BGM���ꎞ��~�I");
                    break;
                }
            case 3:
                {
                    audio_battleBGM.UnPause(); // �I�[�f�B�I���Đ�
                    //UnityEngine.Debug.Log("BGM���ꎞ��~�������I");
                    break;
                }
            default:
                {
                    //UnityEngine.Debug.Log("�͈͊O�̐��l�ł�");
                    break;
                }
        }
    }

    public void PlayThemeBGM(int play)
    {
        switch (play)
        {
            case 0:
                {
                    audio_themeBGM.Stop();
                    //UnityEngine.Debug.Log("BGM���~�߂܂��I");
                    break;
                }
            case 1:
                {
                    audio_themeBGM.Play(); // �I�[�f�B�I���Đ�
                    //UnityEngine.Debug.Log("BGM�𗬂��܂��I");
                    break;
                }
            case 2:
                {
                    audio_themeBGM.Pause(); // �I�[�f�B�I���Đ�
                    //UnityEngine.Debug.Log("BGM���ꎞ��~�I");
                    break;
                }
            case 3:
                {
                    audio_themeBGM.UnPause(); // �I�[�f�B�I���Đ�
                    //UnityEngine.Debug.Log("BGM���ꎞ��~�������I");
                    break;
                }
            case 4:
                {
                    if(!IsButton)
                    {
                        audio_themeBGM.Play(); // �I�[�f�B�I���Đ�
                        //UnityEngine.Debug.Log("BGM�𗬂��܂��I");
                        IsButton = true;
                    }
                    else
                    {
                        audio_themeBGM.Stop();
                        //UnityEngine.Debug.Log("BGM���~�߂܂��I");
                        IsButton = false; ;
                    }
                    break;
                }
            default:
                {
                    //UnityEngine.Debug.Log("�͈͊O�̐��l�ł�");
                    break;
                }
        }
    }

    public void SE_VolumeApply()
    {
        SE_Volume = SE_Slider.value;
        audio_SE.volume = SE_Volume;
    }

    public void BGM_VolumeApply()
    {
        BGM_Volume = BGM_Slider.value;
        audio_battleBGM.volume = BGM_Volume;
        audio_themeBGM.volume = BGM_Volume;
    }
    
    public void VolumeApply()
    {
        audio_SE.volume = SE_Volume;
        audio_battleBGM.volume = BGM_Volume;
        audio_themeBGM.volume = BGM_Volume;
        PlayerPrefs.SetFloat("SE_Volume", SE_Volume);
        PlayerPrefs.SetFloat("BGM_Volume",BGM_Volume);
    }

    public void SliderApply()
    {
        SE_Slider.value = PlayerPrefs.GetFloat("SE_Volume", 0.5f); ;
        BGM_Slider.value = PlayerPrefs.GetFloat("BGM_Volume", 0.5f); ;
    }
}

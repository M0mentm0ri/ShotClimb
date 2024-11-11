using UnityEngine;
using UnityEngine.UI;

public class ClearDisplay : MonoBehaviour
{
    public Text stageTimeText;  // �X�e�[�W�^�C���\���p�e�L�X�gUI
    public Text totalTimeText;  // ���v�^�C���\���p�e�L�X�gUI

    void Start()
    {
        float stageTime = PlayerPrefs.GetFloat("StageTime", 0f);
        float totalTime = PlayerPrefs.GetFloat("TotalTime", 0f);

        stageTimeText.text = "�X�e�[�W�^�C��: " + stageTime.ToString("F2") + "�b";
        totalTimeText.text = "���v�^�C��: " + totalTime.ToString("F2") + "�b";
    }
}

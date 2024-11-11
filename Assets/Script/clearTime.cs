using UnityEngine;
using TMPro;

public class crealTime : MonoBehaviour
{
    public TextMeshProUGUI gameClearTimeText;  // �Q�[���N���A��ʂɕ\�����鍇�v�b����TextMeshPro
    public TextMeshProUGUI stageTimesText;  // �X�e�[�W���Ƃ̕b����\������TextMeshPro

    private void Start()
    {
        // �Q�[���N���A��ʂɍ��v���Ԃ�\��
        gameClearTimeText.text = "Total Time: " + Mathf.Floor(PlayerPrefs.GetFloat("TotalTime")) + "s\n";

        // �e�X�e�[�W�̎��Ԃ�\��
        for (int i = 1; i <= 10; i++)  // �ő�10�X�e�[�W
        {
            float stageTime = PlayerPrefs.GetFloat("Stage" + i);
            if (stageTime > 0)
            {
                stageTimesText.text += "Stage " + i + ": " + Mathf.Floor(stageTime) + "s\n";
            }
        }
    }
}

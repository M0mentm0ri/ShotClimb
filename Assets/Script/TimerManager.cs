using UnityEngine;
using UnityEngine.UI;


public class TimerManager : MonoBehaviour
{
    public Text timerText;  // �^�C�}�[��\������e�L�X�gUI
    private float elapsedTime;  // �o�ߎ��Ԃ�ێ�
    private bool isTiming = true;  // �^�C�}�[�𓮂����t���O
    private static float totalElapsedTime = 0f;  // �S�̂̌o�ߎ��Ԃ��L�^

    void Update()
    {
        if (isTiming)
        {
            elapsedTime += Time.deltaTime;  // �o�ߎ��Ԃ��X�V
            timerText.text = elapsedTime.ToString("F2") + "�b";  // �^�C�}�[��UI�ɕ\��
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("timerTag"))
        {
            isTiming = false;  // �^�C�}�[���~
            totalElapsedTime += elapsedTime;  // ���v�o�ߎ��Ԃɉ��Z
            GameClear();  // �Q�[���N���A��ʂ֑J��
        }
    }

    private void GameClear()
    {
        PlayerPrefs.SetFloat("StageTime", elapsedTime);  // �X�e�[�W�^�C����ۑ�
        PlayerPrefs.SetFloat("TotalTime", totalElapsedTime);  // ���v�^�C����ۑ�
        
    }
}

using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TimerManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;  // �^�C�}�[��\������TextMeshPro��UI
    public string timerTag = "timerTag";  // �Փ˔���p�̃^�O
    public float stageTime = 0f;  // ���݂̃X�e�[�W�̌o�ߎ���
    public static float totalTime = 0f;  // �Q�[���S�̂̍��v����
    private bool isColliding = false;
    private Color originalColor;

    // �X�e�[�W���Ƃ̎��Ԃ�ۑ����郊�X�g
    private List<float> stageTimes = new List<float>();

    private void Start()
    {
        originalColor = timerText.color;
    }

    private void Update()
    {
        if (!isColliding)
        {
            // �X�e�[�W���Ԃ��X�V
            stageTime += Time.deltaTime;
            totalTime += Time.deltaTime;

            // �^�C�}�[��\��
            timerText.text = stageTime.ToString("F1") + "s";
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D���Ă΂�܂���");  // ���\�b�h���Ă΂�Ă��邩�m�F

        if (other.CompareTag(timerTag))
        {
            Debug.Log("�^�C�}�[���Փ˂��܂���");  // �^�O����v�����ꍇ�̊m�F

            isColliding = true;

            // �^�C�}�[��Ԃ�����
            StartCoroutine(ChangeTimerColor());

            // �X�e�[�W�N���A���̕b����ۑ�
            SaveStageTime();

            // ���̃X�e�[�W�ɐi�ނ��߂Ƀ^�C�}�[���Z�b�g�i�X�e�[�W�i�s�j
            if (stageTimes.Count < 10) // �ő�10�X�e�[�W
            {
                stageTime = 0f;
            }
        }
    }


    public void SaveStageTime()
    {
        // ���݂̃X�e�[�W���Ԃ����X�g�ɒǉ�
        stageTimes.Add(stageTime);

        // PlayerPrefs���g���Ċe�X�e�[�W�̎��Ԃ�ۑ�
        PlayerPrefs.SetFloat("Stage" + stageTimes.Count, stageTime);
        PlayerPrefs.SetFloat("TotalTime", totalTime);

        // �X�e�[�W���Ƃ̎��Ԃ�\��
        Debug.Log("Stage " + stageTimes.Count + ": " + stageTime.ToString("F1") + "s");
    }

    private IEnumerator ChangeTimerColor()
    {
        // �^�C�}�[��Ԃ�����
        timerText.color = Color.red;
        yield return new WaitForSeconds(1f);  // 1�b�ҋ@
        timerText.color = originalColor;  // ���̐F�ɖ߂�
    }
}

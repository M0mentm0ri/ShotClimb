using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    public TextMeshProUGUI timerText;  // �^�C�}�[��\������TextMeshPro��UI
    public TimerManager timerManager;  // TimerManager�̎Q�ƁiInspector���犄�蓖�āj

    private Color originalColor;

    private void Start()
    {
        originalColor = timerText.color;

        // TimerManager���C���X�y�N�^���犄�蓖�ĂĂ��Ȃ��ꍇ�́AFindObjectOfType�Ō���
        if (timerManager == null)
        {
            timerManager = FindObjectOfType<TimerManager>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("timerTag"))
        {
            Debug.Log("�^�C�}�[���Փ˂��܂���");

            // �^�C�}�[��Ԃ�����
            StartCoroutine(ChangeTimerColor());

            // �^�C�}�[�̐F��ύX������A�X�e�[�W���Ԃ�ۑ�����
            timerManager.SaveStageTime();
        }
    }

    private IEnumerator ChangeTimerColor()
    {
        // �^�C�}�[��Ԃ�����
        timerText.color = Color.red;
        yield return new WaitForSeconds(1f);  // 1�b�ԐԂ�����
        timerText.color = originalColor;  // ���̐F�ɖ߂�
    }
}

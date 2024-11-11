using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class PlayerCollision : MonoBehaviour
{
    public TextMeshProUGUI timerText;  // �^�C�}�[��\������TextMeshPro��UI
    public TimerManager timerManager;  // TimerManager�̎Q�ƁiInspector���犄�蓖�āj

    private Color originalColor;  // ���̃^�C�}�[�F��ۑ�
    private HashSet<int> collidedObjectIDs = new HashSet<int>();  // �Փˍς݃I�u�W�F�N�gID��ۑ�

    private void Start()
    {
        // �^�C�}�[�̌��̐F��ۑ�
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
            int objectID = other.GetInstanceID();  // �I�u�W�F�N�g�̃��j�[�N��ID���擾

            // �Փˍς݂ł���Ώ������X�L�b�v
            if (collidedObjectIDs.Contains(objectID))
                return;

            // �V�K�Փ˂̏ꍇ�AID���Z�b�g�ɒǉ�
            collidedObjectIDs.Add(objectID);

            Debug.Log("�^�C�}�[���Փ˂��܂���");

            // �^�C�}�[��Ԃ�����
            StartCoroutine(ChangeTimerColor());

            // �X�e�[�W���Ԃ�ۑ�
            timerManager.SaveStageTime();
        }
    }

    private IEnumerator ChangeTimerColor()
    {
        // �^�C�}�[��Ԃ��ύX
        timerText.color = Color.red;
        yield return new WaitForSeconds(1f);  // 1�b�ԐԂ��܂�
        timerText.color = originalColor;  // ���̐F�ɖ߂�
    }
}

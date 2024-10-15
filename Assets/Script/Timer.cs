using UnityEngine;
using UnityEngine.UI; // Text���g�p���邽��

public class Timer : MonoBehaviour
{
    public Text timerText; // UI��Text�R���|�[�l���g
    private float elapsedTime = 0f;

    void Update()
    {
        elapsedTime += Time.deltaTime; // �o�ߎ��Ԃ��X�V
        timerText.text = elapsedTime.ToString("F2") + "�b"; // ��ʂɕ\��
    }
}

using UnityEngine;
using TMPro; // TextMeshPro���g�p���邽��

public class TimerDisplayTMP : MonoBehaviour
{
    public TextMeshProUGUI timerText; // TextMeshProUGUI�̎Q��
    private float elapsedTime = 0f;

    void Update()
    {
        elapsedTime += Time.deltaTime; // �o�ߎ��Ԃ��X�V
        timerText.text = elapsedTime.ToString("F2") + " �b"; // TextMeshPro�ɕ\��
    }
}

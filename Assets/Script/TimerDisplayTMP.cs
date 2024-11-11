using UnityEngine;
using TMPro;

public class TimerDisplayTMP : MonoBehaviour
{
    public TextMeshProUGUI timerText;          // �^�C�}�[�p�̃e�L�X�g
    public TextMeshProUGUI[] bestTimeTexts;    // �x�X�g�^�C���p�̃e�L�X�g�i�z��j
    private float elapsedTime = 0f;
    private bool isTimerRunning = true;

    void Update()
    {
        if (isTimerRunning)
        {
            elapsedTime += Time.deltaTime;
            timerText.text = elapsedTime.ToString("F2") + " �b";
        }
    }

    public void StopTimer()
    {
        isTimerRunning = false;
        UpdateBestTimes();
    }

    void UpdateBestTimes()
    {
        float[] bestTimes = new float[3];
        bestTimes[0] = PlayerPrefs.GetFloat("BestTime1", float.MaxValue);
        bestTimes[1] = PlayerPrefs.GetFloat("BestTime2", float.MaxValue);
        bestTimes[2] = PlayerPrefs.GetFloat("BestTime3", float.MaxValue);

        for (int i = 0; i < bestTimes.Length; i++)
        {
            if (elapsedTime < bestTimes[i])
            {
                for (int j = bestTimes.Length - 1; j > i; j--)
                {
                    bestTimes[j] = bestTimes[j - 1];
                }
                bestTimes[i] = elapsedTime;
                break;
            }
        }

        PlayerPrefs.SetFloat("BestTime1", bestTimes[0]);
        PlayerPrefs.SetFloat("BestTime2", bestTimes[1]);
        PlayerPrefs.SetFloat("BestTime3", bestTimes[2]);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // �S�[���I�u�W�F�N�g�ɓ��������Ƃ�
        if (other.CompareTag("ScoreTriggerTag")) // �S�[���I�u�W�F�N�g�̃^�O���r
        {
            bestTimeTexts[0].gameObject.SetActive(true); // �x�X�g�^�C���e�L�X�g��\��
            UpdateBestTimeTexts();
        }
    }

    private void UpdateBestTimeTexts()
    {
        for (int i = 0; i < bestTimeTexts.Length; i++)
        {
            float bestTime = PlayerPrefs.GetFloat("BestTime" + (i + 1), float.MaxValue);
            bestTimeTexts[i].text = "Best Time " + (i + 1) + ": " +
                (bestTime != float.MaxValue ? bestTime.ToString("F2") : "�Ȃ�") + " �b";
        }
    }



}

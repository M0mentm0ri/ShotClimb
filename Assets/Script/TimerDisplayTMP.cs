using UnityEngine;
using TMPro; // TextMeshProを使用するため

public class TimerDisplayTMP : MonoBehaviour
{
    public TextMeshProUGUI timerText; // TextMeshProUGUIの参照
    private float elapsedTime = 0f;

    void Update()
    {
        elapsedTime += Time.deltaTime; // 経過時間を更新
        timerText.text = elapsedTime.ToString("F2") + " 秒"; // TextMeshProに表示
    }
}

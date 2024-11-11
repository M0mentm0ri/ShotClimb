using UnityEngine;
using UnityEngine.UI;

public class ClearDisplay : MonoBehaviour
{
    public Text stageTimeText;  // ステージタイム表示用テキストUI
    public Text totalTimeText;  // 合計タイム表示用テキストUI

    void Start()
    {
        float stageTime = PlayerPrefs.GetFloat("StageTime", 0f);
        float totalTime = PlayerPrefs.GetFloat("TotalTime", 0f);

        stageTimeText.text = "ステージタイム: " + stageTime.ToString("F2") + "秒";
        totalTimeText.text = "合計タイム: " + totalTime.ToString("F2") + "秒";
    }
}

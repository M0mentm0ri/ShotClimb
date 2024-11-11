using UnityEngine;
using TMPro;

public class GameClearUI : MonoBehaviour
{
    // 10個のTextMeshProUGUIをインスペクタで設定
    public TextMeshProUGUI[] stageTimesText;  // ステージタイム用のTextMeshProの配列
    public TextMeshProUGUI totalTimeText;     // 合計タイム用のTextMeshPro

    private void Start()
    {
        // ステージごとのタイムを表示
        for (int i = 0; i < 9; i++)  // 10ステージ分ループ
        {
            if (i < stageTimesText.Length)  // 配列の範囲内であることを確認
            {
                // PlayerPrefsから "Stage1", "Stage2", ... のタイムを取得
                float stageTime = PlayerPrefs.GetFloat("Stage" + (i + 1), 0f);  // "Stage1"〜"Stage10" のタイムを取得
                stageTimesText[i].text = (i + 1) + "階" + " " + stageTime.ToString("F1") + "秒";  // 秒単位で表示
            }
        }

        // 合計タイムを表示
        float totalTime = PlayerPrefs.GetFloat("TotalTime", 0f);  // "TotalTime" の合計タイムを取得
        totalTimeText.text = "登頂時間:" + totalTime.ToString("F1") + "秒";  // 合計タイムを表示
    }
}

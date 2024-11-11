using UnityEngine;
using TMPro;

public class crealTime : MonoBehaviour
{
    public TextMeshProUGUI gameClearTimeText;  // ゲームクリア画面に表示する合計秒数のTextMeshPro
    public TextMeshProUGUI stageTimesText;  // ステージごとの秒数を表示するTextMeshPro

    private void Start()
    {
        // ゲームクリア画面に合計時間を表示
        gameClearTimeText.text = "Total Time: " + Mathf.Floor(PlayerPrefs.GetFloat("TotalTime")) + "s\n";

        // 各ステージの時間を表示
        for (int i = 1; i <= 10; i++)  // 最大10ステージ
        {
            float stageTime = PlayerPrefs.GetFloat("Stage" + i);
            if (stageTime > 0)
            {
                stageTimesText.text += "Stage " + i + ": " + Mathf.Floor(stageTime) + "s\n";
            }
        }
    }
}

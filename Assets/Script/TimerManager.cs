using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TimerManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;  // タイマーを表示するTextMeshProのUI
    public string timerTag = "timerTag";  // 衝突判定用のタグ
    public float stageTime = 0f;  // 現在のステージの経過時間
    public static float totalTime = 0f;  // ゲーム全体の合計時間
    private bool isColliding = false;
    private Color originalColor;

    // ステージごとの時間を保存するリスト
    private List<float> stageTimes = new List<float>();

    private void Start()
    {
        originalColor = timerText.color;
    }

    private void Update()
    {
        if (!isColliding)
        {
            // ステージ時間を更新
            stageTime += Time.deltaTime;
            totalTime += Time.deltaTime;

            // タイマーを表示
            timerText.text = stageTime.ToString("F1") + "s";
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2Dが呼ばれました");  // メソッドが呼ばれているか確認

        if (other.CompareTag(timerTag))
        {
            Debug.Log("タイマーが衝突しました");  // タグが一致した場合の確認

            isColliding = true;

            // タイマーを赤くする
            StartCoroutine(ChangeTimerColor());

            // ステージクリア時の秒数を保存
            SaveStageTime();

            // 次のステージに進むためにタイマーリセット（ステージ進行）
            if (stageTimes.Count < 10) // 最大10ステージ
            {
                stageTime = 0f;
            }
        }
    }


    public void SaveStageTime()
    {
        // 現在のステージ時間をリストに追加
        stageTimes.Add(stageTime);

        // PlayerPrefsを使って各ステージの時間を保存
        PlayerPrefs.SetFloat("Stage" + stageTimes.Count, stageTime);
        PlayerPrefs.SetFloat("TotalTime", totalTime);

        // ステージごとの時間を表示
        Debug.Log("Stage " + stageTimes.Count + ": " + stageTime.ToString("F1") + "s");
    }

    private IEnumerator ChangeTimerColor()
    {
        // タイマーを赤くする
        timerText.color = Color.red;
        yield return new WaitForSeconds(1f);  // 1秒待機
        timerText.color = originalColor;  // 元の色に戻す
    }
}

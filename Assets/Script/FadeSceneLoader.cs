using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeSceneLoader : MonoBehaviour
{
    public Image fadePanel;             // フェード用のUIパネル（Image）
    public float fadeDuration = 1.0f;   // フェードの完了にかかる時間
    public float delayBeforeFade = 2.0f; // フェード開始前の遅延時間（秒）

    private bool isSpacePressed = false; // Spaceキーが押されたかどうか

    private void Update()
    {
        // Spaceキーが押された場合の処理
        if (Input.GetKeyDown(KeyCode.Space) && !isSpacePressed)
        {
            isSpacePressed = true; // フラグを立てる
            StartCoroutine(StartFadeAfterDelay());
        }
    }

    private IEnumerator StartFadeAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeFade); // 指定した秒数待機
        StartCoroutine(FadeOutAndLoadScene()); // フェードアウトとシーンのロードを開始
    }

    public IEnumerator FadeOutAndLoadScene()
    {
        fadePanel.enabled = true;                 // パネルを有効化
        float elapsedTime = 0.0f;                 // 経過時間を初期化
        Color startColor = fadePanel.color;       // フェードパネルの開始色を取得
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // フェードパネルの最終色を設定

        // フェードアウトアニメーションを実行
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;                        // 経過時間を増やす
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);  // フェードの進行度を計算
            fadePanel.color = Color.Lerp(startColor, endColor, t); // パネルの色を変更してフェードアウト
            yield return null;                                     // 1フレーム待機
        }

        fadePanel.color = endColor;  // フェードが完了したら最終色に設定
        SceneManager.LoadScene("Masato"); // シーンをロードしてメニューシーンに遷移
    }
}

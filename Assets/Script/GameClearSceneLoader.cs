using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameClearSceneLoader : MonoBehaviour
{
    public Image fadePanel;                 // フェード用のUIパネル（Image）
    public float fadeDuration = 1.0f;       // フェードの完了にかかる時間
    public float cameraMoveSpeed = 2.0f;    // カメラの移動速度

    // 衝突時に呼ばれるメソッド
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ゲームクリアタグに衝突した場合
        if (collision.CompareTag("GameClear"))
        {
            StartCoroutine(GameClearRoutine());
        }
    }

    private IEnumerator GameClearRoutine()
    {
        // ゲームクリア画面への遷移
        yield return StartCoroutine(FadeInAndLoadScene("GameClearScene")); // ゲームクリアシーンへフェードイン

        // フェードアウトしてタイトル画面に遷移
        yield return StartCoroutine(EndGameAnimation());
    }

    private IEnumerator FadeInAndLoadScene(string sceneName)
    {
        fadePanel.enabled = true;  // フェードパネルを有効化
        float elapsedTime = 0.0f;  // 経過時間を初期化
        Color startColor = fadePanel.color;  // パネルの開始色を取得
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0.0f); // フェードの最終色を設定（透明）

        // フェードインアニメーション
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadePanel.color = Color.Lerp(startColor, endColor, t); // フェードイン
            yield return null;
        }

        fadePanel.color = endColor;  // 最終色を設定
        SceneManager.LoadScene(sceneName); // ゲームクリアシーンに遷移
    }

    private IEnumerator EndGameAnimation()
    {
        // カメラの初期位置を取得
        float elapsedTime = 0.0f;
        Vector3 initialPosition = Camera.main.transform.position;
        Vector3 targetPosition = initialPosition + new Vector3(0, 5, 0); // 上に5ユニット移動

        while (elapsedTime < fadeDuration)
        {
            // カメラを上に移動
            elapsedTime += Time.deltaTime * cameraMoveSpeed;
            Camera.main.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / fadeDuration);
            yield return null;
        }

        // フェードアウト
        yield return StartCoroutine(FadeOutAndLoadTitleScene());
    }

    private IEnumerator FadeOutAndLoadTitleScene()
    {
        fadePanel.enabled = true; // フェードパネルを有効化
        float elapsedTime = 0.0f; // 経過時間を初期化
        Color startColor = fadePanel.color; // パネルの開始色を取得
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // フェードの最終色を設定（不透明）

        // フェードアウトアニメーション
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadePanel.color = Color.Lerp(startColor, endColor, t); // フェードアウト
            yield return null;
        }

        fadePanel.color = endColor; // 最終色を設定
        SceneManager.LoadScene("TitleScene"); // タイトルシーンに遷移
    }
}

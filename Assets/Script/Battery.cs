using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Battery : MonoBehaviour
{
    // 壊れた状態の画像（常に表示）
    public SpriteRenderer brokenImage;

    // 接触時に表示されるノーマル画像
    public SpriteRenderer normalImage;

    // 切り替えにかかる時間
    public float switchBackTime = 3f;

    // 点滅のためのアルファ値変更速度
    public float fadeSpeed = 0.05f;

    // 色の変化にかかる時間
    public float colorChangeSpeed = 1f;

    // 接触時に再生するパーティクル
    public ParticleSystem activationParticles;

    // 元に戻る時に再生するパーティクル
    public ParticleSystem resetParticles;

    // 接触時に実行するUnityEvent
    public UnityEvent onActivate;

    // 元に戻る時に実行するUnityEvent
    public UnityEvent onReset;

    // LineRenderer コンポーネント
    public LineRenderer lineRenderer;

    // 衝突判定
    private void OnTriggerStay2D(Collider2D other)
    {
        // "ShotGun" タグのオブジェクトにぶつかった時
        if (other.CompareTag("ShotGun"))
        {
            // ノーマル画像を一気に表示
            SetSpriteAlpha(normalImage, 1f);

            // パーティクル再生
            activationParticles?.Play();

            // UnityEventを実行
            onActivate?.Invoke();

            // LineRendererの色を赤に変更
            SetLineRendererAlpha(Color.red, 1f);

            // 元に戻る処理を開始
            StartCoroutine(ResetImageAfterDelay());
        }
    }

    // 透明度を設定するヘルパー関数（SpriteRenderer用）
    private void SetSpriteAlpha(SpriteRenderer spriteRenderer, float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    // LineRendererのアルファ値を設定するヘルパー関数
    private void SetLineRendererAlpha(Color baseColor, float alpha)
    {
        Color startColor = baseColor;
        startColor.a = alpha;
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = startColor;
    }

    // 一定時間後にノーマル画像を緩やかに5回点滅させてから元に戻す処理
    private IEnumerator ResetImageAfterDelay()
    {
        // 指定時間待機
        yield return new WaitForSeconds(switchBackTime);

        // ノーマル画像とラインレンダラーを緩やかに5回点滅させる
        for (int i = 0; i < 4; i++)
        {
            // フェードアウト（透明度を0にする）
            yield return StartCoroutine(FadeTo(0f, Color.blue));

            // フェードイン（透明度を1にする）
            yield return StartCoroutine(FadeTo(1f, Color.red));
        }

        // パーティクル再生
        resetParticles?.Play();

        // UnityEventを実行
        onReset?.Invoke();

        // フェードアウト（透明度を0にする）
        yield return StartCoroutine(FadeTo(0f, Color.blue));
        SetSpriteAlpha(normalImage, 0f);
        SetLineRendererAlpha(Color.blue, 1f);
    }

    // ラインレンダラーの色をシームレスに変更し、SpriteRendererの透明度を変更するコルーチン
    private IEnumerator FadeTo(float targetAlpha, Color targetColor)
    {
        // 現在のアルファ値と色を取得
        float currentAlpha = normalImage.color.a;
        Color currentLineColor = lineRenderer.startColor;

        // 色の変化を滑らかにするための補間
        Color startColor = currentLineColor; // 現在の色を保持
        float timeElapsed = 0f;
        float duration = colorChangeSpeed;

        while (!Mathf.Approximately(currentAlpha, targetAlpha) || currentLineColor != targetColor)
        {
            // 時間の経過を記録
            timeElapsed += Time.deltaTime;

            // アルファ値の変更
            currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, fadeSpeed * Time.deltaTime); // 時間を考慮してスムーズに変更
            SetSpriteAlpha(normalImage, currentAlpha);

            // 色の変更
            float t = Mathf.Clamp01(timeElapsed / duration); // 0から1の範囲に収める
            currentLineColor = Color.Lerp(startColor, targetColor, t); // 色の補間

            // ラインレンダラーに新しい色を適用
            lineRenderer.startColor = currentLineColor;
            lineRenderer.endColor = currentLineColor;

            // フレームごとに少し待機
            yield return null;
        }
    }
}

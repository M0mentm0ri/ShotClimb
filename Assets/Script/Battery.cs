using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Battery : MonoBehaviour
{
    public SpriteRenderer normalImage;
    public SpriteRenderer activatedImage;
    public float switchBackTime = 3f;
    public float fadeSpeed = 0.05f;
    public float colorChangeSpeed = 1f;
    public ParticleSystem activationParticles;
    public ParticleSystem resetParticles;
    public UnityEvent onActivate;
    public UnityEvent onAlert;
    public UnityEvent onReset;
    public LineRenderer lineRenderer;

    // コルーチンの参照を保持する変数
    private Coroutine resetCoroutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ShotGun"))
        {
            SetSpriteAlpha(activatedImage, 0f);
            activationParticles?.Play();           
            SetLineRendererAlpha(Color.red, 1f);
            onActivate?.Invoke();

            // 既存のコルーチンが実行中なら停止してリセット
            if (resetCoroutine != null)
            {
                
                StopCoroutine(resetCoroutine);
            }
            // 新しいコルーチンを開始し、参照を保存
            resetCoroutine = StartCoroutine(ResetImageAfterDelay());
        }
    }

    private void SetSpriteAlpha(SpriteRenderer spriteRenderer, float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    private void SetLineRendererAlpha(Color baseColor, float alpha)
    {
        Color startColor = baseColor;
        startColor.a = alpha;
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = startColor;
        lineRenderer.endColor = startColor;
    }

    private IEnumerator ResetImageAfterDelay()
    {
        yield return new WaitForSeconds(switchBackTime);

        for (int i = 0; i < 4; i++)
        {
            yield return StartCoroutine(FadeTo(1f, Color.red));
            onAlert.Invoke();
            yield return StartCoroutine(FadeTo(0f, Color.blue));
        }

        resetParticles?.Play();
        onReset?.Invoke();
        yield return StartCoroutine(FadeTo(1f, Color.blue));
        SetSpriteAlpha(activatedImage, 1f);
        SetLineRendererAlpha(Color.blue, 1f);

        // コルーチンが終了したら参照をクリア
        resetCoroutine = null;
    }

    private IEnumerator FadeTo(float targetAlpha, Color targetColor)
    {
        float currentAlpha = activatedImage.color.a;
        Color currentLineColor = lineRenderer.startColor;

        Color startColor = currentLineColor;
        float timeElapsed = 0f;
        float duration = colorChangeSpeed;

        while (!Mathf.Approximately(currentAlpha, targetAlpha) || currentLineColor != targetColor)
        {
            timeElapsed += Time.deltaTime;
            currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, fadeSpeed * Time.deltaTime);
            SetSpriteAlpha(activatedImage, currentAlpha);

            float t = Mathf.Clamp01(timeElapsed / duration);
            currentLineColor = Color.Lerp(startColor, targetColor, t);
            lineRenderer.startColor = currentLineColor;
            lineRenderer.endColor = currentLineColor;

            yield return null;
        }
    }
}

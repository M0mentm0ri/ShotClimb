using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Battery : MonoBehaviour
{
    // ��ꂽ��Ԃ̉摜�i��ɕ\���j
    public SpriteRenderer brokenImage;

    // �ڐG���ɕ\�������m�[�}���摜
    public SpriteRenderer normalImage;

    // �؂�ւ��ɂ����鎞��
    public float switchBackTime = 3f;

    // �_�ł̂��߂̃A���t�@�l�ύX���x
    public float fadeSpeed = 0.05f;

    // �F�̕ω��ɂ����鎞��
    public float colorChangeSpeed = 1f;

    // �ڐG���ɍĐ�����p�[�e�B�N��
    public ParticleSystem activationParticles;

    // ���ɖ߂鎞�ɍĐ�����p�[�e�B�N��
    public ParticleSystem resetParticles;

    // �ڐG���Ɏ��s����UnityEvent
    public UnityEvent onActivate;

    // ���ɖ߂鎞�Ɏ��s����UnityEvent
    public UnityEvent onReset;

    // LineRenderer �R���|�[�l���g
    public LineRenderer lineRenderer;

    // �Փ˔���
    private void OnTriggerStay2D(Collider2D other)
    {
        // "ShotGun" �^�O�̃I�u�W�F�N�g�ɂԂ�������
        if (other.CompareTag("ShotGun"))
        {
            // �m�[�}���摜����C�ɕ\��
            SetSpriteAlpha(normalImage, 1f);

            // �p�[�e�B�N���Đ�
            activationParticles?.Play();

            // UnityEvent�����s
            onActivate?.Invoke();

            // LineRenderer�̐F��ԂɕύX
            SetLineRendererAlpha(Color.red, 1f);

            // ���ɖ߂鏈�����J�n
            StartCoroutine(ResetImageAfterDelay());
        }
    }

    // �����x��ݒ肷��w���p�[�֐��iSpriteRenderer�p�j
    private void SetSpriteAlpha(SpriteRenderer spriteRenderer, float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    // LineRenderer�̃A���t�@�l��ݒ肷��w���p�[�֐�
    private void SetLineRendererAlpha(Color baseColor, float alpha)
    {
        Color startColor = baseColor;
        startColor.a = alpha;
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = startColor;
    }

    // ��莞�Ԍ�Ƀm�[�}���摜���ɂ₩��5��_�ł����Ă��猳�ɖ߂�����
    private IEnumerator ResetImageAfterDelay()
    {
        // �w�莞�ԑҋ@
        yield return new WaitForSeconds(switchBackTime);

        // �m�[�}���摜�ƃ��C�������_���[���ɂ₩��5��_�ł�����
        for (int i = 0; i < 4; i++)
        {
            // �t�F�[�h�A�E�g�i�����x��0�ɂ���j
            yield return StartCoroutine(FadeTo(0f, Color.blue));

            // �t�F�[�h�C���i�����x��1�ɂ���j
            yield return StartCoroutine(FadeTo(1f, Color.red));
        }

        // �p�[�e�B�N���Đ�
        resetParticles?.Play();

        // UnityEvent�����s
        onReset?.Invoke();

        // �t�F�[�h�A�E�g�i�����x��0�ɂ���j
        yield return StartCoroutine(FadeTo(0f, Color.blue));
        SetSpriteAlpha(normalImage, 0f);
        SetLineRendererAlpha(Color.blue, 1f);
    }

    // ���C�������_���[�̐F���V�[�����X�ɕύX���ASpriteRenderer�̓����x��ύX����R���[�`��
    private IEnumerator FadeTo(float targetAlpha, Color targetColor)
    {
        // ���݂̃A���t�@�l�ƐF���擾
        float currentAlpha = normalImage.color.a;
        Color currentLineColor = lineRenderer.startColor;

        // �F�̕ω������炩�ɂ��邽�߂̕��
        Color startColor = currentLineColor; // ���݂̐F��ێ�
        float timeElapsed = 0f;
        float duration = colorChangeSpeed;

        while (!Mathf.Approximately(currentAlpha, targetAlpha) || currentLineColor != targetColor)
        {
            // ���Ԃ̌o�߂��L�^
            timeElapsed += Time.deltaTime;

            // �A���t�@�l�̕ύX
            currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, fadeSpeed * Time.deltaTime); // ���Ԃ��l�����ăX���[�Y�ɕύX
            SetSpriteAlpha(normalImage, currentAlpha);

            // �F�̕ύX
            float t = Mathf.Clamp01(timeElapsed / duration); // 0����1�͈̔͂Ɏ��߂�
            currentLineColor = Color.Lerp(startColor, targetColor, t); // �F�̕��

            // ���C�������_���[�ɐV�����F��K�p
            lineRenderer.startColor = currentLineColor;
            lineRenderer.endColor = currentLineColor;

            // �t���[�����Ƃɏ����ҋ@
            yield return null;
        }
    }
}

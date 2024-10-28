using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameClearSceneLoader : MonoBehaviour
{
    public Image fadePanel;                 // �t�F�[�h�p��UI�p�l���iImage�j
    public float fadeDuration = 1.0f;       // �t�F�[�h�̊����ɂ����鎞��
    public float cameraMoveSpeed = 2.0f;    // �J�����̈ړ����x

    // �Փˎ��ɌĂ΂�郁�\�b�h
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �Q�[���N���A�^�O�ɏՓ˂����ꍇ
        if (collision.CompareTag("GameClear"))
        {
            StartCoroutine(GameClearRoutine());
        }
    }

    private IEnumerator GameClearRoutine()
    {
        // �Q�[���N���A��ʂւ̑J��
        yield return StartCoroutine(FadeInAndLoadScene("GameClearScene")); // �Q�[���N���A�V�[���փt�F�[�h�C��

        // �t�F�[�h�A�E�g���ă^�C�g����ʂɑJ��
        yield return StartCoroutine(EndGameAnimation());
    }

    private IEnumerator FadeInAndLoadScene(string sceneName)
    {
        fadePanel.enabled = true;  // �t�F�[�h�p�l����L����
        float elapsedTime = 0.0f;  // �o�ߎ��Ԃ�������
        Color startColor = fadePanel.color;  // �p�l���̊J�n�F���擾
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0.0f); // �t�F�[�h�̍ŏI�F��ݒ�i�����j

        // �t�F�[�h�C���A�j���[�V����
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadePanel.color = Color.Lerp(startColor, endColor, t); // �t�F�[�h�C��
            yield return null;
        }

        fadePanel.color = endColor;  // �ŏI�F��ݒ�
        SceneManager.LoadScene(sceneName); // �Q�[���N���A�V�[���ɑJ��
    }

    private IEnumerator EndGameAnimation()
    {
        // �J�����̏����ʒu���擾
        float elapsedTime = 0.0f;
        Vector3 initialPosition = Camera.main.transform.position;
        Vector3 targetPosition = initialPosition + new Vector3(0, 5, 0); // ���5���j�b�g�ړ�

        while (elapsedTime < fadeDuration)
        {
            // �J��������Ɉړ�
            elapsedTime += Time.deltaTime * cameraMoveSpeed;
            Camera.main.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / fadeDuration);
            yield return null;
        }

        // �t�F�[�h�A�E�g
        yield return StartCoroutine(FadeOutAndLoadTitleScene());
    }

    private IEnumerator FadeOutAndLoadTitleScene()
    {
        fadePanel.enabled = true; // �t�F�[�h�p�l����L����
        float elapsedTime = 0.0f; // �o�ߎ��Ԃ�������
        Color startColor = fadePanel.color; // �p�l���̊J�n�F���擾
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // �t�F�[�h�̍ŏI�F��ݒ�i�s�����j

        // �t�F�[�h�A�E�g�A�j���[�V����
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadePanel.color = Color.Lerp(startColor, endColor, t); // �t�F�[�h�A�E�g
            yield return null;
        }

        fadePanel.color = endColor; // �ŏI�F��ݒ�
        SceneManager.LoadScene("TitleScene"); // �^�C�g���V�[���ɑJ��
    }
}

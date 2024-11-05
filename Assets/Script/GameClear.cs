using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameClear : MonoBehaviour
{
    public Image fadePanel;             // �t�F�[�h�p��UI�p�l���iImage�j
    public float fadeDuration = 1.0f;   // �t�F�[�h�̊����ɂ����鎞��
    public float cameraRiseSpeed = 1.0f; // �J�������㏸���鑬�x

    private void Start()
    {
        StartCoroutine(FadeOutAndLoadScene()); // �V�[���J�n���Ƀt�F�[�h�A�E�g�ƃV�[���̃��[�h���J�n
    }

    private IEnumerator FadeOutAndLoadScene()
    {
        fadePanel.enabled = true;                 // �p�l����L����
        float elapsedTime = 0.0f;                 // �o�ߎ��Ԃ�������
        Color startColor = fadePanel.color;       // �t�F�[�h�p�l���̊J�n�F���擾
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // �t�F�[�h�p�l���̍ŏI�F��ݒ�

        // �t�F�[�h�A�E�g�A�j���[�V���������s
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;                        // �o�ߎ��Ԃ𑝂₷
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);  // �t�F�[�h�̐i�s�x���v�Z
            fadePanel.color = Color.Lerp(startColor, endColor, t); // �p�l���̐F��ύX���ăt�F�[�h�A�E�g

            // �J�������㏸�����鏈��
            Camera.main.transform.position += Vector3.up * cameraRiseSpeed * Time.deltaTime;

            yield return null;                                     // 1�t���[���ҋ@
        }

        fadePanel.color = endColor;  // �t�F�[�h������������ŏI�F�ɐݒ�
        SceneManager.LoadScene("Title"); // �V�[�������[�h���ă��j���[�V�[���ɑJ��
    }
}

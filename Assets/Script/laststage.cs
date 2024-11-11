using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class laststage : MonoBehaviour
{
    public string playerTag = "Player";        // �v���C���[�̃^�O
    public GameObject whiteOutPanel;           // �z���C�g�A�E�g�p��Panel
    public float fadeSpeed = 1.0f;             // �t�F�[�h���x
    public string gameClearScene = "GameClear1";  // �Q�[���N���A��ʂ̃V�[����

    private bool isClearing = false;
    private float alpha = 0;
    private Image panelImage;

    void Start()
    {
        // Panel��Image�R���|�[�l���g���擾
        if (whiteOutPanel != null)
        {
            panelImage = whiteOutPanel.GetComponent<Image>();
            panelImage.color = new Color(1, 1, 1, 0);  // �A���t�@��0�ɐݒ�
        }
        else
        {
            Debug.LogError("WhiteOut Panel is not assigned.");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // �v���C���[�ƐڐG�����ꍇ
        if (collision.CompareTag(playerTag) && !isClearing)
        {
            isClearing = true;
            Time.timeScale = 0;  // �Q�[���̎��Ԃ��~�߂�
        }
    }

    void Update()
    {
        if (isClearing && panelImage != null)
        {
            // �z���C�g�A�E�g�G�t�F�N�g
            alpha += fadeSpeed * Time.unscaledDeltaTime;
            panelImage.color = new Color(1, 1, 1, alpha);

            // �z���C�g�A�E�g�����������玟�̃V�[���ɑJ��
            if (alpha >= 1)
            {
                SceneManager.LoadScene(gameClearScene);
            }
        }
    }
}

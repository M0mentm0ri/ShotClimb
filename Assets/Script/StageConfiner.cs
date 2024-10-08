using UnityEngine;
using Cinemachine;

public class StageConfiner : MonoBehaviour
{
    // �X�e�[�W�̃R���W������ێ�����PolygonCollider2D
    public PolygonCollider2D stageConfiner;

    // Cinemachine�̃o�[�`�����J����
    public CinemachineVirtualCamera virtualCamera;

    // �J������Confiner�R���|�[�l���g
    public CinemachineConfiner2D confiner;

    private void Start()
    {
        // Confiner�R���|�[�l���g���擾
        if (virtualCamera != null)
        {
            confiner = virtualCamera.GetComponent<CinemachineConfiner2D>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �v���C���[�����̃R���W�����ɓ��������m�F
        if (other.CompareTag("Player")) // Player�^�O�����I�u�W�F�N�g
        {
            UpdateCameraConfiner(); // �J������Confiner���X�V
        }
    }

    private void UpdateCameraConfiner()
    {
        if (confiner != null && stageConfiner != null)
        {
            confiner.m_BoundingShape2D = stageConfiner; // �X�e�[�W�̃R���W�������J�����ɐݒ�
            Debug.Log("�J������Confiner���X�V���܂���: " + stageConfiner.name);
        }
    }
}

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

            // �ڐG�����v���C���[���烊�X�|�[���X�N���v�g���擾
            Respawn respawnScript = other.GetComponent<Respawn>();

            if (respawnScript != null)
            {
                // ���g�̃I�u�W�F�N�g�̍��W�����X�|�[���ʒu�ɐݒ�
                Vector3 newRespawnPosition = transform.position; // ���g�̃I�u�W�F�N�g�̍��W
                respawnScript.SetRespawnPosition(newRespawnPosition);
                Debug.Log("���X�|�[���ʒu���X�V���܂���: " + newRespawnPosition);
            }
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

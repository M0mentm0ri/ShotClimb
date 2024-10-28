using UnityEngine;
using Cinemachine;

public class StageConfiner : MonoBehaviour
{
    // �X�e�[�W�̃R���W������ێ�����Collider2D (PolygonCollider2D, BoxCollider2D�Ȃǉ��ł����p�\)
    public Collider2D stageConfiner;

    // Cinemachine�̃o�[�`�����J����
    public CinemachineVirtualCamera virtualCamera;

    // �J������Confiner�R���|�[�l���g
    public CinemachineConfiner2D confiner;

    // �v���C���[�̃��X�|�[���|�C���g�Ƃ��Ďg�p����GameObject
    public GameObject respawnPoint;

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

            if (respawnScript != null && respawnPoint != null)
            {
                // ���X�|�[���|�C���g�̈ʒu�����X�|�[���ʒu�ɐݒ�
                Vector3 newRespawnPosition = respawnPoint.transform.position; // ���X�|�[���|�C���g�̈ʒu
                respawnScript.SetRespawnPosition(newRespawnPosition);
                Debug.Log("���X�|�[���ʒu�����X�|�[���|�C���g�ɍX�V���܂���: " + newRespawnPosition);
            }
            else if (respawnPoint == null)
            {
                Debug.LogWarning("���X�|�[���|�C���g���ݒ肳��Ă��܂���B");
            }
        }
    }

    // �R���e�N�X�g���j���[�ŌĂяo����悤�ɂ��郁�\�b�h
    [ContextMenu("Update Camera Confiner")]
    private void UpdateCameraConfiner()
    {
        if (confiner != null && stageConfiner != null)
        {
            confiner.m_BoundingShape2D = stageConfiner; // �X�e�[�W�̃R���W�������J�����ɐݒ�
            Debug.Log("�J������Confiner���X�V���܂���: " + stageConfiner.name);
        }
    }

    [ContextMenu("Set Default Respawn Point")]
    private void SetDefaultRespawnPoint()
    {
        if (respawnPoint != null)
        {
            respawnPoint.transform.position = transform.position; // ���g�̈ʒu�����X�|�[���|�C���g�ɐݒ�
            Debug.Log("���X�|�[���|�C���g���f�t�H���g�ʒu�ɐݒ肵�܂���: " + transform.position);
        }
        else
        {
            Debug.LogWarning("���X�|�[���|�C���g���ݒ肳��Ă��܂���B");
        }
    }
}

using UnityEngine;

public class Respawn : MonoBehaviour
{
    // ���X�|�[���ʒu��ێ�����ϐ�
    public Vector3 respawnPosition;

    // ���X�|�[�������s���郁�\�b�h
    public void RespawnPlayer()
    {
        // �v���C���[�����X�|�[���ʒu�Ƀe���|�[�g������
        transform.position = respawnPosition;
        Debug.Log("�v���C���[�����X�|�[���ʒu�Ƀe���|�[�g���܂���: " + respawnPosition);
    }

    // ���X�|�[���ʒu��ݒ肷�郁�\�b�h
    public void SetRespawnPosition(Vector3 newRespawnPosition)
    {
        respawnPosition = newRespawnPosition;
        Debug.Log("�V�������X�|�[���ʒu��ݒ肵�܂���: " + respawnPosition);
    }

    private void Update()
    {
        // R�L�[�������ꂽ���m�F
        if (Input.GetKeyDown(KeyCode.R))
        {
            RespawnPlayer(); // ���X�|�[�����\�b�h���Ăяo��
        }
    }
}

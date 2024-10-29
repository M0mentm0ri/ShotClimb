using UnityEngine;

public class CloudMover : MonoBehaviour
{
    [Header("Movement Settings")]
    public Vector3 direction = Vector3.right; // �_�̓��������i�C���X�y�N�^�[�Őݒ�\�j
    public float speed = 1.0f;                // �_�̑��x�i�C���X�y�N�^�[�Őݒ�\�j

    [Header("Position Settings")]
    public Vector3 respawnPosition;           // ���[�v��ʒu�i���[���h���W�j
    public Vector3 endPosition;               // �I���ʒu�i���[���h���W�j

    void Update()
    {
        // �_���w�肳�ꂽ�����ɑ��x�������ړ�
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // �I���ʒu��ʉ߂����ꍇ�A���[�v�ʒu�Ɉړ�
        if (IsBeyondEndPosition())
        {
            Warp();
        }
    }

    void Warp()
    {
        // ���[���h���W�Ń��[�v��ʒu�Ɉړ�
        transform.position = respawnPosition;
        Debug.Log("���[�v��̈ʒu: " + transform.position);
    }

    // ���[���h���W�ŏI���ʒu��ʉ߂������ǂ������m�F���郁�\�b�h
    bool IsBeyondEndPosition()
    {
        Vector3 worldPosition = transform.position;

        // direction �ɂ���āA�ǂ̎��𒴂��������`�F�b�N����
        if (direction.x > 0 && worldPosition.x >= endPosition.x) return true;
        if (direction.x < 0 && worldPosition.x <= endPosition.x) return true;
        if (direction.y > 0 && worldPosition.y >= endPosition.y) return true;
        if (direction.y < 0 && worldPosition.y <= endPosition.y) return true;
        if (direction.z > 0 && worldPosition.z >= endPosition.z) return true;
        if (direction.z < 0 && worldPosition.z <= endPosition.z) return true;

        return false;
    }
}

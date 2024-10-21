using System.Collections;
using UnityEngine;

public class ObjectRespawn : MonoBehaviour
{
    // �v���n�u�i�������ɐ�������I�u�W�F�N�g�j
    public GameObject objectPrefab;

    // �v���n�u�̐����ʒu
    public Transform spawnPosition;

    // �����ɕK�v�Ȏ���
    public float respawnTime = 5f;

    // �j�󎞂̃p�[�e�B�N��
    public ParticleSystem destructionParticles;

    // �������̃p�[�e�B�N��
    public ParticleSystem respawnParticles;

    // ���݂̃I�u�W�F�N�g�Q��
    public GameObject currentObject;

    // ��莞�Ԍ�ɕ������邽�߂̃t���O
    private bool isRespawning = false;

    // ����������
    private void Start()
    {
        // �Q�[���J�n���ɃI�u�W�F�N�g���X�|�[��
        currentObject = Instantiate(objectPrefab, spawnPosition.position, spawnPosition.rotation, transform);
    }

    // ���t���[���̍X�V����
    private void Update()
    {
        // 1�b��1��`�F�b�N
        if (!isRespawning && currentObject == null)
        {
            // �j������m�������_�ŕ����̃R���[�`�����J�n
            StartCoroutine(RespawnObjectAfterDelay());
        }
    }

    // ��莞�Ԍ�ɃI�u�W�F�N�g�𕜊�������R���[�`��
    private IEnumerator RespawnObjectAfterDelay()
    {
        isRespawning = true;

        // �������Ԃ̑ҋ@
        yield return new WaitForSeconds(respawnTime);

        // �������̃p�[�e�B�N�����Đ�
        if (respawnParticles != null)
        {
            respawnParticles.transform.position = spawnPosition.position; // �X�|�[���ʒu�Ƀp�[�e�B�N�����Đ�
            respawnParticles.Play();
        }

        // �v���n�u�𐶐����A���݂̃I�u�W�F�N�g�ɐݒ�
        currentObject = Instantiate(objectPrefab, spawnPosition.position, spawnPosition.rotation, transform);

        // �j��p�[�e�B�N�����Đ�����ʒu�Ɉړ�
        if (destructionParticles != null && currentObject != null)
        {
            destructionParticles.transform.position = currentObject.transform.position;
        }

        // ���������������̂Ńt���O�����Z�b�g
        isRespawning = false;
    }
}

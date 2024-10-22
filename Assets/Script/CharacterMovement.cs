using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // �ړ����x
    public float climbSpeed = 3f; // �K�i��o�鑬�x
    public float targetX = 10f;   // �ڕWX���W
    public bool isOnStairs = false; // �K�i�ɂ��邩�ǂ���

    private bool isMoving = false;   // �L�����N�^�[���ړ������ǂ���

    void Update()
    {
        // Space�L�[�������ꂽ�ꍇ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isMoving = true; // �ړ����J�n
        }

        // �L�����N�^�[���ړ����̏���
        if (isMoving)
        {
            // �ڕW��X���W�ɓ��B����܂ňړ�
            if (!isOnStairs)
            {
                if (transform.position.x < targetX)
                {
                    transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                }
                else
                {
                    isOnStairs = true; // �K�i�ɓ��B�����ƌ��Ȃ�
                }
            }
            // �K�i��o�鏈��
            else
            {
                // �K�i�̍����ɉ����Ĉړ�
                transform.Translate(Vector3.up * climbSpeed * Time.deltaTime);
                // �K�i�̍����ɓ��B������ړ�������
                //if (transform.position.y >= /* �K�i�̍��� */)
                //{
                //    Debug.Log("�ړ�����");
                //    isMoving = false; // �ړ�������A�t���O�����Z�b�g
                //    isOnStairs = false; // �K�i���~���Ȃǂ̏������l��
                //}
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stairs")) // �K�i�ɐݒ肵���^�O
        {
            isOnStairs = true; // �K�i�ɐڐG������t���O�𗧂Ă�
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Stairs"))
        {
            isOnStairs = false; // �K�i���痣�ꂽ��t���O�����낷
        }
    }
}

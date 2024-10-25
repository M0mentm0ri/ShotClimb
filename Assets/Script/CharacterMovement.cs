using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;      // X�����ւ̈ړ����x
    public float targetX = 10f;       // X�����̍ŏI�ڕW���W
    public float startClimbingX = 7f; // Y�����ւ̈ړ����J�n����X���W
    public float climbSpeed = 3f;     // Y�����i������j�ւ̈ړ����x
    public float climbTime = 2f;      // Y�����ւ̈ړ��ɂ����鎞��

    private bool isMoving = false;    // �L�����N�^�[���ړ������ǂ���
    private bool isClimbing = false;  // Y�����Ɉړ������ǂ���
    private float climbTimer = 0f;    // Y�����ړ��̃^�C�}�[

    void Update()
    {
        // Space�L�[�������ꂽ�ꍇ�Ɉړ����J�n
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isMoving = true; // �ړ��t���O���I��
        }

        // �L�����N�^�[��X���W�ւ̈ړ�����
        if (isMoving)
        {
            // X���W���ڕW�ʒu�ɒB����܂ŉE�Ɉړ�
            if (transform.position.x < targetX)
            {
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

                // Y�����ւ̈ړ����J�n����X���W�ɒB�����ꍇ
                if (transform.position.x >= startClimbingX && climbTimer <= 0)
                {
                    isClimbing = true; // Y�����ւ̈ړ��t���O���I��
                    climbTimer = climbTime; // �^�C�}�[��ݒ�
                }
            }
            else
            {
                isMoving = false; // X���W�ւ̈ړ����I��
                Debug.Log("X�����ւ̈ړ�����");
            }
        }

        // Y�����Ɉړ����̏����iX���W�Ɠ�����Y���W���ړ��j
        if (isClimbing && climbTimer > 0)
        {
            transform.Translate(Vector3.up * climbSpeed * Time.deltaTime); // Y�����Ɉړ�
            climbTimer -= Time.deltaTime; // �^�C�}�[������

            // �w��̎��Ԃ��o�߂�����Y�����̈ړ����~
            if (climbTimer <= 0)
            {
                isClimbing = false;
                Debug.Log("Y�����ւ̈ړ�����");
            }
        }
    }
}

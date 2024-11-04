using UnityEngine;

public class ShotGunTrigger : MonoBehaviour
{
    private bool isGroundClose;  // �n�ʂ��߂����ǂ����̃t���O

    // ���݂̒n�ʋߐڃt���O��Ԃ��A�擾��Ƀ��Z�b�g���郁�\�b�h
    public bool CheckGroundClose()
    {
        bool currentGroundClose = isGroundClose;
        isGroundClose = false;  // ��x�擾�����烊�Z�b�g
        return currentGroundClose;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // ����Ώۂ��uGround�v�^�O�Ȃ�t���O�𗧂Ă�
        if (other.CompareTag("Ground"))
        {
            isGroundClose = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ����Ώۂ��uGround�v�^�O�Ȃ�t���O�𗧂Ă�
        if (other.CompareTag("Ground"))
        {
            isGroundClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // ����Ώۂ��uGround�v�^�O�Ȃ�t���O�𗧂Ă�
        if (other.CompareTag("Ground"))
        {
            isGroundClose = false;
        }
    }
}

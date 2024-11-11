using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlatformStretch : MonoBehaviour
{
    public enum StretchDirection
    {
        Right,
        Left
    }

    [Header("Stretch Settings")]
    public StretchDirection direction = StretchDirection.Right; // �L�΂�����
    public float stretchAmount = 2.0f;  // �L�΂�����
    public float stretchSpeed = 1.0f;   // �L�΂����x
    public float resetSpeed = 1.0f;     // ���ɖ߂鑬�x
    public GameObject followPlatform;   // �ǂɒǏ]�����鑫��I�u�W�F�N�g

    private SpriteRenderer spriteRenderer;
    private Vector2 originalSize;       // ���̃X�v���C�g�T�C�Y
    private Vector2 targetSize;         // �L�΂��ڕW�T�C�Y
    private Vector3 originalPosition;   // ���̈ʒu
    private bool isStretching = false;  // �L�΂��t���O
    private bool isResetting = false;   // ���ɖ߂��t���O

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSize = spriteRenderer.size;
        originalPosition = transform.position;  // �����ʒu��ۑ�
    }

    // �G�f�B�^�[��ł̕ύX����������
    private void OnDrawGizmosSelected()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        Vector2 previewSize = spriteRenderer.size; // �v���r���[�T�C�Y�͌��̃T�C�Y���g�p
        Vector3 previewPosition = transform.position; // �v���r���[�ʒu�͌��̈ʒu���g�p

        // �L�΂������Ɋ�Â��ăv���r���[�T�C�Y�ƈʒu��ύX
        if (direction == StretchDirection.Left)
        {
            previewSize.x += stretchAmount; // ���ɐL�΂��ꍇ�A���𑝂₷
            previewPosition = transform.position - Vector3.right * (stretchAmount / 2); // �v���r���[�ʒu�����Ɉړ�
        }
        else if (direction == StretchDirection.Right)
        {
            previewSize.x += stretchAmount; // �E�ɐL�΂��ꍇ�A���𑝂₷
            previewPosition = transform.position + Vector3.right * (stretchAmount / 2); // �v���r���[�ʒu���E�Ɉړ�
        }

        Gizmos.color = Color.green; // Gizmos�̐F��΂ɐݒ�
        Gizmos.DrawWireCube(previewPosition, previewSize); // �v���r���[�̕\��
    }

    // �������L�΂�����
    public void StretchPlatform()
    {
        isStretching = true;  // �L�΂�������J�n
        isResetting = false;  // ���ɖ߂�������~
        SetTargetSizeAndPosition();
    }

    // ������茳�ɖ߂�����
    public void ResetPlatform()
    {
        isStretching = false; // �L�΂�������~
        isResetting = true;   // ���ɖ߂�������J�n
        targetSize = originalSize;
    }

    private void Update()
    {
        if (isStretching)
        {
            // �L�΂�����
            spriteRenderer.size = Vector2.Lerp(spriteRenderer.size, targetSize, stretchSpeed * Time.deltaTime);

            // �T�C�Y�̕ω��Ɋ�Â��Ĉʒu���X�V
            UpdatePositionOnStretch();

            // �ڕW�T�C�Y�ɋ߂Â�����t���O������
            if (Vector2.Distance(spriteRenderer.size, targetSize) < 0.01f)
            {
                spriteRenderer.size = targetSize;
                isStretching = false;
            }
        }
        else if (isResetting)
        {
            // ���ɖ߂�����
            spriteRenderer.size = Vector2.Lerp(spriteRenderer.size, originalSize, resetSpeed * Time.deltaTime);

            UpdatePositionOnStretch();

            // ���̈ʒu�ɖ߂�
            if (Vector2.Distance(spriteRenderer.size, originalSize) < 0.01f)
            {
                spriteRenderer.size = originalSize;
                transform.position = originalPosition;  // ���̈ʒu�ɖ߂�
                isResetting = false;
            }
        }
    }

    // �L�΂�����̍ۂɈʒu���X�V���郁�\�b�h
    private void UpdatePositionOnStretch()
    {
        float stretchChange = spriteRenderer.size.x - originalSize.x;  // �L�΂��̕ω���

        // �ǂ̈ʒu�X�V
        if (direction == StretchDirection.Left)
        {
            transform.position = originalPosition - Vector3.right * (stretchChange / 2);
        }
        else if (direction == StretchDirection.Right)
        {
            transform.position = originalPosition + Vector3.right * (stretchChange / 2);
        }

        // ����I�u�W�F�N�g�̈ʒu��ǂ̒[�ɒǏ]������
        if (followPlatform != null)
        {
            Vector3 platformPosition = transform.position;

            // �Ǐ]��������ɂ���đ���̈ʒu��ǂ̒[�ɐݒ�
            if (direction == StretchDirection.Left)
            {
                platformPosition -= Vector3.right * (spriteRenderer.size.x / 2);
            }
            else if (direction == StretchDirection.Right)
            {
                platformPosition += Vector3.right * (spriteRenderer.size.x / 2);
            }

            followPlatform.transform.position = platformPosition;
        }
    }

    // �L�΂������Ɋ�Â��ĖڕW�T�C�Y�ƈʒu��ݒ�
    private void SetTargetSizeAndPosition()
    {
        targetSize = originalSize;

        if (direction == StretchDirection.Left)
        {
            targetSize.x += stretchAmount; // �E�����ɐL�΂�
            transform.position = originalPosition + Vector3.right * (stretchAmount / 2); // �ʒu���E�Ɉړ�
        }
        else if (direction == StretchDirection.Right)
        {
            targetSize.x += stretchAmount; // �������ɐL�΂�
            transform.position = originalPosition - Vector3.right * (stretchAmount / 2); // �ʒu�����Ɉړ�
        }
    }
}

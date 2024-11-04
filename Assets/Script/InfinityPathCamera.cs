using UnityEngine;

public class InfinityPathCamera : MonoBehaviour
{
    public float speed = 1.0f;      // �J�������ړ����鑬��
    public float width = 5.0f;      // �������̕�
    public float height = 3.0f;     // �c�����̍���
    public Vector3 centerPosition = Vector3.zero; // �ړ��̒��S�ʒu

    private float time;

    void Update()
    {
        // Space�L�[�������ꂽ�Ƃ��ɃX�N���v�g�𖳌���
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.enabled = false;  // �X�N���v�g�𖳌���
            return;
        }
        time += Time.deltaTime * speed;

        // �J�����̈ʒu�����̌`�ɐݒ�
        float x = width * Mathf.Sin(time);
        float y = height * Mathf.Sin(2 * time);

        // �J�����̐V�����ʒu
        transform.position = centerPosition + new Vector3(x, y, transform.position.z);
    }
}

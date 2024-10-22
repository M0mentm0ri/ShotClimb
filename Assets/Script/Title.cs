using UnityEngine;

public class Title : MonoBehaviour
{
    public Vector3 startPosition = new Vector3(0f, 0.31f, -10f);
    public Vector3 endPosition = new Vector3(0f, -27.83f, -10f);
    public float acceleration = 2.0f;
    public float maxSpeed = 20.0f;
    public float decelerationDistance = 5.0f; // ���̋����ɋ߂Â��ƌ������n�܂�
    private float currentSpeed = 0f;
    private bool startSliding = false;

    void Start()
    {
        // �J�����̏����ʒu��ݒ�
        transform.position = startPosition;
    }

    void Update()
    {
        // Space�L�[�������ꂽ��X���C�h�J�n
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startSliding = true;
        }

        // �X���C�h����
        if (startSliding)
        {
            float distanceToEnd = Vector3.Distance(transform.position, endPosition);

            // �I���n�_�ɋ߂Â����猸�����J�n
            if (distanceToEnd < decelerationDistance)
            {
                // �����ɉ����Č���
                currentSpeed = Mathf.Lerp(0, maxSpeed, distanceToEnd / decelerationDistance);
            }
            else
            {
                // �ʏ�̉���
                currentSpeed += acceleration * Time.deltaTime;
                currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
            }

            // �J������y�����Ɉړ�
            transform.position = Vector3.MoveTowards(transform.position, endPosition, currentSpeed * Time.deltaTime);

            // �I���n�_�ɓ��B������X���C�h���~���A�Q�[���J�n
            if (transform.position == endPosition)
            {
                startSliding = false;
                StartGame();
            }
        }
    }

    void StartGame()
    {
        // �Q�[���J�n�̏����������ɒǉ�
        Debug.Log("Game Started!");
    }
}

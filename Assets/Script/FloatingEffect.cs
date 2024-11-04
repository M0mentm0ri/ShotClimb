using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    public float floatSpeed = 1.0f;  // �ӂ�ӂ킷��X�s�[�h
    public float floatHeight = 0.5f; // �ӂ�ӂ킷�鍂��

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; // �����ʒu���L�^
    }

    void Update()
    {
        // �㉺�ɂӂ�ӂ킷�铮����ݒ�
        transform.position = startPos + new Vector3(0.0f, Mathf.Sin(Time.time * floatSpeed) * floatHeight, 0.0f);
    }
}

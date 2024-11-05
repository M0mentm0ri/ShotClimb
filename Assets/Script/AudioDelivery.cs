using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDelivery : MonoBehaviour
{
    // Inspector�Őݒ�\��AudioController
    [SerializeField] private AudioController audioController;

    // Start is called before the first frame update
    void Start()
    {
        // Inspector�Őݒ肳��Ă��Ȃ��ꍇ�́A�����Find���ăL���b�V������
        if (audioController == null)
        {
            audioController = GameObject.Find("Audio").GetComponent<AudioController>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // �I�[�f�B�I���Đ����郁�\�b�h
    public void PlayAudio(int index)
    {
        // audioController���ݒ肳��Ă��Ȃ��ꍇ�͍ēxFind�Ŏ擾����
        if (audioController == null)
        {
            audioController = GameObject.Find("Audio").GetComponent<AudioController>();
        }

        // �����Đ�����
        audioController.PlayOnSound(index);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Shaker : MonoBehaviour
{
    // Cinemachine�̃C���p���X�\�[�X
    public CinemachineImpulseSource impulseSource;

    // Start is called before the first frame update
    void Start()
    {
        // �C���p���X�\�[�X�����ݒ�̏ꍇ�A�����I�u�W�F�N�g����擾
        if (impulseSource == null)
        {
            impulseSource = GetComponent<CinemachineImpulseSource>();
        }
    }

    // �V�F�C�N�����s���郁�\�b�h
    public void Shake()
    {
        // �C���p���X�𐶐����ăJ������h�炷
        impulseSource.GenerateImpulse();
    }
}

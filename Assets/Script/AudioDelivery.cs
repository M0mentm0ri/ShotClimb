using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDelivery : MonoBehaviour
{
    // Inspectorで設定可能なAudioController
    [SerializeField] private AudioController audioController;

    // Start is called before the first frame update
    void Start()
    {
        // Inspectorで設定されていない場合は、初回にFindしてキャッシュする
        if (audioController == null)
        {
            audioController = GameObject.Find("Audio").GetComponent<AudioController>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // オーディオを再生するメソッド
    public void PlayAudio(int index)
    {
        // audioControllerが設定されていない場合は再度Findで取得する
        if (audioController == null)
        {
            audioController = GameObject.Find("Audio").GetComponent<AudioController>();
        }

        // 音を再生する
        audioController.PlayOnSound(index);
    }
}

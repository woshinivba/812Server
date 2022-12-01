using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Mytme : MonoBehaviour
{
    VideoPlayer VideoPlayer;
    Action Action;
    // Start is called before the first frame update
    void Start()
    {
        Action += jkji;
        Action = () => { };
        VideoPlayer.sendFrameReadyEvents=true;
        //VideoPlayer.frameReady
    }

    void jkji()
    {

    }
}

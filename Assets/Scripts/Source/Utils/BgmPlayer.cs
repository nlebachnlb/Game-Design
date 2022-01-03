using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmPlayer : MonoBehaviour
{
    public string bgmId;
    // Start is called before the first frame update
    void Start()
    {
        var clip = AppRoot.Instance.GetService<AudioManager>().GetBgm(bgmId);
        AppRoot.Instance.GetService<BgmController>().Play(clip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbPlayer : MonoBehaviour
{
    public string sfxId;
    public bool loop;
    // Start is called before the first frame update
    void Start()
    {
        AppRoot.Instance.GetService<SfxController>().PlayAmbience(sfxId, loop);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    public float delay;

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(delay);
        Destroy(gameObject);
    }

}

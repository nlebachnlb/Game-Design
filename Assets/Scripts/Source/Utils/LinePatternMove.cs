using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LinePatternMove : MonoBehaviour
{
    public float roundtripTime;
    public float idleTime;
    public List<Transform> points;
    public Ease easing;
    public bool reverseOnEnd;

    private IEnumerator Start()
    {
        if (points.Count > 0)
        {
            int i = 1;
            int dir = 1;
            transform.position = points[0].position;
            while (true)
            {
                transform.DOMove(points[i].position, roundtripTime).SetEase(easing);
                i += dir;
                if (reverseOnEnd)
                {
                    if (i >= points.Count || i < 0)
                    {
                        dir *= -1;
                        i += dir;
                    }
                }
                else
                    if (i >= points.Count) i = 0;

                yield return new WaitForSeconds(roundtripTime + idleTime);
            }
        }

        yield return null;
    }
}

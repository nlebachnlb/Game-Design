using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RuneBoard : BaseTrigger
{
    public List<Rune> RuneList;

    [SerializeField]
    private ParticleSystem liberation, lastLiberation;
    [SerializeField]
    private SpriteRenderer highlight;

    private bool canGetSkill = false;

    public void PlaySkillObtainFX()
    {
        StartCoroutine(SkillObtaining());
    }

    private IEnumerator SkillObtaining()
    {
        GetComponent<Animator>().SetTrigger("Highlight");
        yield return new WaitForSeconds(1);
        app.view.CameraShakeFX.Shake(new Vector2(1f, 0f), 5f, 0.25f);
        liberation.Play();
        yield return new WaitForSeconds(5);
        liberation.Stop();
        yield return new WaitForSeconds(3);
        app.view.CameraShakeFX.Shake(new Vector2(1f, 0f), 5f, 0.5f);
        lastLiberation.Play();
    }

    private void Update()
    {
        if (canGetSkill)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PlaySkillObtainFX();
                canGetSkill = false;
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    public override void OnPlayerEnter(PlayerController player)
    {
        int activatedRune = 0; 

        foreach(Rune rune in RuneList)
        {
            if(rune.isCollected)
            {
                activatedRune++;
                showRuneOnBoard(activatedRune);
            }
        }

        Debug.Log($"Collected {activatedRune}");


        if (activatedRune == RuneList.Count)
        {
            completeRuneBoardAction();
        }
            
    }

    public override void OnPlayerExit(PlayerController player)
    {
        base.OnPlayerExit(player);
    }

    private void showRuneOnBoard(int runeIndex)
    {

    }

    private void completeRuneBoardAction()
    {
        Debug.Log("fuck yeah");
        canGetSkill = true;
    }

    private void Start()
    {
        //PlaySkillObtainFX();
    }
}

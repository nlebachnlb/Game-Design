using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class RuneBoard : BaseTrigger
{
    public List<Rune> RuneList;

    [SerializeField]
    private ParticleSystem liberation, lastLiberation;
    [SerializeField]
    private GameObject playerSkillObtain;
    [SerializeField]
    private Transform skillPosition;
    [SerializeField]
    private FadeAnimation canvas;

    private PlayerController player;

    private bool canGetSkill = false;

    public void PlaySkillObtainFX()
    {
        StartCoroutine(SkillObtaining());
    }

    private IEnumerator SkillObtaining()
    {
        AppRoot.Instance.GetService<BgmController>().Stop(true, 4);

        var sfx = AppRoot.Instance.GetService<SfxController>();
        GetComponent<Animator>().SetTrigger("Highlight");
        var fx = Instantiate(playerSkillObtain, player.PlayerView.transform.position, Quaternion.identity);
        fx.transform.DOMove(skillPosition.position, 2).SetEase(Ease.OutSine);
        player.PlayerView.gameObject.SetActive(false);
        player.enabled = false;

        sfx.Play("sfx-skill-absorb");
        yield return new WaitForSeconds(1);
        app.view.CameraShakeFX.Shake(new Vector2(1f, 0f), 6f, 0.25f);
        liberation.Play();

        yield return new WaitForSeconds(4);
        sfx.Play("sfx-skill-obtain");
        yield return new WaitForSeconds(1);
        liberation.Stop();
        yield return new WaitForSeconds(3);
        app.view.CameraShakeFX.Shake(new Vector2(1f, 0f), 5f, 0.5f);
        lastLiberation.Play();

        yield return new WaitForSeconds(3);
        player.PlayerView.transform.position = fx.gameObject.transform.position;
        player.PlayerView.gameObject.SetActive(true);
        Destroy(fx.gameObject);

        yield return new WaitForSeconds(2);

        Camera.main.transform.DOLocalMoveY(67, 3f).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(5);

        Camera.main.transform.DOLocalMoveY(0, 2f).SetEase(Ease.InOutSine);

        player.enabled = true;
        player.PlayerModel.wallJumpSkill = true;
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

        this.player = player;    
    }

    public override void OnPlayerExit(PlayerController player)
    {
        base.OnPlayerExit(player);
        GetComponent<Animator>().SetTrigger("LightOff");
        canvas.FadeOut();
    }

    private void showRuneOnBoard(int runeIndex)
    {

    }

    private void completeRuneBoardAction()
    {
        Debug.Log("fuck yeah");
        canGetSkill = true;
        GetComponent<Animator>().SetTrigger("Highlight");
        canvas.FadeIn();
    }

    private void Start()
    {
        //PlaySkillObtainFX();
    }
}

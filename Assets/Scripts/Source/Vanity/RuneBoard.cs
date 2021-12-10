using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneBoard : BaseTrigger
{
    public List<Rune> RuneList;

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
    }
}

using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AnimationManager : Singleton_IndependentObject<AnimationManager>
{
    public Image chipsImage;

    public void PlayAnimation(Transform playerChipsStack, Transform betStack, int numChipsToMove = 1, float animationDuration = 0.5f)
    {
        StartCoroutine(Animation(playerChipsStack, betStack, animationDuration, numChipsToMove));
    }

    IEnumerator Animation(Transform playerChipsStack, Transform betStack, float animationDuration, int numChipsToMove)
    {
        chipsImage.enabled = false;
        yield return new WaitForSeconds(0.1f);
        while (numChipsToMove > 0)
        {
            chipsImage.enabled = false;
            transform.position = playerChipsStack.position;
            chipsImage.enabled = true;
            yield return new WaitForSeconds(animationDuration);
            transform.DOMove(betStack.position, animationDuration);

            yield return new WaitForSeconds(animationDuration + 0.1f);
            chipsImage.enabled = false;
            numChipsToMove--;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WaitingForOtherPlayers : UI_Panel
{
    [SerializeField] GameObject chip;
    [SerializeField] Text waitingText;
    bool textAnimate = true;

    private void Update()
    {
        if (textAnimate)
            StartCoroutine(TextStyle(0.5f));
        RotateChips();
    }
    void RotateChips()
    {
        chip.transform.Rotate(0, 0,20 * Time.deltaTime);
    }

    IEnumerator TextStyle(float t)
    {
        textAnimate = false;
        waitingText.text = "Waiting for other players.";
        yield return new WaitForSeconds(t);
        waitingText.text = "Waiting for other players..";
        yield return new WaitForSeconds(t);
        waitingText.text = "Waiting for other players...";
        yield return new WaitForSeconds(t);
        waitingText.text = "Waiting for other players.";
        textAnimate = true;
    }
}

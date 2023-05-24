using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeneratedCard : MonoBehaviour
{
    public Sprite[] suitSprites;

    public Image cardImage;
    public Image unrevealed;
    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("Clicked");
            if (!Dealer.instance) return;
            if (Dealer.instance.currentPlayerBoy.generatedCards[0] == this
            || Dealer.instance.currentPlayerBoy.generatedCards[1] == this)
                RavealCard(unrevealed.enabled);
        });
    }
    public void SetValue(Card card)
    {
        cardImage.sprite = SpritesHolder.instance.HaveCard(card);
        // suitImage.SetNativeSize();
        unrevealed.enabled = false;
    }

    public void RavealCard(bool value) => unrevealed.enabled = !value;
}

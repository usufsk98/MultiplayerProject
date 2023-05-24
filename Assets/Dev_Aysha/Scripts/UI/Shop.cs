using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] Text chips;
    [SerializeField] int _price;
    [SerializeField] GameObject blocker;
    void Start()
    {
        chips.text = EconomyManager.instance.GetEconomy().ToString();
    }

    public void OnPurchaseComplete(Product product)
    {
#if UNITY_EDITOR
        Debug.Log("onpurchase complete: " + product.metadata.localizedPrice);
        StartCoroutine(DisableIAP(product));
#else
            UI_Info panel = (UI_Info)UI_Manager.instance.OpenPanel(typeof(UI_Info),false);
            panel.DisplayMessage("Purchase Successful");
            AddEconomy();
#endif
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        blocker.SetActive(false);
        Debug.Log("Purchase of product :" + product.definition.id + " is failed due to :" + failureReason);
        UI_Info panel = (UI_Info)UI_Manager.instance.OpenPanel(typeof(UI_Info), false);
        panel.DisplayMessage("Purchase failed.Please try again.");
    }
    private IEnumerator DisableIAP(Product product)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("Purchase of the product: is Successful in editor.");
        UI_Info panel = (UI_Info)UI_Manager.instance.OpenPanel(typeof(UI_Info), false);
        panel.DisplayMessage("Purchase Successful");
        AddEconomy();
    }

    public void PurchasePrice(int price)
    {
#if UNITY_EDITOR
        blocker.SetActive(true);
#endif
        _price = price;
        Debug.Log("_price before: " + _price + "obj: " + gameObject.name);
    }

    void AddEconomy()
    {
        int rewardValue = 0;

        Debug.Log("_price after: " + _price + "obj: " + gameObject.name);

        switch (_price)
        {
            case 7:
                EconomyManager.instance.SetEconomy(100);
                rewardValue = 100;
                break;
            case 10:
                EconomyManager.instance.SetEconomy(200);
                rewardValue = 200;
                break;
            case 13:
                EconomyManager.instance.SetEconomy(450);
                rewardValue = 450;
                break;
            case 22:
                EconomyManager.instance.SetEconomy(730);
                rewardValue = 730;
                break;
            case 79:
                EconomyManager.instance.SetEconomy(1250);
                rewardValue = 1250;
                break;
        }
        blocker.SetActive(false);
        chips.text = EconomyManager.instance.GetEconomy().ToString();
        UI_Info panel = (UI_Info)UI_Manager.instance.OpenPanel(typeof(UI_Info), false);
        panel.DisplayMessage("You got " + rewardValue.ToString());
    }

}

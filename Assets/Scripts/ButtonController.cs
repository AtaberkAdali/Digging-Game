using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonController : MonoBehaviour
{
    public GameObject digGameObject;
    public Sprite[] digObjeSprites; //kazma, k�rek, buldozer.
    public GameObject buldozerLockedImage, finishTheGameTextGO, buyBuldozerPanelGO;
    public GameObject[] factoryPanel;

    public GameObject infoCard, infoImage;
    public TextMeshProUGUI infoText;

    private bool isBuldozerUnlocked = true;

    private DigGameObjeScript digObj;
    private GiftManager buttonGiftManager;

    private void Start()
    {
        digObj = FindObjectOfType<DigGameObjeScript>();
        buttonGiftManager = FindObjectOfType<GiftManager>();
    }

    public void ChosePickaxe()
    {
        digGameObject.GetComponent<SpriteRenderer>().sprite = digObjeSprites[0];
        digObj.whichToolStateControl = 0;
    }
    public void ChoseShovle()
    {
        digGameObject.GetComponent<SpriteRenderer>().sprite = digObjeSprites[1];
        digObj.whichToolStateControl = 1;
    }
    public void ChoseBuldozer()
    {
        if (!isBuldozerUnlocked)
        {
            digGameObject.GetComponent<SpriteRenderer>().sprite = digObjeSprites[2];
            digObj.whichToolStateControl = 2;
        }
        else
        {
            StartCoroutine(digObj.DisplayMessage("Buldozer kilitli.", 2));
        }
    }

    public void CloseFactoryPanel()
    {
        factoryPanel[0].SetActive(false);
        factoryPanel[1].SetActive(false);
        digObj.isPanelOpen = false;
    }
    public void OpenFactoryPanel()
    {
        factoryPanel[0].SetActive(true);
        factoryPanel[1].SetActive(true);
        digObj.isPanelOpen = true;
        buttonGiftManager.UpdateMicroDisplays();
        buttonGiftManager.UpdateAllTexts();
    }
    public void GiveEnergy()
    {
        Debug.Log("give enerji0");
        if(buttonGiftManager.goldO.Amount > 45)
        {
            buttonGiftManager.goldO.Amount -= 45;
            buttonGiftManager.energyO.Amount += 1;
            buttonGiftManager.UpdateAllTexts();
            StartCoroutine(buttonGiftManager.DecreaseText(45));
            StartCoroutine(buttonGiftManager.IncreaseText(1));
        }
        else
        {
            StartCoroutine(digObj.DisplayMessage("Yeterli para yok.", 3));
        }
    }
    public void BuyBuldozer()
    {
        if (FindObjectOfType<GiftManager>().goldO.Amount > 500) {
            bool hasAllResearcheBeenDone = false;
            int micro = 0;
            for (int i = 0; i < buttonGiftManager.microGiftO.Length; i++)
            {
                if (buttonGiftManager.microGiftO[i].IsUnlocked)
                {
                    micro++;
                }
            }
            if (buttonGiftManager.microGiftO.Length <= micro)
            {
                hasAllResearcheBeenDone = true;
            }


            if (hasAllResearcheBeenDone)
            {
                //Burada digger'� a�aca��z.
                FindObjectOfType<GiftManager>().goldO.Amount -= 500;
                FindObjectOfType<GiftManager>().UpdateAllTexts();
                StartCoroutine(buttonGiftManager.DecreaseText(500));
                StartCoroutine(buttonGiftManager.IncreaseText(1));
                buyBuldozerPanelGO.SetActive(false);
                finishTheGameTextGO.SetActive(true);
                buldozerLockedImage.SetActive(false);
                isBuldozerUnlocked = false;
            }
            else
            {
                StartCoroutine(digObj.DisplayMessage("B�t�n ara�t�rmalar yap�lmam��.", 2));
            }
        }
        else
        {
            StartCoroutine(digObj.DisplayMessage("Yeterli para yok.", 2));
        }
    }
    public void CloseInfoCard()
    {
        infoCard.SetActive(false);
    }

    public void OpenMicro1()
    {
        OpenCard(0, "Basit Bakteri: Bu bulu� biyoloji alan�nda ilerlemeni sa�lad� ama zaten d�nyaca bilinen bir �eydi daha ke�fetmen gereken bir �ok �ey var.");
    }
    public void OpenMicro2()
    {
        OpenCard(1, "Basit Vir�s: Bu bulu� biyoloji alan�nda ilerlemeni sa�lad� ama sadece bilimini geli�tirdi litaret�re direkt bir faydas� mevcut de�il.");
    }
    public void OpenMicro3()
    {
        OpenCard(2, "Komplex Bakteri: Bu ke�if kimya ve biyoloji alan�nda ilerlemeni sa�lad� ve bu ke�iften esinlenen birisi buldozer di�lisini icat etti!");
    }
    public void OpenMicro4()
    {
        OpenCard(3, "Komplex Vir�s: Bu ke�if kimya ve biyoloji alan�nda �ok b�y�k ilerlemelere yol a�t�. Bu ke�iften esinlenen birisi buldozer motorunu icat etti!!");
    }
    public void OpenMicro5()
    {
        OpenCard(4, "Basit Ta�: Bu bulu� jeofizik alan�nda ilerlemeni sa�lad� ama zaten d�nyaca bilinen bir �eydi daha ke�fetmen gereken bir �ok �ey var.");
    }
    public void OpenMicro6()
    {
        OpenCard(5, "Basit Metal: Bu bulu� jeofizik alan�nda ilerlemeni sa�lad� ama sadece bilimini geli�tirdi litaret�re direkt bir faydas� mevcut de�il.");
    }
    public void OpenMicro7()
    {
        OpenCard(6, "Komplex Ta�: Bu ke�if fizik ve jeofizik alan�nda ilerlemeni sa�lad� ve bu ke�if sayesinde d�nyada buldozer ihtiyac� ortaya ��kt�!");
    }
    public void OpenMicro8()
    {
        OpenCard(7, "Komplex Metal: Bu ke�if fizik ve jeofizik alan�nda �ok b�y�k ilerlemelere yol a�t�. Bu ke�if sayesinde buldozerlerin bilinen b�t�n par�alar�n�n �retimi ba�lad�!!");
    }

    private void OpenCard(int microNum, string text)
    {
        if(buttonGiftManager.microGiftO[microNum].Amount > 0)
        {
            infoCard.SetActive(true);
            infoImage.GetComponent<Image>().sprite = buttonGiftManager.microGiftO[microNum].gift.GetComponent<SpriteRenderer>().sprite;
            if (!buttonGiftManager.microGiftO[microNum].IsUnlocked)
            {
                infoText.text = text.ToString() + "\nK�L�T A�ILDI!";
                buttonGiftManager.microGiftO[microNum].IsUnlocked = true;
                buttonGiftManager.UpdateMicroDisplays();
            }
            else
            {
                infoText.text = text.ToString();
            }
        }
        else
        {
            StartCoroutine(digObj.DisplayMessage("Hen�z ke�fetmedin.", 2));
        }
    }
}

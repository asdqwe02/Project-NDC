using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ItemTooltip : MonoBehaviour
{
    private RectTransform backgroundRectTransfrom;
    private RectTransform canvasRecTransform;
    private RectTransform rectTransform;
    private TextMeshProUGUI modifiersText;
    private RectTransform itemImage;
    
    private Image itemSprite;
    public static ItemTooltip instance; //only having 1 instance of item tooltip for now
    void Awake()
    {
       if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        // Debug.Log("item tool tip instance "+ instance);
        backgroundRectTransfrom = transform.Find("Background").GetComponent<RectTransform>();
        itemImage = transform.Find("ItemImage").GetComponent<RectTransform>();
        itemSprite = itemImage.transform.Find("Image").GetComponent<Image>();
        modifiersText = transform.Find("Modifiers").GetComponent<TextMeshProUGUI>(); 
        canvasRecTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();
        rectTransform = transform.GetComponent<RectTransform>();
        DontDestroyOnLoad(gameObject);
        gameObject.SetActive(false);

    }
    private void Update() {
        if (gameObject.activeSelf)
        {
            Vector2 popupMessagePos =  Input.mousePosition;
            // Debug.Log(popupMessagePos);
            // Right 
            if (popupMessagePos.x + backgroundRectTransfrom.rect.width/2 > canvasRecTransform.rect.width)
                popupMessagePos.x = canvasRecTransform.rect.width - backgroundRectTransfrom.rect.width/2;
            // Left 
            if  ( popupMessagePos.x - backgroundRectTransfrom.rect.width/2 < 0f)
            {
                popupMessagePos.x = backgroundRectTransfrom.rect.width/2;
            }
            // Top
            if (popupMessagePos.y > canvasRecTransform.rect.height)
                popupMessagePos.y = canvasRecTransform.rect.height;
            // Bottom
            if (popupMessagePos.y - backgroundRectTransfrom.rect.height < 0f)
                popupMessagePos.y = backgroundRectTransfrom.rect.height;
            rectTransform.position = popupMessagePos;
        }
    }
    public void ShowItemToolTip(ItemController itemController)
    {
        gameObject.SetActive(true);

        Item iteminfo = itemController.Item;
        itemSprite.sprite = iteminfo.GetCurrentSprite();
        string popupMessage = "";
        foreach(var modifer in iteminfo.modifiers)
        {
            string message = modifer.modData["popupMessage"]["message"].ToString();
            string value =  modifer.modData["popupMessage"]["value"].ToString();
            string roll = "";
            if (modifer.modData["range"]!=null)
            {
                roll = " " + modifer.rollValue.ToString() + " ";
            }
            popupMessage += $"{message}{roll}{value}\n";
        }                        
        modifiersText.SetText(popupMessage);
        modifiersText.ForceMeshUpdate();

        Vector2 textSize = modifiersText.GetRenderedValues(false);
        float paddingWidth = itemImage.sizeDelta.x;
        float paddingHeight = itemImage.sizeDelta.y + 50f;
        Vector2 padding = new Vector2(paddingWidth,paddingHeight);
        backgroundRectTransfrom.sizeDelta = textSize + padding ;

    }
    public void HideItemToolTip()
    {
        modifiersText.SetText("");
        gameObject.SetActive(false);
    }
}

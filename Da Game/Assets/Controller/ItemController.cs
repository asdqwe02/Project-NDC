using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
// using Newtonsoft.Json.Linq;
public class ItemController : Interactable,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler // control item behavior
{
 
    public ItemSpriteData itemSpriteData;
    private Item item = new Item();
    public Item Item { get => item; set => item = value; }
    private Image image;
    private bool inInventory = false;
    public List<Modifier> itemMod = new List<Modifier>(); // for testing and debugging purpose
    [SerializeField] private bool equiped = false;
    private bool mouseOver=false;
    [SerializeField] private GameObject itemContainer;

    void Awake()
    {
        Item.Initialize(itemSpriteData.itemsIcon);
        GetComponent<SpriteRenderer>().sprite = Item.GetCurrentSprite();
        image = GetComponent<Image>();
        image.sprite =  Item.GetCurrentSprite();
        SetUpItemModifiers();
        itemMod = Item.modifiers;
    }
    public override void Interact()
    {
        Debug.Log("interact with item");
        GetComponent<Renderer>().enabled = false;

        inInventory = InventoryController.instance.AddItem(gameObject);
        if (ItemTooltip.instance.gameObject.activeSelf)
            ItemTooltip.instance.HideItemToolTip();
    }

    private void SetUpItemModifiers()
    {
        int amount = Random.Range(1,4);
        switch (Item.itemType)
        {
            case ItemType.attack:
                Item.modifiers = Utilities.WeaponModifierPool.RollMultipleModifier(amount);
                break;
            case ItemType.defense:
                Item.modifiers = Utilities.ArmourModifierPool.RollMultipleModifier(amount);
                break;
            default:
                Debug.Log("error can't find item type of the item " + Item.ItemName);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        TurnOnIIcon(other);

    }
    private void OnTriggerExit2D(Collider2D other) {
       TurnOffIIcon(other);
    }
    public void ApplyItemEffect(){
        Debug.Log($"apply {Item.ItemName} effect");
        foreach (var modifier in Item.modifiers)
        {
            switch (modifier.modData["name"].ToString())
            {
                case "add damage":
                    PlayerController.instance.BaseDamage+=modifier.rollValue;
                    PlayerController.instance.CalculateDamage();
                    break;
                case "increase damage":
                    PlayerController.instance.PercentDamageIncrease+=(float)modifier.rollValue/100f;
                    PlayerController.instance.CalculateDamage();
                    break;
                case "increase attack speed":
                    PlayerController.instance.PercentAttackSpeedIncrease+=(float)modifier.rollValue/100f;
                    PlayerController.instance.CalculateAttackSpeed();
                    break;
                case "spread shot":
                 if (PlayerController.instance.FireType !=2)
                    {
                        InventoryController.instance.SetPlayerPreviousFireType(PlayerController.instance.FireType);
                        PlayerController.instance.FireType = 2;
                    }
                    PlayerController.instance.PercentDamageIncrease -= (float)modifier.modData["damageReduce"];
                    PlayerController.instance.PercentAttackSpeedIncrease -= (float)modifier.modData["attackSpeed"];
                    PlayerController.instance.CalculateAttackSpeed();
                    PlayerController.instance.CalculateDamage();
                    break;
                case "hp":
                    if (PlayerController.instance.Hp == PlayerController.instance.MaxHP)
                        PlayerController.instance.Hp+=modifier.rollValue;
                    PlayerController.instance.MaxHP+=modifier.rollValue;
                    break;
                case "armour":
                    PlayerController.instance.Armour +=modifier.rollValue;
                    break;
                case "fire resist":
                    PlayerController.instance.FireResistance +=modifier.rollValue;
                    break;
                case "cold resist":
                    PlayerController.instance.ColdResistance +=modifier.rollValue;
                    break;
                case "lightning resist":
                    PlayerController.instance.LightningResistance +=modifier.rollValue;
                    break;
                case "move speed":
                    PlayerController.instance.MovementSpeed+=modifier.rollValue;
                    break;
                default:
                    Debug.Log("empty modifer in item");
                    break;
            }
        }
    }
    public void RemoveItemEffect(){
        Debug.Log($"remove {Item.ItemName} effect");
        foreach (var modifier in Item.modifiers)
        {
            switch (modifier.modData["name"].ToString())
            {
                
                case "add damage":
                    PlayerController.instance.BaseDamage -= modifier.rollValue;
                    PlayerController.instance.CalculateDamage();
                    break;
                case "increase damage":
                    PlayerController.instance.PercentDamageIncrease -= (float)modifier.rollValue/100f;
                    PlayerController.instance.CalculateDamage();
                    break;
                case "increase attack speed":
                    PlayerController.instance.PercentAttackSpeedIncrease -= (float)modifier.rollValue/100f;
                    PlayerController.instance.CalculateAttackSpeed();
                    break;
                case "spread shot":
                    if (InventoryController.instance.FireTypeItemEquiped() == false)
                        PlayerController.instance.FireType = InventoryController.instance.GetPlayerPreviousFireType();
                    PlayerController.instance.PercentDamageIncrease += (float)modifier.modData["damageReduce"];
                    PlayerController.instance.PercentAttackSpeedIncrease += (float)modifier.modData["attackSpeed"];
                    PlayerController.instance.CalculateAttackSpeed();
                    PlayerController.instance.CalculateDamage();
                
                    break;
                case "hp":
                    if (PlayerController.instance.Hp == PlayerController.instance.MaxHP)
                        PlayerController.instance.Hp-=modifier.rollValue;
                    PlayerController.instance.MaxHP-=modifier.rollValue;
                    break;
                case "armour":
                    PlayerController.instance.Armour -= modifier.rollValue;
                    break;
                case "fire resist":
                    PlayerController.instance.FireResistance -= modifier.rollValue;
                    break;
                case "cold resist":
                    PlayerController.instance.ColdResistance -= modifier.rollValue;
                    break;
                case "lightning resist":
                    PlayerController.instance.LightningResistance -= modifier.rollValue;
                    break;
                case "move speed":
                    PlayerController.instance.MovementSpeed -= modifier.rollValue;
                    break;
                default:
                    Debug.Log("empty modifer in item");
                    break;
            }
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Right && inInventory)
        {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {    
                transform.parent = null;
                float x_dir = Random.Range(-1f,1f);
                float y_dir = Random.Range(-1f,1f);
                Vector3 dropDirection = new Vector3 (x_dir,y_dir,0);
                transform.parent =  Instantiate(itemContainer.transform,dropDirection + PlayerController.instance.transform.position,Quaternion.identity); // Look stupid
                GetComponent<SpriteRenderer>().enabled = true;
                return;
            }
            if (!equiped)
            {
                if (InventoryController.instance.AddItemToEquipmentSlot(gameObject))
                {
                    ApplyItemEffect();
                    equiped = true;
                }
            }
            else {
                if (InventoryController.instance.RemoveItemFromEquipmentSlot(gameObject))
                {
                    RemoveItemEffect();
                    equiped = false;
                }
            }
        }
       

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver=true;
        ItemTooltip.instance.ShowItemToolTip(this);
         
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        ItemTooltip.instance.HideItemToolTip();    
    }
    private void OnMouseEnter() {
        mouseOver = true;
        ItemTooltip.instance.ShowItemToolTip(this);
    }
    private void OnMouseExit() {
        mouseOver = false;
        ItemTooltip.instance.HideItemToolTip();    
    }
    
}

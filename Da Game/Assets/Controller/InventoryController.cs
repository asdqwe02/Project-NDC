using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class InventoryController : MonoBehaviour
{
    public GameObject Inventory_UI;
    public GameObject Equipment_UI;
    public static InventoryController instance;
    [SerializeField] private GameObject mainCanvas;
    private List<Item> itemList;
    
    private int playerPreviousFireType;

    private void Awake() {
      
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        } 
        if (mainCanvas == null)
            mainCanvas = GameObject.Find("Canvas");

        if (Inventory_UI == null)
        {
            Inventory_UI = mainCanvas.transform.Find("Inventory_UI").gameObject;

        }
        if (Equipment_UI==null)
        {
            Equipment_UI = Inventory_UI.transform.GetChild(1).Find("Equipment").gameObject;
        }
        itemList = new List<Item>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && SceneManager.GetActiveScene().name!="MainMenu")
        {
            ToggleInventory();
        }
    }
    public void ToggleInventory(){
        bool activeState = Inventory_UI.activeSelf; //get the current active state
        activeState = !activeState; // invert it 
        if (ItemTooltip.instance.gameObject.activeSelf)
            ItemTooltip.instance.HideItemToolTip();
        Inventory_UI.SetActive(activeState);
    }
    public bool AddItem(GameObject item){
        foreach (Transform slot in Inventory_UI.transform.GetChild(0)) 
            if (slot.childCount == 0 )
            {
                GameObject itemContainer = item.transform.parent.gameObject;
                item.transform.parent = null;
                GameObject.Destroy(itemContainer); // remove the item container 

                item.transform.SetParent(slot);
                item.GetComponent<RectTransform>().localPosition = Vector3.zero; // reset local position
                return true;
            }
        return false;
    }
    public List<Item> GetIitemList()
    {
        return itemList;
    }
    public bool AddItemToEquipmentSlot(GameObject item)
    {
        foreach (Transform slot in Equipment_UI.transform)
        {
            if (slot.childCount == 0)
            {
                item.transform.SetParent(slot);
                item.GetComponent<RectTransform>().localPosition = Vector3.zero;
                return true;
            }
                
        }
        return false;
    }
    public bool RemoveItemFromEquipmentSlot(GameObject item)
    {
        foreach (Transform slot in Inventory_UI.transform.GetChild(0))
        {
            if (slot.childCount == 0 )
            {
                item.transform.SetParent(slot);
                item.GetComponent<RectTransform>().localPosition = Vector3.zero; // reset local position
                return true;
            }
        }
        return false;
    }
    public bool FireTypeItemEquiped()
    {
        foreach (Transform slot in Equipment_UI.transform)
        {
            if (slot.childCount == 1)
            {
                ItemController item =  slot.GetChild(0).GetComponent<ItemController>();
                foreach (Modifier modifer in item.Item.modifiers)
                    if (modifer.modData["name"].ToString() == "spread shot")
                        return true;
            }
        }
        return false;
    }
    public void SetPlayerPreviousFireType(int firetype)
    {
        playerPreviousFireType = firetype;
    }
    public int GetPlayerPreviousFireType()
    {
        return playerPreviousFireType;
    }
    public void ReloadInventoryUI()
    {
        mainCanvas = GameObject.Find("Canvas");
        Inventory_UI = mainCanvas.transform.Find("Inventory_UI").gameObject;
        Equipment_UI = Inventory_UI.transform.GetChild(1).Find("Equipment").gameObject;
    }
}

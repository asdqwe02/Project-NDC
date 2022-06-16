using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
public class ModifierPool
{
    List<JObject> modifiers;
    JObject modifiersData;
    // JArray data;
    public ModifierPool(string type,string jsonFilePath){
        // string jsonString = File.ReadAllText(Application.dataPath + "/Scriptable Object/ItemModifiers.json");
        // string jsonString = File.ReadAllText(Application.dataPath + jsonFilePath);
        TextAsset itemmodifierjson = Resources.Load(jsonFilePath) as TextAsset;
        string jsonString = itemmodifierjson.ToString();
        JArray data = JArray.Parse(jsonString);
        modifiersData = GetModiferData(type,data);
        if (modifiersData!=null)
        {
            modifiers = new List<JObject>();
            JArray modiferArray = (JArray) modifiersData["modifiers"];
            foreach (JToken modifier in modiferArray)
            {
                modifiers.Add((JObject)modifier);
            }

        }
        SortModifierByWeight();

        // DEBUGGING
        // Debug.Log(GetModifierPoolTotalWeight());
        // RollMultipleModifier(2);
    } 
    public JObject GetModiferData(string type, JArray data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i]["type"] .ToString() == type)
            {
                return  data[i].ToObject<JObject>();
            }
        }
        return null;
    }
    public int GetModifierPoolTotalWeight(){
        int sum = 0;
        foreach(JObject modifer in modifiers)
            // sum += modifer.Value<int>("weight");
            sum += (int)modifer["weight"];

        return sum;
    }

    //Sort the list by Descending Order
    public void SortModifierByWeight()
    {
        // li.Sort((a, b) => a.CompareTo(b)); // ascending sort
        // li.Sort((a, b) => b.CompareTo(a)); // descending sort
        modifiers.Sort((m1,m2)=> ((int)m2["weight"]).CompareTo((int)m1["weight"]));
        // foreach  (JToken modifier in modifiers)
        //     Debug.Log(modifier);
    }
    public List<Modifier> RollMultipleModifier(int amount)
    {
        List<Modifier> rolledModifiers = new List<Modifier>();
        float TotalWeight = GetModifierPoolTotalWeight();
        for (int i = 0 ; i<amount ; i++)
        {
           JObject modifier =  RollModifier(TotalWeight);
            if (modifier!=null)
            {
                if (modifier["range"]!=null)
                {
                    int minRange = (int)modifier["range"]["min"];
                    int maxRange = (int)modifier["range"]["max"];
                    int roll = Random.Range(minRange,maxRange);
                    rolledModifiers.Add(new Modifier(modifier,roll));
                }
                else rolledModifiers.Add(new Modifier(modifier));
            }
                
        }
        // foreach(Modifier mod in rolledModifiers)
        //     Debug.Log(mod.modData);
        return rolledModifiers;
    }
    public JObject RollModifier(float TotalWeight)
    {
        float CumulativeProbability = 0f;
        float RNGRoll = Random.Range(0f, 1f);
        // Debug.Log("RNGROLL:"+RNGRoll);
        // Debug.Log("Total weight:"+TotalWeight);
        foreach (JObject modifier in modifiers)
        {
            // Debug.Log("mod weight: " + (int)modifier["weight"]);
            CumulativeProbability += (float)modifier["weight"] / TotalWeight;
            // Debug.Log(CumulativeProbability);
            if (RNGRoll <= CumulativeProbability)
                return modifier;
        }
        // note: apply some good practice and do some check here
        return null; // should never happen but just in case show error message 

    }

}

[System.Serializable]
public class Modifier 
{
    public JObject modData;
    public int rollValue;
    public int previousFireType; //useless rn
    public Modifier(JObject mod,int roll=0)
    {
        this.modData = mod;
        this.rollValue = roll;
        previousFireType = 0;
    }

}

public enum ItemType{
    attack,
    defense,
}

public class Item
{
    private string itemName;
    public List<Sprite> itemsIcon;
    private Sprite currentSprite;
    public int amount = 0;
    public List<Modifier> modifiers;
    public ItemType itemType;
    public string ItemName { get => itemName; set => itemName = value; }
    // this cause error
    public void Initialize(List<Sprite> iconList){
        itemsIcon = iconList;
        currentSprite = itemsIcon[Random.Range(0,itemsIcon.Count)];
        modifiers = new List<Modifier>();
        int ItemTypeLength = System.Enum.GetValues(typeof(ItemType)).Length;
        int index =Random.Range(0,ItemTypeLength);
        itemType = (ItemType)index;
    }
    public Sprite GetCurrentSprite(){
        return currentSprite;
    }


}

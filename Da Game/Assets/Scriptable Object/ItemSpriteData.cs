using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Item Sprite Data", fileName = "Item Sprite Data" )]
public class ItemSpriteData : ScriptableObject
{
    public List<Sprite> itemsIcon = new List<Sprite>();
  
}


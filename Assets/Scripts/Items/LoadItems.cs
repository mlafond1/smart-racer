using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadItems{
    
    static Dictionary<string, GameObject> items = new Dictionary<string, GameObject>();

    private LoadItems(){}

    public static void LoadItemPrefabs(){
        GameObject[] loadedItems = Resources.LoadAll<GameObject>(@"Prefabs/Items/");
        items.Clear();
        foreach(GameObject item in loadedItems){
            items.Add(item.gameObject.name, item);
        }
    }

    public static GameObject GetItemPrefab(string name){
        return items[name];
    }

}

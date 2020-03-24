using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadItems{
    
    static Dictionary<string, GameObject> items = new Dictionary<string, GameObject>();
    static bool isLoaded = false;

    private LoadItems(){}

    public static void LoadItemPrefabs(){
        if(isLoaded) return;
        GameObject[] loadedItems = Resources.LoadAll<GameObject>(@"Prefabs/Items/");
        items.Clear();
        foreach(GameObject item in loadedItems){
            items.Add(item.gameObject.name, item);
        }
        isLoaded = true;
    }

    public static void ReloadItemPrefabs(){
        isLoaded = false;
        LoadItemPrefabs();
    }

    public static List<string> GetItemNames(){
        return new List<string>(items.Keys);
    }

    public static GameObject GetItemPrefab(string name){
        if(!isLoaded) LoadItemPrefabs();
        return items[name];
    }

}

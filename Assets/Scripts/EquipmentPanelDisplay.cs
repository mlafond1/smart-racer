using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentPanelDisplay : MonoBehaviour
{

    List<Text> playerSlotTexts = new List<Text>();
    List<Image> playerSlotImages = new List<Image>();
    CarController playerCar;

    void Start(){
        LoadItems.LoadItemPrefabs();
        playerCar = GameObject.FindObjectOfType<PlayerController>().gameObject.GetComponent<CarController>();

        playerSlotTexts.Add(GameObject.Find("OffensiveSlotCooldownText").GetComponent<Text>());
        playerSlotTexts.Add(GameObject.Find("DefensiveSlotCooldownText").GetComponent<Text>());

        playerSlotImages.Add(GameObject.Find("OffensiveEquipmentSlotImage").GetComponent<Image>());
        playerSlotImages.Add(GameObject.Find("DefensiveEquipmentSlotImage").GetComponent<Image>());

    }

    void Update(){
        Item offensiveItem = playerCar.GetItem(0);
        Item defensiveItem = playerCar.GetItem(1);
        
        DisplaySlotText(0, offensiveItem);
        DisplaySlotImage(0, offensiveItem);
        
        DisplaySlotText(1, defensiveItem);
        DisplaySlotImage(1, defensiveItem);
    }

    void DisplaySlotText(int index, Item item){
        if(item == null || item.isReady){
            playerSlotTexts[index].enabled = false;
        }
        else {
            playerSlotTexts[index].enabled = true;
            playerSlotTexts[index].text = item.CooldownTimer.ToString();
        }
    }

    void DisplaySlotImage(int index, Item item){
        Sprite currentImage = playerSlotImages[index].sprite;
        Sprite itemImage = item?.GetSprite();
        if(itemImage == null){
            playerSlotImages[index].sprite = null;
            Color transparentColor = playerSlotImages[index].color;
            transparentColor.a = 0f;
            playerSlotImages[index].color = transparentColor;
        }
        else if(!itemImage.Equals(currentImage)){
            playerSlotImages[index].sprite = itemImage;
            Color visibleColor = playerSlotImages[index].color;
            visibleColor.a = 1f;
            playerSlotImages[index].color = visibleColor;
            playerSlotImages[index].type = Image.Type.Simple;
            playerSlotImages[index].preserveAspect = true;
        }
    }

}

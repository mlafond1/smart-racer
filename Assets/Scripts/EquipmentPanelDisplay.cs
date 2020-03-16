using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentPanelDisplay : MonoBehaviour
{

    Text playerOffensiveSlotText;
    Text playerDefensiveSlotText;
    Image playerOffensiveSlotImage;
    Image playerDefensiveSlotImage;
    CarController playerCar;

    void Start(){
        LoadItems.LoadItemPrefabs();
        playerCar = GameObject.FindObjectOfType<PlayerController>().gameObject.GetComponent<CarController>();
        playerOffensiveSlotText = GameObject.Find("OffensiveSlotCooldownText").GetComponent<Text>();
        playerDefensiveSlotText = GameObject.Find("DefensiveSlotCooldownText").GetComponent<Text>();
        playerOffensiveSlotImage = GameObject.Find("OffensiveEquipmentSlotImage").GetComponent<Image>();
        playerDefensiveSlotImage = GameObject.Find("DefensiveEquipmentSlotImage").GetComponent<Image>();
    }

    void Update(){
        Item offensiveItem = playerCar.GetItem(0);
        Item defensiveItem = playerCar.GetItem(1);
        if(offensiveItem == null || offensiveItem.isReady){
            playerOffensiveSlotText.enabled = false;
        }
        else {
            playerOffensiveSlotText.enabled = true;
            playerOffensiveSlotText.text = offensiveItem.CooldownTimer.ToString();
        }
        if(defensiveItem == null || defensiveItem.isReady){
            playerDefensiveSlotText.enabled = false;
        }
        else {
            playerDefensiveSlotText.enabled = true;
            playerDefensiveSlotText.text = defensiveItem.CooldownTimer.ToString();
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePanelDisplay : MonoBehaviour {

    Dictionary<CarController, Text> damagePanelTexts = new Dictionary<CarController, Text>();

    void Start(){
        List<CarController> cars = new List<CarController>(GameObject.FindObjectsOfType<CarController>());
        cars.Reverse();
        Font arial = Resources.GetBuiltinResource<Font>("Arial.ttf");;
        foreach(CarController car in cars){
            GameObject currentTextGameObject = new GameObject("DamageTextCar_" + car.name);
            currentTextGameObject.transform.SetParent(this.transform);
            Text currentText = currentTextGameObject.AddComponent<Text>();
            currentTextGameObject.AddComponent<Outline>();

            currentText.fontSize = 18;
            currentText.color = Color.white;
            currentText.verticalOverflow = VerticalWrapMode.Overflow;
            currentText.font = arial;
            currentText.alignment = TextAnchor.UpperCenter;
            currentText.text = car.gameObject.name;

            damagePanelTexts.Add(car, currentText);
        }
    }

    void Update(){
        foreach(var elem in damagePanelTexts){
            CarController car = elem.Key;
            Text text = elem.Value;
            text.text = string.Format("{0}\n{1:0.#%}", car.name, car.Statistics.ejectionRate -1f);
        }
    }

}
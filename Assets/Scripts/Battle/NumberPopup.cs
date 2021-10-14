using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum NumberType {
    NONE,
    Damage,
    AbsorbDamage,
    Heal,
    SpHeal
}

public class NumberPopup : MonoBehaviour {
    private int number;
    private NumberType numType;
    private TMP_Text numberText;
    private float functionTime;
    private Vector3 originalLocation;

    private Dictionary<NumberType, (Color numColor, Color outlineColor)> colors = new Dictionary<NumberType, (Color numColor, Color outlineColor)>() {
        {NumberType.NONE,  (Color.white, Color.black)},
        {NumberType.Damage, (new Color(1, (float) 120 / 255, 0), Color.black) },
        {NumberType.AbsorbDamage, (new Color((float) 165 / 255, 0, 1), Color.black) },
        {NumberType.Heal, (new Color(0, 1, 0), Color.black) },
        {NumberType.SpHeal, (new Color(0, (float) 90 / 255, 1), Color.black) }
    };

    /// <summary>
    /// Instantiates a NumberPopup on the parent transform
    /// </summary>
    /// <param name="number">The number to display on the popup.</param>
    /// <param name="numberType">The type of number to put on the text. Determines color and animation.</param>
    /// <param name="displayOn">The Transform to instantiate this on.</param>
    /// <returns></returns>
    public static GameObject DisplayNumberPopup(int number, NumberType numberType, Transform displayOn, float offsetX = 0, float offsetY = 1, float offsetZ = 0.1f) {
        GameObject prefab = Resources.Load("UI/DamageNumberPrefab") as GameObject;
        GameObject numberPopup = Instantiate(prefab, displayOn.transform);
        NumberPopup popup = numberPopup.GetComponent<NumberPopup>();

        popup.number = number;
        popup.numType = numberType;
        popup.numberText = numberPopup.GetComponent<TMP_Text>();
        popup.originalLocation = new Vector3(offsetX, offsetY, offsetZ);

        numberPopup.transform.localPosition = popup.originalLocation;

        return numberPopup;
	}

    void Start() {
        numberText.text = number.ToString();
        numberText.color = colors[numType].numColor;
        numberText.outlineColor = colors[numType].outlineColor;
        functionTime = 0;
    }

    void Update() {
        if (functionTime < 1.5) {
            if (numType == NumberType.Damage || numType == NumberType.AbsorbDamage)
                transform.localPosition = getPositionDamage();
            else if (numType == NumberType.Heal || numType == NumberType.SpHeal)
                transform.localPosition = getPositionRecover();
        } else {
            if (numberText.alpha > 0)
                numberText.alpha -= 0.04f;
            else if (numberText.alpha <= 0.05)
                Destroy(gameObject);
        }
    }

    Vector3 getPositionDamage() {
        functionTime += Time.deltaTime;

        if (functionTime > 0.35)
            return originalLocation;

        float funcVal = Mathf.Sin(functionTime) / (functionTime + 0.4f);

        float xVal = originalLocation.x + (funcVal + Random.Range(-0.5f, 0.5f)) / 2;
        float yVal = originalLocation.y + (funcVal + Random.Range(-0.5f, 0.5f)) / 2;
        float zVal = originalLocation.z;

        return new Vector3(xVal, yVal, zVal);
	}

    Vector3 getPositionRecover() {
        functionTime += Time.deltaTime;

        float xVal = originalLocation.x;
        float yVal = originalLocation.y + functionTime / 1.5f;
        float zVal = originalLocation.z + functionTime / 1.5f;

        return new Vector3(xVal, yVal, zVal);
    }
}


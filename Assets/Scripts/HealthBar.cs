using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HealthBar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private GameObject fill;
    private Image fillBackground;

    private float healthValue;

    private float healthMaxValue;

    private UnitMoverManager unitMoverManager;

    private void Awake() {
        fillBackground = GetComponent<Image>();
        unitMoverManager = FindObjectOfType<UnitMoverManager>();
    }

    public void SetHealthValue(float newValue) {
        healthValue = newValue;
        slider.value = healthValue;
    }

    public void SetHealthMaxValue(float newFloat) {
        healthMaxValue = newFloat;
        slider.maxValue = healthMaxValue;
    }

    public void ShowHealthbar(){
        fillBackground.enabled = true;
        fill.SetActive(true);
    }

    public void HideHealthbar(){
        fillBackground.enabled = false;
        fill.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if(unitMoverManager.InitialPosition.x == 400){
            ShowHealthbar();
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if(unitMoverManager.InitialPosition.x == 400){
            HideHealthbar();
        }
    }

    public IEnumerator DecreaseHealthValue(float toReduce) {
        healthValue = healthValue - toReduce;
        yield return StartCoroutine(SliderAnimation(toReduce));
    }

    IEnumerator IncreaseHealthValue(float toReduce) {
        healthValue = healthValue + toReduce;
        yield return StartCoroutine(SliderAnimation(-toReduce));
    }

    IEnumerator SliderAnimation(float health) {
        if (slider != null) {

            float onePercent = slider.maxValue / 100;

            float diff = slider.value - health;

            float removePerCicle = 0f;

            if (diff < 0) {
                removePerCicle = (-diff * 100) / slider.maxValue;
            } else {
                removePerCicle = (diff * 100) / slider.maxValue;
            }

            float timeSlice = 1 / removePerCicle;

            for (int i = 0; i <= removePerCicle; i++) {
                if (diff < 0) {
                    slider.value += onePercent;
                } else {
                    slider.value -= onePercent;
                }
                yield return new WaitForSeconds(timeSlice);
            }

        }
        yield return null;
    }

}

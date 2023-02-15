using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    void OnEnable()
    {
        Entity.ChangeHpBarValue += UpdateValue;
    }

    void OnDisable()
    {
        Entity.ChangeHpBarValue -= UpdateValue;
    }


    Slider bar;
    void Start()
    {
        bar = GetComponentInChildren<Slider>();
    }

    void UpdateValue(float value)
    {
        bar.value = value;
    }
}

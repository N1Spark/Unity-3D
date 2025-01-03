using UnityEngine;
using UnityEngine.UI;

public class BatteryIndicatorScript : MonoBehaviour
{
    private Image image;
    private GameObject markCharge;
    private FlashLightScript flashLightScript;
    void Start()
    {
        image = GetComponent<Image>();
        flashLightScript = GameObject
            .Find("FlashLight")
            .GetComponent<FlashLightScript>();
        markCharge = transform
            .Find("ExtraMark")
            .gameObject;
    }

    void Update()
    {
        image.fillAmount = flashLightScript.chargeLevel;
        image.color = new Color(
            (60 + (1 - image.fillAmount) * 130) / 255f, 
            (30 + (image.fillAmount) * 130) / 255f, 
            (30 + (image.fillAmount) * 30) / 255f 
            );
        if (flashLightScript.chargeLevel > 1.0f)
            markCharge.SetActive(true);
        else
            markCharge.SetActive(false);

        // 60  160 60
        // 190 160 30
        // 190 30  30

    }
}

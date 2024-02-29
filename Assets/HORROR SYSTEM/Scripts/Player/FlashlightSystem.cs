using InteractionSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashlightSystem : MonoBehaviour
{
    [Header("FLASHLIGHT PARAMETERS")]
    [Space(10)]
    [SerializeField] public bool hasFlashlight = false;
    [SerializeField] public GameObject handFlashlight;
    [SerializeField] public GameObject inventoryFlashlight;
    [SerializeField] public Light flashlightSpot = null;
    [SerializeField] public float maxFlashlightIntensity = 1.0f;

    [Header("BATTERY PARAMETERS")]
    [Space(10)]
    [SerializeField] public int batteryCount = 1;
    [SerializeField] private float replaceBatteryTimer = 1.0f;
    [SerializeField] private float maxReplaceBatteryTimer = 1.0f;

    [Header("FLASHLIGHT UI")]
    [Space(10)]
    [SerializeField] private Image batteryLevel = null;
    [SerializeField] private Image batteryUI = null;
    [SerializeField] private Text batteryCountUI = null;
    [SerializeField] private Image radialIndicator = null;

    [Header("FLASHLIGHT SOUNDS")]
    [Space(10)]
    [SerializeField] private string pickUp;
    [SerializeField] private string flashlightClick;
    [SerializeField] private string reloadBattery;

    private bool shouldUpdate = false;
    private bool showOnce = false;
    private bool isFlashlightOn;

    public static FlashlightSystem instance;


    private void Awake()
    {
        if (instance != null) 
        { 
            Destroy(gameObject); 
        }
        else 
        { 
            instance = this;
        }

        if (batteryCount < 1)
        {
            batteryUI.enabled = false;
        }
        else if (batteryCount > 0)
        {
            batteryUI.enabled = true;
        }
    }

    void Start()
    {
        handFlashlight.SetActive(false);
        inventoryFlashlight.SetActive(false);
        flashlightSpot.intensity = maxFlashlightIntensity;
        batteryCountUI.text = batteryCount.ToString("0");
    }

    public void EnableInventory()
    {
        hasFlashlight = true;
        inventoryFlashlight.SetActive(true);
        AudioManager.instance.Play(pickUp);
    }

    public void CollectBattery(int batteries)
    {
        batteryCount = batteryCount + batteries;
        batteryCountUI.text = batteryCount.ToString("0");
        batteryUI.enabled = true;
        AudioManager.instance.Play(pickUp);
    }

    void EnableFlashlight()
    {
        handFlashlight.SetActive(true);
        inventoryFlashlight.SetActive(false);
    }

    void DisableFlashlight()
    {
        handFlashlight.SetActive(false);
        inventoryFlashlight.SetActive(true);
    }

    void FlashlightClickSound()
    {
        AudioManager.instance.Play(this.flashlightClick);

        if (flashlightSpot.enabled == false)
        {
            flashlightSpot.enabled = true;
        }
        else
        {
            flashlightSpot.enabled = false;
        }
    }

    void Update()
    {
        if (hasFlashlight)
        {
            if (Input.GetKeyDown(InputManager.instance.flashlightSwitch) && !showOnce)
            {
                if (flashlightSpot.enabled == false)
                {
                    isFlashlightOn = true;
                }
                else
                {
                    isFlashlightOn = false;
                }
            }

            if (isFlashlightOn)
            {
                if (flashlightSpot.intensity <= maxFlashlightIntensity && flashlightSpot.intensity > 0)
                {
                    flashlightSpot.intensity -= (0.007f * Time.deltaTime) * maxFlashlightIntensity;
                    batteryLevel.fillAmount -= 0.007f * Time.deltaTime;
                }

                if (flashlightSpot.intensity >= maxFlashlightIntensity)
                {
                    flashlightSpot.intensity = maxFlashlightIntensity;
                }

                if (flashlightSpot.intensity <= 0)
                {
                    flashlightSpot.intensity = 0;
                    isFlashlightOn = false;
                }
            }

            if (flashlightSpot.enabled == false)
            {
                {
                    flashlightSpot.intensity += (0.05f * Time.deltaTime) * maxFlashlightIntensity;
                    batteryLevel.fillAmount += 0.05f * Time.deltaTime;
                }

                if (flashlightSpot.intensity >= maxFlashlightIntensity)
                {
                    flashlightSpot.intensity = maxFlashlightIntensity;
                }
            }

            if (Input.GetKey(InputManager.instance.reloadBattery) && batteryCount > 0 && flashlightSpot.intensity < maxFlashlightIntensity)
            {
                shouldUpdate = false;
                replaceBatteryTimer -= Time.deltaTime;
                radialIndicator.enabled = true;
                radialIndicator.fillAmount = replaceBatteryTimer;
                gameObject.GetComponent<PlayerController>().isFlashlight = false;

                if (replaceBatteryTimer <= 0)
                {
                    batteryCount--;
                    batteryCountUI.text = batteryCount.ToString("0");
                    flashlightSpot.intensity += maxFlashlightIntensity;
                    AudioManager.instance.Play(reloadBattery);
                    batteryLevel.fillAmount = maxFlashlightIntensity;
                    replaceBatteryTimer = maxReplaceBatteryTimer;
                    radialIndicator.fillAmount = maxReplaceBatteryTimer;
                    radialIndicator.enabled = false;

                    if (gameObject.GetComponent<PlayerController>().flashlightOn == true)
                    {
                        gameObject.GetComponent<PlayerController>().isFlashlight = true;
                    }
                    else
                    {
                        gameObject.GetComponent<PlayerController>().isFlashlight = false;
                    }

                    if (batteryCount < 1)
                    {
                        batteryUI.enabled = false;
                    }
                }
            }
            else
            {
                if (shouldUpdate)
                {
                    replaceBatteryTimer += Time.deltaTime;
                    radialIndicator.fillAmount = replaceBatteryTimer;

                    if (replaceBatteryTimer >= maxReplaceBatteryTimer)
                    {
                        replaceBatteryTimer = maxReplaceBatteryTimer;
                        radialIndicator.fillAmount = maxReplaceBatteryTimer;
                        radialIndicator.enabled = false;
                        shouldUpdate = false;
                    }
                }
            }

            if (Input.GetKeyUp(InputManager.instance.reloadBattery))
            {
                shouldUpdate = true;

                if (gameObject.GetComponent<PlayerController>().flashlightOn == true)
                {
                    gameObject.GetComponent<PlayerController>().isFlashlight = true;
                }
                else
                {
                    gameObject.GetComponent<PlayerController>().isFlashlight = false;
                }
            }
        }
    }
}

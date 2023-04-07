using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxScript : MonoBehaviour
{
    [SerializeField]
    private GameObject lightObject;

    [SerializeField]
    private float skyRotationSpeed;
    [SerializeField]
    private float lightRotationSpeed;
    [SerializeField]
    private float fogSpeed;
    [SerializeField]
    private float lightColorSpeed;
    [SerializeField]
    private float maxFogDensity;
    [SerializeField]
    private float minFogDensity;

    private float currentRotation;
    private float currentFogDensity;

    private Color DayColor;
    [SerializeField]
    private Color NightColor;

    // Start is called before the first frame update
    void Start()
    {

        currentFogDensity = RenderSettings.fogDensity;
        DayColor = lightObject.GetComponent<Light>().color;
    }

    // Update is called once per frame
    void Update()
    {
        SkyBoxRotation();
        SunRotation();
    }

    private void SkyBoxRotation()
    {
        currentRotation += skyRotationSpeed * Time.deltaTime;

        RenderSettings.skybox.SetFloat("_Rotation", currentRotation);
    }

    private void SunRotation()
    {
        lightObject.transform.Rotate(Vector3.right * lightRotationSpeed * Time.deltaTime);

        if (lightObject.transform.localEulerAngles.x >= 170)
        {
            // ¹ã ¼³Á¤
            if (currentFogDensity <= maxFogDensity)
            {
                currentFogDensity += Time.deltaTime * fogSpeed;
                RenderSettings.fogDensity = currentFogDensity;


            }
            else
            {
                // ³· ¼³Á¤
                if (currentFogDensity >= minFogDensity)
                {
                    currentFogDensity -= Time.deltaTime * fogSpeed;
                    RenderSettings.fogDensity = currentFogDensity;

                }
            }
        }
    }
}

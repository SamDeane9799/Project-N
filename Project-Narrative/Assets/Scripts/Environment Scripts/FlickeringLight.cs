using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    // Start is called before the first frame update
    private Light myLight;
    public float flickerRange;
    public float flickerFrequency;
    private float maxRange;
    private float minRange;
    private bool increasing;
    private float timer = 0;
    void Start()
    {
        myLight = GetComponent<Light>();
        minRange = myLight.intensity;
        maxRange = minRange + flickerRange;
    }

    // Update is called once per frame
    void Update()
    {
        float lightIntensity = myLight.intensity;
        timer += Time.deltaTime;
        if (increasing)
            myLight.intensity = Mathf.Lerp(minRange, maxRange, timer / flickerFrequency);
        else
            myLight.intensity = Mathf.Lerp(maxRange, minRange, timer / flickerFrequency);

        if (timer / flickerFrequency > 1)
        {
            timer = 0;
            increasing = !increasing;
        }
    }
}

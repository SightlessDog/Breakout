using UnityEngine;

public class BulbLight : MonoBehaviour
{
    // Start is called before the first frame update
    private Bulb bulbScript;
    private GameObject bulb;
    Light light; 
    
    void Start()
    {
        bulb = GameObject.Find("Double_Reflctor"); 
        bulbScript = bulb.GetComponent<Bulb>(); 
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bulbScript.hit)
        {
            light.intensity = 200;
            light.range = 100;
            light.color = Color.white;
        }
        else
        {
            light.range = 10;
            light.intensity = 10;
            light.color = new Color(0, 155, 255, 255); 
        }
    }
}

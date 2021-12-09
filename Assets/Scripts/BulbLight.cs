using UnityEngine;

public class BulbLight : MonoBehaviour
{
    // Start is called before the first frame update
    private Bulb bulbScript;
    private GameObject bulb;
    private Light bulbLight; 
    
    void Start()
    {
        bulb = GameObject.Find("Double_Reflctor"); 
        bulbScript = bulb.GetComponent<Bulb>(); 
        bulbLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bulbScript.hit)
        {
            bulbLight.intensity = 200;
            bulbLight.range = 100;
            bulbLight.color = Color.white;
        }
        else
        {
            bulbLight.range = 10;
            bulbLight.intensity = 10;
            bulbLight.color = new Color(0, 155, 255, 255); 
        }
    }
}

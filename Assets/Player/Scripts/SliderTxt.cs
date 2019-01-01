using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderTxt : MonoBehaviour {

    public TMP_Text text;
    
    public void Change ()
    {
        text.text = GetComponent<Slider>().value.ToString();
    }
}

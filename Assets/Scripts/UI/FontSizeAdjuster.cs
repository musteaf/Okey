using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FontSizeAdjuster : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float scale; 
    void Awake()
    {
        Text text = gameObject.GetComponent<Text>();
        text.fontSize = (int)(Screen.height / scale);
    }
}

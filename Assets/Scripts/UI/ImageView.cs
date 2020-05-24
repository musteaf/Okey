using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ImageView : View
{
    protected Image image;

    public void Init()
    {
        base.Init();
        image = gameObject.GetComponent<Image>();
    }
}

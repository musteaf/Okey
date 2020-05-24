using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorView : ImageView
{
    private Text tileText;
    
    public void Init()
    {
        base.Init();
        tileText = gameObject.transform.GetChild(0).transform.GetComponent<Text>();
    }
    
    public override void UpdateRect(float width, float height)
    {
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        tileText.fontSize = (int)height/4*2;
    }
    
    public void UpdateTile(Tile tile)
    {
        tileText.text = (tile.Number + 1) .ToString();
        switch (tile.TileColor)
        {
            case TileColor.Black:
                tileText.color = Color.black;
                break;
            case TileColor.Blue:
                tileText.color = new Color(0,0,230f/255f);
                break;
            case TileColor.Red:
                tileText.color = new Color(179f/255f,0,0);
                break;
            case TileColor.Yellow:
                tileText.color = new Color(240f/255f,170f/255f,4f/255f);
                break;
        }
    }
    
    public override void Hide()
    {
        throw new System.NotImplementedException();
    }

    public override void Show()
    {
        throw new System.NotImplementedException();
    }

    public void SetPosition(Vector2 position)
    {
        rectTransform.localPosition = position;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;

public class TileView : ImageView
{
    private MovementAnimation movementAnimation;
    private FlipAnimation flipAnimation;
    private int slotPos;
    private Text tileText;
    private bool isItJoker;
    private int number;
    private bool fliped = false;
    /*
     * we can separate tileview to tileview and tileanimationcontroller
     */
    public void Init()
    { 
        base.Init();
        rectTransform = gameObject.GetComponent<RectTransform>();
        tileText = gameObject.transform.GetChild(0).transform.GetComponent<Text>();
        movementAnimation = gameObject.GetComponent<MovementAnimation>();
        flipAnimation = gameObject.GetComponent<FlipAnimation>();
        movementAnimation.SetContinuingAction(new ContinuingAction<Vector2>(MoveSlowly));
        movementAnimation.SetStopAction(new StopAction<Vector2>(MoveTargetPosition));
        flipAnimation.SetContinuingAction(new ContinuingAction<Vector2>(MoveSlowly));
        flipAnimation.SetInterimAction(new InterimAction(Flip));
        flipAnimation.SetStopAction(new StopAction<Vector2>(MoveTargetPosition));
    }
    public override void UpdateRect(float width, float height)
    {
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        tileText.fontSize = (int)height/4*2;
    }

    public void UpdateTile(Tile tile)
    {
        //print("UpdateTile");
        tileText.text = (tile.Number + 1) .ToString();
        number = tile.Number + 1;
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
        isItJoker = tile.IsItJoker;
        fliped = false;
    }

    public int SlotPos
    {
        get => slotPos;
        set => slotPos = value;
    }

    public override void Hide()
    {
        tileText.enabled = false;
        image.enabled = false;
    }
    
    public override void Show()
    {
        tileText.enabled = true;
        image.enabled = true;
    }

    public void DistributionAnimation(Vector2 targetPosition, int slot)
    {
        slotPos = slot;
        int y = (Screen.height / 2);
        movementAnimation.StartAnimation(new Vector2( 0,y), targetPosition, targetPosition);
    }
    public void UpdatePosition(Vector2 targetPosition, int slot)
    {
        StopAllAnimations();
        slotPos = slot;
        movementAnimation.StartAnimation( rectTransform.localPosition, 
            targetPosition, targetPosition);
    }
    public void UpdatePosition(Vector2 targetPosition)
    {
        StopAllAnimations();
        movementAnimation.StartAnimation( rectTransform.localPosition, 
            targetPosition, targetPosition);
    }

    public void StopAllAnimations()
    {
        movementAnimation.StopAnimation();
        flipAnimation.ForceStopAnimation();
    }

    public void MoveTargetPosition(Vector2 lastPosition)
    {
        rectTransform.localPosition = lastPosition;
    }

    public void Flip()
    {
        if (isItJoker)
        {
            if (tileText.text.Equals(""))
            {
                tileText.text = number.ToString();
            }
            else
            {
                tileText.text = "";
            }
            print("flip");
            fliped = !fliped;
        }
    }

    public void Holded() {
        if (isItJoker)
        {
            StopAllAnimations();
            int y = (Screen.height / 10);
            flipAnimation.StartAnimation(rectTransform.localPosition,
                rectTransform.localPosition + new Vector3(0, y, 0),
                rectTransform.localPosition);
        }
    }
    
    public void StopHolded() {
        //movementAnimation();
        if(isItJoker)
            flipAnimation.StopAnimation();
    }

    public void MoveSlowly(Vector2 start, Vector2 end, float disc)
    {
        rectTransform.localPosition = Vector2.Lerp(start, end, disc);
    }
}

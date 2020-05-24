using UnityEditor;
using UnityEngine;

public class FlipAnimation : UIAnimation<Vector2, Vector2>
{
    private const float speed = 2f;
    private bool fliped = false;
    public void StopAnimation()
    {
        float disc = (Time.time - startTime)*speed;
        if (started && disc < 1)
        {
            fliped = false;
            started = false;
            stopAction(lastPose);
        }
    }
    
    // when movement animation call
    public void ForceStopAnimation()
    {
        fliped = false;
        if (started)
        {
            started = false;
            stopAction(lastPose);
        }
    }
    public override void Run()
    {
        float disc = (Time.time - startTime)*speed;
        if (disc >= 1 && disc <= 1.5f)
        {
            continuingAction(pose1, pose2, (disc - 1f)*2f);
        }else if (disc >= 1.5f && disc <= 2)
        {
            if (!fliped)
            {
                interimAction();
                fliped = true;
            }
            continuingAction(pose2, pose1, (disc - 1.5f)*2f);
        }else if (disc > 2)
        {
            stopAction(lastPose);
            started = false;
        }
    }
}

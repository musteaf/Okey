using UnityEngine;
public class MovementAnimation : UIAnimation<Vector2, Vector2>
{
    public override void Run()
    {
        float disc = (Time.time - startTime) * 5f;
        if (disc <= 1f)
        {
            continuingAction(pose1,pose2, disc);
        }
        else
        {
            stopAction(lastPose);
            started = false;
        }
    }
}
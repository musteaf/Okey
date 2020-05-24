using JetBrains.Annotations;
using UnityEngine;

public delegate void StopAction<P>(P lastPose);
public delegate void InterimAction();
public delegate void ContinuingAction<T>(T pose1,T pose2, float disc);
public abstract class UIAnimation<T,P> : MonoBehaviour
{
    protected float startTime;
    protected bool started;
    protected ContinuingAction<T> continuingAction;
    protected StopAction<P> stopAction;
    protected InterimAction interimAction;
    protected T pose1;
    protected T pose2;
    protected P lastPose;
    
    public void SetContinuingAction(ContinuingAction<T> continuingAction)
    {
        this.continuingAction = continuingAction;
    }
    public void SetStopAction(StopAction<P> stopAction)
    {
        this.stopAction = stopAction;
    }
    
    public void SetInterimAction(InterimAction interimAction)
    {
        this.interimAction = interimAction;
    }

    public void StartAnimation(T pose1, T pose2, P lastPose)
    {
        started = true;
        startTime = Time.time;
        this.pose1 = pose1;
        this.pose2 = pose2;
        this.lastPose = lastPose;
    }
    
    public void StartAnimation()
    {
        started = true;
        startTime = Time.time;
    }

    public void CancelAnimation()
    {
        if (started)
        {
            started = false;
        }
    }

    public void StopAnimation()
    {
        if (started)
        {
            started = false;
            stopAction(lastPose);
        }
    }
    void Update()
    {
        if (started)
        {
            Run();
        }
    }

    public abstract void Run();
}

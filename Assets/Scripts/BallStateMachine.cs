using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBallStateContext
{
    void SetState(IBallState newState);
}

public interface IBallState
{
    void Glide(IBallStateContext context);
    void Roll(IBallStateContext context);
}

public class BallStateMachine : MonoBehaviour, IBallStateContext
{
    IBallState currentState;

    public void Glide() => currentState.Glide(this);
    public void Roll() => currentState.Roll(this);

    public void SetState(IBallState newState)
    {
        currentState = newState;
    }       
}

class GlideState : IBallState
{
    public void Glide(IBallStateContext context)
    {
        context.SetState(new GlideState());
    }

    public void Roll(IBallStateContext context)
    {
    }
}

class RollState : IBallState
{
    public void Glide(IBallStateContext context)
    {
    }

    public void Roll(IBallStateContext context)
    {
        context.SetState(new RollState());
    }
}

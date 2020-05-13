using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDiscInPlay : GameState
{
    private const float MIN_DISC_SPEED = 1f;
    private const float STOPPED_DURATION = 1.0f;
    private float stoppedTime;
    private GameController gameController;
    private Disc disc;

    public override void OnEnterState(GameController gameController)
    {
        this.gameController = gameController;
        disc = gameController.GetDisc();
        gameController.AddStroke();
        disc.SetColliderEnabled(true);
        gameController.GetCurrentHole().HoleTrigger.OnHoleCompleteListener += OnHoleComplete;
        stoppedTime = 0f;
    }

    public override void OnExitState(GameController gameController)
    {
        //disc.SetColliderEnabled(false);
        gameController.GetCurrentHole().HoleTrigger.OnHoleCompleteListener -= OnHoleComplete;
    }

    public override void OnUpdate(GameController gameController)
    {
            CheckStopped();
    }

    private void CheckStopped()
    {
        Vector3 p = disc.transform.position;
        if (disc.GetScalarVelocity() < MIN_DISC_SPEED)
        {
            stoppedTime += Time.deltaTime;
        }
        else
        {
            stoppedTime = 0f;
        }

        if (stoppedTime > STOPPED_DURATION)
        {
                OnDiscStopped(p);
        }
    }

    private void OnDiscStopped(Vector3 position)
    {
        gameController.SetState(State.STROKE_COMPLETE);
    }

    private void OnHoleComplete()
    {
        gameController.OnHoleComplete();
    }
}

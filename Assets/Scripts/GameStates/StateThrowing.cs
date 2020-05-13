using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateThrowing : GameState
{
    private ThrowController throwController;
    private GvrControllerInputDevice controller;

    public override void OnEnterState(GameController gameController)
    {
        throwController = gameController.GetThrowController();
        controller = gameController.GetController();
        throwController.StartThrow(controller);
        gameController.GetArrow().SetActive(true);
        Disc disc = gameController.GetDisc();
        disc.SetColliderEnabled(false);
        //TransparencyUtil.MakeObjectOpaque(disc.gameObject);
    }

    public override void OnExitState(GameController gameController)
    {
        gameController.GetArrow().SetActive(false);
        gameController.GetCurrentHole().HoleTrigger.SetTriggerEnabled(true);
    }

    public override void OnUpdate(GameController gameController)
    {
        if (controller.GetButton(GvrControllerButton.TouchPadButton))
            throwController.UpdateDiscVelocity(controller);
        else if (controller.GetButtonUp(GvrControllerButton.TouchPadButton))
        {
            if(throwController.ReleaseDisc(controller))
                gameController.SetState(State.DISC_IN_PLAY);
            else
                gameController.SetState(State.PRE_THROW);
        }
    }
}

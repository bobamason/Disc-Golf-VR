using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePreThrow : GameState
{
    private Vector3 direction;
    private GameObject arrow;

    public override void OnEnterState(GameController gameController)
    {
        arrow = gameController.GetArrow();
        //arrow.SetActive(true);
        arrow.SetActive(false);
        direction = Camera.main.transform.forward;
        direction.y = 0;
        direction.Normalize();

        Disc disc = gameController.GetDisc();
        disc.SetColliderEnabled(false);
        ThrowController throwController = gameController.GetThrowController();
        disc.Grab(throwController.heldTransform);

        Vector3 holePosition = gameController.GetCurrentHole().HoleTrigger.transform.position;
        throwController.SetTargetPosition(holePosition + new Vector3(0, 1, 0));
        SetArrowTransform(gameController);
    }

    public override void OnExitState(GameController gameController)
    {
        arrow.SetActive(false);
    }

    public override void OnUpdate(GameController gameController)
    {
        GvrControllerInputDevice controller = gameController.GetController();

        //Vector3 lookDirection = Camera.main.transform.forward;
        //lookDirection.y = 0;
        //lookDirection.Normalize();

        //if(Vector3.Dot(direction, lookDirection) < 0.95f)
        //{
        //direction = Vector3.Slerp(direction, lookDirection, Time.deltaTime);
        //SetArrowTransform(gameController);
        //}


        if (controller.GetButtonDown(GvrControllerButton.TouchPadButton))
        {
            gameController.GetThrowController().SetDirection(direction);
            gameController.SetState(State.THROWING);
        }
    }

    private void SetArrowTransform(GameController gameController)
    {
        //arrow.transform.position = gameController.GetPlayer().GetComponent<Transform>().position;
        //arrow.transform.localRotation = Quaternion.LookRotation(direction);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState : ScriptableObject{

    public enum State
    {
        PAUSED, PRE_THROW, THROWING, DISC_IN_PLAY, STROKE_COMPLETE, HOLE_COMPLETE, COURSE_COMPLETE
    }

    public abstract void OnEnterState(GameController gameController);
    public abstract void OnUpdate(GameController gameController);
    public abstract void OnExitState(GameController gameController);
}

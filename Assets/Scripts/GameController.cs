using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public GameObject laser;
    public GvrPointerPhysicsRaycaster raycaster;
    public Disc disc;
    public ThrowController throwController;
    public GameObject marker;
    public GameObject arrow;
    public GameObject pauseMenu;
    public ScoreboardController scoreboardController;
    public Popup popup;
    public Vector3 markerOffset;
    public Vector3 playerOffset;
    public BoundsHandler boundsHandler;

    public Hole[] holes;

    private int currentHoleIndex = 0;
    private GameObject player;
    private GvrControllerInputDevice controller;
    private GameState currentState = null;
    private GameState previousState = null;

    private StatePreThrow statePreThrow;
    private StateThrowing stateThrowing;
    private StateDiscInPlay stateDiscInPlay;
    private StateStrokeComplete stateStrokeComplete;
    private StateOutOfBounds stateOutOfBounds;
    private StateHoleComplete stateHoleComplete;
    private StateCourseComplete stateCourseComplete;
    private StatePaused statePaused;
    private bool paused = false;
    private bool hidePopupDuringPause = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        controller = GvrControllerInput.GetDevice(GvrControllerHand.Dominant);
        boundsHandler.OutOfBoundsListener += OutOfBounds;
        InitStates();
        SetScoreboardPars();
        SetupHoleSigns();
        SetRaycastingEnabled(false);
        MovePlayerToCurrentHoleStart();
        SetState(statePreThrow);
        disc.gameObject.SetActive(false);
        scoreboardController.gameObject.SetActive(false);
        pauseMenu.SetActive(false);
        popup.gameObject.SetActive(false);
    }

    private void InitStates()
    {
        statePreThrow = ScriptableObject.CreateInstance<StatePreThrow>();
        stateThrowing = ScriptableObject.CreateInstance<StateThrowing>();
        stateDiscInPlay = ScriptableObject.CreateInstance<StateDiscInPlay>();
        stateStrokeComplete = ScriptableObject.CreateInstance<StateStrokeComplete>();
        stateOutOfBounds = ScriptableObject.CreateInstance<StateOutOfBounds>();
        stateHoleComplete = ScriptableObject.CreateInstance<StateHoleComplete>();
        stateCourseComplete = ScriptableObject.CreateInstance<StateCourseComplete>();
        statePaused = ScriptableObject.CreateInstance<StatePaused>();
    }

    void Update()
    {
        if (controller == null)
        {
            Debug.LogError("there is no dominant hand Daydream controller connected");
            return;
        }

        if (controller.GetButtonDown(GvrControllerButton.App))
        {
            if (paused)
            {
                if (hidePopupDuringPause)
                    popup.gameObject.SetActive(true);
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (currentState != null && !paused)
            currentState.OnUpdate(this);
    }

    private void ResumeGame()
    {
        SetRaycastingEnabled(false);
        pauseMenu.SetActive(false);
        SetScoreboardActive(false);
        Button3D[] buttons = pauseMenu.GetComponentsInChildren<Button3D>();
        foreach (Button3D b in buttons)
        {
            b.ResetAnimation();
        }
        paused = false;
    }

    private void PauseGame()
    {
        hidePopupDuringPause = popup.isActiveAndEnabled;
        if (hidePopupDuringPause)
            popup.gameObject.SetActive(false);

        SetRaycastingEnabled(true);
        pauseMenu.SetActive(true);
        Button3D[] buttons = pauseMenu.GetComponentsInChildren<Button3D>();
        foreach (Button3D b in buttons)
        {
            b.Animate();
        }
        SetScoreboardActive(true);
        paused = true;
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void MovePlayerToCurrentHoleStart()
    {
        ResetHoleTriggers();
        Hole currentHole = GetCurrentHole();
        popup.SetHole(currentHoleIndex, currentHole.par);
        Transform holeStartTransform = currentHole.HoleStart.gameObject.transform;
        Transform holeTriggerTransform = currentHole.HoleTrigger.gameObject.transform;

        player.transform.SetPositionAndRotation(holeStartTransform.position + playerOffset, currentHole.HoleStart.gameObject.transform.rotation);
        marker.SetActive(true);
        marker.transform.position = holeTriggerTransform.position + markerOffset;
    }

    private void ResetHoleTriggers()
    {
        for (int i = 0; i < holes.Length; i++)
        {
            holes[i].HoleTrigger.SetTriggerEnabled(i == currentHoleIndex);
        }
    }

    private void SetScoreboardPars()
    {
        for (int i = 0; i < holes.Length; i++)
        {
            scoreboardController.SetParForHole(i, holes[i].par);
        }
    }

    private void SetupHoleSigns()
    {
        for (int i = 0; i < holes.Length; i++)
        {
            HoleStart holeStart = holes[i].HoleStart;
            Transform holeTriggerTransform = holes[i].HoleTrigger.gameObject.transform;
            holeStart.SetHoleNumber(i + 1);
            holeStart.SetRange(Mathf.RoundToInt(Vector3.Distance(holeStart.gameObject.transform.position, holeTriggerTransform.position)), "m");
        }
    }

    public void MovePlayerToDiscPosition(Vector3 position)
    {
        Debug.Log("Moving player to position: " + position);
        //todo remove need for hole test
        if (currentHoleIndex >= holes.Length) return;

        Hole currentHole = GetCurrentHole();
        Vector3 holePosition = currentHole.HoleTrigger.transform.position;

        Vector2 pos2d = new Vector2(position.x, position.z);
        Vector2 holePos2d = new Vector2(holePosition.x, holePosition.z);
        if (Vector2.Distance(pos2d, holePos2d) < 1f)
        {
            Vector2 offset = (pos2d - holePos2d).normalized * 1f;
            player.transform.position = new Vector3(holePosition.x + offset.x, holePosition.y, holePosition.z + offset.y) + playerOffset;
        }
        else
        {
            player.transform.position = position + playerOffset;
        }

        player.transform.LookAt(new Vector3(holePosition.x, player.transform.position.y, holePosition.z));
    }

    public void RestartHole()
    {
        MovePlayerToCurrentHoleStart();
        disc.gameObject.SetActive(false);

        scoreboardController.ClearScoreForHole(currentHoleIndex);
        GetCurrentHole().HoleTrigger.SetTriggerEnabled(true);
        SetState(statePreThrow);
        
        ResumeGame();
    }

    public void RestartCourse()
    {
        disc.gameObject.SetActive(false);

        scoreboardController.ClearScore();
        currentHoleIndex = 0;
        ResetHoleTriggers();

        MovePlayerToCurrentHoleStart();
        SetState(statePreThrow);
        
        ResumeGame();
    }

    public void OnHoleComplete()
    {
        marker.SetActive(false);
        SetState(stateHoleComplete);
    }

    public void GoToNextHole()
    {
        if (currentHoleIndex >= holes.Length - 1)
        {
            GameObject.FindGameObjectWithTag("Environment").SetActive(false);
            disc.gameObject.SetActive(false);
            Debug.Log("Course Complete!!!");
            scoreboardController.CalculateAndDisplayDifferenceTotal();
            SetState(stateCourseComplete);
            return;
        }
        else
        {
            currentHoleIndex++;
            MovePlayerToCurrentHoleStart();
            SetState(statePreThrow);
        }
    }

    public void OutOfBounds()
    {
        SetState(stateOutOfBounds);
    }

    private void SetState(GameState state)
    {
        if (currentState != null)
            currentState.OnExitState(this);

        previousState = currentState;

        currentState = state;

        if (currentState != null)
            currentState.OnEnterState(this);

        //Debug.Log("Current State: " + currentState == null ? "null" : currentState.GetType().Name);
    }

    public void SetState(GameState.State state)
    {
        switch (state)
        {
            case GameState.State.PAUSED:
                SetState(statePaused);
                break;
            case GameState.State.PRE_THROW:
                SetState(statePreThrow);
                break;
            case GameState.State.THROWING:
                SetState(stateThrowing);
                break;
            case GameState.State.DISC_IN_PLAY:
                SetState(stateDiscInPlay);
                break;
            case GameState.State.STROKE_COMPLETE:
                SetState(stateStrokeComplete);
                break;
            case GameState.State.HOLE_COMPLETE:
                SetState(stateHoleComplete);
                break;
            case GameState.State.COURSE_COMPLETE:
                SetState(stateCourseComplete);
                break;
        }
    }

    public void AddStroke()
    {
        int score = scoreboardController.GetScoreForHole(currentHoleIndex);
        scoreboardController.SetScoreForHole(currentHoleIndex, score + 1);
    }

    public void SetScoreboardActive(bool active)
    {
        scoreboardController.gameObject.SetActive(active);
        if (active)
        {
            Vector3 direction = Camera.main.transform.forward;
            direction.y = 0;
            direction.Normalize();
            scoreboardController.transform.parent.transform.localRotation = Quaternion.Inverse(player.transform.rotation) * Quaternion.LookRotation(direction);
            scoreboardController.Animate();
        }
        else scoreboardController.ResetAnimation();
    }

    public void SetPopupActive(bool active)
    {
        popup.gameObject.SetActive(active);
        if (active)
        {
            Vector3 direction = Camera.main.transform.forward;
            direction.y = 0;
            direction.Normalize();
            popup.transform.parent.transform.localRotation = Quaternion.Inverse(player.transform.rotation) * Quaternion.LookRotation(direction);
            //popup.Animate();
        }
        //else popup.ResetAnimation();
    }

    public GameState GetPreviousState()
    {
        return previousState;
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public GameObject GetMarker()
    {
        return marker;
    }

    public GameObject GetArrow()
    {
        return arrow;
    }

    public GameObject GetPauseMenu()
    {
        return pauseMenu;
    }

    public Disc GetDisc()
    {
        return disc;
    }

    public GvrControllerInputDevice GetController()
    {
        return controller;
    }

    public ThrowController GetThrowController()
    {
        return throwController;
    }

    public ScoreboardController GetScoreboardController()
    {
        return scoreboardController;
    }

    public Popup GetPopup()
    {
        return popup;
    }

    public Hole GetCurrentHole()
    {
        return holes[currentHoleIndex];
    }

    public int GetStrokes()
    {
        return scoreboardController.GetScoreForHole(currentHoleIndex);
    }

    public int GetPar()
    {
        return GetCurrentHole().par;
    }

    public void SetRaycastingEnabled(bool enabled)
    {
        raycaster.enabled = enabled;
        laser.SetActive(enabled);
    }
}

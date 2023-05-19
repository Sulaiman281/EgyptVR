using System;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TabletPhone : NetworkBehaviour
{
    [SerializeField] private PlayerInput.ActionEvent onTabletStart;

    [Header("Labels")] [SerializeField] private TMP_Text relayCodeTmp;
    [SerializeField] private TMP_Text pointsTmp;

    [Header("Objectives")]
    [SerializeField] private TMP_Text[] objectivesTmp;
    [SerializeField] private Color solvedObjective;

    [Header("Network Ref")] [SerializeField]
    private XRGrabInteractable grabInteractable;

    public string[] objectives = new string[3];
    
    private void Start()
    {
        
        onTabletStart.Invoke(default);
        relayCodeTmp.text = GameManager.instance.joinCode;
        // set the lessons 
        if (!IsOwner)
        {
            grabInteractable.enabled = false;
            return;
        }
        var lessons = FindObjectsOfType<RobotScript>().Where(robot => robot.isGivingLesson).ToList().Count;
        Debug.Log(lessons+" Lesson To Take");
        GameManager.instance.localPlayer.lessonsToTake = lessons;
        objectives[0] =
            $"Take {lessons} Lessons From the robot.";
        objectives[1] = "Take Quizzes";
        objectives[2] = "Create Your 3D Model";
        UpdateObjectives(GameManager.instance.localPlayer.data.Value.priorityState);
    }
    
    public override void OnGainedOwnership()
    {
        base.OnGainedOwnership();
        if (!IsOwner) return;
        GameManager.instance.playerMapRef.phone = this;
        GameManager.instance.playerMapRef.PutTabletIntoSocket();
    }

    private void LateUpdate()
    {
        if (!IsOwner) return;
        if (grabInteractable.isSelected) return;
        GameManager.instance.playerMapRef.PutTabletIntoSocket();
    }

    public void RoomPos()
    {
        if (GameManager.instance == null) return;
        GameManager.instance.ResetPlayerPosition();
    }

    public void UpdateObjectives(PriorityState valuePriorityState)
    {
        switch (valuePriorityState)
        {
            case PriorityState.Lessons:
                objectivesTmp[0].text = objectives[0];

                objectivesTmp[0].color = Color.white;
                objectivesTmp[1].color = Color.white;
                objectivesTmp[2].color = Color.white;
                break;
            case PriorityState.Quiz:
                objectivesTmp[1].text = objectives[1];

                objectivesTmp[0].color = solvedObjective;
                objectivesTmp[1].color = Color.white;
                objectivesTmp[2].color = Color.white;
                break;
            case PriorityState.Modeling:
                objectivesTmp[2].text = objectives[2];
                
                objectivesTmp[0].color = solvedObjective;
                objectivesTmp[1].color = solvedObjective;
                objectivesTmp[2].color = Color.white;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(valuePriorityState), valuePriorityState, null);
        }
    }

    #region TrackPlayers

    [Header("Prefabs")] [SerializeField] private GameObject playerBox;
    [SerializeField] private Transform contentPlayers;

    public void ShowPlayers()
    {
        ClearOldContent();
        var players = FindObjectsOfType<PlayerVisualControls>();
        foreach (var playerVisualControls in players)
        {
            var pBox = Instantiate(playerBox, contentPlayers);
            var pTmp = pBox.GetComponentsInChildren<TMP_Text>();
            pTmp[0].text = playerVisualControls.data.Value.playerName;
            pTmp[1].text = $"{playerVisualControls.data.Value.points}";
        }
    }
    private void ClearOldContent()
    {
        for (var i = 0; i < contentPlayers.childCount; i++)
        {
            DestroyImmediate(contentPlayers.GetChild(i).gameObject);
        }
    }

    #endregion
}
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class RobotScript : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayableAsset[] playerDirectories;
    [SerializeField] private TMP_Text instructionText;
    [SerializeField] private PlayableDirector director;
    [SerializeField] private Transform playerTarget;
    public void SetRobotInstructionText(string txt)
    {
        instructionText.text = txt;
    }

    public void ActionResponse(int action)
    {
        animator.SetInteger("ResponseAction", action);
    }

    private void Update()
    {
        var dist = Vector3.Distance(transform.position, playerTarget.position);
        if (dist > 5)
        {
            transform.LookAt(playerTarget.position, Vector3.up);
            
        }
    }


    public void Pause(bool value = true)
    {
        if (value)
        {
            director.Pause();
        }
        else
        {
            director.Resume();
        }
    }

    public void StopDirector()
    {
        director.Stop();
    }
}

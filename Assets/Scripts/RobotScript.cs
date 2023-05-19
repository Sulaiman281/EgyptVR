using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class RobotScript : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private GameObject hintObject;

    [SerializeField] private PlayerInput.ActionEvent onDialogFinished;

    public bool isGivingLesson;
    public bool isLessonComplete;

    private bool _inSideRadius;
    private bool _isPlaying;

    private void Start()
    {
        InputManager.instance.leftController.onTriggerPressStart.AddListener(_=> OnTriggerPress());
        InputManager.instance.rightController.onTriggerPressStart.AddListener(_=> OnTriggerPress());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _inSideRadius = true;
        hintObject.SetActive(true);
    }

    private void OnValidate()
    {
        if (animator == null) animator = GetComponent<Animator>();
    }

    public void OnTriggerPress()
    {
        if (!_inSideRadius) return;
        if (audioClip == null) return;
        if (isLessonComplete && isGivingLesson) return;
        if (_isPlaying) return;
        AudioSource.PlayClipAtPoint(audioClip, transform.position, 1f);
        _isPlaying = true;
        Invoke(nameof(InvokeDialogFinished), audioClip.length);
        isLessonComplete = true;
        if (isGivingLesson)
            GameManager.instance.localPlayer.LessonTaken();
    }

    private void InvokeDialogFinished()
    {
        _isPlaying = false;
        onDialogFinished.Invoke(default);
    }

    private void OnTriggerExit(Collider other)
    {
        _inSideRadius = false;
        hintObject.SetActive(false);
    }
}
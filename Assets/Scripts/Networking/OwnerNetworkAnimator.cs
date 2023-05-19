using Unity.Netcode.Components;
using UnityEngine;

public class OwnerNetworkAnimator : NetworkAnimator
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }

    private void OnValidate()
    {
        if (Animator == null)
            Animator = GetComponent<Animator>();
    }
}
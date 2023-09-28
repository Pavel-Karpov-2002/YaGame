using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> ragdollElements;
    [SerializeField] private Animator _animator;

    public List<Rigidbody> RagdollElements => ragdollElements;

    private void Awake()
    {
        DisablePhysics();
    }

    public void EnablePhysics()
    {
        _animator.enabled = false;

        for (int i = 0; i < ragdollElements.Count; i++)
            ragdollElements[i].isKinematic = false;
    }

    public void DisablePhysics()
    {
        _animator.enabled = true;

        for (int i = 0; i < ragdollElements.Count; i++)
            ragdollElements[i].isKinematic = true;
    }
}

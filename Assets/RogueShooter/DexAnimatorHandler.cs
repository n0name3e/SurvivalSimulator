using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeonEscape
{
    public class DexAnimatorHandler : MonoBehaviour
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
        }
        public void UpdateAnimatorValues(float h, float v, float speed)
        {
            animator.speed = speed;
            animator.SetFloat("Speed", Mathf.Abs(v) + Mathf.Abs(h));
        }
    }
}

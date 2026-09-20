using System;
using UnityEngine;
    public class Arrow : MonoBehaviour
    {
        [SerializeField] private float speed = 12f;

        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            rb.linearVelocity = transform.right * speed;
        }
    }
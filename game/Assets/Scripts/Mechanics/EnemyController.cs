using System.Collections;
using System.Collections.Generic;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// A simple controller for enemies. Provides movement control over a patrol path.
    /// </summary>
    [RequireComponent(typeof(AnimationController), typeof(Collider2D))]
    public class EnemyController : MonoBehaviour
    {
        public PatrolPath path;
        public AudioClip ouch;
        public float viewDistance = 5f;

        private Transform player;
        internal PatrolPath.Mover mover;
        internal AnimationController control;
        internal Collider2D _collider;
        internal AudioSource _audio;
        SpriteRenderer spriteRenderer;

        public Bounds Bounds => _collider.bounds;

        void Awake()
        {
            control = GetComponent<AnimationController>();
            _collider = GetComponent<Collider2D>();
            _audio = GetComponent<AudioSource>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        void Start()
        {
            
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                var ev = Schedule<PlayerEnemyCollision>();
                ev.player = player;
                ev.enemy = this;
            }
        }

        void Update()
        {
            if (path != null)
            {
                if (mover == null) mover = path.CreateMover(control.maxSpeed * 0.5f);
                control.move.x = Mathf.Clamp(mover.Position.x - transform.position.x, -1, 1);
            }

            Vector2 direction = player.position - transform.position;
            Debug.DrawRay(transform.position, direction.normalized * viewDistance, Color.red);
            int layerMask = LayerMask.GetMask("Default", "Player");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, viewDistance, layerMask);
            if (hit)
            {

                if (hit.transform.CompareTag("Player"))
                {
                    Debug.Log("Enemy spotted the player!");
                    // Replace with alert UI or other handling
                }
                else
                {
                    // Log the name of the object that was hit if it was not the player
                    Debug.Log("Raycast hit: " + hit.transform.name + " on layer: " + LayerMask.LayerToName(hit.transform.gameObject.layer));
                }
            }
        }

    }
}
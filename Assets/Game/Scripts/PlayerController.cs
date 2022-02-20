using System;
using Game.Scripts.Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts
{
    public enum MovementMode
    {
        Platformer,
        Strafe
    }
    
    public class PlayerController : MonoBehaviour
    {
        [Header("Player settings")]
        [SerializeField] private MovementMode _movementMode = MovementMode.Strafe;
        [SerializeField] private float _walkSpeed = 3f;
        [SerializeField] private float _runningSpeed = 6f;
        [SerializeField] private float _gravity = 9.81f;
        [SerializeField] private float _gravityPlatformer = -12f;
        [SerializeField] private float _jumpSpeed = 3.5f;
        [SerializeField] private float _doubleJumpMultiplier = 0.5f;
        [SerializeField] private GameObject _cameraRig;
        [Tooltip("Damage received when falling at the maximum speed")] 
        [SerializeField]
        private float _fallDamage = 10.0f;
        public float jumpHeight = 1;
        public float turnSmoothTime = 0.2f;
        public bool IsDead { get; private set; }
        private CharacterController _controller;
        private float _directionY;
        private float _currentSpeed;
        private bool _canDoubleJump = false;
        private float _turnSmoothVelocity;
        public float speedSmoothTime = 0.1f;
        private float _speedSmoothVelocity;
        private float _velocityY;
        private Health _health;

        [Header("Audio")] 
        [Tooltip("Sound played when player walk")]
        public AudioClip WalkSfx;

        [Tooltip("Sound played when player jump")]
        public AudioClip JumpSfx;
        
        [Tooltip("Sound played when falling to death")]
        [SerializeField]
        public AudioClip FallScreamSfx;

        [Tooltip("Audio source for footsteps, jump, etc...")]
        public AudioSource AudioSource;

        void Start()
        {
            _controller = GetComponent<CharacterController>();
            _health = GetComponent<Health>();
            
            var ring = GameObject.FindWithTag("ring");
            ring.gameObject.SetActive(true);
            
            _health.OnDie += OnDie;
        }

        private void OnDie()
        {
            IsDead = true;
            EventManager.Broadcast(Events.PlayerDeathEvent);
        }

        void Update()
        {
            if (_movementMode == MovementMode.Strafe)
            {
                MovementStafe();
            }
            
            if (_movementMode == MovementMode.Platformer)
            {
                MovementPlatformer();
            }
        }

        private void MovementStafe()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);

            if (_controller.isGrounded)
            {
                _canDoubleJump = true;

                if (Input.GetButtonDown("Jump"))
                {
                    _directionY = _jumpSpeed;
                }
            }
            else
            {
                if (Input.GetButtonDown("Jump") && _canDoubleJump)
                {
                    _directionY = _jumpSpeed * _doubleJumpMultiplier;
                    _canDoubleJump = false;
                }
            }

            _directionY -= _gravity * Time.deltaTime;

            moveDirection = transform.TransformDirection(moveDirection);

            bool running = Input.GetKey(KeyCode.LeftShift);
            float targetSpeed = (running) ? _runningSpeed : _walkSpeed;
            _currentSpeed = Mathf.SmoothDamp(_currentSpeed, targetSpeed, ref _speedSmoothVelocity, speedSmoothTime);

            moveDirection.y = _directionY;

            _controller.Move(_currentSpeed * Time.deltaTime * moveDirection);
        }

        private bool _playWalkingSound;
        private bool _isPlayDeathSfx;

        private void MovementPlatformer()
        {
            var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            var inputDir = input.normalized;
            var running = Input.GetKey(KeyCode.LeftShift);

            if (inputDir != Vector2.zero)
            {
                float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg +
                                       _cameraRig.transform.eulerAngles.y;
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation,
                    ref _turnSmoothVelocity, turnSmoothTime);
            }

            // if the player is falling play the death sound
            if (Math.Abs(_velocityY - (-10.0f)) < 1)
            {
                if (!_isPlayDeathSfx)
                {
                    _isPlayDeathSfx = true;
                    var audioSource = gameObject.AddComponent<AudioSource>();
                    audioSource.clip = FallScreamSfx;
                    audioSource.playOnAwake = false;
                    audioSource.Play();
                }
            }

            // end of the game
            if (Math.Abs(_velocityY - (-30.0f)) < 1)
                _health.TakeDamage(_fallDamage, gameObject);

            float targetSpeed = ((running) ? _runningSpeed : _walkSpeed) * inputDir.magnitude;
            _currentSpeed = Mathf.SmoothDamp(_currentSpeed, targetSpeed, ref _speedSmoothVelocity, speedSmoothTime);

            _velocityY += Time.deltaTime * _gravityPlatformer;
            Vector3 velocity = transform.forward * _currentSpeed + Vector3.up * _velocityY;

            _controller.Move(velocity * Time.deltaTime);
            _currentSpeed = new Vector2(_controller.velocity.x, _controller.velocity.z).magnitude;

            if (_currentSpeed > 0)
            {
                // I think I'm moving...
                if (!_playWalkingSound)
                {
                    AudioSource.PlayOneShot(WalkSfx);
                    _playWalkingSound = true;
                }
            }

            if (_controller.isGrounded)
            {
                _velocityY = 0;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_controller.isGrounded)
                {
                    float jumpVelocity = Mathf.Sqrt(-2 * _gravityPlatformer * jumpHeight);
                    _velocityY = jumpVelocity;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals("ring"))
                EventManager.Broadcast(Events.AllObjectivesCompletedEvent);
        }
    }
}

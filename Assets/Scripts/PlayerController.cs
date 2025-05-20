using System;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
	private Rigidbody2D playerRb;

	[ SerializeField ] private bool enablePlayerWrap = true;
	[ SerializeField ] private float jumpForce;
	[ SerializeField ] private float movementSpeed;
	[ SerializeField ] private float touchMovementSpeed = 10f;
	[SerializeField] private float touchDeadZone = 0.1f;

	[ Header("Jump Tween Parameters") ] [ SerializeField ]
	private Vector2 squashScale;

	[ SerializeField ] private Vector2 stretchScale;
	[ SerializeField ] private float totalDuration = 0.3f;

	[ Header( "Death Tween Parameters" ) ] [ SerializeField ]
	private float popDuration = 0.1f;

	[ SerializeField ] private Sprite hitOutlineSprite;

	[ SerializeField ] private float fadeDuration = 0.3f;
	[ SerializeField ] private float sinkDuration = 0.4f;

	[ SerializeField ] private Vector3 popScale = new Vector2( 1.3f, 1.3f );
	[ SerializeField ] private float sinkDistance = 1f;

	private float moveInput;
	private Transform playerVisual;
	private SpriteRenderer playerSpriteRenderer;
	private InputActions inputActions;
	private GameManager gameManagerRef;
	private Camera mainCam;

	private bool isAlive = true;
	private bool boostInvincibilityEnabled = false;

	private void Awake()
	{
		playerRb = GetComponent<Rigidbody2D>();
		inputActions = new InputActions();
	}

	private void Start()
	{
		gameManagerRef = GameManager.Instance;
		mainCam = Camera.main;
		playerVisual = transform.GetChild( 0 );
		playerSpriteRenderer = playerVisual.GetComponent<SpriteRenderer>();
	}

	private void OnEnable()
	{
		inputActions.Enable();

		inputActions.Player.Enable();
		inputActions.Player.Move.started += OnMoveAction;
		inputActions.Player.Move.performed += OnMoveAction;
		inputActions.Player.Move.canceled += OnMoveAction;
	}

	private void Update()
	{
#if UNITY_ANDROID || UNITY_IOS
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			Vector3 touchPosition = mainCam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 1f));
			touchPosition.z = 0f;

			if (touchPosition.x < transform.position.x - touchDeadZone)
			{
				moveInput = -1;
			}
			else if (touchPosition.x > transform.position.x + touchDeadZone)
			{
				moveInput = 1;
			}
			else
			{
				moveInput = 0;
			}
		}
		else
		{
			moveInput = 0f;
		}
#endif

		if ( enablePlayerWrap )
		{
			CheckPlayerWrap();
		}

		if ( IsFalling() )
		{
			boostInvincibilityEnabled = false;
		}

		if ( (int) transform.position.y > gameManagerRef.MaxHeightReached )
		{
			gameManagerRef.MaxHeightReached = ( int )transform.position.y;
		}
	}

	private void FixedUpdate()
	{
		playerRb.linearVelocityX = moveInput * movementSpeed;
		if (playerRb.linearVelocityX < 0)
		{
			playerSpriteRenderer.flipX = true;
		}
		else if (playerRb.linearVelocityX > 0)
		{
			playerSpriteRenderer.flipX = false;
		}
	}

	private void OnDisable()
	{
		playerVisual.transform.DOKill();

		inputActions.Player.Move.started -= OnMoveAction;
		inputActions.Player.Move.performed -= OnMoveAction;
		inputActions.Player.Move.canceled -= OnMoveAction;
		inputActions.Player.Disable();

		inputActions.Disable();
	}

	private void OnCollisionEnter2D( Collision2D collision )
	{
		if ( collision.gameObject.CompareTag( "KillTrigger" ) )
		{
			Die();
			return;
		}

		if ( IsFalling() && collision.gameObject.CompareTag( "Ground" ) )
		{
			DoSquashStretch();
			playerRb.linearVelocityY = jumpForce;
		}
	}

	private void OnTriggerEnter2D( Collider2D other )
	{
		if ( other.CompareTag( "Boost" ) )
		{
			OnBoostTriggered( other.GetComponent<Boost>() );
		}

		if ( other.CompareTag( "Coin" ) )
		{
			OnCoinCollected( other.GetComponent<Coin>() );
		}

		if ( !boostInvincibilityEnabled && other.CompareTag( "Obstacle" ) )
		{
			Die();
		}
	}

	private void CheckPlayerWrap()
	{
		if ( transform.position.x < gameManagerRef.screenBoundsXAxis.x )
		{
			transform.position = new Vector3( gameManagerRef.screenBoundsXAxis.y, transform.position.y, transform.position.z );
		}
		else if ( transform.position.x > gameManagerRef.screenBoundsXAxis.y )
		{
			transform.position = new Vector3( gameManagerRef.screenBoundsXAxis.x, transform.position.y, transform.position.z );
		}
	}

	private void Die()
	{
		if ( !isAlive )
		{
			return;
		}

		isAlive = false;
		playerRb.linearVelocity = Vector2.zero;
		playerRb.gravityScale = 0f;

		DoDeath();
	}

	public bool IsFalling()
	{
		return playerRb.linearVelocityY <= 0;
	}

	private void OnMoveAction( InputAction.CallbackContext context )
	{
#if !UNITY_ANDROID || !UNITY_IOS
		moveInput = context.ReadValue<float>();
#endif
	}

	private void OnBoostTriggered( Boost boost )
	{
		playerRb.linearVelocityY = boost.GetBoostAmount();
		boostInvincibilityEnabled = true;

		Destroy( boost.gameObject );
	}

	private void OnCoinCollected( Coin coin )
	{
		gameManagerRef.AddCoins( coin.GetValue() );

		Destroy( coin.gameObject );
	}

	private void OnBladeHit()
	{
		Die();
	}

	private void OnSpikeTouched()
	{
		Die();
	}

#region DOTween Sequences

	private void DoSquashStretch()
	{
		playerVisual.transform.DOKill();
		playerVisual.transform.localScale = Vector3.one;
		Sequence seq = DOTween.Sequence();

		float duration = totalDuration / 3f;

		//Squash
		seq.Append( playerVisual.transform.DOScale( new Vector2( squashScale.x, squashScale.y ), duration ).SetEase( Ease.OutQuad ) );
		//Stretch
		seq.Append( playerVisual.transform.DOScale( new Vector2( stretchScale.x, stretchScale.y ), duration ).SetEase( Ease.OutQuad ) );
		//Reset
		seq.Append( playerVisual.transform.DOScale( 1f, duration ).SetEase( Ease.OutBack ) );
	}

	private void DoDeath()
	{
		enabled = false;
		playerVisual.transform.DOKill();
		playerSpriteRenderer = playerVisual.GetComponent<SpriteRenderer>();
		playerSpriteRenderer.DOKill();

		playerSpriteRenderer.sprite = hitOutlineSprite;

		Sequence seq = DOTween.Sequence();

		seq.Append( playerVisual.transform.DOScale( popScale, popDuration ).SetEase( Ease.OutFlash ) );
		seq.Append( playerVisual.transform.DOScale( Vector2.zero, fadeDuration ).SetEase( Ease.InQuad ) );
		seq.Join( playerSpriteRenderer.DOFade( 0, fadeDuration ) );
		seq.Join( playerVisual.transform.DOMoveY( playerVisual.transform.position.y - sinkDistance, sinkDuration ).SetEase( Ease.InQuad ) );

		seq.OnComplete( () => gameManagerRef.PlayerDied() );
	}

#endregion
}
using InteractionSystem;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
	[Header("PLAYER PARAMETRS")]
	[Space(10)]
	public bool isFlashlight = false;
	public bool flashlightOn = false;
	public bool macheteOn = false;
	public float lookIKWeight;
	public float bodyWeight;

	[Header("PLAYER TARGET")]
	[Space(10)]
	public Transform targetPos;
	public float angularSpeed;
	bool isPlayerRot;
	public float luft;
	public Camera_Controller cameraController;

	[Header("STAMINA UI")]
	[Space(10)]
	[SerializeField] public Image staminaLevel = null;
	[Space(10)]
	public float restoringStamina;
	public float reducedStamina;

	[Header("PLAYER SOUND")]
	[Space(10)]
	[SerializeField] private string getFrom;
	[SerializeField] private string attackMachete;

	private Transform shoulder;
	private Transform pivot;

	Animator anim;

	Vector3 targetPosVec;
	float newRunWeight = 1f;
	float walk = 0f;
	float strafe = 0f;


	void CmdClientState(Vector3 targetPosVec,  float newRunWeight, float walk, float strafe)
	{
		this.targetPosVec = targetPosVec;
		this.newRunWeight = newRunWeight;
		this.walk = walk;
		this.strafe = strafe;
	}


	void Start()
	{
		anim = GetComponent<Animator>();

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}


    void Update() 
	{
		Locomotion();
		Running();

		Treatment();

		CheckMachete();
		Attack();

		CheckFlashlight();
        ReloadBattery();
	}


    void Locomotion()
	{
		targetPosVec = targetPos.position;

		walk = Input.GetAxis("Vertical");
		strafe = Input.GetAxis("Horizontal");

		anim.SetFloat("Strafe", strafe); 
		anim.SetFloat("Walk", walk);

		if (walk != 0 || strafe != 0 || isFlashlight == true || macheteOn == true || CameraType.FPS == cameraController.cameraType)
        {
			Vector3 rot = transform.eulerAngles;
			transform.LookAt(targetPosVec);
			float angleBetween = Mathf.DeltaAngle(transform.eulerAngles.y, rot.y);
			if ((Mathf.Abs(angleBetween) > luft) || strafe != 0)
			{
				isPlayerRot = true;
			}
			if (isPlayerRot == true)
			{
				float bodyY = Mathf.LerpAngle(rot.y, transform.eulerAngles.y, Time.deltaTime * angularSpeed);
				transform.eulerAngles = new Vector3(0, bodyY, 0);
			}
			else
			{
				transform.eulerAngles = new Vector3(0f, rot.y, 0f);
			}
		}
		transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
	}


	private void Running()
	{
		staminaLevel.fillAmount += (restoringStamina/150) * Time.deltaTime;

		if (Input.GetKey(InputManager.instance.running) && walk != 0)
        {
			anim.SetBool("Running", true);

			staminaLevel.fillAmount -= (reducedStamina/100) * Time.deltaTime;

			if (staminaLevel.fillAmount <= 0)
			{
				anim.SetBool("Running", false);
			}
		}
		else if (Input.GetKeyUp(InputManager.instance.running))
		{
			anim.SetBool("Running", false);
		}
	}

	private void Treatment()
	{
		if (Input.GetKey(InputManager.instance.treatment) && macheteOn == false)
        {
			if (gameObject.GetComponent<HealthSystem>().Health < gameObject.GetComponent<HealthSystem>().maxHealth)
            {
				if (gameObject.GetComponent<HealthSystem>().kitCount > 0)
                {
					anim.SetBool("Treatment", true);
					anim.SetBool("Running", false);
				}
			}
			if (gameObject.GetComponent<HealthSystem>().Health >= gameObject.GetComponent<HealthSystem>().maxHealth)
            {
				anim.SetBool("Treatment", false);
			}
		}
		else
		{
			anim.SetBool("Treatment", false);
		}
	}

	private void CheckMachete()
	{
		if (Input.GetKeyDown(InputManager.instance.machete) && macheteOn == false)
		{
			if (gameObject.GetComponent<CombatSystem>().hasMachete == true)
			{
				macheteOn = true;
				anim.SetBool("Machete", true);
			}
		}
		else if (Input.GetKeyDown(InputManager.instance.machete) && macheteOn == true)
		{
			macheteOn = false;
			anim.SetBool("Machete", false);
		}
	}

	void Attack()
	{
		if (macheteOn == true)
        {
			if (Input.GetMouseButtonDown(0))
				anim.SetTrigger("LeftMouseClick");
		}
	}

	void AttackSoundEvent()
	{
		AudioManager.instance.Play(attackMachete);
	}

	private void ReloadBattery()
	{
		if (Input.GetKey(InputManager.instance.reloadBattery))
        {
			if (gameObject.GetComponent<FlashlightSystem>().flashlightSpot.intensity < gameObject.GetComponent<FlashlightSystem>().maxFlashlightIntensity)
            {
				if (gameObject.GetComponent<FlashlightSystem>().batteryCount > 0)
                {
					anim.SetBool("Reload", true);
					anim.SetBool("Running", false);
				}
				if (gameObject.GetComponent<FlashlightSystem>().batteryCount < 1)
				{
					anim.SetBool("Reload", false);
				}
			}
		}
		else
		{
			anim.SetBool("Reload", false);
		}
	}

	void OnAnimatorIK()
	{
		if (isFlashlight)
		{
			anim.SetLookAtWeight(lookIKWeight, bodyWeight);
			anim.SetLookAtPosition(targetPosVec);
		}
	}
 
	private void CheckFlashlight()
    {
		if (Input.GetKeyDown(InputManager.instance.flashlightSwitch) && isFlashlight == false)
		{
			if (gameObject.GetComponent<FlashlightSystem>().hasFlashlight == true)
            {
				flashlightOn = true;
				isFlashlight = true;
				anim.SetBool("Flashlight", true);
			}
		}
		else if (Input.GetKeyDown(InputManager.instance.flashlightSwitch) && isFlashlight == true)
		{
			flashlightOn = false;
			isFlashlight = false;
			anim.SetBool("Flashlight", false);
		}

		if (gameObject.GetComponent<FlashlightSystem>().flashlightSpot.intensity <= 0)
        {
			flashlightOn = false;
			isFlashlight = false;
			anim.SetBool("Flashlight", false);
		}
	}

	protected void IKFlashlight()
	{
		this.pivot.position = this.shoulder.position;

		if (isFlashlight)
		{
			this.pivot.LookAt(this.targetPos);
			this.SetisFlashlightWeight(1f, 0.3f, 1f);
		}
		else
		{
			this.SetisFlashlightWeight(0.3f, 0, 0);
		}
	}

	private void SetisFlashlightWeight(float weight, float bodyWeight, float headWeight)
	{
		this.anim.SetLookAtWeight(weight, bodyWeight, headWeight);
		this.anim.SetLookAtPosition(this.targetPos.position);
	}

	void GetFromSoundEvent()
	{
		AudioManager.instance.Play(getFrom);
	}
}

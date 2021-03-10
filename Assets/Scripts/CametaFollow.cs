using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class CametaFollow : MonoBehaviour
{

	public float damping = 1.5f;
	public float SpeedChangeSizeCamera = 5;
	public float MaxCameraSize = 15;
	public Vector2 offset = new Vector2(2f, 1f);
	private Transform player;
	private Rigidbody2D PlayerSpeed;

	private Camera _cameraSize;
	private float offsetCameraSize;

	void Start()
	{
		offset = new Vector2(Mathf.Abs(offset.x), offset.y);
		player = GameObject.FindGameObjectWithTag("Player").transform;
		PlayerSpeed = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
		offsetCameraSize = GetComponent<Camera>().orthographicSize;
		_cameraSize = GetComponent<Camera>();
		if (!player)
        {
			Debug.Log("No Player in the scene");
			gameObject.GetComponent<CametaFollow>().enabled = false;
        }
		transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
	}

    private void FixedUpdate()
    {

	}

    void Update()
	{
		/*if (ModuleVector(PlayerSpeed.velocity) > 1)
		{
			_cameraSize.orthographicSize = offsetCameraSize + (ModuleVector(PlayerSpeed.velocity) * SpeedChangeSizeCamera);
		}*/
		if( ModuleVector(PlayerSpeed.velocity)*0.8 + 0.1> _cameraSize.orthographicSize && _cameraSize.orthographicSize < MaxCameraSize)
        {
			_cameraSize.orthographicSize += 5*SpeedChangeSizeCamera * Time.deltaTime;

		}
        else if((ModuleVector(PlayerSpeed.velocity) * 0.8 - 0.1< _cameraSize.orthographicSize) && _cameraSize.orthographicSize > offsetCameraSize)
		{
			_cameraSize.orthographicSize -= 10 * SpeedChangeSizeCamera * Time.deltaTime;
		}
		Vector3 target = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
		transform.position = Vector3.Lerp(transform.position, target, damping * Time.deltaTime);
	}



	public float ModuleVector(Vector2 Vector)
	{
		return math.sqrt(Vector.x * Vector.x + Vector.y * Vector.y);
	}
}
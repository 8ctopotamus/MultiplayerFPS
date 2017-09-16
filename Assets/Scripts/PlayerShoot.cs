using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

	private const string PLAYER_TAG = "Player";

	public PlayerWeapon weapon;

	[SerializeField]
	private Camera cam;

	[SerializeField]
	private LayerMask mask;

	void Start () {
		if (cam == null) {
			Debug.Log("PlayerShoot: No camera referenced!");
			this.enabled = false;
		}
	}

	void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			Shoot();
		}
	}

	[Client]
	void Shoot () {
		RaycastHit _hit;
		if (Physics.Raycast (cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask)) {
			if (_hit.collider.tag == PLAYER_TAG) {
				CmdPlayerShot (_hit.collider.name, weapon.damage);
			}
		}
	}

	[Command]
	void CmdPlayerShot (string _playerId, int _damage) {
		Debug.Log(_playerId + " has been shot.");

		Player _player = GameManager.GetPlayer (_playerId);
		_player.RpcTakeDamage (_damage);
	}

}

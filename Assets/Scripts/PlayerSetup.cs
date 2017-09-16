// ----------------------------
// Responsible for setting up the player.
// This includings adding/removing it correctly on the network
// ----------------------------

using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	Behaviour[] componentsToDisable;

	[SerializeField]
	string remoteLayerName = "RemotePlayer";

	Camera sceneCamera;

	void Start () {
		// disable components that should only be
		// active on the player that we control
		if (!isLocalPlayer) {
			DisableComponents ();
			AssignRemoteLayer ();
		} else {
			// we are the local player: disable the scene camera
			sceneCamera = Camera.main;
			if (sceneCamera != null) {
				sceneCamera.gameObject.SetActive (false);
			}
		}
	}

	public override void OnStartClient () {
		base.OnStartClient ();

		string _netID = GetComponent<NetworkIdentity> ().netId.ToString ();
		Player _player = GetComponent<Player> ();

		GameManager.RegisterPlayer (_netID, _player);
	}

	void AssignRemoteLayer () {
		gameObject.layer = LayerMask.NameToLayer (remoteLayerName);
	}

	void DisableComponents () {
		for (int i = 0; i < componentsToDisable.Length; i++) {
			componentsToDisable[i].enabled = false;
		}
	}

	// when we are destroyed
	void OnDisable () {
		// re-enable the scene camera
		if (sceneCamera != null) {
			sceneCamera.gameObject.SetActive (true);
		}

		GameManager.UnRegisterPlayer (transform.name);
	}

}

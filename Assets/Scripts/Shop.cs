using UnityEngine;

public class Shop : MonoBehaviour {

	public TurretBlueprint antivirus;
	public TurretBlueprint encryption;
	public TurretBlueprint biometrics;
	public TurretBlueprint firewall;

	BuildManager buildManager;

	void Start ()
	{
		buildManager = BuildManager.instance;
	}

	public void SelectAntivirus()
	{
		Debug.Log("Antivirus Selected");
		buildManager.SelectTurretToBuild(antivirus);
	}

	public void SelectEncryption()
	{
		Debug.Log("Encryption Selected");
		buildManager.SelectTurretToBuild(encryption);
	}

	public void SelectBiometrics()
	{
		Debug.Log("Biometrics Selected");
		buildManager.SelectTurretToBuild(biometrics);
	}

	public void SelectFirewall()
	{
		Debug.Log("Firewall Selected");
		buildManager.SelectTurretToBuild(firewall);
	}
}

using HutongGames.PlayMaker;
using MSCLoader;
using System;
using UnityEngine;

namespace DUSTMAN
{
    public class DUSTMAN : Mod
    {
        public override string ID => "Plymouth";
        public override string Name => "Plymouth";
        public override string Author => "Flat0ut Reincarnated";
        public override string Version => "1.0";
        public override Game SupportedGames => Game.MyWinterCar;

		private GameObject DOORR;
		private GameObject DOORTRIGGERR;
		private GameObject DOORL;
		private GameObject DOORTRIGGERL;
		private GameObject interiorlight;
		private GameObject SPECLIGHTS;
		private GameObject BRAKELIGHTS;
		private GameObject Exhaust;
		private GameObject TRUNKTRIGGER;
		private GameObject TRUNK;
		private GameObject PLAYER;
		private GameObject DOORCLOSEL;
		private GameObject DOORCLOSER;
		private GameObject DOOROPENL;
		private GameObject DOOROPENR;
		private GameObject TRUNKCLOSE;
		private GameObject TRUNKOPEN;
		private GameObject NEEDLE;
		private GameObject NEEDLESPEED;
		private GameObject HANDBRAKE;
		private GameObject HANDBRAKETRIGGER;
		private GameObject PEDAL_gas;
		private GameObject PEDAL_clutch;
		private GameObject PEDAL_brake;
		public GameObject KEY;
		public GameObject WHEELFL;
		public GameObject WHEELFR;
		public GameObject WHEELRL;
		public GameObject WHEELRR;
		private GameObject IMPACT_FRONT_R;
		private GameObject IMPACT_FRONT_L;
		private GameObject IMPACT_REAR_R;
		private GameObject IMPACT_REAR_L;
		public GameObject SHIFTER;
		public GameObject positionRR;
		public GameObject positionRL;
		public GameObject IGNITION;
		public GameObject OLDRIMS1;
		public GameObject OLDRIMS2;
		public GameObject OLDRIMS3;
		public GameObject OLDRIMS4;
		public GameObject NEWRIMS1;
		public GameObject NEWRIMS2;
		public GameObject NEWRIMS3;
		public GameObject NEWRIMS4;
		public GameObject OLDCHASSIS;
		public GameObject OLDFENDERS;
		public GameObject OLDHOOD;
		public GameObject OLDTRUNK;
		public GameObject OLDDOOR1;
		public GameObject OLDDOOR2;
		public GameObject NEWCHASSIS;
		public GameObject NEWTRUNK;
		public GameObject NEWDOOR1;
		public GameObject NEWDOOR2;
		public GameObject SUBMASK;
		public GameObject LODACTIVATOR;

		public bool newrims;
		public bool GTCAR;
		public bool in_game;
		public float interactR = 1f;
		public float interactL = 1f;
		public float interactREAR = 1f;
		private float actualangle;
		private float SPEEDFACTOR = -1.5733f;
		private float anglespeedo;
		private float TIMER = 2f;
		private float RRELEVATION;
		private float RLELEVATION;
		private float FINALPOSITIONRL;
		private float FINALPOSITIONRR;
		public float DISTANCE;
		private GameObject CAR;
		private GameObject BUYING;
		private GameObject BUYING2;
		private GameObject SIGN;
		private GameObject PURHCASERIMS;
		public GameObject BUYTRIGGER;
		public GameObject BUYTRIGGER2;
		public GameObject BUYTRIGGER3;
		public bool bought;
		public Drivetrain drivetrain;
		private AxisCarController acc;
		private Collider doorTriggerRCollider;
		private Collider doorTriggerLCollider;
		private Collider trunkTriggerCollider;
		private Collider handbrakeTriggerCollider;
		private Collider buyTriggerCollider;
		private Collider buyTrigger2Collider;
		private Collider buyTrigger3Collider;

		private bool loggedPlayMakerCheck;

		private static readonly Vector3 SPAWN_POS = new Vector3(1546.388f, 5.24076f, 734.0383f);
		private static readonly Vector3 SPAWN_ROT = new Vector3(7.431609f, 164.599f, 0.6475128f);

		public override void ModSetup()
		{
			SetupFunction(Setup.OnMenuLoad, Mod_OnMenuLoad);
			SetupFunction(Setup.OnLoad, Mod_OnLoad);
			SetupFunction(Setup.OnSave, Mod_OnSave);
			SetupFunction(Setup.Update, Mod_Update);
		}

		private void Mod_OnMenuLoad()
        {
			AssetBundle assetBundle = LoadAssets.LoadBundle(this, "plymouth.unity3d");
			this.CAR = GameObject.Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("plymouth.prefab"));
			this.CAR.transform.position = SPAWN_POS;
			this.CAR.transform.rotation = Quaternion.Euler(SPAWN_ROT);
			assetBundle.Unload(false);
			GameObject.DontDestroyOnLoad(this.CAR);
			this.CAR.GetComponent<Rigidbody>().isKinematic = true;
			this.CAR.name = "DUSTMAN(1408kg)";
			GameObject.Find("DUSTMAN(1408kg)/collisionsworld/coll").layer = 17;
        }

        private void Mod_OnLoad()
        {
			AssetBundle storeBundle = LoadAssets.LoadBundle(this, "store.unity3d");
			this.BUYING = GameObject.Instantiate<GameObject>(storeBundle.LoadAsset<GameObject>("BUYTRIGGER.prefab"));
			storeBundle.Unload(false);
			AssetBundle signBundle = LoadAssets.LoadBundle(this, "sign.unity3d");
			this.SIGN = GameObject.Instantiate<GameObject>(signBundle.LoadAsset<GameObject>("sign.prefab"));
			signBundle.Unload(false);
			AssetBundle brochureBundle = LoadAssets.LoadBundle(this, "brochuregt.unity3d");
			this.BUYING2 = GameObject.Instantiate<GameObject>(brochureBundle.LoadAsset<GameObject>("brochureGT.prefab"));
			brochureBundle.Unload(false);
			AssetBundle rimsBundle = LoadAssets.LoadBundle(this, "purchaserims.unity3d");
			this.PURHCASERIMS = GameObject.Instantiate<GameObject>(rimsBundle.LoadAsset<GameObject>("PURCHASERIMS.prefab"));
			rimsBundle.Unload(false);
			this.BUYING.transform.position = new Vector3(1553.735f, 5.55f, 740.55f);
			this.BUYING2.transform.position = new Vector3(1553.783f, 5.668f, 740.649f);
			this.PURHCASERIMS.transform.position = new Vector3(1553.719f, 6.537184f, 740.8297f);
			this.PURHCASERIMS.transform.rotation = Quaternion.Euler(0f, 65.00002f, 0f);
			this.SIGN.transform.position = new Vector3(1554f, 6.5f, 741.6f);
			this.acc = this.CAR.GetComponent<AxisCarController>();
			this.drivetrain = this.CAR.GetComponent<Drivetrain>();
			this.SteeringEnabler(false);
			this.CAR.transform.FindChild("PlayerTrigger").gameObject.AddComponent<PlayerTrigger>().tm = this;
			try
			{
				GameObject.Find("DUSTMAN(1408kg)/collisionsworld/coll").layer = 17;
				GameObject.Find("DUSTMAN(1408kg)").layer = 18;
				GameObject.Find("DUSTMAN(1408kg)/collisionsplayer/seatcoll").layer = 9;
				GameObject.Find("DUSTMAN(1408kg)/collisionsplayer/seatcolltop").layer = 9;
				GameObject.Find("DUSTMAN(1408kg)/collisionsplayer/groundcollider").layer = 22;
				GameObject.Find("DUSTMAN(1408kg)/collisionsplayer/bonnetcollider").layer = 22;
				GameObject.Find("DUSTMAN(1408kg)/collisionsplayer/DASHBOARDCOLL").layer = 22;
				GameObject.Find("DUSTMAN(1408kg)/collisionsplayer/DASHBOARDCOLL 1").layer = 22;
				GameObject.Find("DUSTMAN(1408kg)/collisionsplayer/maskcollider").layer = 22;
				GameObject.Find("DUSTMAN(1408kg)/collisionsplayer/windshieldcollider").layer = 22;
				GameObject.Find("DUSTMAN(1408kg)/collisionsplayer/rearwindowcollider").layer = 22;
				GameObject.Find("DUSTMAN(1408kg)/collisionsplayer/underwindshield").layer = 22;
				GameObject.Find("DUSTMAN(1408kg)/collisionsplayer/bodysideR").layer = 22;
				GameObject.Find("DUSTMAN(1408kg)/collisionsplayer/bodysideL").layer = 22;
				GameObject.Find("DUSTMAN(1408kg)/collisionsplayer/REARSEAT_lower").layer = 9;
				GameObject.Find("DUSTMAN(1408kg)/collisionsplayer/REARSEAT_upper").layer = 9;
				GameObject.Find("DUSTMAN(1408kg)/collisionsplayer/Front_int_right").layer = 22;
				GameObject.Find("DUSTMAN(1408kg)/collisionsplayer/Front_int_left").layer = 22;
				GameObject.Find("DUSTMAN(1408kg)/collisionsplayer/roofside_l").layer = 9;
				GameObject.Find("DUSTMAN(1408kg)/collisionsplayer/roofside_r").layer = 9;
				GameObject.Find("DUSTMAN(1408kg)/collisionsplayer/roofcollider").layer = 22;
				GameObject.Find("DUSTMAN(1408kg)/collisionsplayer/trunkcolground").layer = 22;
				GameObject.Find("DUSTMAN(1408kg)/collisionsplayer/trunkcollr").layer = 22;
				GameObject.Find("DUSTMAN(1408kg)/collisionsplayer/trunkcolll").layer = 22;
				GameObject.Find("DUSTMAN(1408kg)/collisionsplayer/trunkcollback").layer = 22;
				GameObject.Find("DUSTMAN(1408kg)/collisionsplayer/trunkcollfront").layer = 22;
				GameObject.Find("DUSTMAN(1408kg)/collisionsplayer/trunkcollfront2").layer = 22;
			}
			catch (Exception e)
			{
				ModConsole.Error("[DUSTMAN] Layer setup failed: " + e.Message);
			}
			this.PLAYER = GameObject.Find("PLAYER");
			this.positionRR = GameObject.Find("DUSTMAN(1408kg)/Wheels/WheelRR/TireRR");
			this.positionRL = GameObject.Find("DUSTMAN(1408kg)/Wheels/WheelRL/TireRL");
			this.NEEDLE = GameObject.Find("DUSTMAN(1408kg)/speedometer");
			this.NEEDLESPEED = GameObject.Find("DUSTMAN(1408kg)/rpmmeter");
			this.PEDAL_gas = GameObject.Find("DUSTMAN(1408kg)/PEDALS/PEDAL_gas");
			this.PEDAL_brake = GameObject.Find("DUSTMAN(1408kg)/PEDALS/PEDAL_brake");
			this.PEDAL_clutch = GameObject.Find("DUSTMAN(1408kg)/PEDALS/PEDAL_clutch");
			this.KEY = GameObject.Find("DUSTMAN(1408kg)/DashBoard/KEYS");
			this.IMPACT_FRONT_R = GameObject.Find("DUSTMAN(1408kg)/SOUNDS/SUS_IMPACT_FRONT_R");
			this.IMPACT_FRONT_L = GameObject.Find("DUSTMAN(1408kg)/SOUNDS/SUS_IMPACT_FRONT_L");
			this.IMPACT_REAR_R = GameObject.Find("DUSTMAN(1408kg)/SOUNDS/SUS_IMPACT_REAR_R");
			this.IMPACT_REAR_L = GameObject.Find("DUSTMAN(1408kg)/SOUNDS/SUS_IMPACT_REAR_L");
			this.WHEELFR = GameObject.Find("DUSTMAN(1408kg)/Wheels/WheelFR");
			this.WHEELFL = GameObject.Find("DUSTMAN(1408kg)/Wheels/WheelFL");
			this.WHEELRR = GameObject.Find("DUSTMAN(1408kg)/Wheels/WheelRR");
			this.WHEELRL = GameObject.Find("DUSTMAN(1408kg)/Wheels/WheelRL");
			this.IGNITION = GameObject.Find("DUSTMAN(1408kg)/collisionsplayer/ignition");
			this.OLDRIMS1 = GameObject.Find("DUSTMAN(1408kg)/Wheels/WheelFL/TireFL/oldrims");
			this.OLDRIMS2 = GameObject.Find("DUSTMAN(1408kg)/Wheels/WheelFR/TireFR/oldrims");
			this.OLDRIMS3 = GameObject.Find("DUSTMAN(1408kg)/Wheels/WheelRL/TireRL/oldrims");
			this.OLDRIMS4 = GameObject.Find("DUSTMAN(1408kg)/Wheels/WheelRR/TireRR/oldrims");
			this.NEWRIMS1 = GameObject.Find("DUSTMAN(1408kg)/Wheels/WheelFL/TireFL/newrims");
			this.NEWRIMS2 = GameObject.Find("DUSTMAN(1408kg)/Wheels/WheelFR/TireFR/newrims");
			this.NEWRIMS3 = GameObject.Find("DUSTMAN(1408kg)/Wheels/WheelRL/TireRL/newrims");
			this.NEWRIMS4 = GameObject.Find("DUSTMAN(1408kg)/Wheels/WheelRR/TireRR/newrims");
			this.OLDCHASSIS = GameObject.Find("DUSTMAN(1408kg)/Body/DUSTMAN_BODY");
			this.OLDFENDERS = GameObject.Find("DUSTMAN(1408kg)/Body/fender");
			this.OLDHOOD = GameObject.Find("DUSTMAN(1408kg)/Body/Hood");
			this.OLDTRUNK = GameObject.Find("DUSTMAN(1408kg)/RearDoors/Bootlid/Mesh/bootlid");
			this.OLDDOOR1 = GameObject.Find("DUSTMAN(1408kg)/DriverDoors/DOOR_RIGHT/MESHES/door_right");
			this.OLDDOOR2 = GameObject.Find("DUSTMAN(1408kg)/DriverDoors/DOOR_LEFT/MESHES/door_left");
			this.NEWCHASSIS = GameObject.Find("DUSTMAN(1408kg)/GTCAR");
			this.NEWDOOR1 = GameObject.Find("DUSTMAN(1408kg)/DriverDoors/DOOR_RIGHT/MESHES/DOORR");
			this.NEWDOOR2 = GameObject.Find("DUSTMAN(1408kg)/DriverDoors/DOOR_LEFT/MESHES/DOORL");
			this.NEWTRUNK = GameObject.Find("DUSTMAN(1408kg)/RearDoors/Bootlid/Mesh/TRUNK");
			this.SUBMASK = GameObject.Find("DUSTMAN(1408kg)/Body/plymouth 1970 submask");
			this.SHIFTER = GameObject.Find("DUSTMAN(1408kg)/SHIFTER/Armature2/Bone/Bone.001");
			this.CAR.AddComponent<Deformable>().meshFilter = this.CAR.transform.GetChild(3).FindChild("DUSTMAN_BODY").GetComponent<MeshFilter>();
			this.CAR.AddComponent<Deformable>().meshFilter = this.CAR.transform.GetChild(3).FindChild("Hood").GetComponent<MeshFilter>();
			this.CAR.AddComponent<Deformable>().meshFilter = this.CAR.transform.GetChild(3).FindChild("fender").GetComponent<MeshFilter>();
			this.CAR.AddComponent<Deformable>().meshFilter = this.CAR.transform.GetChild(3).FindChild("Maska").GetComponent<MeshFilter>();
			this.CAR.AddComponent<Deformable>().meshFilter = this.CAR.transform.GetChild(3).FindChild("grille").GetComponent<MeshFilter>();
			this.CAR.AddComponent<Deformable>().meshFilter = this.CAR.transform.GetChild(3).FindChild("BUMPER_REAR").GetComponent<MeshFilter>();
			this.CAR.AddComponent<Deformable>().meshFilter = this.CAR.transform.GetChild(3).FindChild("BUMPER_FRONT").GetComponent<MeshFilter>();
			this.CAR.AddComponent<Deformable>().meshFilter = this.CAR.transform.GetChild(3).GetChild(47).GetComponent<MeshFilter>();
			this.CAR.AddComponent<Deformable>().meshFilter = this.CAR.transform.GetChild(3).GetChild(49).GetComponent<MeshFilter>();
			this.CAR.AddComponent<Deformable>().meshFilter = this.CAR.transform.FindChild("DriverDoors").FindChild("DOOR_RIGHT").GetChild(2).FindChild("door_right").GetComponent<MeshFilter>();
			this.CAR.AddComponent<Deformable>().meshFilter = this.CAR.transform.FindChild("DriverDoors").FindChild("DOOR_LEFT").GetChild(2).FindChild("door_left").GetComponent<MeshFilter>();
			GameObject.Find("DUSTMAN(1408kg)").GetComponents<Deformable>()[0].MaxVertexMov = 0.06f;
			GameObject.Find("DUSTMAN(1408kg)").GetComponents<Deformable>()[0].Hardness = 1f;
			GameObject.Find("DUSTMAN(1408kg)").GetComponents<Deformable>()[1].MaxVertexMov = 0.1f;
			GameObject.Find("DUSTMAN(1408kg)").GetComponents<Deformable>()[1].Hardness = 1.5f;
			GameObject.Find("DUSTMAN(1408kg)").GetComponents<Deformable>()[2].MaxVertexMov = 0.12f;
			GameObject.Find("DUSTMAN(1408kg)").GetComponents<Deformable>()[2].Hardness = 1.5f;
			GameObject.Find("DUSTMAN(1408kg)").GetComponents<Deformable>()[3].MaxVertexMov = 0.03f;
			GameObject.Find("DUSTMAN(1408kg)").GetComponents<Deformable>()[5].MaxVertexMov = 0.04f;
			GameObject.Find("DUSTMAN(1408kg)").GetComponents<Deformable>()[5].Hardness = 0.5f;
			GameObject.Find("DUSTMAN(1408kg)").GetComponents<Deformable>()[6].MaxVertexMov = 0.03f;
			GameObject.Find("DUSTMAN(1408kg)").GetComponents<Deformable>()[6].Hardness = 0.89f;
			GameObject.Find("DUSTMAN(1408kg)").GetComponents<Deformable>()[9].MaxVertexMov = 0.02f;
			GameObject.Find("DUSTMAN(1408kg)").GetComponents<Deformable>()[9].Hardness = 0.89f;
			GameObject.Find("DUSTMAN(1408kg)").GetComponents<Deformable>()[10].MaxVertexMov = 0.02f;
			GameObject.Find("DUSTMAN(1408kg)").GetComponents<Deformable>()[10].Hardness = 0.89f;
			this.CAR.transform.FindChild("collisionsplayer").gameObject.AddComponent<CarDashBoard>().drivetrain = this.drivetrain;
			try
			{
				this.CAR.GetComponent<CarDynamics>().physicMaterials = GameObject.Find("BACHGLOTZ(1905kg)").GetComponent<CarDynamics>().physicMaterials;
			}
			catch (Exception e)
			{
				ModConsole.Error("[DUSTMAN] Failed to copy physicMaterials from BACHGLOTZ: " + e.Message);
			}
			try
			{
				GameObject radioPivot = GameObject.Instantiate<GameObject>(GameObject.Find("BACHGLOTZ(1905kg)/RadioPivot"));
				radioPivot.transform.SetParent(GameObject.Find("DUSTMAN(1408kg)").transform);
				radioPivot.transform.localPosition = new Vector3(0f, 0.26f, 0.78f);
				radioPivot.transform.localRotation = Quaternion.Euler(310f, 5.008889E-06f, 180f);
			}
			catch (Exception e)
			{
				ModConsole.Error("[DUSTMAN] Failed to clone RadioPivot from BACHGLOTZ: " + e.Message);
			}
			try
			{
				GameObject repairShop = GameObject.Find("REPAIRSHOP");
				repairShop.transform.Find("LOD").gameObject.SetActive(true);
				GameObject.Find("REPAIRSHOP/LOD/Garbage/Flatbed").SetActive(false);
				GameObject.Find("REPAIRSHOP/LOD/Garbage/bus_stop").SetActive(false);
			}
			catch (Exception e)
			{
				ModConsole.Error("[DUSTMAN] REPAIRSHOP setup failed: " + e.Message);
			}
			this.LODACTIVATOR = GameObject.Find("MAP/StreetLights/4/LOD");
			this.interiorlight = GameObject.Find("DUSTMAN(1408kg)/Interiorlight/interiorlight");
			this.interiorlight.SetActive(false);
			this.SPECLIGHTS = GameObject.Find("DUSTMAN(1408kg)/SPECLIGHTS");
			this.SPECLIGHTS.SetActive(false);
			this.BRAKELIGHTS = GameObject.Find("DUSTMAN(1408kg)/BRAKELIGHTS");
			this.HANDBRAKE = GameObject.Find("DUSTMAN(1408kg)/HANDBRAKE");
			this.HANDBRAKETRIGGER = GameObject.Find("DUSTMAN(1408kg)/HANDBRAKE/TRIGGER");
			this.handbrakeTriggerCollider = this.HANDBRAKETRIGGER.GetComponent<Collider>();
			this.HANDBRAKE.transform.localPosition = new Vector3(-0.5578f, 0.1605f, 0.8454f);
			this.Exhaust = GameObject.Find("DUSTMAN(1408kg)/Exhaust");
			this.DOORCLOSER = GameObject.Find("DUSTMAN(1408kg)/DriverDoors/DOOR_RIGHT/SOUND");
			this.DOORCLOSEL = GameObject.Find("DUSTMAN(1408kg)/DriverDoors/DOOR_LEFT/SOUND");
			this.DOOROPENR = GameObject.Find("DUSTMAN(1408kg)/DriverDoors/DOOR_RIGHT/SOUNDOPEN");
			this.DOOROPENL = GameObject.Find("DUSTMAN(1408kg)/DriverDoors/DOOR_LEFT/SOUNDOPEN");
			this.TRUNKCLOSE = GameObject.Find("DUSTMAN(1408kg)/RearDoors/Bootlid/SOUND");
			this.TRUNKOPEN = GameObject.Find("DUSTMAN(1408kg)/RearDoors/Bootlid/SOUNDOPEN");
			this.DOORR = GameObject.Find("DUSTMAN(1408kg)/DriverDoors/DOOR_RIGHT");
			this.DOORTRIGGERR = GameObject.Find("DUSTMAN(1408kg)/DriverDoors/DOOR_RIGHT/door_trigger");
			this.doorTriggerRCollider = this.DOORTRIGGERR.GetComponent<Collider>();
			this.DOORL = GameObject.Find("DUSTMAN(1408kg)/DriverDoors/DOOR_LEFT");
			this.DOORTRIGGERL = GameObject.Find("DUSTMAN(1408kg)/DriverDoors/DOOR_LEFT/door_trigger");
			this.doorTriggerLCollider = this.DOORTRIGGERL.GetComponent<Collider>();
			this.TRUNK = GameObject.Find("DUSTMAN(1408kg)/RearDoors/Bootlid");
			this.TRUNKTRIGGER = GameObject.Find("DUSTMAN(1408kg)/RearDoors/Bootlid/triger");
			this.trunkTriggerCollider = this.TRUNKTRIGGER.GetComponent<Collider>();
			this.BUYTRIGGER = GameObject.Find("BUYTRIGGER(Clone)/COLLISION");
			this.buyTriggerCollider = this.BUYTRIGGER.GetComponent<Collider>();
			this.BUYTRIGGER2 = this.BUYING2.transform.GetChild(1).gameObject;
			this.buyTrigger2Collider = this.BUYTRIGGER2.GetComponent<Collider>();
			this.BUYTRIGGER3 = this.PURHCASERIMS.transform.GetChild(1).gameObject;
			this.buyTrigger3Collider = this.BUYTRIGGER3.GetComponent<Collider>();
			Dustmansave dustmansave = SaveLoad.DeserializeSaveFile<Dustmansave>(this, "trophy.save");
			if (dustmansave != null)
			{
				this.CAR.transform.position = dustmansave.pos;
				this.CAR.transform.rotation = Quaternion.Euler(dustmansave.rotX, dustmansave.rotY, dustmansave.rotZ);
				this.bought = dustmansave.bought;
				this.newrims = dustmansave.newrims;
				this.GTCAR = dustmansave.GTCAR;
			}
			this.CAR.GetComponent<Rigidbody>().isKinematic = false;
			this.in_game = true;
        }

		public void SteeringEnabler(bool enable)
		{
			if (enable)
			{
				this.acc.throttleAxis = "Throttle";
				this.acc.brakeAxis = "Brake";
				this.acc.steerAxis = "Horizontal";
				this.acc.handbrakeAxis = "Handbrake";
				this.acc.clutchAxis = "Clutch";
				this.acc.shiftUpButton = "ShiftUp";
				this.acc.shiftDownButton = "ShiftDown";
			}
			else
			{
				this.acc.throttleAxis = null;
				this.acc.brakeAxis = null;
				this.acc.steerAxis = null;
				this.acc.handbrakeAxis = null;
				this.acc.clutchAxis = null;
				this.acc.shiftUpButton = null;
				this.acc.shiftDownButton = null;
			}
		}

		private void Mod_OnSave()
		{
			SaveLoad.SerializeSaveFile<Dustmansave>(this, new Dustmansave
			{
				pos = this.CAR.transform.position,
				rotX = this.CAR.transform.rotation.eulerAngles.x,
				rotY = this.CAR.transform.rotation.eulerAngles.y,
				rotZ = this.CAR.transform.rotation.eulerAngles.z,
				bought = this.bought,
				newrims = this.newrims,
				GTCAR = this.GTCAR
			}, "trophy.save");
		}

        private void Mod_Update()
        {
			if (!this.in_game)
				return;

			if (!this.loggedPlayMakerCheck)
			{
				this.loggedPlayMakerCheck = true;
				try
				{
					var guiUse = PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse");
					var guiDrive = PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIdrive");
					var guiInteraction = PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction");
					var playerSeated = PlayMakerGlobals.Instance.Variables.FindFsmBool("PlayerSeated");
					if (guiUse == null) ModConsole.Error("[DUSTMAN] PlayMaker global 'GUIuse' not found");
					if (guiDrive == null) ModConsole.Error("[DUSTMAN] PlayMaker global 'GUIdrive' not found");
					if (guiInteraction == null) ModConsole.Error("[DUSTMAN] PlayMaker global 'GUIinteraction' not found");
					if (playerSeated == null) ModConsole.Error("[DUSTMAN] PlayMaker global 'PlayerSeated' not found");
					if (guiUse != null && guiDrive != null && guiInteraction != null && playerSeated != null)
						ModConsole.Log("[DUSTMAN] PlayMaker globals OK");
				}
				catch (Exception e)
				{
					ModConsole.Error("[DUSTMAN] PlayMaker globals check failed: " + e.Message);
				}
			}

			if (this.bought)
			{
				this.IGNITION.SetActive(true);
				this.WHEELFL.SetActive(true);
				this.WHEELFR.SetActive(true);
				this.WHEELRL.SetActive(true);
				this.WHEELRR.SetActive(true);
			}
			else
			{
				this.IGNITION.SetActive(false);
				this.WHEELFL.SetActive(false);
				this.WHEELFR.SetActive(false);
				this.WHEELRL.SetActive(false);
				this.WHEELRR.SetActive(false);
			}
			if (this.newrims)
			{
				this.NEWRIMS1.SetActive(true);
				this.NEWRIMS2.SetActive(true);
				this.NEWRIMS3.SetActive(true);
				this.NEWRIMS4.SetActive(true);
				this.OLDRIMS1.SetActive(false);
				this.OLDRIMS2.SetActive(false);
				this.OLDRIMS3.SetActive(false);
				this.OLDRIMS4.SetActive(false);
			}
			else
			{
				this.NEWRIMS1.SetActive(false);
				this.NEWRIMS2.SetActive(false);
				this.NEWRIMS3.SetActive(false);
				this.NEWRIMS4.SetActive(false);
				this.OLDRIMS1.SetActive(true);
				this.OLDRIMS2.SetActive(true);
				this.OLDRIMS3.SetActive(true);
				this.OLDRIMS4.SetActive(true);
			}
			if (this.GTCAR)
			{
				this.OLDCHASSIS.SetActive(false);
				this.OLDDOOR1.SetActive(false);
				this.OLDDOOR2.SetActive(false);
				this.OLDFENDERS.SetActive(false);
				this.OLDHOOD.SetActive(false);
				this.OLDTRUNK.SetActive(false);
				this.NEWCHASSIS.SetActive(true);
				this.NEWDOOR1.SetActive(true);
				this.NEWDOOR2.SetActive(true);
				this.NEWTRUNK.SetActive(true);
				this.SUBMASK.SetActive(false);
				this.drivetrain.maxPower = 288f;
				this.drivetrain.maxTorque = 340f;
				this.drivetrain.maxTorqueRPM = 2300f;
				this.drivetrain.maxRPM = 4500f;
				this.drivetrain.gearRatios = new float[]
				{
					-3.21f,
					0f,
					2.8f,
					1.45f,
					0.95f,
					0.6f
				};
				this.drivetrain.finalDriveRatio = 3f;
				this.drivetrain.revLimiterTime = 0.165f;
				this.drivetrain.engineFrictionFactor = 0.41f;
				this.drivetrain.maxNetTorqueRPM = 2100f;
			}
			else
			{
				this.OLDCHASSIS.SetActive(true);
				this.OLDDOOR1.SetActive(true);
				this.OLDDOOR2.SetActive(true);
				this.OLDFENDERS.SetActive(true);
				this.OLDHOOD.SetActive(true);
				this.OLDTRUNK.SetActive(true);
				this.NEWCHASSIS.SetActive(false);
				this.NEWDOOR1.SetActive(false);
				this.NEWDOOR2.SetActive(false);
				this.NEWTRUNK.SetActive(false);
				this.SUBMASK.SetActive(true);
				this.drivetrain.maxPower = 145f;
				this.drivetrain.maxTorque = 215f;
				this.drivetrain.maxTorqueRPM = 2000f;
				this.drivetrain.maxRPM = 6000f;
				this.drivetrain.gearRatios = new float[]
				{
					-3.21f,
					0f,
					2.7f,
					1.7f,
					1f
				};
				this.drivetrain.finalDriveRatio = 3.4f;
				this.drivetrain.revLimiterTime = 0.13f;
				this.drivetrain.engineFrictionFactor = 0.28f;
				this.drivetrain.maxNetTorqueRPM = 1800f;
			}
			RaycastHit[] hits = UnifiedRaycast.GetRaycastHits();
			for (int j = 0; j < hits.Length; j++)
			{
				RaycastHit raycastHit2 = hits[j];
				if (raycastHit2.collider == this.buyTriggerCollider)
				{
					PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "BUY THE CAR (56,900 MK)";
					if (Input.GetMouseButtonDown(0))
					{
						if (!this.bought)
						{
							if (FsmVariables.GlobalVariables.FindFsmFloat("PlayerMoney").Value < 56900f)
							{
								PlayMakerGlobals.Instance.Variables.FindFsmString("GUIsubtitle").Value = "Im not wasting my time with poor, go smell somewhere else...";
							}
							else
							{
								this.bought = true;
								this.BUYING.transform.GetChild(2).gameObject.SetActive(false);
								this.BUYING.transform.GetChild(2).gameObject.SetActive(true);
								FsmVariables.GlobalVariables.FindFsmFloat("PlayerMoney").Value -= 56900f;
								PlayMakerGlobals.Instance.Variables.FindFsmString("GUIsubtitle").Value = "Enjoy the car! No money back guaranteed!!";
							}
						}
					}
				}
				if (raycastHit2.collider == this.buyTrigger2Collider)
				{
					PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "GT UPGRADE (38,600 MK)";
					if (Input.GetMouseButtonDown(0))
					{
						if (!this.GTCAR && this.bought)
						{
							if (FsmVariables.GlobalVariables.FindFsmFloat("PlayerMoney").Value < 38600f)
							{
								PlayMakerGlobals.Instance.Variables.FindFsmString("GUIsubtitle").Value = "This is a business place, stop wasting my time!!";
							}
							else
							{
								this.GTCAR = true;
								this.BUYING.transform.GetChild(2).gameObject.SetActive(false);
								this.BUYING.transform.GetChild(2).gameObject.SetActive(true);
								FsmVariables.GlobalVariables.FindFsmFloat("PlayerMoney").Value -= 38600f;
								PlayMakerGlobals.Instance.Variables.FindFsmString("GUIsubtitle").Value = "Good choice! Have good fun !";
							}
						}
					}
				}
				if (raycastHit2.collider == this.buyTrigger3Collider)
				{
					PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "GT RIMS (7600 MK)";
					if (Input.GetMouseButtonDown(0))
					{
						if (!this.newrims && this.bought)
						{
							if (FsmVariables.GlobalVariables.FindFsmFloat("PlayerMoney").Value < 7600f)
							{
								PlayMakerGlobals.Instance.Variables.FindFsmString("GUIsubtitle").Value = "Seriously?! How you got this car that you dont have money for rims?";
							}
							else
							{
								this.newrims = true;
								this.BUYING.transform.GetChild(2).gameObject.SetActive(false);
								this.BUYING.transform.GetChild(2).gameObject.SetActive(true);
								FsmVariables.GlobalVariables.FindFsmFloat("PlayerMoney").Value -= 7600f;
								PlayMakerGlobals.Instance.Variables.FindFsmString("GUIsubtitle").Value = "This should do it!";
							}
						}
					}
				}
				if (raycastHit2.collider == this.doorTriggerRCollider)
				{
					PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = true;
					if (Input.GetMouseButtonDown(0))
					{
						this.interactR = this.interactR + 1f;
					}
				}
				if (raycastHit2.collider == this.doorTriggerLCollider)
				{
					PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = true;
					if (Input.GetMouseButtonDown(0))
					{
						this.interactL = this.interactL + 1f;
					}
				}
				if (raycastHit2.collider == this.trunkTriggerCollider)
				{
					PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = true;
					if (Input.GetMouseButtonDown(0))
					{
						this.interactREAR += 1f;
					}
				}
				if (raycastHit2.collider == this.handbrakeTriggerCollider)
				{
					PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = true;
					PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "HANDBRAKE";
					if (Input.GetMouseButtonDown(0))
					{
						this.HANDBRAKE.transform.localPosition = new Vector3(-0.5578f, 0.1663f, 0.795f);
						this.acc.normalizeBrakesInput = true;
					}
					if (Input.GetMouseButtonDown(1))
					{
						this.HANDBRAKE.transform.localPosition = new Vector3(-0.5578f, 0.1605f, 0.8454f);
						this.acc.normalizeBrakesInput = false;
					}
				}
			}
			if (this.interactR > 1f)
			{
				if (Input.GetMouseButtonUp(0))
				{
					this.interactR = 0f;
				}
			}
			if (this.interactR == 2f)
			{
				HingeJoint component = this.DOORR.GetComponent<HingeJoint>();
				JointSpring spring = component.spring;
				spring.targetPosition = 60f;
				spring.spring = 600f;
				component.spring = spring;
				this.interiorlight.SetActive(true);
				this.DOOROPENR.SetActive(true);
				this.DOORCLOSER.SetActive(false);
			}
			if (this.interactR == 1f)
			{
				HingeJoint component2 = this.DOORR.GetComponent<HingeJoint>();
				JointSpring spring2 = component2.spring;
				spring2.targetPosition = -5f;
				spring2.spring = 99999f;
				component2.spring = spring2;
				this.interiorlight.SetActive(false);
				this.DOORCLOSER.SetActive(true);
				this.DOOROPENR.SetActive(false);
			}
			if (this.interactR == 0f)
			{
				HingeJoint component3 = this.DOORR.GetComponent<HingeJoint>();
				JointSpring spring3 = component3.spring;
				spring3.targetPosition = 0f;
				spring3.spring = 0f;
				component3.spring = spring3;
			}
			if (this.interactL > 1f)
			{
				if (Input.GetMouseButtonUp(0))
				{
					this.interactL = 0f;
				}
			}
			if (this.interactL == 2f)
			{
				HingeJoint component4 = this.DOORL.GetComponent<HingeJoint>();
				JointSpring spring4 = component4.spring;
				spring4.targetPosition = 60f;
				spring4.spring = 600f;
				component4.spring = spring4;
				this.interiorlight.SetActive(true);
				this.DOORCLOSEL.SetActive(false);
				this.DOOROPENL.SetActive(true);
			}
			if (this.interactL == 1f)
			{
				HingeJoint component5 = this.DOORL.GetComponent<HingeJoint>();
				JointSpring spring5 = component5.spring;
				spring5.targetPosition = -5f;
				spring5.spring = 99999f;
				component5.spring = spring5;
				this.interiorlight.SetActive(false);
				this.DOORCLOSEL.SetActive(true);
				this.DOOROPENL.SetActive(false);
			}
			if (this.interactL == 0f)
			{
				HingeJoint component6 = this.DOORL.GetComponent<HingeJoint>();
				JointSpring spring6 = component6.spring;
				spring6.targetPosition = 0f;
				spring6.spring = 0f;
				component6.spring = spring6;
			}
			if (this.interactREAR == 1f)
			{
				HingeJoint component7 = this.TRUNK.GetComponent<HingeJoint>();
				JointSpring spring7 = component7.spring;
				spring7.targetPosition = 50f;
				spring7.spring = 999999f;
				component7.spring = spring7;
				this.TRUNKCLOSE.SetActive(true);
				this.TRUNKOPEN.SetActive(false);
			}
			if (this.interactREAR == 2f)
			{
				HingeJoint component8 = this.TRUNK.GetComponent<HingeJoint>();
				JointSpring spring8 = component8.spring;
				spring8.targetPosition = -100f;
				spring8.spring = 200f;
				component8.spring = spring8;
				this.TRUNKCLOSE.SetActive(false);
				this.TRUNKOPEN.SetActive(true);
			}
			if (this.interactREAR > 2f)
			{
				this.interactREAR = 1f;
			}
			if (this.drivetrain.gear == 0)
			{
				this.SPECLIGHTS.SetActive(true);
			}
			else
			{
				this.SPECLIGHTS.SetActive(false);
			}
			if (this.acc.brakeKey)
			{
				this.BRAKELIGHTS.SetActive(true);
			}
			if (!this.acc.brakeKey)
			{
				this.BRAKELIGHTS.SetActive(false);
			}
			if (this.drivetrain.rpm >= 450f)
			{
				this.Exhaust.SetActive(true);
			}
			else
			{
				this.Exhaust.SetActive(false);
			}
			if (Input.GetKey(KeyCode.Return))
			{
				this.acc.steerInput = 0f;
				this.acc.throttleInput = 0f;
				this.acc.handbrakeInput = 0f;
				this.acc.clutchInput = 0f;
			}
			this.actualangle = this.drivetrain.rpm * -0.032f + 12f;
			this.NEEDLE.transform.localRotation = Quaternion.Euler(10f, 0f, this.actualangle);
			this.anglespeedo = this.drivetrain.differentialSpeed * this.SPEEDFACTOR * 0.6213712f;
			this.NEEDLESPEED.transform.localRotation = Quaternion.Euler(10f, 0f, this.anglespeedo);
			if (this.WHEELFR.GetComponent<Wheel>().normalForce > 10900f)
			{
				this.IMPACT_FRONT_R.SetActive(true);
			}
			else
			{
				this.TIMER -= Time.deltaTime;
				if (this.TIMER < 0f)
				{
					this.IMPACT_FRONT_R.SetActive(false);
					this.TIMER = 2f;
				}
			}
			if (this.WHEELFL.GetComponent<Wheel>().normalForce > 10900f)
			{
				this.IMPACT_FRONT_L.SetActive(true);
			}
			else
			{
				this.TIMER -= Time.deltaTime;
				if (this.TIMER < 0f)
				{
					this.IMPACT_FRONT_L.SetActive(false);
					this.TIMER = 2f;
				}
			}
			if (this.WHEELRR.GetComponent<Wheel>().normalForce > 10900f)
			{
				this.IMPACT_REAR_R.SetActive(true);
			}
			else
			{
				this.TIMER -= Time.deltaTime;
				if (this.TIMER < 0f)
				{
					this.IMPACT_REAR_R.SetActive(false);
					this.TIMER = 2f;
				}
			}
			if (this.WHEELRL.GetComponent<Wheel>().normalForce > 10900f)
			{
				this.IMPACT_REAR_L.SetActive(true);
			}
			else
			{
				this.TIMER -= Time.deltaTime;
				if (this.TIMER < 0f)
				{
					this.IMPACT_REAR_L.SetActive(false);
					this.TIMER = 2f;
				}
			}
			if (this.acc.brakeInput == 1f)
			{
				this.PEDAL_brake.transform.localPosition = new Vector3(0f, -0.334f, 0.085f);
				this.PEDAL_brake.transform.localRotation = Quaternion.Euler(-20f, 0f, 0f);
			}
			else
			{
				this.PEDAL_brake.transform.localPosition = new Vector3(0f, 0f, 0f);
				this.PEDAL_brake.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
			}
			if (this.acc.clutchInput > 0.5f)
			{
				this.PEDAL_clutch.transform.localPosition = new Vector3(0f, -0.2441f, 0.0653f);
				this.PEDAL_clutch.transform.localRotation = Quaternion.Euler(-14.72681f, 0f, 0f);
			}
			else
			{
				this.PEDAL_clutch.transform.localPosition = new Vector3(0f, 0f, 0f);
				this.PEDAL_clutch.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
			}
			if (this.acc.throttleInput > 0.2f)
			{
				this.PEDAL_gas.transform.localPosition = new Vector3(0f, -0.2597f, 0.0561f);
				this.PEDAL_gas.transform.localRotation = Quaternion.Euler(-14.32629f, 0f, 0f);
			}
			else
			{
				this.PEDAL_gas.transform.localPosition = new Vector3(0f, 0f, 0f);
				this.PEDAL_gas.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
			}
			if (!this.drivetrain.canStall)
			{
				this.KEY.SetActive(true);
			}
			else
			{
				this.KEY.SetActive(false);
			}
			if (this.drivetrain.gear == 1)
			{
				this.SHIFTER.transform.localRotation = Quaternion.Euler(-8.439514f, 1.525879E-05f, 0f);
				this.SHIFTER.transform.localPosition = new Vector3(0f, 0.002418704f, 0f);
			}
			if (this.drivetrain.gear == 2)
			{
				this.SHIFTER.transform.localRotation = Quaternion.Euler(-20f, 0.160614f, 11f);
			}
			if (this.drivetrain.gear == 3)
			{
				this.SHIFTER.transform.localRotation = Quaternion.Euler(-3f, 0.160614f, 11f);
			}
			if (this.drivetrain.gear == 4)
			{
				this.SHIFTER.transform.localRotation = Quaternion.Euler(-20f, 0.160614f, 0f);
			}
			if (this.drivetrain.gear == 5)
			{
				this.SHIFTER.transform.localRotation = Quaternion.Euler(-3f, 0.160614f, 0f);
			}
			if (this.drivetrain.gear == 0)
			{
				this.SHIFTER.transform.localRotation = Quaternion.Euler(-20f, 0.160614f, -11f);
			}
			this.RRELEVATION = this.positionRR.transform.localPosition.y;
			this.RLELEVATION = this.positionRL.transform.localPosition.y;
			this.FINALPOSITIONRL = this.RLELEVATION * -38.25001f - this.RRELEVATION * -38.25001f;
			this.FINALPOSITIONRR = this.RRELEVATION * -38.25001f - this.RLELEVATION * -38.25001f;
			this.WHEELRL.transform.localRotation = Quaternion.Euler(0f, 0f, this.FINALPOSITIONRL);
			this.WHEELRR.transform.localRotation = Quaternion.Euler(0f, 0f, -this.FINALPOSITIONRR);
			this.LODACTIVATOR.SetActive(true);
			this.DISTANCE = Vector3.Distance(this.CAR.transform.position, this.PLAYER.transform.position);
			if (this.DISTANCE < 0.8f)
			{
				PlayMakerGlobals.Instance.Variables.FindFsmBool("PlayerSeated").Value = true;
			}
			if (this.DISTANCE > 0.8f)
			{
				if (this.DISTANCE < 1.2f)
				{
					PlayMakerGlobals.Instance.Variables.FindFsmBool("PlayerSeated").Value = false;
				}
			}
        }
    }
}

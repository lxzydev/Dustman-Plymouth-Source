using HutongGames.PlayMaker;
using MSCLoader;
using UnityEngine;

namespace DUSTMAN
{
    public class PlayerTrigger : MonoBehaviour
	{
		public DUSTMAN tm;
		private bool inCar;
		private bool wasSeated;
		private GameObject gears_obj;
		private TextMesh gears;
		private void Start()
		{
			this.gears_obj = GameObject.Find("GUI/Indicators").transform.FindChild("Gear").gameObject;
			this.gears = this.gears_obj.GetComponent<TextMesh>();
		}

		private void Update()
		{
			bool isSeated = false;
			GameObject cam = GameObject.Find("FPSCamera");
			if (cam != null)
			{
				isSeated = cam.transform.root.name == "DUSTMAN(1408kg)";
			}

			if (isSeated && !this.wasSeated)
			{
				this.inCar = true;
				this.tm.SteeringEnabler(true);
				this.gears_obj.SetActive(true);
				this.gears_obj.GetComponent<PlayMakerFSM>().enabled = false;
				Rigidbody cloneRB = this.tm.bachClone.GetComponent<Rigidbody>();
				if (cloneRB != null)
					GameObject.Destroy(cloneRB);
			}

			if (!isSeated && this.wasSeated)
			{
				this.inCar = false;
				this.tm.SteeringEnabler(false);
				this.gears_obj.SetActive(false);
				this.gears_obj.GetComponent<PlayMakerFSM>().enabled = true;
				if (this.tm.bachClone.GetComponent<Rigidbody>() == null)
				{
					Rigidbody rb = this.tm.bachClone.AddComponent<Rigidbody>();
					rb.isKinematic = true;
				}
			}

			this.wasSeated = isSeated;
		}

		private void GearsText(string text)
		{
			this.gears.text = text;
			this.gears_obj.transform.GetChild(0).GetComponent<TextMesh>().text = text;
		}

		private void LateUpdate()
		{
			if (this.inCar)
				this.tm.cameraPivot.localPosition = this.tm.cameraPivotInitPos;
		}

		private void FixedUpdate()
		{
			if (this.inCar)
			{
				if (this.tm.drivetrain.gear < this.tm.drivetrain.neutral)
				{
					this.GearsText("R");
				}
				else if (this.tm.drivetrain.gear == this.tm.drivetrain.neutral)
				{
					this.GearsText("N");
				}
				else
				{
					this.GearsText((this.tm.drivetrain.gear - this.tm.drivetrain.neutral).ToString());
				}
			}
		}
	}
}

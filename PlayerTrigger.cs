using System;
using System.Collections;
using HutongGames.PlayMaker;
using MSCLoader;
using UnityEngine;

namespace DUSTMAN
{
    public class PlayerTrigger : MonoBehaviour
	{
		private bool entered;
		private bool inCar;
		private bool wait;
		private GameObject player;
		public DUSTMAN tm;
		private GameObject gears_obj;
		private TextMesh gears;
		private GameObject DUSTMANCAR;

		private void Start()
		{
			try
			{
				this.gears_obj = GameObject.Find("GUI/Indicators").transform.FindChild("Gear").gameObject;
				this.gears = this.gears_obj.GetComponent<TextMesh>();
			}
			catch (Exception e)
			{
				ModConsole.Error("[DUSTMAN] PlayerTrigger - GUI/Indicators/Gear not found: " + e.Message);
			}
			this.DUSTMANCAR = GameObject.Find("DUSTMAN(1408kg)");
		}

		private void Update()
		{
			if (this.entered && this.player != null && !this.wait)
			{
				PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIdrive").Value = true;
				PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "Enter Driving Mode";
				if (Input.GetKeyDown(KeyCode.Return))
				{
					this.wait = true;
					base.StartCoroutine(this.WaitForASec());
					PlayMakerGlobals.Instance.Variables.FindFsmBool("PlayerSeated").Value = true;
					PlayMakerGlobals.Instance.Variables.FindFsmBool("PlayerCarControl").Value = true;
					this.entered = false;
					this.player.GetComponent<CharacterController>().enabled = false;
					PlayMakerGlobals.Instance.Variables.FindFsmBool("PlayerStop").Value = true;
					this.player.transform.SetParent(base.transform);
					this.tm.SteeringEnabler(true);
					this.inCar = true;
					PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIdrive").Value = false;
					PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = string.Empty;
					this.gears_obj.SetActive(true);
					this.gears_obj.GetComponent<PlayMakerFSM>().enabled = false;
				}
			}
			if (this.inCar && !this.wait)
			{
				if (this.DUSTMANCAR.GetComponent<CarDynamics>().velo < 0.3f)
				{
					if (Input.GetKeyDown(KeyCode.Return))
					{
						this.wait = true;
						base.StartCoroutine(this.WaitForASec());
						PlayMakerGlobals.Instance.Variables.FindFsmBool("PlayerSeated").Value = false;
						PlayMakerGlobals.Instance.Variables.FindFsmBool("PlayerCarControl").Value = false;
						this.player.transform.SetParent(null);
						this.player.GetComponent<CharacterController>().enabled = true;
						PlayMakerGlobals.Instance.Variables.FindFsmBool("PlayerStop").Value = false;
						this.tm.SteeringEnabler(false);
						this.inCar = false;
						this.gears_obj.SetActive(false);
						this.gears_obj.GetComponent<PlayMakerFSM>().enabled = true;
						this.player.transform.eulerAngles = new Vector3(0f, this.player.transform.eulerAngles.y, 0f);
					}
				}
			}
		}

		private void GearsText(string text)
		{
			this.gears.text = text;
			this.gears_obj.transform.GetChild(0).GetComponent<TextMesh>().text = text;
		}

		private IEnumerator WaitForASec()
		{
			yield return new WaitForSeconds(2f);
			this.wait = false;
			yield break;
		}

		private void FixedUpdate()
		{
			if (this.inCar)
			{
				if (this.tm.drivetrain.gear < this.tm.drivetrain.neutral)
				{
					this.GearsText("R");
				}
				else
				{
					if (this.tm.drivetrain.gear == this.tm.drivetrain.neutral)
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

		private void OnTriggerEnter(Collider other)
		{
			if (other.name == "PLAYER" && !this.inCar)
			{
				this.player = other.gameObject;
				this.entered = true;
				PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIdrive").Value = true;
				PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "Enter Driving Mode";
				PlayMakerGlobals.Instance.Variables.FindFsmBool("PlayerSeated").Value = true;
			}
		}

		private void OnTriggerExit()
		{
			if (!this.inCar)
			{
				this.entered = false;
				PlayMakerGlobals.Instance.Variables.FindFsmBool("PlayerSeated").Value = false;
				this.player = null;
				PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIdrive").Value = false;
				PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = string.Empty;
			}
		}
	}
}

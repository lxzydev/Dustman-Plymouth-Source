using System;
using System.Collections;
using HutongGames.PlayMaker;
using MSCLoader;
using UnityEngine;

namespace DUSTMAN
{
    public class CarDashBoard : MonoBehaviour
    {
		public Drivetrain drivetrain;
		private GameObject lights;
		private bool tryToStopEngine = false;
		private bool engineStopped = false;
		private AudioSource starter;
		private float timer = 0f;
		private GameObject SHUTOFF;
		public GameObject INCAR;
		public GameObject CHILD;
		private Collider lightsCollider;
		private Collider ignitionCollider;

		private void Start()
		{
			base.StartCoroutine(this.TryStopEngine(true));
			this.lights = base.transform.parent.FindChild("ZLIGHTS").gameObject;
			this.lights.SetActive(false);
			this.starter = base.transform.GetChild(16).GetComponent<AudioSource>();
			this.starter.volume = 0.5f;
			this.SHUTOFF = GameObject.Find("DUSTMAN(1408kg)/SOUNDS/Engine_ShutOff");
			this.INCAR = GameObject.Find("DUSTMAN(1408kg)/PlayerTrigger");
			this.CHILD = GameObject.Find("PLAYER");
			this.lightsCollider = base.transform.GetChild(14).GetComponent<Collider>();
			this.ignitionCollider = base.transform.GetChild(15).GetComponent<Collider>();
		}

		private IEnumerator TryStopEngine(bool wait)
		{
			this.tryToStopEngine = true;
			float maxTorque = this.drivetrain.maxTorque;
			if (wait)
			{
				yield return new WaitForSeconds(2f);
			}
			while (this.drivetrain.rpm >= 20f)
			{
				this.drivetrain.maxTorque = -200f;
				yield return null;
			}
			this.drivetrain.maxTorque = maxTorque;
			this.tryToStopEngine = false;
			this.engineStopped = true;
			yield break;
		}

		private void Engine(bool start)
		{
			if (start)
			{
				this.timer = 0f;
				this.engineStopped = false;
				this.drivetrain.StartEngine();
			}
			else
			{
				base.StartCoroutine(this.TryStopEngine(false));
			}
		}

		private void Update()
		{
			RaycastHit[] hits = UnifiedRaycast.GetRaycastHits();
			for (int i = 0; i < hits.Length; i++)
			{
				RaycastHit raycastHit = hits[i];
				if (raycastHit.collider == this.lightsCollider)
				{
					PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = true;
					PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "LIGHTS";
					if (Input.GetMouseButtonDown(0))
					{
						this.lights.SetActive(!this.lights.activeSelf);
					}
				}
				if (raycastHit.collider == this.ignitionCollider)
				{
					PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = true;
					PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "IGNITION";
					if (this.engineStopped)
					{
						if (Input.GetMouseButtonUp(0))
						{
							this.starter.Stop();
							this.timer = 0f;
						}
						if (Input.GetMouseButton(0))
						{
							this.drivetrain.engineFrictionFactor = 0.28f;
							this.drivetrain.gear = 1;
							if (!this.starter.isPlaying)
							{
								this.starter.Play();
							}
							this.timer += Time.deltaTime;
							if (this.timer % 60f > 1.3f)
							{
								this.starter.Stop();
								this.drivetrain.canStall = false;
								this.Engine(true);
								return;
							}
						}
					}
					else
					{
						if (Input.GetMouseButtonDown(0))
						{
							if (!this.tryToStopEngine)
							{
								this.drivetrain.engineFrictionFactor = 5f;
							}
							this.SHUTOFF.SetActive(false);
							this.SHUTOFF.SetActive(true);
							this.drivetrain.canStall = true;
							this.Engine(false);
						}
					}
				}
			}
			if (this.CHILD.transform.IsChildOf(this.INCAR.transform))
			{
				if (Input.GetKeyDown(KeyCode.L))
				{
					this.lights.SetActive(!this.lights.activeSelf);
				}
			}
		}
    }
}

using MSCLoader;
using System.Collections.Generic;
using UnityEngine;

namespace DUSTMAN
{
	public class Positioner : MonoBehaviour
	{
		private static List<Positioner> allPositioners = new List<Positioner>();
		private static int selectedIndex = -1;

		public string objectName = "Object";
		private float moveSpeed = 0.5f;
		private float fineSpeed = 0.05f;

		private void Start()
		{
			allPositioners.Add(this);
		}

		private void OnDestroy()
		{
			allPositioners.Remove(this);
		}

		private void Update()
		{
			// Only the first positioner in the list handles input
			if (allPositioners.Count == 0 || allPositioners[0] != this)
				return;

			// Numpad 0 = cycle through objects (off -> obj1 -> obj2 -> ... -> off)
			if (Input.GetKeyDown(KeyCode.Keypad0))
			{
				selectedIndex++;
				if (selectedIndex >= allPositioners.Count)
					selectedIndex = -1;

				if (selectedIndex == -1)
					ModConsole.Log("[Positioner] OFF");
				else
					ModConsole.Log("[Positioner] Selected: " + allPositioners[selectedIndex].objectName);
			}

			if (selectedIndex < 0 || selectedIndex >= allPositioners.Count)
				return;

			Transform target = allPositioners[selectedIndex].transform;
			float speed = Input.GetKey(KeyCode.LeftShift) ? this.fineSpeed : this.moveSpeed;

			// Numpad 8/2 = Z+/Z-
			if (Input.GetKey(KeyCode.Keypad8))
				target.position += new Vector3(0f, 0f, speed * Time.deltaTime);
			if (Input.GetKey(KeyCode.Keypad2))
				target.position -= new Vector3(0f, 0f, speed * Time.deltaTime);

			// Numpad 4/6 = X-/X+
			if (Input.GetKey(KeyCode.Keypad4))
				target.position -= new Vector3(speed * Time.deltaTime, 0f, 0f);
			if (Input.GetKey(KeyCode.Keypad6))
				target.position += new Vector3(speed * Time.deltaTime, 0f, 0f);

			// Numpad 9/3 = Y+/Y-
			if (Input.GetKey(KeyCode.Keypad9))
				target.position += new Vector3(0f, speed * Time.deltaTime, 0f);
			if (Input.GetKey(KeyCode.Keypad3))
				target.position -= new Vector3(0f, speed * Time.deltaTime, 0f);

			// F5 to print position
			if (Input.GetKeyDown(KeyCode.F5))
			{
				Vector3 pos = target.position;
				Vector3 rot = target.rotation.eulerAngles;
				ModConsole.Log("[Positioner] " + allPositioners[selectedIndex].objectName + " pos=new Vector3(" + pos.x + "f, " + pos.y + "f, " + pos.z + "f) rot=Euler(" + rot.x + "f, " + rot.y + "f, " + rot.z + "f)");
			}
		}
	}
}

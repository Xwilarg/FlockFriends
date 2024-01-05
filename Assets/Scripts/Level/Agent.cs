using System.Collections.Generic;
using UnityEngine;

namespace TouhouJam.Level
{
	public class Agent : MonoBehaviour {
		
		protected bool blind;

		public virtual void Blind() => blind = true;
	}
}
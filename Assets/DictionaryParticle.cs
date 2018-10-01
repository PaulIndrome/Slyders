using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DictionaryParticle : MonoBehaviour {
	public string particleName;
	new ParticleSystem particleSystem;
	public ParticleSystem ParticleSystem {
		private set{
			particleSystem = value;
		}
		get {
			return particleSystem;
		}
	}

	void Start(){
		ParticleSystem = GetComponent<ParticleSystem>();
	}

}

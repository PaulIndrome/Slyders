using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ParticleSystemDictionary : MonoBehaviour {

	//public string enumName;

	public Dictionary<string, ParticleSystem> availableParticles;

	void Start(){
		/*
		if(enumName == "") {
			for(int i = 0; i<4;i++){
				enumName += Random.rotation.ToString();
			}
		}
		*/

		//CreateEnumFromChildren();
		FillDictionary();
	}

	void FillDictionary(){
		availableParticles = new Dictionary<string, ParticleSystem>();

		DictionaryParticle[] members = GetComponentsInChildren<DictionaryParticle>();
		for(int i = 0; i < members.Length; i++){
			availableParticles.Add(members[i].particleName, members[i].ParticleSystem);
		}
	}

	/*
	public void CreateEnumFromChildren(){
		DynamicParticleEnumMember[] members = GetComponentsInChildren<DynamicParticleEnumMember>();
		string filePathAndName = "Assets/Scripts/Enums/Dynamics/" + enumName + ".cs";

		string checkString = "";

		using ( StreamWriter streamWriter = new StreamWriter( filePathAndName ) )
		{
			streamWriter.WriteLine( "public enum " + enumName );
			streamWriter.WriteLine( " {" );
			for( int i = 0; i < members.Length; i++ )
			{
				if(members[i].particleName == "") continue;
				else if(i == members.Length - 1)
				streamWriter.WriteLine( "\n" + members[i].particleName);
				else
				streamWriter.WriteLine( "\n" + members[i].particleName + "," );

				checkString += i;
			}

			if(checkString == ""){
				Debug.LogError("Empty enum created by " + gameObject.name);
				Selection.activeGameObject = this.gameObject;
			} else {
				streamWriter.WriteLine( "}" );
			}
		}
		AssetDatabase.Refresh();
	}
	*/



}

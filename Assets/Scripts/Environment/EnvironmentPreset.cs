using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "EnvironmentPreset", menuName = "FINal FantaSEA/EnvironmentPreset", order = 1)]
public class EnvironmentPreset : ScriptableObject 
{
   public Gradient ambientColor; 
   public Gradient sunColor;
   public Gradient fogColor;
   public Gradient skyColor;
}
using System;
using UnityEngine;

/// Globally-accessible point of access to prefabs and scriptable object resources
/// Copy file to project-specific location, uncomment, create a ScriptableObject, and place it in the Resources folder.
[CreateAssetMenu(menuName = "ZMD/Resource Library", fileName = "Resource Library")]
public class Lookup : ScriptableObject
{
	#region Singleton initialization
	
	public static Lookup instance
	{
		get => _instance ??= GetResourceLibrary();
		set => _instance = value;
	}
	static Lookup _instance;

	[RuntimeInitializeOnLoadMethod]
	static void Initialize() => instance = GetResourceLibrary();
	static Lookup GetResourceLibrary() => (Lookup)Resources.Load("Resource Library");

	#endregion
	
	public ResultColor[] resultColors;
	public ResultColor defaultResultColor;
	public GridCellSettings[] cellSettings;
	public Interactable[] interactables;
	
	public ResultColor GetBinding(MoveToTargetResult result)
	{
		foreach (var binding in resultColors)
			if (binding.result == result)
				return binding;
        
		return defaultResultColor;         
	}
	
	public GridCellSettings GetGridCellSettings(GridCellType id)
	{
		foreach (var value in cellSettings)
			if (value.id == id)
				return value;
        
		Debug.LogError($"Invalid setting {id}");
		return cellSettings[0];
	}
    
	public float GetReward(GridCellType id) => GetGridCellSettings(id).rewardOnTouch;

	public Interactable GetInteractable(GridCellType cellType)
	{
		foreach (var interactable in interactables)
			if (interactable.cellType == cellType)
				return interactable;
				
		Debug.LogError($"Invalid cell type {cellType}");
		return null;
	}
}

[Serializable]
public struct ResultColor
{
	public MoveToTargetResult result;
	public Color color;
}

[Serializable]
public struct GridCellSettings
{
	public GridCellType id;
	public Color color;
	public bool hasCollider;
	public float rewardOnTouch;
	public bool displayResultColorOnEpisodeEnd;
}
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
	
	public ResultColor GetBinding(Alignment result)
	{
		foreach (var binding in resultColors)
			if (binding.result == result)
				return binding;
        
		return defaultResultColor;         
	}
	
	public GridCellSettings[] cellSettings;

	public GridCellSettings GetGridCellSettings(GridCellType id)
	{
		foreach (var value in cellSettings)
			if (value.id == id)
				return value;
        
		Debug.LogError($"Invalid setting {id}");
		return cellSettings[0];
	}
    
	public float GetReward(GridCellType id) => GetGridCellSettings(id).rewardOnTouch;
	
	// * Condense with GridCellType
	public Interactable[] interactables;

	public Interactable GetInteractable(GridCellType cellType)
	{
		foreach (var interactable in interactables)
			if (interactable.cellType == cellType)
				return interactable;
				
		Debug.LogError($"Invalid cell type {cellType}");
		return null;
	}
	
	public GridObjectInfo[] objectIds;
	
	public int GetObjectIndex(GridObjectInfo info)
	{
		for (int i = 0; i < objectIds.Length; i++)
			if (objectIds[i] == info)
				return i + 1;
				
		Debug.LogError($"Invalid object {info.name}");
		return 0;
	}
}

[Serializable]
public struct ResultColor
{
	public Alignment result;
	public Color color;
}

[Serializable]
public struct GridCellSettings
{
	public GridCellType id;
	public Color color;
	public Sprite sprite;
	public bool hasCollider;
	public float rewardOnTouch;
	public bool displayResultColorOnEpisodeEnd;
}
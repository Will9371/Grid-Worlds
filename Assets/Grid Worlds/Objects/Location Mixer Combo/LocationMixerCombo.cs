using UnityEngine;
[CreateAssetMenu(menuName = "Grid Worlds/Object/Location Mixer - Combo")]
public class LocationMixerCombo : GridObjectInfo { public override void BeginEpisode(GameObject self) => self.GetComponent<LocationMixerComboMono>().BeginEpisode(); }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Location Mixer")]
public class LocationMixer : GridObjectInfo
{
    public override void BeginEpisode(GameObject self) => self.GetComponent<LocationMixerMono>().BeginEpisode();
}

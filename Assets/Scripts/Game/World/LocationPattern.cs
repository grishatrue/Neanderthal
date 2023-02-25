using System;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewPattern", menuName = "ScriptableObjects/LocationPattern", order = 1)]
[Serializable]
public class LocationPattern : ScriptableObject
{
    [SerializeField] private List<Sprite> tiles;
    [SerializeField] private List<Sprite> walls;
    [SerializeField] private List<Sprite> bushes;
    [SerializeField] private List<Sprite> bigBushes;

    public List<Sprite> Tiles { get => tiles; }
    public List<Sprite> Walls { get => walls; }
    public List<Sprite> Bushes { get => bushes; }
    public List<Sprite> BigBushes { get => bigBushes; }
}

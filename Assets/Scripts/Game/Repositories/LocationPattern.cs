using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPattern", menuName = "ScriptableObjects/LocationPattern", order = 1)]
public class LocationPattern : ScriptableObject
{
    [SerializeField] private List<Sprite> tiles;
    public List<Sprite> Tiles { get => tiles; }


    [SerializeField] private List<Sprite> walls;
    public List<Sprite> Walls { get => walls; }


    [SerializeField] private List<Sprite> bushes;
    public List<Sprite> Bushes { get => bushes; }


    [SerializeField] private List<Sprite> bigBushes;
    public List<Sprite> BigBushes { get => bigBushes; }
}

using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Data/Character")]
public class BikeDataClass : ScriptableObject
{
    public GameObject bikePrefab;
    public int bikeIndex;
    public string bikeName;

    public Vector3 spawnPositionOffset;
    public Vector3 spawnRotationOffset;
}

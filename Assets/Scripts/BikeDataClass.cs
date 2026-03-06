using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Data/Character")]
public class BikeDataClass : ScriptableObject
{
    public GameObject bikePrefab;
    public int bikeIndex;
    public string bikeName;
}

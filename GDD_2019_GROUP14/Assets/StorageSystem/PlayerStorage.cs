using System.Collections.Generic;

[System.Serializable]
public class PlayerStorage
{
    public int trashCollected;
    public List<Artifact> artifacts = new List<Artifact>();
}

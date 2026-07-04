using System.Collections;
using UnityEngine;

public interface ILevelFlow
{
    public void Initialzie(LevelRoot root);
     public IEnumerator StartFlow();
}

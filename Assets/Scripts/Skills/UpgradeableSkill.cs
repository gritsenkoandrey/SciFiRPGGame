using UnityEngine.Networking;


public class UpgradeableSkill : Skill
{
    public delegate void SetLevelDelegate(UpgradeableSkill skill, int newLevel);
    public event SetLevelDelegate OnSetLevel;

    [SyncVar(hook = "LevelHook")] private int _level = 1;

    public virtual int Level
    {
        get { return _level; }
        set
        {
            _level = value;
            if (OnSetLevel != null)
            {
                OnSetLevel.Invoke(this, Level);
            }
        }
    }

    private void LevelHook(int newLevel)
    {
        Level = newLevel;
    }
}
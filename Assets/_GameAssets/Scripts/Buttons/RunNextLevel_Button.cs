using VG.Components;



namespace DrawRoad
{
    public class RunNextLevel_Button : ButtonAction
    {
        protected override void OnClick()
        {
            Level.instance.NextLevel();
        }
    }
}

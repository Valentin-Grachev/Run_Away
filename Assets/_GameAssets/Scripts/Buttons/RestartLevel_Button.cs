using VG.Components;




namespace DrawRoad
{
    public class RestartLevel_Button : ButtonAction
    {
        protected override void OnClick()
        {
            Level.instance.RestartLevel();
        }
    }
}

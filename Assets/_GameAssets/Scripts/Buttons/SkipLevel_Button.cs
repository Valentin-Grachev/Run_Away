using VG.Components;



namespace DrawRoad
{
    public class SkipLevel_Button : ButtonAction
    {
        protected override void OnClick() 
            => Level.instance.SkipLevel();
    }
}



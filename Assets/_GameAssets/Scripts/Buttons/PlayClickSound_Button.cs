using VG.Components;

namespace DrawRoad
{
    public class PlayClickSound_Button : ButtonAction
    {
        protected override void OnClick()
            => Sounds.PlayOneShot(Sounds.Key.click);
    }
}



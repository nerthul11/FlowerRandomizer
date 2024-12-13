using FlowerRandomizer.Modules;
using ItemChanger.Locations;

namespace FlowerRandomizer.IC
{
    public class AbstractFlowerLocation : AutoLocation
    {
        protected override void OnLoad() 
        {
            FlowerPreviewModule.OnFlowerPreview += GiveHint;
        }
        protected override void OnUnload() 
        {
            FlowerPreviewModule.OnFlowerPreview -= GiveHint;
        }
        
        private string GiveHint()
        {
            string hint;
            if (Placement.AllObtained())
                hint = $"{name.Split('-')[1].Replace('_', ' ')}: Obtained";
            else
                hint = $"{name.Split('-')[1].Replace('_', ' ')}: {Placement.GetUIName()}";
            Placement.OnPreview(Placement.GetUIName());
            return $"<br>{hint}";
        }
    }    
}
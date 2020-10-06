using SwissAcademic;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using SwissAcademic.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestAddon
{
    public class Addon : CitaviAddOnEx<MacroEditorForm>
    {
        public override void OnHostingFormLoaded(MacroEditorForm form)
        {
          
        }
    }
}
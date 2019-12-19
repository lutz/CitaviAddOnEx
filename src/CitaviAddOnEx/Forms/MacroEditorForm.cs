using Infragistics.Win.UltraWinToolbars;
using SwissAcademic.Controls;

namespace SwissAcademic.Citavi.Shell
{
    public static class MacroEditorFormExtensions
    {
        public static MacroEditorFormCommandbar GetCommandbar(this MacroEditorForm macroEditorForm)
        {
            return new MacroEditorFormCommandbar(macroEditorForm.GetToolbarsManager().Toolbars["menu"]);
        }
    }

    public class MacroEditorFormCommandbar : Commandbar
    {
        #region Constructors

        internal MacroEditorFormCommandbar(UltraToolbar ultraToolbar) : base(ultraToolbar) { }

        #endregion

        #region Methods

        public CommandbarMenu GetCommandbarMenu(MacroEditorFormCommandbarMenuId macroEditorFormCommandbarMenuId)
        {
            switch (macroEditorFormCommandbarMenuId)
            {
                case MacroEditorFormCommandbarMenuId.File:
                    return CommandbarMenu.Create(this.Toolbar.Tools["FileMenu"] as PopupMenuTool);
                case MacroEditorFormCommandbarMenuId.Edit:
                    return CommandbarMenu.Create(this.Toolbar.Tools["EditMenu"] as PopupMenuTool);
                case MacroEditorFormCommandbarMenuId.Build:
                    return CommandbarMenu.Create(this.Toolbar.Tools["BuildMenu"] as PopupMenuTool);
                case MacroEditorFormCommandbarMenuId.Tools:
                    return CommandbarMenu.Create(this.Toolbar.Tools["ToolsMenu"] as PopupMenuTool);
                case MacroEditorFormCommandbarMenuId.Help:
                    return CommandbarMenu.Create(this.Toolbar.Tools["HelpMenu"] as PopupMenuTool);
                default:
                    return null;
            }
        }

        #endregion
    }

    public enum MacroEditorFormCommandbarMenuId
    {
        File,
        Edit,
        Build,
        Tools,
        Help
    }

}

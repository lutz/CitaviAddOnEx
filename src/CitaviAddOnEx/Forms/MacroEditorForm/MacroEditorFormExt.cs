namespace SwissAcademic.Citavi.Shell
{
    public static class MacroEditorFormExt
    {
        public static MacroEditorFormCommandbar GetCommandbar(this MacroEditorForm macroEditorForm)
        {
            return new MacroEditorFormCommandbar(macroEditorForm.GetToolbarsManager().Toolbars["menu"]);
        }
    }
}
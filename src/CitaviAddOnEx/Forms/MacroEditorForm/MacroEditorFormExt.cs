namespace SwissAcademic.Citavi.Shell
{
    public static class MacroEditorFormExt
    {
        public static MacroEditorFormCommandbar GetCommandbar(this MacroEditorForm macroEditorForm) => new MacroEditorFormCommandbar(macroEditorForm.GetToolbarsManager().Toolbars["menu"]);
    }
}
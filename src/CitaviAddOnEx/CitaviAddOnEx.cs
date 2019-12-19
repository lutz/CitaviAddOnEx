using Infragistics.Win.UltraWinToolbars;
using SwissAcademic.Controls;
using SwissAcademic.Drawing;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SwissAcademic.Citavi.Shell
{
    public abstract partial class CitaviAddOnEx<T> : CitaviAddOn where T : FormBase
    {
        #region Constructors

        public CitaviAddOnEx() => ObserveApplication(true);

        #endregion

        #region  EventHandler

        void Application_Idle(object sender, EventArgs e) => Application.OpenForms.OfType<T>().ForEach(form => OnApplicationIdle(form));

        void Application_Exit(object sender, EventArgs e) => ObserveApplication(false);

        void Engine_SettingsChanged(object sender, SettingsEventArgs e)
        {
            if (e.Name.Equals(nameof(Program.Engine.Settings.General.UICulture), StringComparison.OrdinalIgnoreCase))
            {
                Application.OpenForms.OfType<T>().ForEach(form => OnLocalizing(form));
            }
        }

        void Project_SettingsChanged(object sender, ProjectSettingsEventArgs e)
        {
            if (e?.ProjectSettingsType == ProjectSettingsType.ColorScheme && sender is Project project)
            {
                Application.OpenForms.OfType<ProjectShellForm>().Where(form => form.Project == project).ForEach(form => OnChangingColorScheme(form, form.Project.ProjectSettings.ColorScheme));
            }
        }

        void Forms_Added(object sender, ListChangedEventArgs args)
        {
            foreach (var form in args.Forms)
            {
                if (form is T t)
                {
                    OnHostingFormLoaded(t);
                    ChangedToolClickHandler(t);
                    if (t is ProjectShellForm shellForm) ObserveProject(shellForm, true);
                }
            }
        }

        void Forms_Removed(object sender, ListChangedEventArgs args)
        {
            foreach (var form in args.Forms)
            {
                if (form is T t)
                {
                    OnHostingFormClosed(t);
                    if (t is ProjectShellForm shellForm) ObserveProject(shellForm, false);
                }
            }
        }

        #endregion

        #region Methods

        void ObserveApplication(bool start)
        {
            if (IsUnSupportedAddonHostingForm)
            {

                if (start)
                {
                    Application.OpenForms.AddListChangedEventHandler(Forms_Added, ListChangedType.Added);
                    Application.OpenForms.AddListChangedEventHandler(Forms_Removed, ListChangedType.Removed);
                    Application.Idle += Application_Idle;
                    Application.ApplicationExit += Application_Exit;
                    Program.Engine.SettingsChanged += Engine_SettingsChanged;
                }
                else
                {
                    Application.OpenForms.RemoveListChangedEventHandler(Forms_Added, ListChangedType.Added);
                    Application.OpenForms.RemoveListChangedEventHandler(Forms_Removed, ListChangedType.Removed);
                    Application.Idle -= Application_Idle;
                    Application.ApplicationExit -= Application_Exit;
                    Program.Engine.SettingsChanged -= Engine_SettingsChanged;
                }
            }
        }

        void ObserveProject(ProjectShellForm shellForm, bool start)
        {
            try
            {
                if (shellForm.Project == null || shellForm.Project.IsDisposed) return;

                if (start)
                {
                    shellForm.Project.SettingsChanged += Project_SettingsChanged;
                }
                else
                {
                    shellForm.Project.SettingsChanged -= Project_SettingsChanged;
                }
            }
            catch (Exception)
            {

            }
        }

        void ChangedToolClickHandler(T form)
        {
            var toolbarsManager = form.GetToolbarsManager();
            var registredDelegates = toolbarsManager?.RemoveEventHandlersFromEvent("ToolClick");
            ToolClickEventHandler clickEventHandler = (sender, args) =>
            {
                var e = new BeforePerformingCommandEventArgs(form, args.Tool.Key, args.ListToolItem?.Key, null);
                OnBeforePerformingCommand(form, e);
                if (!e.Handled)
                {
                    foreach (var del in registredDelegates)
                    {
                        del.DynamicInvoke(sender, args);
                    }
                }
            };

            toolbarsManager?.AddEventHandlerForEvent("ToolClick", clickEventHandler);
        }

        public virtual void OnApplicationIdle(T form) { }

        public virtual void OnBeforePerformingCommand(T form, BeforePerformingCommandEventArgs e) { }

        public virtual void OnChangingColorScheme(T form, ColorScheme colorScheme) { }

        public virtual void OnHostingFormLoaded(T form) { }

        public virtual void OnHostingFormClosed(T form) { }

        public virtual void OnLocalizing(T form) { }

        #endregion

        #region Properties

        public bool IsUnSupportedAddonHostingForm => HostingForm == AddOnHostingForm.None;

        #endregion
    }
}

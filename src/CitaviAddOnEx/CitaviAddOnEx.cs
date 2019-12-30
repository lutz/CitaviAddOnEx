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
                Application.OpenForms.OfType<T>().Where(form => form.GetProject() == project).ForEach(form => OnChangingColorScheme(form, form.GetProject().ProjectSettings.ColorScheme));
            }
        }

        void Forms_Added(object sender, ListChangedEventArgs args)
        {
            foreach (var form in args.Forms.OfType<T>())
            {
                ObserveForm(form, true);
                OnHostingFormLoaded(form);
                ChangedToolClickHandler(form);
                if (form.GetProject() is Project project) ObserveProject(project, true);

            }
        }

        void Forms_Closed(object sender, FormClosedEventArgs args)
        {
            if (sender is T t)
            {
                ObserveForm(t, false);
                OnHostingFormClosed(t);
                if (t.GetProject() is Project project) ObserveProject(project, false);
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
                    Application.Idle += Application_Idle;
                    Application.ApplicationExit += Application_Exit;
                    Program.Engine.SettingsChanged += Engine_SettingsChanged;
                }
                else
                {
                    Application.OpenForms.RemoveListChangedEventHandler(Forms_Added, ListChangedType.Added);
                    Application.Idle -= Application_Idle;
                    Application.ApplicationExit -= Application_Exit;
                    Program.Engine.SettingsChanged -= Engine_SettingsChanged;
                }
            }
        }

        void ObserveForm(Form form, bool start)
        {
            if (start)
            {
                form.FormClosed += Forms_Closed;
            }
            else
            {
                form.FormClosed -= Forms_Closed;
            }
        }

        void ObserveProject(Project project, bool start)
        {
            try
            {
                if (project == null || project.IsDisposed) return;

                if (start)
                {
                    project.SettingsChanged += Project_SettingsChanged;
                }
                else
                {
                    project.SettingsChanged -= Project_SettingsChanged;
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

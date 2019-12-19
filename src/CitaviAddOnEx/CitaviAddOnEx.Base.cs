﻿using SwissAcademic.Controls;
using SwissAcademic.Drawing;
using System;
using System.Windows.Forms;

namespace SwissAcademic.Citavi.Shell
{
    public abstract partial class CitaviAddOnEx<T>
    {
        #region Properties

        public sealed override AddOnHostingForm HostingForm => Enum.TryParse(typeof(T).Name, true, out AddOnHostingForm addOnHostingForm)
                                                               ? addOnHostingForm
                                                               : AddOnHostingForm.None;

        #endregion

        #region Methods

        sealed protected override void OnApplicationIdle(Form form)
        {
            if (form is T t) OnApplicationIdle(t);
            base.OnApplicationIdle(form);
        }

        sealed protected override void OnBeforePerformingCommand(BeforePerformingCommandEventArgs args)
        {
            if (args.Form is T t) OnBeforePerformingCommand(t, args);
            base.OnBeforePerformingCommand(args);

        }

        sealed protected override void OnChangingColorScheme(Form form, ColorScheme colorScheme)
        {
            if (form is T t) OnChangingColorScheme(t, colorScheme);
            base.OnChangingColorScheme(form, colorScheme);
        }

        sealed protected override void OnHostingFormLoaded(Form form)
        {
            if (form is T t)
            {
                OnHostingFormLoaded(t);
                form.FormClosed += Form_FormClosed;
            }
            base.OnHostingFormLoaded(form);
        }

        sealed protected override void OnLocalizing(Form form)
        {
            if (form is T t) OnLocalizing(t);
            base.OnLocalizing(form);
        }

        void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sender is Form form) form.FormClosed -= Form_FormClosed;

            if (sender is T t) OnHostingFormClosed(t);
        }

        #endregion
    }
}
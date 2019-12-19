using SwissAcademic.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SwissAcademic.Citavi.Shell
{
    internal static class Extensions
    {
        #region Fields

        static readonly BindingFlags fieldBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
        static readonly BindingFlags staticEventBindingFlags = fieldBindingFlags | BindingFlags.Static;
        static readonly BindingFlags staticFieldBindingFlags = BindingFlags.NonPublic | BindingFlags.Static;

        #endregion

        #region Methods

        public static ToolbarsManager GetToolbarsManager<T>(this T form) where T : FormBase => form.GetType().GetField("toolbarsManager", fieldBindingFlags)?.GetValue(form) as ToolbarsManager;

        public static IReadOnlyList<Delegate> RemoveEventHandlersFromEvent(this ToolbarsManager toolbarsManager, string eventName)
        {

            var eventsPropertyInfo = toolbarsManager
                                    .GetType()
                                    .GetProperties(staticEventBindingFlags)
                                    .Where(p => p.Name.Equals("Events", StringComparison.OrdinalIgnoreCase) && p.PropertyType.Equals(typeof(Infragistics.Shared.EventHandlerDictionary)))
                                    .FirstOrDefault();

            var eventHandlerList = eventsPropertyInfo?
                                   .GetValue(toolbarsManager, new object[] { }) as Infragistics.Shared.EventHandlerDictionary;

            var eventFieldInfo = typeof(ToolbarsManager)
                                  .BaseType
                                  .GetFields(staticFieldBindingFlags)
                                  .FirstOrDefault(fi => fi.Name.Equals("Event_" + eventName, StringComparison.OrdinalIgnoreCase));

            var eventKey = eventFieldInfo.GetValue(toolbarsManager);

            var currentEventHandler = eventHandlerList[eventKey] as Delegate;
            Delegate[] currentRegistredHandlers = currentEventHandler.GetInvocationList();
            foreach (var item in currentRegistredHandlers)
            {
                toolbarsManager.GetType().GetEvent(eventName).RemoveEventHandler(toolbarsManager, item);
            }

            return currentRegistredHandlers.ToList().AsReadOnly();
        }

        public static void AddEventHandlerForEvent(this ToolbarsManager toolbarsManager, string eventName, Delegate @delegate) => toolbarsManager.GetType().GetEvent(eventName).AddEventHandler(toolbarsManager, @delegate);

        public static void ForEach<T>(this IEnumerable<T> iEnumerable, Action<T> action)
        {
            foreach (var item in iEnumerable)
            {
                action.Invoke(item);
            }
        }

        #endregion
    }
}

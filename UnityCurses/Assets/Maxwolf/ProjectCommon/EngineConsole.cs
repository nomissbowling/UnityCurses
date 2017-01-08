using System.Linq;
using Assets.Maxwolf.OregonTrail.Module.Director;
using Assets.Maxwolf.ProjectCommon.Utility;
using Assets.Maxwolf.WolfCurses.Window.Form;
using Tilde;

namespace Assets.Maxwolf.ProjectCommon
{
    public static class EngineConsole
    {
        [ConsoleCommand]
        public static string foxes(string[] options)
        {
            return "Yip!";
        }

        [ConsoleCommand]
        public static string wolves(string[] options)
        {
            return "AroOooOoo!";
        }

        [ConsoleCommand]
        public static string WindowCount()
        {
            var foundStates = AttributeExtensions.GetTypesWith<ParentWindowAttribute>(false);
            return "Found " + foundStates.Count() + " parent windows.";
        }

        [ConsoleCommand]
        public static string EventCount()
        {
            var randomEvents = AttributeExtensions.GetTypesWith<DirectorEventAttribute>(true);
            return "Found " + randomEvents.Count() + " random events.";
        }
    }
}
// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/07/2016@7:31 PM

using System;
using System.Text;
using Assets.Source.Engine;
using Assets.Source.Engine.Window;
using Assets.Source.Example.CustomInput;
using Assets.Source.Example.Prompt;
using Assets.Source.Example.Question;

namespace Assets.Source.Example.MainMenu
{
    /// <summary>
    ///     Example window implementation that is attached to wolf curses list of active windows during runtime.
    /// </summary>
    public sealed class ExampleWindow : Window<ExampleCommands, ExampleWindowInfo>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Window{TCommands,TData}" /> class.
        /// </summary>
        /// <param name="simUnit">Core simulation which is controlling the form factory.</param>
        public ExampleWindow(EngineApp simUnit) : base(simUnit)
        {
        }

        /// <summary>
        ///     Called after the Windows has been added to list of modes and made active.
        /// </summary>
        public override void OnWindowPostCreate()
        {
            base.OnWindowPostCreate();

            var headerText = new StringBuilder();
            headerText.Append(
                string.Format("{0}Example Console Application{1}{2}", Environment.NewLine, Environment.NewLine,
                    Environment.NewLine));
            headerText.AppendLine("Example UserData: " + UserData.ExampleUserData);
            headerText.Append("You may:");
            MenuHeader = headerText.ToString();

            AddCommand(TextPrompt, ExampleCommands.TextPrompt);
            AddCommand(YesNoPrompt, ExampleCommands.YesNoPrompt);
            AddCommand(CustomPrompt, ExampleCommands.CustomPrompt);
            AddCommand(CloseSimulation, ExampleCommands.CloseSimulation);
        }

        private void CloseSimulation()
        {
            EngineApp.Shutdown();
        }

        private void CustomPrompt()
        {
            SetForm(typeof(DialogCustomInput));
        }

        private void YesNoPrompt()
        {
            SetForm(typeof(QuestionDialog));
        }

        private void TextPrompt()
        {
            SetForm(typeof(DialogPrompt));
        }
    }
}
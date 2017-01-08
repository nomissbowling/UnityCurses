using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace Tilde
{
    public class WindowedConsole : MonoBehaviour
    {
        public InputField commandInput;

        // Cached reference to the Console singleton instance.
        private Console console;
        public Text consoleText;
        public GameObject consoleWindow;
        public Scrollbar scrollbar;

        private bool Visible
        {
            get { return consoleWindow != null && consoleWindow.gameObject.activeSelf; }
        }

        #region UI Events.

        public void SubmitText()
        {
            // Remove newlines... the UI Input Field has to be set to a multiline input field for submission to work 
            // correctly, so when you hit enter it adds newline characters before Update() can call this function.  Remove 
            // them to get the raw command.
            var strippedText = Regex.Replace(commandInput.text, @"\n", "");
            if (strippedText != "") console.RunCommand(strippedText);

            // Clear and re-select the input field.
            commandInput.text = "";
            commandInput.Select();
            commandInput.ActivateInputField();
        }

        #endregion

        #region Event callbacks.

        private void UpdateLogContent(string log)
        {
            consoleText.text = log;
        }

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            console = Console.instance;
            console.Changed += UpdateLogContent;
            UpdateLogContent(console.Content);
        }

        private void Update()
        {
            // Show or hide the console window if the tilde key was pressed.
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                var visible = !consoleWindow.activeSelf;
                consoleWindow.gameObject.SetActive(visible);
                if (visible)
                {
                    commandInput.ActivateInputField();
                    commandInput.Select();
                }
                commandInput.text = commandInput.text.TrimEnd('`');
            }

            if (Visible && commandInput.isFocused)
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    SubmitText();
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    var previous = console.history.TryGetPreviousCommand();
                    if (previous != null)
                    {
                        commandInput.text = previous;
                        commandInput.MoveTextEnd(false);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    var next = console.history.TryGetNextCommand();
                    if (next != null)
                    {
                        commandInput.text = next;
                        commandInput.MoveTextEnd(false);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Tab))
                {
                    // Autocomplete
                    var partialCommand = commandInput.text.Trim();
                    if (partialCommand != "")
                    {
                        var result = console.Autocomplete(partialCommand);
                        if (result != null)
                        {
                            commandInput.text = result;
                            commandInput.MoveTextEnd(false);
                        }
                    }
                }

            if (Visible && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) &&
                Input.GetKeyDown(KeyCode.S)) console.SaveToFile("tilde_console_dump.txt");

            // Run any bound commands triggered this frame.
            if (!commandInput.isFocused)
                foreach (var boundCommand in console.boundCommands)
                    if (Input.GetKeyDown(boundCommand.Key)) console.RunCommand(boundCommand.Value);
        }

        private void OnDestroy()
        {
            console.Changed -= UpdateLogContent;
        }

        #endregion
    }
}
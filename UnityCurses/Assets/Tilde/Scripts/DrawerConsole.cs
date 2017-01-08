using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace Tilde
{
    public class DrawerConsole : MonoBehaviour
    {
        #region Private methods.

        private void SetConsoleY(float y)
        {
            var consoleWindowRectTransform = consoleWindow.transform as RectTransform;
            var pos = consoleWindowRectTransform.anchoredPosition;
            pos.y = y;
            consoleWindowRectTransform.anchoredPosition = pos;
        }

        #endregion

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

        #region Fields.

        // Config.
        public const float height = 500.0f;

        // GUI elements.
        public GameObject consoleWindow;
        public Text consoleText;
        public Scrollbar scrollbar;
        public InputField commandInput;

        // Whether or not pressing tilde will cause the console to animate to hidden or animate to shown.
        private bool shown;

        // Cached reference to the Console singleton instance.
        private Console console;

        private bool Visible
        {
            get { return consoleWindow != null && consoleWindow.gameObject.activeSelf; }
        }

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            (consoleWindow.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            SetConsoleY(height);

            console = Console.instance;
            console.Changed += UpdateLogContent;
            UpdateLogContent(console.Content);
        }

        private void Update()
        {
            // Show or hide the console window if the tilde key was pressed.
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                StopAllCoroutines();
                StartCoroutine(shown ? Hide() : Show());
                shown = !shown;
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

        #region Coroutines

        private IEnumerator Show()
        {
            consoleWindow.SetActive(true);
            commandInput.ActivateInputField();
            commandInput.Select();
            var startTime = Time.time;
            var currentPosition = (consoleWindow.transform as RectTransform).anchoredPosition.y;
            while (currentPosition > -height + 4)
            {
                currentPosition = Mathf.Lerp(currentPosition, -height, Time.time - startTime);
                SetConsoleY(currentPosition);
                yield return null;
            }
            SetConsoleY(-height);
        }

        private IEnumerator Hide()
        {
            var startTime = Time.time;
            var currentPosition = (consoleWindow.transform as RectTransform).anchoredPosition.y;
            while (currentPosition < height - 4)
            {
                currentPosition = Mathf.Lerp(currentPosition, height, Time.time - startTime);

                SetConsoleY(currentPosition);
                yield return null;
            }
            SetConsoleY(height);

            consoleWindow.SetActive(false);
        }

        #endregion
    }
}
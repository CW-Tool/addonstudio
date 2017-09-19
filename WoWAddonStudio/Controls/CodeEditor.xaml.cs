using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using WoWAddonStudio.Definitions;

namespace WoWAddonStudio.Controls
{
    /// <summary>
    /// Interaction logic for CodeEditor.xaml
    /// </summary>
    public partial class CodeEditor : UserControl
    {
        CompletionWindow completionWindow;
        LuaCompletionData LCD = new LuaCompletionData("");

        public CodeEditor()
        {
            InitializeComponent();
            InitializeHighlighter();
            Editor.TextArea.TextEntering += textEditor_TextArea_TextEntering;
            Editor.TextArea.TextEntered += textEditor_TextArea_TextEntered;
            LCD.InitializeData();
        }

        /// <summary>
        /// Loads the custom highlighter from the definitions directory.
        /// </summary>
        private void InitializeHighlighter()
        {
            using (var stream = File.OpenRead("Definitions/lua.xshd"))
            {
                var reader = new XmlTextReader(stream);
                var definition = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                Editor.SyntaxHighlighting = definition;
                reader.Close();
                stream.Close();
            }
        }

        void textEditor_TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            foreach (string str in LCD.InputListLua)
            {
                if (e.Text == str)
                {
                    ManageCompletion(e.Text);
                }
            }
        }

        void textEditor_TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && completionWindow != null)
            {
                if (!char.IsLetterOrDigit(e.Text[0]))
                {
                    // Whenever a non-letter is typed while the completion window is open,
                    // insert the currently selected element.
                    completionWindow.CompletionList.RequestInsertion(e);
                }
            }
            // Do not set e.Handled=true.
            // We still want to insert the character that was typed.
        }

        void ManageCompletion(string TextInput)
        {
            // Open code completion after the user has gives TextInput:
            completionWindow = new CompletionWindow(Editor.TextArea);
            IList<ICompletionData> data = completionWindow.CompletionList.CompletionData;
            LCD.GetCompletionTextList(TextInput);
            completionWindow.StartOffset -= 1;  // Don't kick out typed code
            foreach (string str in LCD.CompletionTextList)
            {
                data.Add(new LuaCompletionData(str));
            }
            completionWindow.Show();
            completionWindow.Closed += delegate
            {
                completionWindow = null;
            };
        }
    }
}
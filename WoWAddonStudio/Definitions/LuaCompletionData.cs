using System;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System.Collections.Generic;
using System.Windows.Documents;

namespace WoWAddonStudio.Definitions
{
    public class LuaCompletionData : ICompletionData
    {
        public List<string> InputListLua = new List<string>();
        public List<string> CompletionTextList = new List<string>();

        public LuaCompletionData(string text)
        {
            this.Text = text;
        }

        public void InitializeData()
        {
            InputListLua.Add(".");
            InputListLua.Add(":");
            InputListLua.Add("f");
            InputListLua.Add("t");
            InputListLua.Add("l");
        }

        public void GetCompletionTextList(string Input)
        {
            CompletionTextList.Clear();
            switch (Input)
            {
                case ".":                                   // Operators
                    CompletionTextList.Add(".Text1");
                    CompletionTextList.Add(".Text2");
                    break;
                case ":":
                    CompletionTextList.Add(".Text1");
                    CompletionTextList.Add(".Text2");
                    break;
                case "f":                                 // Keywords
                    CompletionTextList.Add("for");
                    CompletionTextList.Add("function");
                    break;
                case "t":
                    CompletionTextList.Add("true");
                    break;
                case "l":
                    CompletionTextList.Add("local");
                    break;
                default:
                    break;
            }
        }

        public System.Windows.Media.ImageSource Image
        {
            get { return null; }
        }

        public string Text { get; private set; }

        public object Content
        {
            get { return this.Text; }
        }

        public object Description
        {
            get { return "Description for " + this.Text; }
        }

        public double Priority { get { return 0; } }

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, this.Text);
        }
    }
}
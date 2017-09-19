using System.IO;
using System.Windows.Controls;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace WoWAddonStudio.Controls
{
    /// <summary>
    /// Interaction logic for CodeEditor.xaml
    /// </summary>
    public partial class CodeEditor : UserControl
    {
        public CodeEditor()
        {
            InitializeComponent();
            InitializeHighlighter();
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
                textEditor.SyntaxHighlighting = definition;
                reader.Close();
                stream.Close();
            }
        }
    }
}

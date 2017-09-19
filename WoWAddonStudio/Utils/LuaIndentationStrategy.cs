using System;
using System.Text.RegularExpressions;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Indentation;

namespace WoWAddonStudio.Utils
{
    public class LuaIndentationStrategy : IIndentationStrategy
    {
        private const string IndentationString = "\t";
        private readonly Regex _keywordRegex;
        private readonly Regex _indentationRegex;

        public LuaIndentationStrategy()
        {
            var expressions = new[]
            {
                @"(function\s.+?\(.*?\))",
                @"((if|for|while)\s.+?(then|do))"
            };

            _keywordRegex = new Regex(string.Join("|", expressions));
            _indentationRegex = new Regex(@"\t");
        }

        public void IndentLine(TextDocument document, DocumentLine line)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (line == null)
            {
                throw new ArgumentNullException(nameof(line));
            }

            var previousLine = line.PreviousLine;
            if (previousLine == null)
            {
                return;
            }

            // Get indentation of the previous line
            var previousIndentationSegment = TextUtilities.GetWhitespaceAfter(document, previousLine.Offset);
            var indentation = document.GetText(previousIndentationSegment);

            var text = document.GetText(previousLine);

            // Check if line is end statement
            if (text.Replace(@"\t", "").Trim() == "end")
            {
                indentation = RemoveIndentations(indentation, 1);
                document.Replace(previousIndentationSegment.Offset, previousIndentationSegment.Length, indentation, OffsetChangeMappingType.RemoveAndInsert);
            }
            else
            {
                // Get indentations after function, loops, etc.
                indentation += GetAdditionalIndentations(text);
            }

            // Get current line indentation
            var indentationSegment = TextUtilities.GetWhitespaceAfter(document, line.Offset);

            // Replace current line indentation
            document.Replace(indentationSegment.Offset, indentationSegment.Length, indentation, OffsetChangeMappingType.RemoveAndInsert);
        }

        public void IndentLines(TextDocument document, int beginLine, int endLine)
        {
            // Not implemented
        }

        private string GetAdditionalIndentations(string previousLine) => _keywordRegex.IsMatch(previousLine) ? IndentationString : string.Empty;

        private string RemoveIndentations(string indentation, int count) => _indentationRegex.Replace(indentation, "", count);
    }
}
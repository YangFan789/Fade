using System.IO;

namespace Fade.Common
{
    public class CharCacheSource : ICacheSource<TextReader, char>
    {
        public CharCacheSource(TextReader source) {
            Source = source;
        }

        public char Eof { get; } = '\uffff';

        public TextReader Source { get; }

        public char GetNextResult() {
            return unchecked((char) Source.Read());
        }
    }
}
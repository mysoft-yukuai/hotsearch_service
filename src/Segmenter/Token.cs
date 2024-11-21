namespace Segmenter
{
    public class Token(string word, int startIndex, int endIndex)
    {
        public string Word { get; set; } = word;
        public int StartIndex { get; set; } = startIndex;
        public int EndIndex { get; set; } = endIndex;

        public override string ToString()
        {
            return string.Format("[{0}, ({1}, {2})]", Word, StartIndex, EndIndex);
        }
    }
}

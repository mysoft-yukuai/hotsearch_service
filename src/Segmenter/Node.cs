namespace Segmenter
{
    public class Node(char value, Node? parent)
    {
        public char Value { get; private set; } = value;
        public Node? Parent { get; private set; } = parent;
    }
}
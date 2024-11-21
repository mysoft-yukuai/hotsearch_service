namespace Segmenter
{
    public class Pair<TKey>(TKey key, double freq)
    {
        public TKey Key { get; set; } = key;
        public double Freq { get; set; } = freq;

        public override string ToString()
        {
            return "Candidate [Key=" + Key + ", Freq=" + Freq + "]";
        }
    }
}

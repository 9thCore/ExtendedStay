namespace ExtendedStay.Util
{
    internal static class RankUtil
    {
        public static Rank GetHighestRank(string hash)
        {
            Rank normal = Persistence.GetCustomLevelRank(hash, 1f);
            Rank slow = Persistence.GetCustomLevelRank(hash, 0f);
            Rank fast = Persistence.GetCustomLevelRank(hash, 2f);

            return Max(Max(normal, slow), fast);
        }

        public static Rank Max(Rank a, Rank b)
        {
            int compA = a.ComparativeInt();
            int compB = b.ComparativeInt();

            if (compA == compB)
            {
                return a > b ? a : b;
            }

            return compA > compB ? a : b;
        }

        public static bool Unplayed(this Rank rank)
        {
            return rank == Rank.NotFinished;
        }
    }
}

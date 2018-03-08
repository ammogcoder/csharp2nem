using System;



namespace CSharp2nem.Utils
{
    public static class TimeDateUtils
    {
        public static int EpochTimeInSeconds()
        {
            var timeSpan = DateTime.UtcNow - new DateTime(2015, 03, 29, 0, 6, 25, 0);
            var secondsSinceEpoch = (int)timeSpan.TotalSeconds;
            return secondsSinceEpoch;
        }

        public static DateTime TimeStampToDateTime(int ticks)
        {
            return new DateTime(2015, 03, 29, 0, 6, 25, 0).AddSeconds(ticks);
        }
    }
}
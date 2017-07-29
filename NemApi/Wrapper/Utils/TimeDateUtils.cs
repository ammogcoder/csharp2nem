using System;



namespace CSharp2nem.Utils
{
    public static class TimeDateUtils
    {
        public static int EpochTimeInMilliSeconds()
        {
            var timeSpan = DateTime.UtcNow - new DateTime(2015, 03, 29, 0, 6, 25, 0);
            var secondsSinceEpoch = (int) timeSpan.TotalSeconds;
            return secondsSinceEpoch;
        }
    }
}
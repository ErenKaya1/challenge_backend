namespace Challenge.Core.Constants
{
    public static class CronTimes
    {
        public const string EVERY_3_SECONDS = "*/1 * * * * *";
        public const string EVERY_1_MINS = "* * * * *";
        public const string EVERY_3_MINS = "*/3 * * * *";
        public const string EVERY_5_MINS = "*/5 * * * *";
        public const string EVERY_10_MINS = "*/10 * * * *";
        public const string EVERY_15_MINS = "*/15 * * * *";
        public const string EVERY_30_MINS = "*/30 * * * *";
        public const string EVERY_1_HOURS = "0 * * * *";
        public const string EVERY_6_HOURS = "0 */6 * * *";
        public const string EVERY_DAY_AT_8 = "0 8 * * *";
        public const string EVERY_DAY_AT_12 = "0 12 * * *";
        public const string EVERY_DAY_AT_15 = "0 15 * * *";
        public const string EVERY_DAY_AT_22 = "0 22 * * *";
        public const string EVERY_DAY_AT_2 = "0 2 * * *";
        public const string EVERY_DAY_AT_4 = "0 4 * * *";
        public const string EVERY_WOMANS_DAY = "0 0 8 3 *";
    }
}
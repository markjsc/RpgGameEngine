namespace Hitcents.Interview.AwesomeRpg.Tests
{
    /// <summary>
    /// This is used to prevent having to repeat constants across multiple test classes.
    /// </summary>
    public static class Constants
    {
        public const string SampleXml = "<Element Id=\"CoolGame\">" +
        "<Element Id=\"Player\">" +
           "<Element Id=\"Level\" Value=\"1\" />" +
              "<Element Id=\"XP\" Value=\"0\" />" +
                "<Element Id=\"HP\" Value=\"50\" />" +
                "<Action Id=\"GainXP\">" +
                      "<Setter Target=\"XP\" Operation=\"+\" Value=\"10\" />" +
                "</Action>" +
                "<Trigger Target=\"XP\" Comparison=\">=\" Value=\"10\">" +
                    "<Setter Target=\"Level\" Operation=\"=\" Value=\"2\" />" +
                "</Trigger>" +
            "</Element>" +
        "</Element>";
    }
}

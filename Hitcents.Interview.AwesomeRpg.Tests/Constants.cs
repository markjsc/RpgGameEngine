﻿namespace Hitcents.Interview.AwesomeRpg.Tests
{
    /// <summary>
    /// This is used to prevent having to repeat constants across multiple test classes.
    /// </summary>
    public static class Constants
    {
        public const string SampleXml = "<Element Id=\"CoolGame\">" + 
                                        "  <Element Id=\"Player\">" + 
                                        "	<Element Id=\"Level\" Value=\"1\" />" + 
                                        "	<Element Id=\"XP\" Value=\"0\" />" + 
                                        "	<Element Id=\"HP\" Value=\"50\" />" + 
                                        "	<Element Id=\"Name\" Value=\"Bob\" />" + 
                                        "	<Element Id=\"Mode\" Value=\"\" />" + 
                                        "	<Action Id=\"GainXP\">" + 
                                        "	  <Setter Target=\"XP\" Operation=\"+\" Value=\"5\" />" + 
                                        "	</Action>" + 
                                        "	<Action Id=\"ClearHP\">" + 
                                        "	  <Setter Target=\"HP\" Operation=\"=\" Value=\"0\" />" + 
                                        "	  <Setter Target=\"XP\" Operation=\"-\" Value=\"5\" />" + 
                                        "	</Action>" + 
                                        "	<Trigger Target=\"XP\" Comparison=\">=\" Value=\"20\">" + 
                                        "	  <Setter Target=\"Level\" Operation=\"=\" Value=\"2\" />" + 
                                        "	  <Setter Target=\"Name\" Operation=\"=\" Value=\"Super Bob\" />" + 
                                        "	</Trigger>" + 
                                        "	<Trigger Target=\"Name\" Comparison=\"==\" Value=\"Super Bob\">" + 
                                        "		<Setter Target=\"Mode\" Operation=\"=\" Value=\"Superman Mode!\" />" + 
                                        "	</Trigger>" + 
                                        "	<Trigger Target=\"Mode\" Comparison=\"==\" Value=\"Superman Mode!\">" + 
                                        "		<Setter Target=\"Level\" Operation=\"=\" Value=\"3\" />" + 
                                        "	</Trigger>" + 
                                        "  </Element>" + 
                                        "</Element>";
    }
}




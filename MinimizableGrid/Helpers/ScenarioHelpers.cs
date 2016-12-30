using MinimizableGrid.Scenarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimizableGrid.Helpers
{
    public static class ScenarioHelpers
    {
        public const string ProjectTarget = "Master Minimizable Grid";

        public static List<Scenario> scenarios = new List<Scenario>
        {
            new Scenario() { Title="Simple Minimize the Grid", ClassType=typeof(MinimizeTheGrid)},
            new Scenario() { Title="Header Changed On Minimize", ClassType=typeof(HeaderChangeOnMinimize)},
            new Scenario() { Title="Minimize with visbility", ClassType=typeof(MinimizeWithVisibility)},
            
        };
    }
    public class Scenario
    {
        public string Title { get; set; }
        public Type ClassType { get; set; }
    }
}

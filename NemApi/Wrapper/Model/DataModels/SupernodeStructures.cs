namespace CSharp2nem.Model.DataModels
{
    public class SupernodeStructures
    {
        public class Location
        {
            public double latitude { get; set; }
            public double longitude { get; set; }
            public int numNodes { get; set; }
        }

        public class TestResultRequestData
        {
            public string alias { get; set; }
            public int roundFrom { get; set; }
            public int numRounds { get; set; }
        }
    }


}

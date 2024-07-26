namespace MasterKinder.Models
{
    public class MyModels
    {
        public class ResponseRate
        {
            public int Id { get; set; }
            public int TotalResponses { get; set; }
            public int UnitResponses { get; set; }
            public double TotalResponseRate { get; set; }
            public double UnitResponseRate { get; set; }
        }

        public class SurveyQuestion
        {
            public int Id { get; set; }
            public string QuestionText { get; set; }
            public string ResponseType { get; set; }
            public List<double> ResponseDistribution { get; set; }
        }

       
    }
    }

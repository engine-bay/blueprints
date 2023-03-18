namespace EngineBay.Blueprints
{
    using System;

    public class WorkbookComplexityScoreDto
    {
        public Guid WorkbookId { get; set; }

        public double TotalExpressions { get; set; }

        public double TotalVariables { get; set; }

        public double TotalLinks { get; set; }

        public double Score { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace ROFit.Model
{
    public class TrainingPlanDto
    {

        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MuscleGroup { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid UserId { get; set; }

    }
}

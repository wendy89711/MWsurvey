using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MWsurvey.Models
{
    public class V_SURVEY_LIST
    {
        public SurveyBasic basic { get; set; }
        public SurveyGroup gRoup { get; set; }
        public SurveyQuestion question { get; set; }
        public SurveyOption option { get; set; }
    }
}
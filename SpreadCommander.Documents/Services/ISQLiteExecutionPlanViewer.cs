using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Documents.Services
{
    public interface ISQLiteExecutionPlanViewer
    {
        void ShowExecutionPlan(DataTable tablePlan);
    }
}

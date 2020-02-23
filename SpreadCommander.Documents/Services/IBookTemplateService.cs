using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Documents.Services
{
    public interface IBookTemplateService
    {
        void EditBookTemplate(object dataSource, string dataMember = null);
    }
}

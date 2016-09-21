using ILSPMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Services
{
    public interface IProjectService
    {
        void Submit(Project project);
    }
}

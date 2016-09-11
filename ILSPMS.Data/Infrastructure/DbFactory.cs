using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Data
{
    public class DbFactory : Disposable, IDbFactory
    {
        ILSPMSContext dbContext;
        
        public ILSPMSContext Init()
        {
            return dbContext ?? (dbContext = new ILSPMSContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }

    }
}

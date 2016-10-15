using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Data.Infrastructure
{
  public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private MyFinanceContext dataContext = null;
        public MyFinanceContext Get()
        {
            return dataContext ?? (dataContext = new MyFinanceContext());
        }
        protected override void DisposeCore()
        {
            if (dataContext != null)
                dataContext.Dispose();
        }
    }
}

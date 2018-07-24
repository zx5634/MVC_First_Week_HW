using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace MVC_First_Week_HW.Models
{   
	public  class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
	{
        客戶資料Repository repo = RepositoryHelper.Get客戶資料Repository();
        public IQueryable<客戶銀行資訊> All()
        {
            return base.All().Where(x => x.Disable == false);
        }

        public IQueryable<客戶資料> 客戶資料()
        {
            return repo.All();
        }

        public IQueryable FindName(string keyword)
        {
            var client = this.All();
            if (!string.IsNullOrEmpty(keyword))
            {
                client = client.Where(x => x.銀行名稱.Contains(keyword));
            }
            return client;
        }

        public IQueryable<客戶銀行資訊> FindName(string keyword, IQueryable<客戶銀行資訊> items)
        {
            var client = items;
            if (!string.IsNullOrEmpty(keyword))
            {
                client = client.Where(x => x.銀行名稱.Contains(keyword));
            }
            return client;
        }

        public 客戶銀行資訊 Find(int id)
        {
            return this.All().FirstOrDefault(x => x.Id == id);
        }

        public override void Delete(客戶銀行資訊 client)
        {
            client.Disable = true;
        }

        public IQueryable<客戶銀行資訊> GetFilterItem(string keyword, string sort_col, bool isSort)
        {
            var client = this.All().Include(x => x.客戶資料);
            if (!string.IsNullOrEmpty(keyword))
            {
                client = this.FindName(keyword, client);
            }
            if (!string.IsNullOrEmpty(sort_col))
            {
                bool sort = isSort;
                if (sort == false)
                {
                    if (sort_col == "客戶名稱")
                    {
                        client = (from o in client
                                  orderby o.客戶資料.客戶名稱
                                  select o);
                    }
                    else
                        client = client.OrderByField(sort_col, true);
                }
                else
                {
                    if (sort_col == "客戶名稱")
                    {
                        client = (from o in client
                                  orderby o.客戶資料.客戶名稱 descending
                                  select o);
                    }
                    else
                        client = client.OrderByField(sort_col, false);
                }
            }
            return client;
        }
    }

	public  interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
	{

	}
}
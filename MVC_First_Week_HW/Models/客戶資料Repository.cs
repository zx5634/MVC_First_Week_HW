using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC_First_Week_HW.Models
{   
	public  class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
	{
        public IQueryable<客戶資料> All()
        {
            return base.All().Where(x => x.Disable == false);
        }

        public IQueryable FindName(string keyword)
        {
            var client = this.All();
            if (!string.IsNullOrEmpty(keyword))
            {
                client = client.Where(x => x.客戶名稱.Contains(keyword));
            }
            return client;
        }

        public IQueryable<客戶資料> FindName(string keyword, IQueryable<客戶資料> items)
        {
            var client = items;
            if (!string.IsNullOrEmpty(keyword))
            {
                client = client.Where(x => x.客戶名稱.Contains(keyword));
            }
            return client;
        }

        public 客戶資料 Find(int id)
        {
            return this.All().FirstOrDefault(x => x.Id == id);
        }

        public override void Delete(客戶資料 client)
        {
            client.Disable = true;
        }
        
        public IEnumerable<string> FindCategory()
        {
            return this.All().Where(x => x.客戶分類 != null).Select(x => x.客戶分類).Distinct();
            //return this.All().Where(x => x.客戶分類 != null);
        }

        public IQueryable GetCategory(string keyword)
        {
            var client = this.All();
            if (!string.IsNullOrEmpty(keyword))
            {
                client = client.Where(x => x.客戶分類 == keyword);
            }
            return client;
        }

        public IQueryable<客戶資料> GetCategory(string keyword, IQueryable<客戶資料> items)
        {
            var client = items;
            if (!string.IsNullOrEmpty(keyword))
            {
                client = client.Where(x => x.客戶分類 == keyword);
            }
            return client;
        }

        public IQueryable<客戶資料> GetFilterItem(string keyword, string category, string sort_col, string current_sort)
        {
            var client = this.All();
            if (!string.IsNullOrEmpty(keyword))
            {
                client = this.FindName(keyword, client);
            }
            if (!string.IsNullOrEmpty(category))
            {
                client = this.GetCategory(category, client);
            }
            if (!string.IsNullOrEmpty(sort_col))
            {
                if (sort_col != current_sort)
                {
                    client = client.OrderByField(sort_col, true);
                }
                else
                {
                    client = client.OrderByField(sort_col, false);
                }
            }
            return client;
        }
    }

	public  interface I客戶資料Repository : IRepository<客戶資料>
	{

	}
}
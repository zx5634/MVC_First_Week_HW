using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace MVC_First_Week_HW.Models
{   
	public  class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
	{
        客戶資料Repository repo = RepositoryHelper.Get客戶資料Repository();
        public IQueryable<客戶聯絡人> All()
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
                client = client.Where(x => x.姓名.Contains(keyword));
            }
            return client;
        }

        public IQueryable<客戶聯絡人> FindName(string keyword, IQueryable<客戶聯絡人> items)
        {
            var client = items;
            if (!string.IsNullOrEmpty(keyword))
            {
                client = client.Where(x => x.姓名.Contains(keyword));
            }
            return client;
        }

        public 客戶聯絡人 Find(int id)
        {
            return this.All().FirstOrDefault(x => x.Id == id);
        }

        public override void Delete(客戶聯絡人 client)
        {
            client.Disable = true;
        }

        //public bool IsContactPersonDuplicate(客戶聯絡人 client)
        //{
        //    var data = this.All().Where(x => x.客戶Id == client.客戶Id && x.Email == client.Email && x.Id != client.Id).ToList();
        //    return data.Count >= 1 ? true : false;
        //}

        public IEnumerable<string> FindPosition()
        {
            return this.All().Where(x => x.職稱 != null).Select(x => x.職稱).Distinct();
            //return this.All().Where(x => x.職稱 != null);
        }

        public IQueryable GetPosition(string keyword)
        {
            var client = this.All();
            if (!string.IsNullOrEmpty(keyword))
            {
                client = client.Where(x => x.職稱 == keyword);
            }
            return client;
        }

        public IQueryable<客戶聯絡人> GetPosition(string keyword, IQueryable<客戶聯絡人> items)
        {
            var client = items;
            if (!string.IsNullOrEmpty(keyword))
            {
                client = client.Where(x => x.職稱 == keyword);
            }
            return client;
        }

        public IQueryable<客戶聯絡人> GetFilterItem(string keyword, string position, string sort_col, bool isSort)
        {
            var client = this.All().Include(x => x.客戶資料);

            if (!string.IsNullOrEmpty(keyword))
            {
                client = this.FindName(keyword, client);
            }
            if (!string.IsNullOrEmpty(position))
            {
                client = this.GetPosition(position, client);
            }
            if (!string.IsNullOrEmpty(sort_col))
            {
                if (isSort == false)
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
            else
            {
                client = client.OrderBy(c => c.Id);
            }
            return client;
        }
    }

    public  interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
	{

	}
}
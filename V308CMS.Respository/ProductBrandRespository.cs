﻿using System.Collections.Generic;
using System.Linq;
using V308CMS.Data;

namespace V308CMS.Respository
{
    public interface IProductBrandRespository
    {
        string Insert(string name,int categoryId, string image,byte status);
        string Update(int id, string name, int categoryId, string image, byte status);
    }

    public class ProductBrandRespository : IBaseRespository<Brand>, IProductBrandRespository
    {

      

        public ProductBrandRespository()
        {
           
        }


        public Brand Find(int id)
        {
            using (var entities = new V308CMSEntities())
            {
                return (from brand in entities.Brand
                        where brand.id == id
                        select brand).FirstOrDefault();
            }
            
        }

        public string Delete(int id)
        {
            using (var entities = new V308CMSEntities())
            {
                var brandItem = (from brand in entities.Brand
                                 where brand.id == id
                                 select brand).FirstOrDefault();
                if (brandItem != null)
                {
                    entities.Brand.Remove(brandItem);
                    entities.SaveChanges();
                    return "ok";
                }
                return "not_exists";
            }
            
        }

        public string Update(Brand data)
        {
            using (var entities = new V308CMSEntities())
            {
                var brandItem = (from brand in entities.Brand
                                 where brand.id == data.id
                                 select brand).FirstOrDefault();
                if (brandItem != null)
                {
                    brandItem.category_default = data.category_default;
                    brandItem.name = data.name;
                    brandItem.image = data.image;
                    brandItem.status = data.status;
                    entities.SaveChanges();
                    return "ok";
                }
                return "not_exists";
            }
           
        }

        public string Insert(Brand data)
        {
            using (var entities = new V308CMSEntities())
            {
                var brandItem = (from brand in entities.Brand
                                 where brand.name == data.name
                                 select brand).FirstOrDefault();
                if (brandItem == null)
                {
                    entities.Brand.Add(data);
                    entities.SaveChanges();
                    return "ok";
                }
                return "exists";
            }
           
        }

        public List<Brand> GetAll()
        {
            using (var entities = new V308CMSEntities())
            {
                return (from brand in entities.Brand
                        orderby brand.id descending
                        select brand
                ).ToList();
            }
           
        }


        public List<Brand> GetListWithProductType(int page = 1, int pageSize = 10)
        {
            using (var entities = new V308CMSEntities())
            {
                return (from brand in entities.Brand.Include("ProductType")
                orderby brand.id descending
                select brand
                   ).ToList();
            }
           

        }

        public string Update(int id, string name, int categoryId, string image, byte status)
        {
            using (var entities = new V308CMSEntities())
            {
                var brandItem = (from brand in entities.Brand
                                 where brand.id == id
                                 select brand).FirstOrDefault();
                if (brandItem != null)
                {
                    brandItem.name = name;
                    brandItem.category_default = categoryId;
                    brandItem.image = image;
                    brandItem.status = status;
                    entities.SaveChanges();
                    return "ok";
                }
                return "not_exists";

            }
           
        }

        public string Insert(string name, int categoryId, string image, byte status)
        {
            using (var entities = new V308CMSEntities())
            {
                var brandItem = (from brand in entities.Brand
                                 where brand.name == name
                                 select brand).FirstOrDefault();
                if (brandItem == null)
                {
                    var brand = new Brand
                    {
                        name = name,
                        category_default = categoryId,
                        image = image,
                        status = status
                    };
                    entities.Brand.Add(brand);
                    entities.SaveChanges();
                    return "ok";
                }
                return "exists";
            }
           
        }

        public List<Brand> GetAll(bool state = true)
        {
            using (var entities = new V308CMSEntities())
            {
                return state ? (from brand in entities.Brand
                                orderby brand.id descending
                                where brand.status == 1
                                select brand).ToList() :
                       (from brand in entities.Brand
                        orderby brand.id descending
                        select brand).ToList();

            }
           
        }
    }
}

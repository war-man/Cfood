﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using V308CMS.Data.Models;
using V308CMS.Common;

namespace V308CMS.Data
{
    public interface VisisterInterface
    {
        void UpdateView(int uid = 0, int affiliate_id = 0);
    }

    public class VisisterRepository : VisisterInterface
    {

        public Visister CurrentUser() {
            var visister = new Visister {
                ip_address = HttpContext.Current.Request.UserHostAddress,
                useragent = HttpContext.Current.Request.UserAgent,
                platform = HttpContext.Current.Request.Browser.Platform,
                browser = HttpContext.Current.Request.Browser.Id,
                browser_type = HttpContext.Current.Request.Browser.Type,
                browser_version = HttpContext.Current.Request.Browser.Version,
                host = HttpContext.Current.Request.Url.Host,
                timestamp = DateTime.Now

            };

            //visister.ip_address = HttpContext.Current.Request.UserHostAddress;
            //visister.ip_address = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            //visister.useragent = System.Web.HttpContext.Current.Request.UserAgent;
            //visister.platform = System.Web.HttpContext.Current.Request.Browser.Platform;
            //visister.browser = System.Web.HttpContext.Current.Request.Browser.Id;

            //visister.timestamp = DateTime.Now;
            //visister.host = System.Web.HttpContext.Current.Request.Url.Host;
            return visister;
        }
        static int MinuteLimitUpdateView = 3;
        public void UpdateView(int uid = 0, int affiliate_id=0)
        {
            try
            {
                using (var entiry = new V308CMSEntities()) {
                    var visister = CurrentUser();
                    visister.uid = uid;
                    int visister_id = visister.id;
                    var item = entiry.VisisterTbl.Where(v => v.ip_address == visister.ip_address && v.useragent == visister.useragent).FirstOrDefault();
                    if (item == null)
                    {
                        visister.affiliate_id = affiliate_id;
                        entiry.VisisterTbl.Add(visister);
                        entiry.SaveChanges();
                    }
                    else {
                        visister_id = item.id;
                    }

                    UpdateViewTime(visister_id);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                //throw;
            }
        }

        public void UpdateViewTime(int visister_id) {
            using (var entiry = new V308CMSEntities())
            {
                DateTime now = DateTime.Now;
                var view_time = entiry.VisisterTimeTbl
                            .Where(t => t.updated.Year == now.Year && t.updated.Month == now.Month && t.updated.Day == now.Day)
                            .Where(t => t.visister_id == visister_id)
                            .OrderByDescending(t=>t.updated)
                            .Select(t => t).FirstOrDefault();
                if (view_time != null)
                {
                    long time_diff = now.toUnixTime() - view_time.updated.toUnixTime();
                    if (time_diff > 60 * MinuteLimitUpdateView)
                    {
                        view_time.count++;
                        view_time.updated = DateTime.Now;
                        entiry.SaveChanges();
                    }
                }
                else
                {
                    AddViewTime(visister_id);
                }
            }
        }

        public void AddViewTime(int visister_id)
        {
            using (var entiry = new V308CMSEntities())
            {
                var view_new = new VisisterTime
                {
                    updated = DateTime.Now,
                    created = DateTime.Now,
                    visister_id = visister_id,
                    count = 1
                };
                entiry.VisisterTimeTbl.Add(view_new);
                entiry.SaveChanges();
            }
        }

        public Boolean isFirstTime() {
            Boolean first = false;
            var visister = CurrentUser();
            using (var entiry = new V308CMSEntities())
            {
                var item = entiry.VisisterTbl.Where(v => v.ip_address == visister.ip_address && v.platform == visister.platform && v.browser == visister.browser).FirstOrDefault();
            }
                


            return first;
        }

        public int CurrentVisisterId() {
            int id = 0;
            using (var entiry = new V308CMSEntities())
            {
                var visister = CurrentUser();
                var item = entiry.VisisterTbl.Where(v => v.ip_address == visister.ip_address && v.useragent == visister.useragent).FirstOrDefault();
                if (item != null) {
                    id = item.id;
                }
            }
                return id;
        }
    }
}

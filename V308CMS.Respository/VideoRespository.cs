﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using V308CMS.Data;
using V308CMS.Data.Enum;
using V308CMS.Data.Models;

namespace V308CMS.Respository
{
    public interface IVideoRespository
    {
        Task<List<Video>> GetListHomeVideo(byte position = (byte) VideoPosition.Home, int limit = 3);
        Video Find(int id);
        Task<List<Video>> GetListRelatived(int id, int limit =10);
        List<Video> GetListVideo(int page = 1, int pageSize = 10);

    }
    public class VideoRespository: IVideoRespository
    {
        public async  Task<List<Video>> GetListHomeVideo(byte position = (byte)VideoPosition.Home,int limit =3)
        {
            using (var entities = new V308CMSEntities())
            {
                var listVideo = await entities.Video.Where(video => video.Status == (byte) StateEnum.Active && video.Position == position)
                        .OrderBy(video=>video.Order)
                        .Take(limit).ToListAsync();
                if (listVideo.Count < 3)
                {
                    listVideo = await entities.Video.Where(video => video.Status == (byte)StateEnum.Active)
                       .OrderBy(video => video.UpdatedAt)
                       .Take(limit).ToListAsync();
                }
                return listVideo;
            }

        }

        public Video Find(int id)
        {
            using (var entities = new V308CMSEntities())
            {
                return entities.Video.FirstOrDefault(video => video.Id == id);
            }
        }

        public async Task<List<Video>> GetListRelatived(int id, int limit = 10)
        {
            using (var entities = new V308CMSEntities())
            {
                return await entities.Video.Where(video => video.Id != id && video.Status == (byte)StateEnum.Active).OrderBy(video => video.UpdatedAt)
                       .Take(limit).ToListAsync();
            }
        }

        public List<Video> GetListVideo(int page = 1, int pageSize = 10)
        {
            using (var entities = new V308CMSEntities())
            {
                return entities.Video.Where(video => video.Status == (byte)StateEnum.Active).OrderBy(video => video.UpdatedAt)
                     .Skip((page-1)*pageSize).Take(pageSize).ToList();
            }
           
        }
    }

}
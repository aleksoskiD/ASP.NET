using HotelWeb.DAL.Context;
using HotelWeb.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelWeb.DAL.Repositories
{
    public interface IRoomsRepository
    {
        Room GetRoom(int id);
        IList<Room> GetRooms();
    }

    public class RoomsRepository : IRoomsRepository
    {
        private HotelContext context;
        public RoomsRepository(HotelContext context)
        {
            this.context = context;         
        }



        public Room GetRoom(int id)
        {
            return context.Rooms.Where(x => x.Id == id).FirstOrDefault();
        }

        public IList<Room> GetRooms()
        {
            return context.Rooms.ToList();
        }




    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelApp.Domain.Entities;
using HotelApp.Domain.Interfaces;
using HotelApp.Models;

namespace HotelApp.Repository
{
    public class AdminRepository : IAdminRepository
    {
       // private HotelContext db = new HotelContext();
        private ApplicationDbContext appDb = new ApplicationDbContext();

        // for floor
        public List<Floor> GetAllFloors()
        {
            return appDb.Floors.ToList();
        }

        public Floor GetFloorById(int id)
        {
            return appDb.Floors.FirstOrDefault(x => x.ID == id);
        }

        public bool CreateFloor(Floor floor)
        {
            var neso = GetAllFloors().Select(x => x.FloorNo).ToList();

            if (floor != null && floor.FloorNo != 0)
            {
                for (int i = 0; i < neso.Count; i++)
                {
                    if (neso[i] == floor.FloorNo)
                    {
                        return false;
                    }
                }
                appDb.Floors.Add(floor);
                appDb.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeactivateFloor(int floorId)
        {
            var deactivatedFloor = GetFloorById(floorId);
            if (deactivatedFloor != null)
            {
                // deactivate floor
                deactivatedFloor.IsActive = false;
                appDb.Entry(deactivatedFloor).CurrentValues.SetValues(deactivatedFloor);
                appDb.SaveChanges();

                // when floor is deactivated - rooms on that floor are deactivated too
                var numRooms = GetAllRooms(floorId);
                for (int i = 0; i < numRooms.Count; i++)
                {
                    // if room is reserved you can deactivate that room
                    if(numRooms[i].IsReserved == true)
                    {
                        continue;
                    }
                    else
                    {
                        numRooms[i].IsActive = false;
                        appDb.SaveChanges();
                    }
                }
                return true;
            }
            return false;
        }

        public bool ActivateFloor(int floorId)
        {
            var activateFloor = GetFloorById(floorId);
            if (activateFloor != null)
            {
                activateFloor.IsActive = true;
                appDb.Entry(activateFloor).CurrentValues.SetValues(activateFloor);
                appDb.SaveChanges();

                // when floor is activated - rooms on that floor are activated too
                var numRooms = GetAllRooms(floorId);
                for (int i = 0; i < numRooms.Count; i++)
                {
                    numRooms[i].IsActive = true;
                    appDb.SaveChanges();
                }
                return true;
            }
            return true;
        }

        public bool UpdateFloor(Floor floor)
        {
            var dbFloor = GetFloorById(floor.ID);
            if(dbFloor != null && dbFloor.NumberOfRooms <= floor.NumberOfRooms)
            {
                appDb.Entry(dbFloor).CurrentValues.SetValues(floor);
                appDb.SaveChanges();
                return true;
            }
            return false;
        }


        /// for room
        public List<Room> GetAllRooms()
        {
            return appDb.Rooms.ToList();
        }

        public List<Room> GetAllRooms(int floorId)
        {
            var room = appDb.Rooms.Where(x => x.FloorId == floorId).ToList();
            return room;
        }

        public Room GetRoomById(int roomId)
        {
            return appDb.Rooms.FirstOrDefault(x => x.ID == roomId);
        }

        public bool CreateRoom(Room room, int numOfRooms)
        {
            if (room != null)
            {
                // all created rooms by default are not reserved
                room.IsReserved = false;

                int allRooms = GetFloorById(room.FloorId).NumberOfRooms;
                int adededRooms = GetAllRooms(room.FloorId).Select(x => x.ID).Count();
                int avaliableRooms = allRooms - adededRooms;

                if (avaliableRooms >= numOfRooms)
                {
                    for (int i = 0; i < numOfRooms; i++)
                    {
                        appDb.Rooms.Add(room);
                        appDb.SaveChanges();
                    }
                    return true;
                }
            }
            return false;
        }

        public bool UpdateRoom(Room room)
        {
            var dbRoom = appDb.Rooms.FirstOrDefault(x => x.ID == room.ID);
            if (dbRoom != null)
            {
                appDb.Entry(dbRoom).CurrentValues.SetValues(room);
                appDb.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeactivateRoom(int roomId)
        {
            var deactivateRoom = appDb.Rooms.FirstOrDefault(x => x.ID == roomId);
            if (deactivateRoom != null && deactivateRoom.IsReserved != true)
            {
                deactivateRoom.IsActive = false;
                appDb.Entry(deactivateRoom).CurrentValues.SetValues(deactivateRoom);
                appDb.SaveChanges();
                return true;
            }
            return false;
        }

        public bool ActivateRoom(int roomId)
        {
            var activateRoom = appDb.Rooms.FirstOrDefault(x => x.ID == roomId);
            if (activateRoom != null)
            {
                activateRoom.IsActive = true;
                appDb.Entry(activateRoom).CurrentValues.SetValues(activateRoom);
                appDb.SaveChanges();
                return true;
            }
            return false;
        }


        // for guests
        public List<ApplicationUser> GetAllGuests()
        {
            var guests = appDb.Users.ToList();
            return guests;
        }
    }
}

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
        private HotelContext db = new HotelContext();
        private ApplicationDbContext appDb = new ApplicationDbContext();

        // for floor
        public List<Floor> GetAllFloors()
        {
            return db.Floors.ToList();
        }

        public Floor GetFloorById(int id)
        {
            return db.Floors.FirstOrDefault(x => x.ID == id);
        }

        public bool CreateFloor(Floor floor)
        {
            var neso = GetAllFloors().Select(x => x.FloorNo).ToList();

            if (floor != null)
            {
                for (int i = 0; i < neso.Count; i++)
                {
                    if (neso[i] == floor.FloorNo)
                    {
                        return false;
                    }
                }
                db.Floors.Add(floor);
                db.SaveChanges();
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
                db.Entry(deactivatedFloor).CurrentValues.SetValues(deactivatedFloor);
                db.SaveChanges();

                // when floor is deactivated - rooms on that floor are deactivated too
                var numRooms = GetAllRooms(floorId);
                for (int i = 0; i < numRooms.Count; i++)
                {
                    numRooms[i].IsActive = false;
                    db.SaveChanges();
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
                db.Entry(activateFloor).CurrentValues.SetValues(activateFloor);
                db.SaveChanges();

                // when floor is activated - rooms on that floor are activated too
                var numRooms = GetAllRooms(floorId);
                for (int i = 0; i < numRooms.Count; i++)
                {
                    numRooms[i].IsActive = true;
                    db.SaveChanges();
                }
                return true;
            }
            return true;
        }


        /// for room
        public List<Room> GetAllRooms()
        {
            return db.Rooms.ToList();
        }

        public List<Room> GetAllRooms(int floorId)
        {
            var room = db.Rooms.Where(x => x.FloorId == floorId).ToList();
            return room;
        }

        public Room GetRoomById(int roomId)
        {
            return db.Rooms.FirstOrDefault(x => x.ID == roomId);
        }

        public bool CreateRoom(Room room, int numOfRooms)
        {
            if (room != null)
            {
                int allRooms = GetFloorById(room.FloorId).NumberOfRooms;
                int submitedRooms = GetAllRooms(room.FloorId).Select(x => x.ID).Count();
                int avaliableRooms = allRooms - submitedRooms;

                if (avaliableRooms >= numOfRooms)
                {
                    for (int i = 0; i < numOfRooms; i++)
                    {
                        db.Rooms.Add(room);
                        db.SaveChanges();
                    }
                    return true;
                }
            }
            return false;
        }

        public bool UpdateRoom(Room room)
        {
            var updateRoom = db.Rooms.FirstOrDefault(x => x.ID == room.ID);
            if (updateRoom != null)
            {
                db.Entry(updateRoom).CurrentValues.SetValues(room);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeactivateRoom(int roomId)
        {
            var deactivateRoom = db.Rooms.FirstOrDefault(x => x.ID == roomId);
            if (deactivateRoom != null)
            {
                deactivateRoom.IsActive = false;
                db.Entry(deactivateRoom).CurrentValues.SetValues(deactivateRoom);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool ActivateRoom(int roomId)
        {
            var activateRoom = db.Rooms.FirstOrDefault(x => x.ID == roomId);
            if (activateRoom != null)
            {
                activateRoom.IsActive = true;
                db.Entry(activateRoom).CurrentValues.SetValues(activateRoom);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        //guests
        public List<ApplicationUser> GetAllGuests()
        {
            var guests = appDb.Users.ToList();

            return guests;
        }
    }
}

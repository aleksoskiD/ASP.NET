using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelApp.Domain.Entities;
using HotelApp.Domain.Interfaces;
using HotelApp.Models;
using System.Data.Entity;
using HotelApp.Domain.Entities.ViewModels;

namespace HotelApp.Repository
{
    public class AdminRepository : IAdminRepository
    {
       // private HotelContext db = new HotelContext();
        private ApplicationDbContext appDb = new ApplicationDbContext();

        // for floor
        public List<FloorViewModel> GetAllFloors()
        {
            List<Floor> dbFloors = appDb.Floors.OrderBy(x => x.FloorNo).ToList();

            List<FloorViewModel> fvms = new List<FloorViewModel>();
            FloorViewModel floor = new FloorViewModel();
            foreach (var item in dbFloors)
            {
                floor = FloorToViewModel(item);
                fvms.Add(floor);
                floor = null;
            }

            return fvms;
        }

        public Floor GetFloorById(int id)
        {
            return appDb.Floors.FirstOrDefault(x => x.ID == id);
        }

        private FloorViewModel GetFloor(Floor floor)
        {
            Floor fl = appDb.Floors.First(x => x.FloorNo == floor.FloorNo && floor.IsActive == floor.IsActive && floor.NumberOfRooms == floor.NumberOfRooms);
            FloorViewModel fvm = FloorToViewModel(fl);
            return fvm;
        }

        public FloorViewModel CreateFloor(Floor floor)
        {
            Floor lastFloor;
            try
            {
                lastFloor = appDb.Floors.OrderByDescending(x => x.FloorNo).FirstOrDefault();
                floor.FloorNo = lastFloor.FloorNo + 1;
            }
            catch
            {
                floor.FloorNo = 1;
            }

            appDb.Floors.Add(floor);
            appDb.SaveChanges();

            FloorViewModel f = GetFloor(floor);
            return f;
        }

        public FloorViewModel DeactivateFloor(int floorId)
        {
            var dbFloor = GetFloorById(floorId);
            if (dbFloor != null)
            {
                // deactivate floor
                var deactivatedFloor = dbFloor;
                deactivatedFloor.IsActive = false;
                appDb.Entry(dbFloor).CurrentValues.SetValues(deactivatedFloor);

                // when floor is deactivated - rooms on that floor are deactivated too
                var numRooms = GetAllRooms(floorId);
                foreach (var room in numRooms)
                {
                    // if room is reserved you can deactivate that room
                    if(room.IsReserved == true)
                    {
                        continue;
                    }
                    else
                    {
                        var chRoom = room;
                        chRoom.IsActive = false;
                        appDb.Entry(room).CurrentValues.SetValues(chRoom);
                    }
                }
                appDb.SaveChanges();
                FloorViewModel f = GetFloor(dbFloor);
                return f;
            }
            return null;
        }

        public FloorViewModel ActivateFloor(int floorId)
        {
            var dbFloor = GetFloorById(floorId);
            if (dbFloor != null)
            {
                var activateFloor = dbFloor;
                activateFloor.IsActive = true;
                appDb.Entry(activateFloor).CurrentValues.SetValues(activateFloor);

                // when floor is activated - rooms on that floor are activated too
                var numRooms = GetAllRooms(floorId);
                foreach (var room in numRooms)
                {
                    var chRoom = room;
                    chRoom.IsActive = true;
                    appDb.Entry(room).CurrentValues.SetValues(chRoom);
                }
                appDb.SaveChanges();
                FloorViewModel f = GetFloor(dbFloor);
                return f;
            }
            return null;
        }

        public FloorViewModel UpdateFloor(Floor floor)
        {
            var dbFloor = GetFloorById(floor.ID);
            if(dbFloor != null && dbFloor.NumberOfRooms <= floor.NumberOfRooms)
            {
                appDb.Entry(dbFloor).CurrentValues.SetValues(floor);
                appDb.SaveChanges();

                FloorViewModel f = GetFloor(floor);
                return f;
            }
            return null;
        }

        private FloorViewModel FloorToViewModel(Floor fls)
        {
            FloorViewModel floor = new FloorViewModel();
            List<Room> rooms = GetAllRooms().Where(x => x.FloorId == fls.ID).ToList();
            floor.ID = fls.ID;
            floor.FloorNo = fls.FloorNo;
            floor.TotalRooms = fls.NumberOfRooms;
            floor.IsActive = fls.IsActive;
            floor.Entered = rooms.Count();
            floor.ForEnter = fls.NumberOfRooms - rooms.Count();
            floor.Free = rooms.Where(c => c.IsReserved == false).Count();
            floor.Reserved = rooms.Where(c => c.IsReserved == true).Count();

            return floor;
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

        public List<Room> AllFreeRooms()
        {
            var freeRooms = appDb.Rooms.Where(x => x.IsReserved == false).ToList();
            return freeRooms;
        }
        
        public List<Room> AllReservedRooms()
        {
            var reservedRooms = appDb.Rooms.Where(x => x.IsReserved == true).ToList();
            return reservedRooms;
        }

        public List<Room> AllActiveRooms()
        {
            var activeRooms = appDb.Rooms.Where(x => x.IsActive == true && x.IsReserved == false).ToList();
            return activeRooms;
        }

        public List<Room> AllInactiveRooms()
        {
            var inactiveRooms = appDb.Rooms.Where(x => x.IsActive == false).ToList();
            return inactiveRooms;
        }

        public Room GetRoomById(int roomId)
        {
            return appDb.Rooms.FirstOrDefault(x => x.ID == roomId);
        }

        public Room GetRoomByType(RoomType type)
        {
            var room = appDb.Rooms.Where(x => x.RoomType == type && x.IsReserved == false && x.IsActive == true).FirstOrDefault();
            return room;
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

        public bool ReserveRoom(int roomId)
        {
            var room = GetRoomById(roomId);
            if (room != null && room.IsReserved == false)
            {
                room.IsReserved = true;
                appDb.Entry(room).State = EntityState.Modified;
                appDb.SaveChanges();
                return true;
            }

            return false;
        }

        public bool CancelRoom(int roomId)
        {
            var room = GetRoomById(roomId);
            if (room != null)
            {
                room.IsReserved = false;
                appDb.Entry(room).State = EntityState.Modified;
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
            var role = appDb.Roles.FirstOrDefault(x => x.Name == "Admin");
            var t = role.Id;
            var guests = appDb.Users.ToList().Where(x => x.Roles.First().RoleId != t).ToList();
            return guests;
        }

        public ApplicationUser GetGuestById(string id)
        {
            return appDb.Users.FirstOrDefault(x => x.Id == id);
        }
    }
}

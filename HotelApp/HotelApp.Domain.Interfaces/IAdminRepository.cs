using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelApp.Domain.Entities;
using HotelApp.Models;

namespace HotelApp.Domain.Interfaces
{
    public interface IAdminRepository
    {
        // for floor
        List<Floor> GetAllFloors();
        Floor GetFloorById(int id);
        bool CreateFloor(Floor floor);
        bool ActivateFloor(int floorId);
        bool DeactivateFloor(int floorId);

        //for room
        List<Room> GetAllRooms();
        List<Room> GetAllRooms(int floorId);
        Room GetRoomById(int id);
        bool CreateRoom(Room room, int numOfRooms);
        bool UpdateRoom(Room room);
        bool ActivateRoom(int roomId);
        bool DeactivateRoom(int roomId);

        // for guests
        List<ApplicationUser> GetAllGuests();
    }
}

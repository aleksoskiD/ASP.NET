using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelApp.Domain.Entities;
using HotelApp.Models;
using HotelApp.Domain.Entities.ViewModels;

namespace HotelApp.Domain.Interfaces
{
    public interface IAdminRepository
    {
        // for floor
        List<FloorViewModel> GetAllFloors();
        Floor GetFloorById(int id);
        FloorViewModel CreateFloor(Floor floor);
        FloorViewModel UpdateFloor(Floor floor);
        FloorViewModel ActivateFloor(int floorId);
        FloorViewModel DeactivateFloor(int floorId);

        //for room
        List<Room> GetAllRooms();
        List<Room> GetAllRooms(int floorId);
        List<Room> AllFreeRooms();
        List<Room> AllReservedRooms();
        List<Room> AllActiveRooms();
        List<Room> AllInactiveRooms();
        Room GetRoomById(int id);
        Room GetRoomByType(RoomType type);
        bool CreateRoom(Room room, int numOfRooms);
        bool UpdateRoom(Room room);
        bool ActivateRoom(int roomId);
        bool ReserveRoom(int roomId);
        bool DeactivateRoom(int roomId);

        // for guests
        List<ApplicationUser> GetAllGuests();
        ApplicationUser GetGuestById(string id);
    }
}

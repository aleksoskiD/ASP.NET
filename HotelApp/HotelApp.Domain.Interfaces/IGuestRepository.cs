using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Domain.Interfaces
{
    public interface IGuestRepository
    {
        bool MakeRezervation(int roomId);
    }
}

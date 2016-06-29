using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Interfaces
{
    public interface ITaskRepository
    {
        List<Task> GetAll();
        Task GetById(int id);
        bool Create(Task task);
        bool Update(Task task);
        bool Delete(int id);

        List<TaskComment> GetComments(int taskId);
        string GetAssignee(int taskId);
    }
}

using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;

namespace Repository.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private Database db = new Database();

        public bool Create(Task task)
        {
            var role = db.Users.FirstOrDefault(x => x.ID == task.UserId).Role;
            if(task != null && role != "Admin")
            {
                task.DateCreated = DateTime.Now;
                db.Tasks.Add(task);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Task> GetAll()
        {
            return db.Tasks.ToList();
        }

        public string GetAssignee(int taskId)
        {
            throw new NotImplementedException();
        }

        public Task GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<TaskComment> GetComments(int taskId)
        {
            throw new NotImplementedException();
        }

        public bool Update(Task task)
        {
            throw new NotImplementedException();
        }
    }
}

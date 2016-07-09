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
            var dbTask = GetById(id);
            var newTask = dbTask;
            if(dbTask != null)
            {
                newTask.IsActive = false;
                db.Entry(dbTask).CurrentValues.SetValues(newTask);
                db.SaveChanges();
                return true;
            }
            return false;
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
            return db.Tasks.FirstOrDefault(x=>x.ID == id);
        }

        public List<TaskComment> GetComments(int taskId)
        {
            throw new NotImplementedException();
        }

        public bool Update(Task task)
        {
            var dbTask = GetById(task.ID);
            if(dbTask != null)
            {
                db.Entry(dbTask).CurrentValues.SetValues(task);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdateTaskStatus(int id, string status) {
            var dbTask = GetById(id);
            var newTask = dbTask;
            TaskStatus taskStatus;
          
            switch (status)
            {
                case "ToDo":
                    {
                        taskStatus = TaskStatus.ToDo;
                        if(dbTask.Status.ToString() == "InProgress" || dbTask.Status.ToString() == "InTesting")
                        {
                            newTask.Status = taskStatus;
                            db.Entry(dbTask).CurrentValues.SetValues(newTask);
                            db.SaveChanges();
                            return true;
                        }
                        return false;
                        //break;
                    }
                case "InProgress":
                    {
                        taskStatus = TaskStatus.InProgress;
                        if (dbTask.Status.ToString() == "ToDo")
                        {
                            newTask.Status = taskStatus;
                            db.Entry(dbTask).CurrentValues.SetValues(newTask);
                            db.SaveChanges();
                            return true;
                        }
                        return false;
                        //break;
                    }
                case "InTesting":
                    {
                        taskStatus = TaskStatus.InTesting;
                        if (dbTask.Status.ToString() == "InProgress" || dbTask.Status.ToString() == "InTesting" || dbTask.Status.ToString() == "ToDo")
                        {
                            newTask.Status = taskStatus;
                            db.Entry(dbTask).CurrentValues.SetValues(newTask);
                            db.SaveChanges();
                            return true;
                        }
                        return false;
                        //break;
                    }
                case "Done":
                    {
                        taskStatus = TaskStatus.Done;
                        if (dbTask.Status.ToString() == "InProgress" || dbTask.Status.ToString() == "InTesting")
                        {
                            newTask.Status = taskStatus;
                            db.Entry(dbTask).CurrentValues.SetValues(newTask);
                            db.SaveChanges();
                            return true;
                        }
                        return false;
                        //break;
                    }
                default:
                    break;
            }

            return false;
        }
    }
}

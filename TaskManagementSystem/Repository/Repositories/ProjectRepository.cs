using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Repository.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        Database db = new Database();

        public bool Create(Project project)
        {
           if(project != null)
            {
                project.DateCreated = DateTime.Now;
                db.Projects.Add(project);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Project> GetAll()
        {
            return db.Projects.ToList();
        }

        public Project GetById(int id)
        {
            return db.Projects.FirstOrDefault(x => x.ID == id);
        }

        public bool Update(Project project)
        {
            throw new NotImplementedException();
        }
    }
}

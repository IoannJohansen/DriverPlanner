using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DriverPlanner.Data;
using DriverPlanner.Entities;

namespace DriverPlanner.Data
{
	public class DriverPlanner_DB : DbContext
	{
		public DriverPlanner_DB() : base("DefaultConnection")
		{
			Database.SetInitializer(new DbInitializer());
		}

		public virtual DbSet<Categories> Categories { get; set; }
		public virtual DbSet<ClassInterval> Intervals { get; set; }
		public virtual DbSet<Gender> Genders { get; set; }
		public virtual DbSet<Cars> Cars { get; set; }
		public virtual DbSet<User> Users { get; set; }
		public virtual DbSet<Instructor> Instructors { get; set; }
		public virtual DbSet<Admin> Admins { get; set; }
		public virtual DbSet<FeedBacks> Feedbacks { get; set; }
		public virtual DbSet<TimeTable> Timetable { get; set; }
	}
}

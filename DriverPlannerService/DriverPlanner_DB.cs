using DriverPlannerService.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverPlannerService
{
	public class DriverPlanner_DB : DbContext
	{
		public DriverPlanner_DB() : base("DriverPlanner_DB")
		{
			Database.SetInitializer(new DBInitializer());
		}

		public virtual DbSet<Categories> CATEGORIES { get; set; }
		public virtual DbSet<ClassInterval> INTERVALS { get; set; }
		public virtual DbSet<Gender> GENDERS { get; set; }
		public virtual DbSet<Cars> CARS { get; set; }
		public virtual DbSet<User> USERS { get; set; }
		public virtual DbSet<Instructor> INSTRUCTORS { get; set; }
		public virtual DbSet<Admin> ADMINS { get; set; }
		public virtual DbSet<FeedBacks> FEEDBACKS { get; set; }
		public virtual DbSet<TimeTable> TIMETABLE { get; set; }

	}
}

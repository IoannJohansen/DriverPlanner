using DriverPlanner.Infrastructure.ImageConverter;
using DriverPlannerService.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverPlannerService
{
	public class DbInitializer : CreateDatabaseIfNotExists<DriverPlanner_DB>
	{
		protected override void Seed(DriverPlanner_DB context)
		{

			#region FillGender
			var male = new Gender("Мужской", "Мужской пол");
			var female = new Gender("Женский", "Женский пол");
			var genders = new List<Gender>();
			genders.Add(male);
			genders.Add(female);
			genders.ForEach(t => context.Genders.Add(t));
			context.SaveChanges();
			#endregion

			#region FillCategory
			var a = new Categories("A", "Мотоциклы");
			var am = new Categories("AM", "Мопеды");
			var b = new Categories("B", "Автомобили с массой до 3,5 тонн и менее");
			var c = new Categories("C", "Автомобили с массой 3,5 тонны и более");

			var categories = new List<Categories>();
			categories.Add(a);
			categories.Add(am);
			categories.Add(b);
			categories.Add(c);
			categories.ForEach(t => context.Categories.Add(t));
			context.SaveChanges();
			#endregion

			#region FillIntervals
			var first = new ClassInterval(1, "8:00-9:30");
			var second = new ClassInterval(2, "10:00-11:30");
			var third = new ClassInterval(3, "12:00-13:30");
			var fourth = new ClassInterval(4, "14:00-15:30");
			var fifth = new ClassInterval(5, "16:00-17:30");

			var intervals = new List<ClassInterval>();
			intervals.Add(first);
			intervals.Add(second);
			intervals.Add(third);
			intervals.Add(fourth);
			intervals.Add(fifth);
			intervals.ForEach(t => context.Intervals.Add(t));
			context.SaveChanges();
			#endregion

			#region fillCars

			#region ImageConverts
			//var vwImg = @"D:\Desktop\Labs\DriverPlanner\DriverPlanner\Resources\Images\vw.png";
			//byte[] vwBytes = ImageConverter.ImageToBytes(vwImg);

			//var ladaImg = @"D:\Desktop\Labs\DriverPlanner\DriverPlanner\Resources\Images\lada.png";
			//byte[] ladaBytes = ImageConverter.ImageToBytes(ladaImg);

			//var hondaImg = @"D:\Desktop\Labs\DriverPlanner\DriverPlanner\Resources\Images\honda.png";
			//byte[] hondaBytes = ImageConverter.ImageToBytes(hondaImg);
			#endregion

			var vw = new Cars("VW POLO SEDAN", 2002, b);
			var lada = new Cars("LADA GRANTA SEDAN", 2010, b);
			var honda = new Cars("HONDA CBR RR", 2004, a);

			var cars = new List<Cars>();
			cars.Add(vw);
			cars.Add(lada);
			cars.Add(honda);
			cars.ForEach(t => context.Cars.Add(t));
			context.SaveChanges();
			#endregion

			#region Admin init
			var admin = new Admin();
			admin.AdminEmail = "admin@mail.ru";
			admin.HashPass = "GZ0s3/R2NXrmXDtikekaRQ==";
			admin.Login = "admino";
			admin.Name = "Johny";
			context.Admins.Add(admin);
			context.SaveChanges();

			#endregion

			Console.WriteLine("Base initialised");
			base.Seed(context);
		}

	}
}

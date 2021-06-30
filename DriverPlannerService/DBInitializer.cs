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
	public class DBInitializer : CreateDatabaseIfNotExists<DriverPlanner_DB>
	{
		protected override void Seed(DriverPlanner_DB context)
		{

			#region FillGender
			var male = new Gender("Мужской", "Мужской пол");
			var female = new Gender("Женский", "Женский пол");
			var genders = new List<Gender>();
			genders.Add(male);
			genders.Add(female);
			genders.ForEach(t => context.GENDERS.Add(t));
			context.SaveChanges();
			#endregion

			#region FillCategory
			var A = new Categories("A", "Мотоциклы");
			var AM = new Categories("AM", "Мопеды");
			var B = new Categories("B", "Автомобили с массой до 3,5 тонн и менее");
			var C = new Categories("C", "Автомобили с массой 3,5 тонны и более");

			var categories = new List<Categories>();
			categories.Add(A);
			categories.Add(AM);
			categories.Add(B);
			categories.Add(C);
			categories.ForEach(t => context.CATEGORIES.Add(t));
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
			intervals.ForEach(t => context.INTERVALS.Add(t));
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

			var vw = new Cars("VW POLO SEDAN", 2002, B);
			var lada = new Cars("LADA GRANTA SEDAN", 2010, B);
			var HONDA = new Cars("HONDA CBR RR", 2004, A);

			var cars = new List<Cars>();
			cars.Add(vw);
			cars.Add(lada);
			cars.Add(HONDA);
			cars.ForEach(t => context.CARS.Add(t));
			context.SaveChanges();
			#endregion

			#region Admin init
			var admin = new Admin();
			admin.AdminEmail = "admin@mail.ru";
			admin.HashPass = "GZ0s3/R2NXrmXDtikekaRQ==";
			admin.Login = "admino";
			admin.Name = "Johny";
			context.ADMINS.Add(admin);
			context.SaveChanges();

			#endregion

			Console.WriteLine("Base initialised");
			base.Seed(context);
		}

	}
}

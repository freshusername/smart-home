using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Infrastructure.Data.DbInitialize
{
    public class DbInitializer : IDisposable
    {
        private bool _disposed = false;
        private SafeHandle _handle = new SafeFileHandle(IntPtr.Zero, true);

        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        public DbInitializer(IServiceProvider serviceProvider)
        {
            _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            _userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        }

        public async Task SeedData()
        {
            await SeedRoles();
            await SeedUsers();
            await SeedIcons();
            await SeedSensorTypes();
            await SeedSensors();
        }

        public async Task SeedRoles()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }
        }

        public async Task SeedUsers()
        {
            var admin = new AppUser
            {
                Email = "admin@admin.com",
                FirstName = "Admin",
                LastName = "Admin",
                UserName = "admin@admin.com",
                PhoneNumber = "+380930000000",
                EmailConfirmed = true
            };

            var user = new AppUser
            {
                Email = "user@user.com",
                FirstName = "User",
                LastName = "User",
                PhoneNumber = "+380930000000",
                EmailConfirmed = true
            };

            if (await _userManager.FindByNameAsync(admin.Email) == null)
            {
                IdentityResult result;
                result = await _userManager.CreateAsync(admin, "admin12345");

                if (result.Succeeded)
                    _userManager.AddToRoleAsync(admin, "Admin").Wait();
            }

            if (_userManager.FindByNameAsync(user.Email).Result == null)
            {
                IdentityResult result;
                result = await _userManager.CreateAsync(user, "user12345");

                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(user, "User");
            }
        }

        public async Task SeedIcons()
        {
            var icons = await _unitOfWork.IconRepo.GetAll();
            List<Icon> iconsInsert = new List<Icon>
            {
                new Icon { Id = 1, Path = "/images/Icons/temperature_sensor.png" },
                new Icon { Id = 2, Path = "/images/Icons/pressure_sensor.png" },
                new Icon { Id = 3, Path = "/images/Icons/open_close_sensor.png" },
                new Icon { Id = 4, Path = "/images/Icons/movement_sensor.png" },
                new Icon { Id = 5, Path = "/images/Icons/light_sensor.png" }
            };

            if (!icons.Any())
            {
                foreach (var i in iconsInsert)
                {
                    await _unitOfWork.IconRepo.Insert(i);
                }
            }

            _unitOfWork.Save();
        }

        public async Task SeedSensorTypes()
        {
            var sensorTypes = await _unitOfWork.SensorTypeRepo.GetAll();
            List<SensorType> sTypesInsert = new List<SensorType>()
            {
                new SensorType { Id = 1, Name = "Humidity",             Comment = "Monitors level of humidity", MeasurementType = MeasurementType.Double,       MeasurementName = "%",          IconId = 1 },
                new SensorType { Id = 2, Name = "Temperature",          Comment = "Monitors temperature", MeasurementType = MeasurementType.Int,                MeasurementName = "°C",         IconId = 1 },
                new SensorType { Id = 3, Name = "IP",                   Comment = "Monitors IP", MeasurementType = MeasurementType.String,                      MeasurementName = "",           IconId = 1 },
                new SensorType { Id = 4, Name = "Fire",                 Comment = "Fire alarm", MeasurementType = MeasurementType.Bool,                         MeasurementName = "Logical",    IconId = 1 },
                new SensorType { Id = 5, Name = "Movement/Vibration",   Comment = "Monitors movement and vibration", MeasurementType = MeasurementType.Bool,    MeasurementName = "Logical",    IconId = 4 },
                new SensorType { Id = 6, Name = "WiFiPSSI",             Comment = "Checks WiFi signal quality", MeasurementType = MeasurementType.Int,          MeasurementName = "dBm",        IconId = 1 },
                new SensorType { Id = 7, Name = "Sound",                Comment = "Sensors of this type produce sound", MeasurementType = MeasurementType.Bool, MeasurementName = "",           IconId = 1 },
                new SensorType { Id = 8, Name = "Light",                Comment = "Sensors of this type produce light", MeasurementType = MeasurementType.Bool, MeasurementName = "",           IconId = 5 },
                new SensorType { Id = 9, Name = "Fan",                  Comment = "Make Fan", MeasurementType = MeasurementType.Bool,                           MeasurementName = "",           IconId = 1 }

            };

            if (!sensorTypes.Any())
            {
                foreach (var st in sTypesInsert)
                {
                    await _unitOfWork.SensorTypeRepo.Insert(st);
                }
            }

            _unitOfWork.Save();
        }

        public async Task SeedSensors()
        {
            var findAdmin = await _unitOfWork.UserManager.FindByNameAsync("admin@admin.com");
            //var findUser = await _unitOfWork.UserManager.FindByNameAsync("user@user.com");

            string adminId = findAdmin.Id;
            //string userId = findUser.Id;

            var sensors = await _unitOfWork.SensorRepo.GetAll();
            List<Sensor> sensorsInsert = new List<Sensor>()
            {
                new Sensor { Id = 1, Name = "Hum-Sens-01", Comment = "Monitors humidity",                           Token = new Guid("4b4e6872-9ca7-4616-97fc-521f9308deb9"), CreatedOn = DateTimeOffset.Parse("21/01/2020 20:24:25", null, DateTimeStyles.AssumeLocal), IsValid = true, IsActive = true, AppUserId = adminId, SensorTypeId = 1, IconId = 1, },
                new Sensor { Id = 2, Name = "Temp-Sens-01", Comment = "Monitors temperature",                       Token = new Guid("28d53665-9ba3-4769-bc32-882d710123fb"), CreatedOn = DateTimeOffset.Parse("21/01/2020 20:24:25", null, DateTimeStyles.AssumeLocal), IsValid = true, IsActive = true, AppUserId = adminId, SensorTypeId = 2, IconId = 1, },
                new Sensor { Id = 3, Name = "IP-Sens-01", Comment = "Monitors IP",                                  Token = new Guid("c4d34d90-a68e-4de0-a98b-ac2cd7ef9f09"), CreatedOn = DateTimeOffset.Parse("21/01/2020 20:24:25", null, DateTimeStyles.AssumeLocal), IsValid = true, IsActive = true, AppUserId = adminId, SensorTypeId = 3, IconId = 1, },
                new Sensor { Id = 4, Name = "Fire-Sens-01", Comment = "Monitors existence of fire",                 Token = new Guid("b09e04bc-b92c-49d1-adb9-6984d6a31e92"), CreatedOn = DateTimeOffset.Parse("21/01/2020 20:24:25", null, DateTimeStyles.AssumeLocal), IsValid = true, IsActive = true, AppUserId = adminId, SensorTypeId = 4, IconId = 1, },
                new Sensor { Id = 5, Name = "Mov-Sens-01", Comment = "Monitors movement",                           Token = new Guid("89b62e85-b6c3-4816-b692-ce2fc00b08ae"), CreatedOn = DateTimeOffset.Parse("21/01/2020 20:24:25", null, DateTimeStyles.AssumeLocal), IsValid = true, IsActive = true, AppUserId = adminId, SensorTypeId = 5, IconId = 1, },
                new Sensor { Id = 6, Name = "WiFi-Sens-01", Comment = "WiFi Receiver Signal Strength Indicator",    Token = new Guid("1b4af968-38db-44ce-81dd-c7a2615a3761"), CreatedOn = DateTimeOffset.Parse("21/01/2020 20:24:25", null, DateTimeStyles.AssumeLocal), IsValid = true, IsActive = true, AppUserId = adminId, SensorTypeId = 6, IconId = 1, },
                new Sensor { Id = 7, Name = "Vibr-Sens-01", Comment = "Monitors vibration",                         Token = new Guid("76711a7c-2fed-4376-83d3-1c00a490792e"), CreatedOn = DateTimeOffset.Parse("21/01/2020 20:24:25", null, DateTimeStyles.AssumeLocal), IsValid = true, IsActive = true, AppUserId = adminId, SensorTypeId = 5, IconId = 1, },
                new Sensor { Id = 8, Name = "Beep-Sens-01", Comment = "Produces beep sound",                        Token = new Guid("0f94812c-2501-4a00-987d-10eb7111873e"), CreatedOn = DateTimeOffset.Parse("21/01/2020 20:24:25", null, DateTimeStyles.AssumeLocal), IsValid = true, IsActive = true, AppUserId = adminId, SensorTypeId = 7, IconId = 1, },
                new Sensor { Id = 9, Name = "Lamp-Sens-01", Comment = "Produces light",                             Token = new Guid("ed14d1d7-e79f-4b7d-9a2c-8efd38845fd9"), CreatedOn = DateTimeOffset.Parse("21/01/2020 20:24:25", null, DateTimeStyles.AssumeLocal), IsValid = true, IsActive = true, AppUserId = adminId, SensorTypeId = 8, IconId = 1, },
                new Sensor { Id = 10, Name = "Fan-Sens-01", Comment = "Fans",                                       Token = new Guid("fc37f8d2-ce62-4788-a186-e5bcd741db87"), CreatedOn = DateTimeOffset.Parse("21/01/2020 20:24:25", null, DateTimeStyles.AssumeLocal), IsValid = true, IsActive = true, AppUserId = adminId, SensorTypeId = 9, IconId = 1, }
            };

            if (!sensors.Any())
            {
                foreach (var s in sensorsInsert)
                {
                    await _unitOfWork.SensorRepo.Insert(s);
                    _unitOfWork.Save();
                }
            }

            _unitOfWork.Save();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _handle.Dispose();
                //Free any other managed objects here.

            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            //Suppress finalization.
            GC.SuppressFinalize(this);
        }
    }
}

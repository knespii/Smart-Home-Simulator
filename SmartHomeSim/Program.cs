using SmartHomeSim.Data;
using SmartHomeSim.Models;
using SmartHomeSim.UI;
using Microsoft.Extensions.Configuration;
using System;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var dbService = new MongoService(config);

var menu = new MenuHandler(dbService);
menu.RunLogin();
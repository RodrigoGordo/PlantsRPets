using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;

namespace PlantsRPetsProjeto.Server.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly PlantsRPetsProjetoServerContext _context;

        public NotificationsController(PlantsRPetsProjetoServerContext context)
        {
            _context = context;
        }
    }
}

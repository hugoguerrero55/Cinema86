using Cinema86Movie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ClubCinema86.Controllers
{
    public class TrailerController : Controller
    {
        DB_A4B7B0_Cinema86Entities objCon = new DB_A4B7B0_Cinema86Entities();

        // GET: Trailer
        [OutputCache(Duration = 60, Location = OutputCacheLocation.Client)]
        public ActionResult Index(int _idPeli)
        {
            try
            {
                List<CN86_Pelicula> _listPelicula = new List<CN86_Pelicula>();

                _listPelicula = objCon.CN86_Pelicula.Where(model => model.Id_Pelicula == _idPeli).ToList();
                ViewBag.Title = "Trailer - " + _listPelicula[0].Titulo.ToString();

                if(_listPelicula[0].URL_Trailer == null)
                {
                    return RedirectToAction("Main", "Home");
                }

                return View(_listPelicula);
            }
            catch (Exception)
            {

                return View();
            }
        }
    }
}
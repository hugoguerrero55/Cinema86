using Cinema86Movie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ClubCinema86.Controllers
{
    public class ReproductorController : Controller
    {
        DB_A4B7B0_Cinema86Entities objCon = new DB_A4B7B0_Cinema86Entities();

        // GET: Reproductor
        [OutputCache(Duration = 60, Location = OutputCacheLocation.Client)]
        public ActionResult Index(int _idPeli)
        {
            try
            {
                List<CN86_Pelicula> _listPelicula = new List<CN86_Pelicula>();

                _listPelicula = objCon.CN86_Pelicula.Where(model => model.Id_Pelicula == _idPeli).ToList();
                ViewBag.Title = _listPelicula[0].Titulo.ToString();

                return View(_listPelicula);
            }
            catch (Exception)
            {

                return View();
            }           
        }

        public FileResult Download(int _idPeli)
        {
            List<CN86_Pelicula> _listPelicula = new List<CN86_Pelicula>();
            _listPelicula = objCon.CN86_Pelicula.Where(model => model.Id_Pelicula == _idPeli).ToList();

            string _urlTorrent = _listPelicula[0].TorrentName.ToString();

            string _pathFile = Server.MapPath("~/Torrents/" + _urlTorrent);
            byte[] fileBytes = System.IO.File.ReadAllBytes(_pathFile);
            string fileName = _urlTorrent;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}
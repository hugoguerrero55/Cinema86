using Cinema86Movie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ClubCinema86.Controllers
{
    public class PreviosController : Controller
    {
        DB_A4B7B0_Cinema86Entities objCon = new DB_A4B7B0_Cinema86Entities();

        // GET: Reproductor
        [OutputCache(Duration = 60, Location = OutputCacheLocation.Client)]
        public ActionResult Index(int _idPeli)
        {
            try
            {
                List<CN86_Capitulos_Anterior> _listCapitulos = new List<CN86_Capitulos_Anterior>();

                _listCapitulos = objCon.CN86_Capitulos_Anterior.Where(model => model.Id_Pelicula == _idPeli).ToList();
                
                if(_listCapitulos != null)
                {
                    ViewBag.Title = "Capítulos previos al actual"; //+ _listCapitulos[0].Titulo.ToString();

                    return View(_listCapitulos);
                }
                else
                {
                    ViewBag.Title = "No Existen Capítulos Anteriores de la Serie"; //+ _listCapitulos[0].Titulo.ToString();

                    return RedirectToAction("NoCapituloPrevio", "Previos");
                }

            }
            catch (Exception)
            {

                return View();
            }
        }

        public ActionResult NoCapitulosPrevios()
        {
            return View();
        }

        public FileResult Download(int _idCap)
        {
            List<CN86_Capitulos_Anterior> _listCapitulos = new List<CN86_Capitulos_Anterior>();
            _listCapitulos = objCon.CN86_Capitulos_Anterior.Where(model => model.Id_Capitulo == _idCap).ToList();

            string _urlTorrent = _listCapitulos[0].TorrentName.ToString();

            string _pathFile = Server.MapPath("~/Torrents/" + _urlTorrent);
            byte[] fileBytes = System.IO.File.ReadAllBytes(_pathFile);
            string fileName = _urlTorrent;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}
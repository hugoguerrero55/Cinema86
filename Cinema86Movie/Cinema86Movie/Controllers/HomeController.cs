using Cinema86Movie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.UI;
using PagedList;


namespace Cinema86Movie.Controllers
{
    public class HomeController : Controller
    {
        DB_A4B7B0_Cinema86Entities objCon = new DB_A4B7B0_Cinema86Entities();

        // GET: Home
        [OutputCache(Duration = 0, VaryByParam = "None")]
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(Duration = 0, VaryByParam = "None")]
        public ActionResult Main(string search, int? page)
        {
            List<CN86_Pelicula> _listPelicula = new List<CN86_Pelicula>();

            if (search != null)
            {
                page = 1;
            }

            if (!String.IsNullOrEmpty(search))
            {
                _listPelicula = objCon.CN86_Pelicula.OrderBy(x => x.Titulo).Where(x => x.Titulo.Contains(search) || search == null).ToList();
            }
            else
            {
                _listPelicula = objCon.CN86_Pelicula.OrderBy(x => x.Titulo).ToList();
            }

            //Aqui armo la lista de generos separadas por tabs
            var _listageneros = objCon.CN86_Genero.GroupJoin(objCon.CN86_Pelicula,
                                             e => e.Id_Genero,
                                             d => d.Id_Genero,
                                             (genero, pelicula) => new
                                             {
                                                 _id = genero.Id_Genero,
                                                 _descrip = genero.DescripcionGenero
                                             }).ToList();


            string _generos = string.Empty;

            foreach (var item in _listageneros)
            {
                _generos = _generos + item._id + "-" + item._descrip + "\t";
            }

            string[] _datosGenero = _generos.Split('\t');

            ViewBag.Generos = _datosGenero;

            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(_listPelicula.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Genero(string search, int? page, int _idGenero)
        {
            List<CN86_Pelicula> _listPelicula = new List<CN86_Pelicula>();

            if (search != null)
            {
                page = 1;
            }

            if (!String.IsNullOrEmpty(search))
            {
                _listPelicula = objCon.CN86_Pelicula.OrderBy(x => x.Titulo)
                    .Where(x => x.Titulo.Contains(search) || search == null)
                    .Where(x => x.Id_Genero == _idGenero).ToList();
            }
            else
            {
                _listPelicula = objCon.CN86_Pelicula.OrderBy(x => x.Titulo)
                    .Where(s => s.Id_Genero == _idGenero).ToList();
            }


            ViewBag.CveGenero = _idGenero;

            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(_listPelicula.ToPagedList(pageNumber, pageSize));
        }
    }
}
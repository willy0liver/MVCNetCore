using ExamenMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

namespace ExamenMVC.Controllers
{
    public class AlumnoItemsController : Controller
    {
        HttpClientHandler _clientHandler = new HttpClientHandler();

        AlumnoItem _oAlumnoItem = new AlumnoItem();
        List<AlumnoItem> _oAlumnoItems = new List<AlumnoItem>();

        public AlumnoItemsController()
        {
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<List<AlumnoItem>> GetAllAlumnoItems()
        {
            _oAlumnoItems = new List<AlumnoItem>();

            using (var httpClient = new HttpClient(_clientHandler))
            {
                using (var response = await httpClient.GetAsync("https://localhost:44325/api/alumnos"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oAlumnoItems = JsonConvert.DeserializeObject<List<AlumnoItem>>(apiResponse);
                }
            }

            return _oAlumnoItems;
        }

        [HttpGet]
        public async Task<AlumnoItem> GetById(int alumnoItemId)
        {
            _oAlumnoItem = new AlumnoItem();
            using (var httpClient = new HttpClient(_clientHandler))
            {
                using (var response = await httpClient.GetAsync("https://localhost:44325/api/alumnos/" + alumnoItemId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oAlumnoItem = JsonConvert.DeserializeObject<AlumnoItem>(apiResponse);
                }
            }
            return _oAlumnoItem;
        }

        [HttpPost]
        public async Task<AlumnoItem> AddAlumnoItem(int alumnoItemId, [FromForm] AlumnoItem oAlumnoItem)
        {
            _oAlumnoItem = new AlumnoItem();
            using (var httpClient = new HttpClient(_clientHandler))
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(oAlumnoItem), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:44325/api/alumnos?id=" + alumnoItemId, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oAlumnoItem = JsonConvert.DeserializeObject<AlumnoItem>(apiResponse);
                }
            }
            return _oAlumnoItem;
        }

        [HttpPut]
        public async void updateAlumnoItem(int alumnoItemId, [FromForm] AlumnoItem oAlumnoItem)
        {
            _oAlumnoItem = new AlumnoItem();
            using (var httpClient = new HttpClient(_clientHandler))
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(oAlumnoItem), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync("https://localhost:44325/api/alumnos?id=" + alumnoItemId, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    //_oAlumnoItem = JsonConvert.DeserializeObject<AlumnoItem>(apiResponse);
                }
            }
        }

        [HttpDelete]
        public async Task<string> Delete(int alumnoItemId)
        {
            string message = "";
            using (var httpClient = new HttpClient(_clientHandler))
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:44325/api/alumnos/" + alumnoItemId))
                {
                    message = await response.Content.ReadAsStringAsync();
                }
            }
            return message;
        }
    }
}
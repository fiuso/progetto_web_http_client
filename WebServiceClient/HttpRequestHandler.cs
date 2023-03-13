using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace WebServiceClient
{
    public class HttpRequestHandler
    {

        private static readonly string URL = "https://www.omdbapi.com/?apikey=7952c406";
        private readonly HttpClient _httpClient;

        private int page = 1;
        
        public HttpRequestHandler(HttpClient httpClient) {
            this._httpClient = httpClient;
        }


        // Trova tutti i film con la stringa cercata nel titolo e ne da qualche informazione
        public async Task<List<Film>> GetAllFilm(string title) {
            
            HttpResponseMessage response = await this._httpClient.GetAsync(URL + $"&s={title}" + $"&page={page}");
            string listaFilm = await response.Content.ReadAsStringAsync();
            try {
                FilmSearchContent searchResult = JsonSerializer.Deserialize<FilmSearchContent>(listaFilm);
                if (searchResult == null || searchResult.totalResults == 0) return new List<Film>();
                return searchResult.Search;

            } catch (Exception ex) {
                return new List<Film>();
            }
        }

        public async Task<List<Film>> GetUpdatePage(string title)
        {

            page++;
           
            HttpResponseMessage response = await this._httpClient.GetAsync(URL + $"&s={title}" + $"&page={page}");
            string listaFilm = await response.Content.ReadAsStringAsync();
            try
            {
                FilmSearchContent searchResult = JsonSerializer.Deserialize<FilmSearchContent>(listaFilm);
                if (searchResult == null || searchResult.totalResults == 0) return new List<Film>();
                return searchResult.Search;
            }
            catch (Exception ex)
            {
                return new List<Film>();
            }
        }

        public async Task<List<Film>> GoPreviousPage(string title) {
            
            if(page > 1) page--;
            
            HttpResponseMessage response = await this._httpClient.GetAsync(URL + $"&s={title}" + $"&page={page}");
            string listaFilm = await response.Content.ReadAsStringAsync();
            try
            {
                FilmSearchContent searchResult = JsonSerializer.Deserialize<FilmSearchContent>(listaFilm);
                if (searchResult == null || searchResult.totalResults == 0) return new List<Film>();
                return searchResult.Search;
            }
            catch (Exception ex)
            {
                return new List<Film>();
            }
        }



        // Contiene tutte le info per il film cercato
        public async Task<Film?> GetFilm(string id)
        {
            HttpResponseMessage response = await this._httpClient.GetAsync(URL + $"&i={id}");
            string film = await response.Content.ReadAsStringAsync();
            try {
                return JsonSerializer.Deserialize<Film>(film);
            } catch (Exception) {
                return null;
            }
        }
    }
}

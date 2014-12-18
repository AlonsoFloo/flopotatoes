using System;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using Xamarin;

namespace FloPotatoes
{
	public class PotatoesManager
	{	
		public static readonly string FRESH = "fresh";
		public static readonly string ROTTEN = "rotten";
		public static readonly string CERTIFIED_FRESH = "certified Fresh";
		public static readonly PotatoesManager Instance = new PotatoesManager ();

		private const string API_URL = "http://api.rottentomatoes.com/api/public/v1.0/";
		private const string API_KEY = "f9zyjbdqxfqwfc3mfe5vpjq9";

		public int Limit { get; set; }
		public string Country { get; set; }

		public PotatoesManager ()
		{
			this.Limit = 10;
			this.Country = "us";
		}

		public async Task<List<Review>> GetReviews(int Id)
		{
			return await Task.Factory.StartNew (() => {
				try {
					string extraParams = "";
					var request = CreateRequest ("movies/"+ Id.ToString() +"/reviews.json", extraParams);

					string response = ReadResponseText (request);
					ReviewList rvList = Newtonsoft.Json.JsonConvert.DeserializeObject<ReviewList> (response);
					return rvList.Reviews;
				} catch (Exception ex) {
					Console.WriteLine (ex);
					Insights.Report(ex);
					return null;
				}
			});
		}

		public async Task<Movie> GetFullMovieData(int Id)
		{
			return await Task.Factory.StartNew (() => {
				try {
					string extraParams = "";
					var request = CreateRequest ("movies/"+ Id.ToString() +".json", extraParams);

					string response = ReadResponseText (request);
					return Newtonsoft.Json.JsonConvert.DeserializeObject<Movie> (response);
				} catch (Exception ex) {
					Console.WriteLine (ex);
					Insights.Report(ex);
					return null;
				}
			});
		}

		public async Task<List<Movie>> GetMoviesOpening()
		{
			return await Task.Factory.StartNew (() => {
				try {
					string extraParams = "";
					var request = CreateRequest ("lists/movies/opening.json", extraParams);

					string response = ReadResponseText (request);
					MovieList mvList = Newtonsoft.Json.JsonConvert.DeserializeObject<MovieList> (response);
					return mvList.movies;
				} catch (Exception ex) {
					Console.WriteLine (ex);
					Insights.Report(ex);
					return new List<Movie> ();
				}
			});
		}

		public async Task<List<Movie>> GetMoviesBoxOffice()
		{
			return await Task.Factory.StartNew (() => {
				try {
					string extraParams = "";
					var request = CreateRequest ("lists/movies/box_office.json", extraParams);

					string response = ReadResponseText (request);
					MovieList mvList = Newtonsoft.Json.JsonConvert.DeserializeObject<MovieList> (response);
					return mvList.movies;
				} catch (Exception ex) {
					Console.WriteLine (ex);
					Insights.Report(ex);
					return new List<Movie> ();
				}
			});
		}

		public async Task<List<Movie>> GetMoviesTheater()
		{
			return await Task.Factory.StartNew (() => {
				try {
					string extraParams = "";
					var request = CreateRequest ("lists/movies/in_theaters.json", extraParams);

					string response = ReadResponseText (request);
					MovieList mvList = Newtonsoft.Json.JsonConvert.DeserializeObject<MovieList> (response);
					return mvList.movies;
				} catch (Exception ex) {
					Console.WriteLine (ex);
					Insights.Report(ex);
					return new List<Movie> ();
				}
			});
		}

		private HttpWebRequest CreateRequest(string location, string extraParams)
		{
			string finalURL = API_URL + location + "?limit=" + this.Limit + "&page="+ 1 + "&country=" + this.Country + "&apiKey="+ API_KEY;
			var request = (HttpWebRequest)WebRequest.Create (finalURL+ extraParams);
			request.Method = "GET";
			request.ContentType = "application/json";
			request.Accept = "application/json";
			return request;
		}

		private string ReadResponseText (HttpWebRequest req) {
			using (WebResponse resp = req.GetResponse ()) {
				using (Stream s = (resp).GetResponseStream ()) {
					using (var r = new StreamReader (s, Encoding.UTF8)) {
						return r.ReadToEnd ();
					}
				}
			}
		}
	}
}


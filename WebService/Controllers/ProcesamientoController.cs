using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using WebService.Scripts;

namespace WebService.Controllers
{
    public class ProcesamientoController : ApiController
    {
		[HttpPost]
		[ActionName("EscalaGrises")]
		public async Task<HttpResponseMessage> EscalaGrises()
		{
			try
			{
				if (!Request.Content.IsMimeMultipartContent())
					throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

				var provider = new MultipartMemoryStreamProvider();
				await Request.Content.ReadAsMultipartAsync(provider);

				var file = provider.Contents.First();
				var stream = await file.ReadAsStreamAsync();
				Bitmap image = new Bitmap(stream);

				Bitmap grayImage = MetodosProcesamiento.Escala_Grises(image);

				MemoryStream ms = new MemoryStream();
				grayImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
				ms.Position = 0;

				HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
				result.Content = new StreamContent(ms);
				result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");

				return result;
			}
			catch (Exception e)
			{
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
			}
		}

		[HttpPost]
		[ActionName("Binarizar")]
		public async Task<HttpResponseMessage> Binarizar()
		{
			try
			{
				if (!Request.Content.IsMimeMultipartContent())
					throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

				var provider = new MultipartMemoryStreamProvider();
				await Request.Content.ReadAsMultipartAsync(provider);

				var file = provider.Contents.First();
				var stream = await file.ReadAsStreamAsync();
				Bitmap image = new Bitmap(stream);

				Bitmap grayImage = MetodosProcesamiento.Binarizar(image);

				MemoryStream ms = new MemoryStream();
				grayImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
				ms.Position = 0;

				HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
				result.Content = new StreamContent(ms);
				result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");

				return result;
			}
			catch (Exception e)
			{
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
			}
		}

		[HttpPost]
		[ActionName("DetectarBordes")]
		public async Task<HttpResponseMessage> DetectarBordes()
		{
			try
			{
				if (!Request.Content.IsMimeMultipartContent())
					throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

				var provider = new MultipartMemoryStreamProvider();
				await Request.Content.ReadAsMultipartAsync(provider);

				var file = provider.Contents.First();
				var stream = await file.ReadAsStreamAsync();
				Bitmap image = new Bitmap(stream);

				Bitmap grayImage = MetodosProcesamiento.Detectar_Bordes(image);

				MemoryStream ms = new MemoryStream();
				grayImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
				ms.Position = 0;

				HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
				result.Content = new StreamContent(ms);
				result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");

				return result;
			}
			catch (Exception e)
			{
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
			}
		}

		[HttpPost]
		[ActionName("Etiquetado")]
		public async Task<HttpResponseMessage> Etiquetado()
		{
			try
			{
				if (!Request.Content.IsMimeMultipartContent())
					throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

				var provider = new MultipartMemoryStreamProvider();
				await Request.Content.ReadAsMultipartAsync(provider);

				var file = provider.Contents.First();
				var stream = await file.ReadAsStreamAsync();
				Bitmap image = new Bitmap(stream);

				Bitmap grayImage = MetodosProcesamiento.Etiquetado(image);

				MemoryStream ms = new MemoryStream();
				grayImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
				ms.Position = 0;

				HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
				result.Content = new StreamContent(ms);
				result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");

				return result;
			}
			catch (Exception e)
			{
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
			}
		}

		[HttpPost]
		[ActionName("InvariantesHu")]
		public async Task<HttpResponseMessage> InvariantesHu()
		{
			try
			{
				if (!Request.Content.IsMimeMultipartContent())
					throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

				var provider = new MultipartMemoryStreamProvider();
				await Request.Content.ReadAsMultipartAsync(provider);

				var file = provider.Contents.First();
				var stream = await file.ReadAsStreamAsync();
				Bitmap image = new Bitmap(stream);

				//Bitmap grayImage = MetodosProcesamiento.CalcularMomentosHu(image);

				MemoryStream ms = new MemoryStream();
				//grayImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
				ms.Position = 0;

				HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
				result.Content = new StreamContent(ms);
				result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");

				return result;
			}
			catch (Exception e)
			{
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
			}
		}
	}
}

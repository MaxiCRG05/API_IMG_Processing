using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
		private static readonly Lazy<Pen> _lazyRedPen = new Lazy<Pen>(() => new Pen(Color.Red, 3));
		private static readonly Lazy<Font> _lazyFont = new Lazy<Font>(() => new Font("Arial", 12, FontStyle.Bold));
		private static readonly Lazy<SolidBrush> _lazyRedBrush = new Lazy<SolidBrush>(() => new SolidBrush(Color.Red));

		private static Pen RedPen => _lazyRedPen.Value;
		private static Font Font => _lazyFont.Value;
		private static SolidBrush RedBrush => _lazyRedBrush.Value;

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

				using (var stream = await file.ReadAsStreamAsync())
				using (var originalImage = new Bitmap(stream))
				using (var gris = MetodosProcesamiento.Escala_Grises(originalImage))
				using (var ms = new MemoryStream())
				{
					gris.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
					ms.Position = 0;

					var result = new HttpResponseMessage(HttpStatusCode.OK);
					result.Content = new StreamContent(ms);
					result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
					return result;
				}
			}
			catch (Exception e)
			{
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
					"Error en EscalaGrisesOptimizado: " + e.Message);
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

				Bitmap bin = MetodosProcesamiento.Binarizar(image);

				MemoryStream ms = new MemoryStream();
				bin.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
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

				Bitmap bordes = MetodosProcesamiento.Detectar_Bordes(image);

				MemoryStream ms = new MemoryStream();
				bordes.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
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

				Bitmap etiquetado = MetodosProcesamiento.Etiquetado(image);

				MemoryStream ms = new MemoryStream();
				etiquetado.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
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
				Bitmap originalImage = new Bitmap(stream);

				List<ResultadoMomentosHu> momentosHu = MetodosProcesamiento.CalcularMomentosHuPorObjeto(originalImage);
				int totalObjetos = momentosHu.Count;

				Bitmap invariancias = new Bitmap(MetodosProcesamiento.Binarizar(originalImage));

				using (Graphics g = Graphics.FromImage(invariancias))
				{
					Pen redPen = new Pen(Color.Red, 3);
					Font font = new Font("Arial", 12, FontStyle.Bold);
					Brush redBrush = new SolidBrush(Color.Red);

					foreach (var momento in momentosHu)
					{
						PointF center = momento.Center;
						float radius = 10;

						g.DrawEllipse(redPen, center.X - radius, center.Y - radius,
									 radius * 2, radius * 2);

						float xSize = 8;
						g.DrawLine(redPen, center.X - xSize, center.Y - xSize,
								  center.X + xSize, center.Y + xSize);
						g.DrawLine(redPen, center.X - xSize, center.Y + xSize,
								  center.X + xSize, center.Y - xSize);

						int index = momentosHu.IndexOf(momento) + 1;
						g.DrawString(index.ToString(), font, redBrush,
									center.X + radius + 2, center.Y - radius - 2);
					}

					string textoTotal = $"Total objetos: {totalObjetos}";
					SizeF textSize = g.MeasureString(textoTotal, font);
					g.DrawString(textoTotal, font, redBrush,
								invariancias.Width - textSize.Width - 10,
								invariancias.Height - textSize.Height - 10);
				}

				MemoryStream ms = new MemoryStream();
				invariancias.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
				string imageBase64 = Convert.ToBase64String(ms.ToArray());

				var response = new
				{
					Imagen = imageBase64,
					TotalObjetos = totalObjetos,
					MomentosHu = momentosHu.Select(m => new
					{
						Moments = m.Moments,
						Center = new { X = m.Center.X, Y = m.Center.Y }
					}).ToList()
				};

				HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.OK, response);
				result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

				originalImage.Dispose();
				invariancias.Dispose();
				ms.Dispose();

				return result;
			}
			catch (Exception e)
			{
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
			}
		}

		[HttpPost]
		[ActionName("MultiplesInvariantesHu")]
		public async Task<HttpResponseMessage> MultiplesInvariantesHu()
		{
			try
			{
				if (!Request.Content.IsMimeMultipartContent())
					throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

				var provider = new MultipartFormDataStreamProvider(Path.GetTempPath());
				var result = await Request.Content.ReadAsMultipartAsync(provider);
				var resultados = new List<object>();

				foreach (var file in result.FileData)
				{
					try
					{
						using (var stream = new FileStream(file.LocalFileName, FileMode.Open))
						using (var originalImage = new Bitmap(stream))
						{
							var resultado = await ProcesarImagenHu(originalImage);
							resultados.Add(resultado);
						}
						File.Delete(file.LocalFileName);
					}
					catch (Exception ex)
					{
						System.Diagnostics.Debug.WriteLine($"Error procesando archivo: {ex.Message}");
					}
				}

				return Request.CreateResponse(HttpStatusCode.OK, resultados);
			}
			catch (Exception e)
			{
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
			}
		}

		private async Task<object> ProcesarImagenHu(Bitmap originalImage)
		{
			return await Task.Run(() =>
			{
				var momentosHu = MetodosProcesamiento.CalcularMomentosHuPorObjeto(originalImage);
				int totalObjetos = momentosHu.Count;

				using (var imagenBinarizada = MetodosProcesamiento.Binarizar(originalImage))
				using (var invariancias = new Bitmap(imagenBinarizada))
				using (var g = Graphics.FromImage(invariancias))
				using (var ms = new MemoryStream())
				{
					g.CompositingQuality = CompositingQuality.HighSpeed;
					g.SmoothingMode = SmoothingMode.None;
					g.InterpolationMode = InterpolationMode.Low;
					g.PixelOffsetMode = PixelOffsetMode.HighSpeed;

					for (int i = 0; i < momentosHu.Count; i++)
					{
						var momento = momentosHu[i];
						PointF center = momento.Center;
						float radius = 10;

						g.DrawEllipse(RedPen, center.X - radius, center.Y - radius, radius * 2, radius * 2);

						float xSize = 8;
						g.DrawLine(RedPen, center.X - xSize, center.Y - xSize, center.X + xSize, center.Y + xSize);
						g.DrawLine(RedPen, center.X - xSize, center.Y + xSize, center.X + xSize, center.Y - xSize);

						g.DrawString((i + 1).ToString(), Font, RedBrush, center.X + radius + 2, center.Y - radius - 2);
					}

					string textoTotal = $"Total objetos: {totalObjetos}";
					SizeF textSize = g.MeasureString(textoTotal, Font);
					g.DrawString(textoTotal, Font, RedBrush,
								invariancias.Width - textSize.Width - 10,
								invariancias.Height - textSize.Height - 10);

					invariancias.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
					string imageBase64 = Convert.ToBase64String(ms.ToArray());

					return new
					{
						Imagen = imageBase64,
						TotalObjetos = totalObjetos,
						MomentosHu = momentosHu.Select(m => new
						{
							Moments = m.Moments,
							Center = new { X = m.Center.X, Y = m.Center.Y }
						}).ToList()
					};
				}
			});
		}

		[HttpPost]
		[ActionName("MultiplesEscalaGrises")]
		public async Task<HttpResponseMessage> MultiplesEscalaGrises()
		{
			return await ProcesarMultiplesImagenesJson(MetodosProcesamiento.Escala_Grises);
		}

		[HttpPost]
		[ActionName("MultiplesBinarizar")]
		public async Task<HttpResponseMessage> MultiplesBinarizar()
		{
			return await ProcesarMultiplesImagenesJson(MetodosProcesamiento.Binarizar);
		}

		[HttpPost]
		[ActionName("MultiplesDetectarBordes")]
		public async Task<HttpResponseMessage> MultiplesDetectarBordes()
		{
			return await ProcesarMultiplesImagenesJson(MetodosProcesamiento.Detectar_Bordes);
		}

		[HttpPost]
		[ActionName("MultiplesEtiquetado")]
		public async Task<HttpResponseMessage> MultiplesEtiquetado()
		{
			return await ProcesarMultiplesImagenesJson(MetodosProcesamiento.Etiquetado);
		}

		private async Task<HttpResponseMessage> ProcesarMultiplesImagenesJson(Func<Bitmap, Bitmap> funcionProcesamiento)
		{
			try
			{
				if (!Request.Content.IsMimeMultipartContent())
					throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

				var provider = new MultipartFormDataStreamProvider(Path.GetTempPath());
				var result = await Request.Content.ReadAsMultipartAsync(provider);
				var processedImages = new List<string>();

				foreach (var file in result.FileData)
				{
					try
					{
						using (var stream = new FileStream(file.LocalFileName, FileMode.Open))
						using (var originalImage = new Bitmap(stream))
						using (var processedImage = MetodosProcesamiento.Escala_Grises(originalImage))
						using (var ms = new MemoryStream())
						{
							processedImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
							string base64Image = Convert.ToBase64String(ms.ToArray());
							processedImages.Add(base64Image);
						}
						File.Delete(file.LocalFileName);
					}
					catch (Exception ex)
					{
						System.Diagnostics.Debug.WriteLine($"Error procesando archivo: {ex.Message}");
					}
				}

				return Request.CreateResponse(HttpStatusCode.OK, new { Imagenes = processedImages });
			}
			catch (Exception e)
			{
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
			}
		}
	}
}
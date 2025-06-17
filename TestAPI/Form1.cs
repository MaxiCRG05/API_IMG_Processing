﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace TestAPI
{
	public partial class Form1 : Form
	{
		Stopwatch sw = new Stopwatch();
		API api;
		Bitmap btm_cargada, btm_recibida;
		bool imgSubida = false, opcionSeleccionada = false;
		int opcion;
		private string url = "http://localhost:3193/api/";

		public Form1()
		{
			InitializeComponent();
			CrearAPI(url);
			ConfigurarTabla();
		}

		private void LimpiarLabels()
		{
			lbTiempo.Text = "";
			lblObjetos.Text = "";
		}

		private void LimpiarImagenes()
		{
			imgEnviar.Image = null;
			imgRecibir.Image = null;
		}

		private void LimpiarTabla()
		{
			tabla.Columns.Clear();
			ConfigurarTabla();
			tabla.Visible = false;
		}

		private void ConfigurarTabla()
		{
			tabla.Columns.Clear();
			tabla.Columns.Add("ID", "ID");
			for (int i = 1; i <= 7; i++)
			{
				tabla.Columns.Add($"Hu{i}", $"Hu {i}");
			}

			tabla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			tabla.AllowUserToAddRows = false;
			tabla.ReadOnly = true;
		}

		public void PonerTiempo()
		{
			lbTiempo.Visible = true;
			lbTiempo.Text = (sw.ElapsedMilliseconds > 1000) ? (sw.ElapsedMilliseconds / 1000 < 60) ? ($"{sw.ElapsedMilliseconds / 1000.0:F2} segundos") : $"{sw.ElapsedMilliseconds / 60000}m {sw.ElapsedMilliseconds / 1000 % 60:F2}s" : $"{sw.ElapsedMilliseconds} ms";
		}

		public void VerificarEnviar()
		{
			btnEnviar.Enabled = imgSubida && opcionSeleccionada;
		}

		public void PonerNumObjetos()
		{
			if (opcion == 4)
			{
				lblObjetos.Text = $"{api.GetObjetos()} objetos encontrados";
				lblObjetos.Visible = true;
				label5.Visible = true;
			}
			else
			{
				lblObjetos.Visible = false;
				label5.Visible = false;
			}
		}

		private void CrearAPI(string url)
		{
			api = new API(url);
			Console.WriteLine("API creada con URL: " + url);
		}

		private System.Drawing.Imaging.ImageCodecInfo GetEncoderInfo(string mimeType)
		{
			System.Drawing.Imaging.ImageCodecInfo[] codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
			foreach (var codec in codecs)
			{
				if (codec.MimeType == mimeType)
					return codec;
			}
			return null;
		}

		private async void btnEnviar_MouseClick(object sender, MouseEventArgs e)
		{
			try
			{
				if (imgSubida && opcionSeleccionada)
				{
					sw.Start();
					btm_recibida = await api.Enviar(opcion, btm_cargada);
					PonerNumObjetos();
					imgRecibir.Image = btm_recibida;

					if (opcion == 4)
					{
						tabla.Visible = true;
						MostrarMomentosHuEnTabla();
					}
					else
						tabla.Visible = false;

					sw.Stop();
					PonerTiempo();
					sw.Reset();
				}
				else
					MessageBox.Show("Por favor, sube una imagen y selecciona una opción antes de enviar.");
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error al enviar la imagen: " + ex.Message);
			}
		}

		private void MostrarMomentosHuEnTabla()
		{
			tabla.Rows.Clear();

			var momentosHu = api.GetMomentosHu();

			for (int i = 0; i < momentosHu.Count; i++)
			{
				var momento = momentosHu[i];

				DataGridViewRow row = new DataGridViewRow();
				row.CreateCells(tabla);

				row.Cells[0].Value = i + 1; 
				for (int j = 0; j < 7; j++)
				{
					if (j < momento.Moments.Length)
					{
						row.Cells[j + 1].Value = momento.Moments[j];
					}
				}

				tabla.Rows.Add(row);
			}

			for (int i = 1; i < tabla.Columns.Count; i++)
			{
				tabla.Columns[i].DefaultCellStyle.Format = "N6";
			}
		}

		private void imgRecibir_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			try
			{
				if (imgRecibir.Image != null)
				{
					using (SaveFileDialog saveDialog = new SaveFileDialog())
					{
						saveDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp";
						saveDialog.Title = "Guardar imagen procesada";
						saveDialog.FileName = $"imagen_procesada_{DateTime.Now:yyyyMMddHHmmss}";
						saveDialog.OverwritePrompt = true;
						saveDialog.AddExtension = true;

						if (saveDialog.ShowDialog() == DialogResult.OK)
						{
							string extension = Path.GetExtension(saveDialog.FileName).ToLower();
							System.Drawing.Imaging.ImageFormat format;

							if (extension == ".jpg" || extension == ".jpeg")
								format = System.Drawing.Imaging.ImageFormat.Jpeg;
							else if (extension == ".bmp")
								format = System.Drawing.Imaging.ImageFormat.Bmp;
							else
								format = System.Drawing.Imaging.ImageFormat.Png;

							if (format == System.Drawing.Imaging.ImageFormat.Jpeg)
							{
								var encoderParams = new System.Drawing.Imaging.EncoderParameters(1);
								encoderParams.Param[0] = new System.Drawing.Imaging.EncoderParameter(
									System.Drawing.Imaging.Encoder.Quality, 100L);

								var jpegCodec = GetEncoderInfo("image/jpeg");
								imgRecibir.Image.Save(saveDialog.FileName, jpegCodec, encoderParams);
							}
							else
							{
								imgRecibir.Image.Save(saveDialog.FileName, format);
							}

							MessageBox.Show("Imagen guardada correctamente", "Éxito",
										  MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
					}
				}
				else
				{
					MessageBox.Show("No hay imagen procesada para guardar", "Advertencia",
								  MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error al guardar la imagen: {ex.Message}", "Error",
							  MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnLimpiar_MouseClick(object sender, MouseEventArgs e)
		{
			LimpiarImagenes();
			LimpiarLabels();
			LimpiarTabla();
		}

		private void btnSubir_MouseClick(object sender, MouseEventArgs e)
		{
			imgEnviar.Image = btm_cargada = Globales.CargarImagen();
			imgSubida = true;
			VerificarEnviar();
		}

		private void cbOpciones_SelectedIndexChanged(object sender, EventArgs e)
		{
			opcion = cbOpciones.SelectedIndex;
			opcionSeleccionada = true;
			VerificarEnviar();
		}
	}
	public static class Globales
	{
		public static Bitmap CargarImagen()
		{
			using (OpenFileDialog finder = new OpenFileDialog())
			{
				try
				{
					finder.Filter = "Imagenes|*.jpg;*.jpeg;*.png;*.bmp;*.jfif";
					finder.Title = "Selecciona una imagen";
					finder.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
					finder.ShowDialog();
					return (Bitmap)Image.FromFile(finder.FileName);
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error al cargar la imagen: " + ex.Message);
					return null;
				}
			}
		}

		public static Bitmap RecibirImagen(API api)
		{
			try
			{
				Bitmap btm = new Bitmap(1920, 1080);

				return btm;
			}
			catch(IOException e)
			{
				Console.WriteLine("Error al recibir la imagen: " + e.Message);
				return null;
			}
		}
	}

	public class ResultadoMomentosHu
	{
		public PointF Center { get; set; }
		public double[] Moments { get; set; }
	}

	public class API
	{
		private static HttpClient client = new HttpClient();
		string url;
		string[] opciones = { "EscalaGrises", "Binarizar", "DetectarBordes", "Etiquetado", "InvariantesHu" };
		private int objetos = 0;
		private List<ResultadoMomentosHu> momentosHu = new List<ResultadoMomentosHu>();

		public List<ResultadoMomentosHu> GetMomentosHu()
		{
			return momentosHu;
		}

		public API(string url)
		{
			this.url = url.TrimEnd('/');
		}

		public int GetObjetos()
		{
			return objetos;
		}

		public async Task<Bitmap> Enviar(int opcion, Bitmap img)
		{
			string endpoint = $"{url}/Procesamiento/{opciones[opcion]}";

			try
			{
				using (MemoryStream ms = new MemoryStream())
				using (var content = new MultipartFormDataContent())
				{
					img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
					ms.Position = 0;

					var imageContent = new ByteArrayContent(ms.ToArray());
					imageContent.Headers.ContentType =
						new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");

					content.Add(imageContent, "file", "image.png");

					var response = await client.PostAsync(endpoint, content);

					if (response.IsSuccessStatusCode)
					{
						if (opcion == 4) 
						{
							var jsonResponse = await response.Content.ReadAsStringAsync();
							var jsonObj = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

							string imageBase64 = jsonObj.Imagen;
							byte[] imageBytes = Convert.FromBase64String(imageBase64);

							objetos = jsonObj.TotalObjetos;

							if (jsonObj.MomentosHu != null)
							{
								momentosHu = JsonConvert.DeserializeObject<List<ResultadoMomentosHu>>(
									jsonObj.MomentosHu.ToString());
							}

							using (MemoryStream imageStream = new MemoryStream(imageBytes))
							{
								return new Bitmap(imageStream);
							}
						}
						else
						{
							var responseStream = await response.Content.ReadAsStreamAsync();
							objetos = 0;
							momentosHu.Clear(); 
							return new Bitmap(responseStream);
						}
					}
					else
					{
						throw new Exception($"Error: {response.ReasonPhrase}");
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: " + ex.Message);
				throw;
			}
		}
	}
}

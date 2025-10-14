using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Microsoft.VisualBasic;

namespace TestAPI
{
	public partial class Form1 : Form
	{
		Stopwatch sw = new Stopwatch();
		API api;
		List<Bitmap> btm_cargadas = new List<Bitmap>();
		Bitmap btm_recibida;
		bool imgSubida = false, opcionSeleccionada = false;
		int opcion;
		private string url = "http://localhost:35271/api";
		private int indiceCargadasActual = 0;
		private int indiceProcesadasActual = 0;
		private List<Bitmap> imagenesProcesadas = new List<Bitmap>();

		public Form1()
		{
			InitializeComponent();
			CrearAPI(url);
			ConfigurarTabla();
			ActualizarEstadoBotones();
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
			btm_cargadas.Clear();
			imagenesProcesadas.Clear();
			indiceCargadasActual = 0;
			indiceProcesadasActual = 0;
			lblImgs.Text = "0";
			imgSubida = false;
			ActualizarEstadoBotones();
			VerificarEnviar();
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
			tabla.Columns.Add("Imagen", "Imagen");
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
			lbTiempo.Text = (sw.ElapsedMilliseconds > 1000) ?
				(sw.ElapsedMilliseconds / 1000 < 60) ?
					($"{sw.ElapsedMilliseconds / 1000.0:F2} segundos") :
					$"{sw.ElapsedMilliseconds / 60000}m {sw.ElapsedMilliseconds / 1000 % 60:F2}s" :
				$"{sw.ElapsedMilliseconds} ms";
		}

		public void VerificarEnviar()
		{
			btnEnviar.Enabled = imgSubida && opcionSeleccionada;
		}

		public void PonerNumObjetos()
		{
			if (opcion == 4)
			{
				lblObjetos.Text = $"{api.GetObjetos()} objetos encontrados en total";
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

					if (btm_cargadas.Count > 1)
					{
						var resultados = await api.EnviarMultiplesImagenes(opcion, btm_cargadas);
						if (resultados.Count > 0)
						{
							var imagenesProcesadasList = resultados.Select(r => r.ImagenProcesada).ToList();
							ActualizarListaProcesadas(imagenesProcesadasList);

							if (opcion == 4)
							{
								api.SetMomentosHu(resultados);
								PonerNumObjetos();
								tabla.Visible = true;
								MostrarMomentosHuEnTabla();
							}
							else
							{
								tabla.Visible = false;
								lblObjetos.Visible = false;
								label5.Visible = false;
							}
						}
					}
					else
					{
						if (btm_cargadas.Count > 0)
						{
							btm_recibida = await api.Enviar(opcion, btm_cargadas[indiceCargadasActual]);
							ActualizarListaProcesadas(new List<Bitmap> { btm_recibida });

							if (opcion == 4)
							{
								PonerNumObjetos();
								tabla.Visible = true;
								MostrarMomentosHuEnTabla();
							}
							else
							{
								tabla.Visible = false;
								lblObjetos.Visible = false;
								label5.Visible = false;
							}
						}
					}

					sw.Stop();
					PonerTiempo();
					sw.Reset();
				}
				else
					MessageBox.Show("Por favor, sube al menos una imagen y selecciona una opción antes de enviar.");
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error al enviar la(s) imagen(es): " + ex.Message + "\n\nDetalles técnicos: " + ex.InnerException?.Message);
			}
		}

		private void MostrarMomentosHuEnTabla()
		{
			tabla.Rows.Clear();

			var momentosHu = api.GetMomentosHu();
			int rowIndex = 0;

			for (int imgIndex = 0; imgIndex < momentosHu.Count; imgIndex++)
			{
				var momentosPorImagen = momentosHu[imgIndex];

				for (int i = 0; i < momentosPorImagen.Count; i++)
				{
					var momento = momentosPorImagen[i];

					DataGridViewRow row = new DataGridViewRow();
					row.CreateCells(tabla);

					row.Cells[0].Value = rowIndex + 1;
					row.Cells[1].Value = $"Imagen {imgIndex + 1}";
					for (int j = 0; j < 7; j++)
					{
						if (j < momento.Moments.Length)
						{
							row.Cells[j + 2].Value = momento.Moments[j];
						}
					}

					tabla.Rows.Add(row);
					rowIndex++;
				}
			}

			for (int i = 2; i < tabla.Columns.Count; i++)
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

		private void button1_MouseClick(object sender, MouseEventArgs e)
		{
			string nombreArchivo = Globales.ObtenerNombreArchivo();
			if (nombreArchivo != null && opcion == 4)
			{
				var todosMomentosHu = api.GetMomentosHu();
				Globales.GuardarMomentosHu(todosMomentosHu, nombreArchivo);
			}
		}

		private void btnSubir_MouseClick(object sender, MouseEventArgs e)
		{
			var imagenes = Globales.CargarMultiplesImagenes();
			if (imagenes != null && imagenes.Count > 0)
			{
				btm_cargadas = imagenes;
				indiceCargadasActual = 0;
				imgEnviar.Image = btm_cargadas[indiceCargadasActual];
				imgSubida = true;
				VerificarEnviar();

				MessageBox.Show($"{btm_cargadas.Count} imagen(es) cargada(s) correctamente.",
							  "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

				ActualizarIndicesNavegacion();
				ActualizarEstadoBotones();
			}
		}

		private void btnPrev_MouseClick(object sender, MouseEventArgs e)
		{
			if (imagenesProcesadas.Count > 0)
			{
				indiceProcesadasActual--;
				indiceCargadasActual--;

				if (indiceProcesadasActual < 0)
				{
					indiceProcesadasActual = imagenesProcesadas.Count - 1;
					indiceCargadasActual = btm_cargadas.Count - 1;
				}

				if (indiceCargadasActual < 0) indiceCargadasActual = 0;
				if (indiceCargadasActual >= btm_cargadas.Count)
					indiceCargadasActual = btm_cargadas.Count - 1;

				if (indiceProcesadasActual < 0) indiceProcesadasActual = 0;
				if (indiceProcesadasActual >= imagenesProcesadas.Count)
					indiceProcesadasActual = imagenesProcesadas.Count - 1;

				imgEnviar.Image = btm_cargadas[indiceCargadasActual];
				imgRecibir.Image = imagenesProcesadas[indiceProcesadasActual];

				ActualizarIndicesNavegacion();
			}
			else if (btm_cargadas.Count > 0)
			{
				indiceCargadasActual--;
				if (indiceCargadasActual < 0)
					indiceCargadasActual = btm_cargadas.Count - 1;

				imgEnviar.Image = btm_cargadas[indiceCargadasActual];
				ActualizarIndicesNavegacion();
			}

			ActualizarEstadoBotones();
		}

		private void btnProx_MouseClick(object sender, MouseEventArgs e)
		{
			if (imagenesProcesadas.Count > 0)
			{
				indiceProcesadasActual++;
				indiceCargadasActual++;

				if (indiceProcesadasActual >= imagenesProcesadas.Count)
				{
					indiceProcesadasActual = 0;
					indiceCargadasActual = 0;
				}

				if (indiceCargadasActual < 0) indiceCargadasActual = 0;
				if (indiceCargadasActual >= btm_cargadas.Count)
					indiceCargadasActual = btm_cargadas.Count - 1;

				if (indiceProcesadasActual < 0) indiceProcesadasActual = 0;
				if (indiceProcesadasActual >= imagenesProcesadas.Count)
					indiceProcesadasActual = imagenesProcesadas.Count - 1;

				imgEnviar.Image = btm_cargadas[indiceCargadasActual];
				imgRecibir.Image = imagenesProcesadas[indiceProcesadasActual];

				ActualizarIndicesNavegacion();
			}
			else if (btm_cargadas.Count > 0)
			{
				indiceCargadasActual++;
				if (indiceCargadasActual >= btm_cargadas.Count)
					indiceCargadasActual = 0;

				imgEnviar.Image = btm_cargadas[indiceCargadasActual];
				ActualizarIndicesNavegacion();
			}

			ActualizarEstadoBotones();
		}

		private void btnDel_MouseClick(object sender, MouseEventArgs e)
		{
			if (imagenesProcesadas.Count > 0)
			{
				if (btm_cargadas.Count > 0 && imagenesProcesadas.Count > 0)
				{
					btm_cargadas.RemoveAt(indiceCargadasActual);
					imagenesProcesadas.RemoveAt(indiceProcesadasActual);

					if (btm_cargadas.Count == 0 || imagenesProcesadas.Count == 0)
					{
						LimpiarImagenes();
					}
					else
					{
						if (indiceCargadasActual >= btm_cargadas.Count)
							indiceCargadasActual = btm_cargadas.Count - 1;
						if (indiceProcesadasActual >= imagenesProcesadas.Count)
							indiceProcesadasActual = imagenesProcesadas.Count - 1;

						imgEnviar.Image = btm_cargadas[indiceCargadasActual];
						imgRecibir.Image = imagenesProcesadas[indiceProcesadasActual];
						ActualizarIndicesNavegacion();
					}
				}
			}
			else if (btm_cargadas.Count > 0)
			{
				btm_cargadas.RemoveAt(indiceCargadasActual);

				if (btm_cargadas.Count == 0)
				{
					LimpiarImagenes();
				}
				else
				{
					if (indiceCargadasActual >= btm_cargadas.Count)
						indiceCargadasActual = btm_cargadas.Count - 1;

					imgEnviar.Image = btm_cargadas[indiceCargadasActual];
					ActualizarIndicesNavegacion();
				}
			}

			ActualizarEstadoBotones();
			VerificarEnviar();
		}

		private void btnDescargarImgs_MouseClick(object sender, MouseEventArgs e)
		{
			try
			{
				if (btm_cargadas.Count == 0 && imagenesProcesadas.Count == 0)
				{
					MessageBox.Show("No hay imágenes para descargar.", "Advertencia",
								  MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}

				string nombreBase = ObtenerNombreBase();
				if (string.IsNullOrEmpty(nombreBase))
				{
					return;
				}

				var tiposSeleccionados = MostrarDialogoTiposImagen();
				if (tiposSeleccionados == null)
				{
					return;
				}

				using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
				{
					folderDialog.Description = "Seleccionar carpeta donde guardar las imágenes";
					folderDialog.ShowNewFolderButton = true;
					folderDialog.RootFolder = Environment.SpecialFolder.Desktop;

					if (folderDialog.ShowDialog() == DialogResult.OK)
					{
						string baseFolder = folderDialog.SelectedPath;
						string carpetaDestino = Path.Combine(baseFolder, $"{nombreBase}_{DateTime.Now:yyyyMMddHHmmss}");

						Directory.CreateDirectory(carpetaDestino);

						int contadorGuardadas = 0;

						string carpetaOriginal = Path.Combine(carpetaDestino, "Original");
						string carpetaProcesada = Path.Combine(carpetaDestino, "Procesada");

						if (tiposSeleccionados.Value.GuardarOriginales && btm_cargadas.Count > 0)
						{
							Directory.CreateDirectory(carpetaOriginal);
						}

						if (tiposSeleccionados.Value.GuardarProcesadas && imagenesProcesadas.Count > 0)
						{
							Directory.CreateDirectory(carpetaProcesada);
						}

						if (tiposSeleccionados.Value.GuardarOriginales)
						{
							for (int i = 0; i < btm_cargadas.Count; i++)
							{
								try
								{
									string nombreArchivo = $"{nombreBase}_{i + 1}.png";
									string rutaCompleta = Path.Combine(carpetaOriginal, nombreArchivo);
									btm_cargadas[i].Save(rutaCompleta, System.Drawing.Imaging.ImageFormat.Png);
									contadorGuardadas++;
								}
								catch (Exception ex)
								{
									MessageBox.Show($"Error al guardar imagen original {i + 1}: {ex.Message}",
												  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
								}
							}
						}

						if (tiposSeleccionados.Value.GuardarProcesadas)
						{
							for (int i = 0; i < imagenesProcesadas.Count; i++)
							{
								try
								{
									string nombreArchivo = $"{nombreBase}_{i + 1}.png";
									string rutaCompleta = Path.Combine(carpetaProcesada, nombreArchivo);
									imagenesProcesadas[i].Save(rutaCompleta, System.Drawing.Imaging.ImageFormat.Png);
									contadorGuardadas++;
								}
								catch (Exception ex)
								{
									MessageBox.Show($"Error al guardar imagen procesada {i + 1}: {ex.Message}",
												  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
								}
							}
						}

						if (tiposSeleccionados.Value.GuardarProcesadas && api.GetMomentosHu().Count > 0 && opcion == 4)
						{
							string archivoHu = Path.Combine(carpetaDestino, $"{nombreBase}_momentos_hu.hu");
							Globales.GuardarMomentosHu(api.GetMomentosHu(), archivoHu);
						}

						if (contadorGuardadas > 0)
						{
							string mensaje = $"{contadorGuardadas} imágenes guardadas correctamente en:\n{carpetaDestino}";

							List<string> detalles = new List<string>();
							if (tiposSeleccionados.Value.GuardarOriginales && btm_cargadas.Count > 0)
								detalles.Add($"{btm_cargadas.Count} originales en carpeta 'Original'");
							if (tiposSeleccionados.Value.GuardarProcesadas && imagenesProcesadas.Count > 0)
								detalles.Add($"{imagenesProcesadas.Count} procesadas en carpeta 'Procesada'");

							if (detalles.Count > 0)
							{
								mensaje += $"\n\n{string.Join("\n", detalles)}";
							}

							MessageBox.Show(mensaje, "Descarga Exitosa",
										  MessageBoxButtons.OK, MessageBoxIcon.Information);

							Process.Start("explorer.exe", carpetaDestino);
						}
						else
						{
							MessageBox.Show("No se guardó ninguna imagen.", "Información",
										  MessageBoxButtons.OK, MessageBoxIcon.Information);
							if (Directory.Exists(carpetaDestino) &&
								!Directory.EnumerateFileSystemEntries(carpetaDestino).Any())
							{
								Directory.Delete(carpetaDestino);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error durante la descarga: {ex.Message}", "Error",
							  MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private string ObtenerNombreBase()
		{
			using (Form inputForm = new Form())
			{
				inputForm.Text = "Nombre base para las imágenes";
				inputForm.Size = new Size(350, 150);
				inputForm.StartPosition = FormStartPosition.CenterParent;
				inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
				inputForm.MaximizeBox = false;
				inputForm.MinimizeBox = false;

				Label label = new Label()
				{
					Text = "Ingrese el nombre base para las imágenes:",
					Location = new Point(10, 10),
					Size = new Size(300, 20)
				};

				TextBox textBox = new TextBox()
				{
					Location = new Point(10, 35),
					Size = new Size(300, 20),
					Text = "Imagen"
				};

				Button okButton = new Button()
				{
					Text = "Aceptar",
					Location = new Point(150, 70),
					Size = new Size(75, 25),
					DialogResult = DialogResult.OK
				};

				Button cancelButton = new Button()
				{
					Text = "Cancelar",
					Location = new Point(235, 70),
					Size = new Size(75, 25),
					DialogResult = DialogResult.Cancel
				};

				inputForm.Controls.Add(label);
				inputForm.Controls.Add(textBox);
				inputForm.Controls.Add(okButton);
				inputForm.Controls.Add(cancelButton);

				inputForm.AcceptButton = okButton;
				inputForm.CancelButton = cancelButton;

				textBox.SelectAll();

				if (inputForm.ShowDialog() == DialogResult.OK)
				{
					string nombre = textBox.Text.Trim();

					if (string.IsNullOrEmpty(nombre))
					{
						MessageBox.Show("El nombre no puede estar vacío.", "Error",
									  MessageBoxButtons.OK, MessageBoxIcon.Error);
						return ObtenerNombreBase(); 
					}

					foreach (char c in Path.GetInvalidFileNameChars())
					{
						nombre = nombre.Replace(c, '_');
					}

					return nombre;
				}

				return null;
			}
		}

		private (bool GuardarOriginales, bool GuardarProcesadas)? MostrarDialogoTiposImagen()
		{
			using (Form formTipos = new Form())
			{
				formTipos.Text = "Seleccionar tipos de imágenes a guardar";
				formTipos.Size = new Size(350, 200);
				formTipos.StartPosition = FormStartPosition.CenterParent;
				formTipos.FormBorderStyle = FormBorderStyle.FixedDialog;
				formTipos.MaximizeBox = false;
				formTipos.MinimizeBox = false;

				Label label = new Label()
				{
					Text = "Seleccione qué tipos de imágenes desea guardar:",
					Location = new Point(10, 10),
					Size = new Size(300, 20)
				};

				CheckBox chkOriginales = new CheckBox()
				{
					Text = $"Imágenes originales ({btm_cargadas.Count} imágenes)",
					Location = new Point(10, 40),
					Size = new Size(250, 20),
					Checked = btm_cargadas.Count > 0,
					Enabled = btm_cargadas.Count > 0
				};

				CheckBox chkProcesadas = new CheckBox()
				{
					Text = $"Imágenes procesadas ({imagenesProcesadas.Count} imágenes)",
					Location = new Point(10, 70),
					Size = new Size(250, 20),
					Checked = imagenesProcesadas.Count > 0,
					Enabled = imagenesProcesadas.Count > 0
				};

				Button okButton = new Button()
				{
					Text = "Aceptar",
					Location = new Point(150, 110),
					Size = new Size(75, 25),
					DialogResult = DialogResult.OK
				};

				Button cancelButton = new Button()
				{
					Text = "Cancelar",
					Location = new Point(235, 110),
					Size = new Size(75, 25),
					DialogResult = DialogResult.Cancel
				};

				formTipos.Controls.Add(label);
				formTipos.Controls.Add(chkOriginales);
				formTipos.Controls.Add(chkProcesadas);
				formTipos.Controls.Add(okButton);
				formTipos.Controls.Add(cancelButton);

				formTipos.AcceptButton = okButton;
				formTipos.CancelButton = cancelButton;

				okButton.Click += (s, e) =>
				{
					if (!chkOriginales.Checked && !chkProcesadas.Checked)
					{
						MessageBox.Show("Debe seleccionar al menos un tipo de imagen para guardar.",
									  "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return;
					}
					formTipos.DialogResult = DialogResult.OK;
					formTipos.Close();
				};

				if (formTipos.ShowDialog() == DialogResult.OK)
				{
					return (chkOriginales.Checked, chkProcesadas.Checked);
				}

				return null;
			}
		}

		private void cbOpciones_SelectedIndexChanged(object sender, EventArgs e)
		{
			opcion = cbOpciones.SelectedIndex;
			opcionSeleccionada = true;
			VerificarEnviar();
		}

		private void ActualizarIndicesNavegacion()
		{
			if (imagenesProcesadas.Count > 0 && btm_cargadas.Count > 0)
			{
				lblImgs.Text = $"{indiceCargadasActual + 1} / {btm_cargadas.Count} (Carg) - {indiceProcesadasActual + 1} / {imagenesProcesadas.Count} (Proc)";
			}
			else if (btm_cargadas.Count > 0)
			{
				lblImgs.Text = $"{indiceCargadasActual + 1} / {btm_cargadas.Count}";
			}
			else
			{
				lblImgs.Text = "0";
			}
		}

		private void ActualizarListaProcesadas(List<Bitmap> nuevasImagenes)
		{
			imagenesProcesadas = nuevasImagenes;
			indiceProcesadasActual = 0;
			indiceCargadasActual = 0; 

			if (imagenesProcesadas.Count > 0 && btm_cargadas.Count > 0)
			{
				imgEnviar.Image = btm_cargadas[indiceCargadasActual];
				imgRecibir.Image = imagenesProcesadas[indiceProcesadasActual];
				ActualizarIndicesNavegacion();
			}
			ActualizarEstadoBotones();
		}

		private void ActualizarEstadoBotones()
		{
			if (imagenesProcesadas.Count > 0 && btm_cargadas.Count > 0)
			{
				bool hayMultipleProcesadas = imagenesProcesadas.Count > 1;
				bool hayMultipleCargadas = btm_cargadas.Count > 1;

				btnPrev.Enabled = (indiceProcesadasActual > 0) && (indiceCargadasActual > 0);
				btnProx.Enabled = (indiceProcesadasActual < imagenesProcesadas.Count - 1) &&
								  (indiceCargadasActual < btm_cargadas.Count - 1);
				btnDel.Enabled = true;
			}
			else if (btm_cargadas.Count > 0)
			{
				if (btm_cargadas.Count == 1)
				{
					btnPrev.Enabled = false;
					btnProx.Enabled = false;
					btnDel.Enabled = true;
				}
				else
				{
					btnPrev.Enabled = (indiceCargadasActual > 0);
					btnProx.Enabled = (indiceCargadasActual < btm_cargadas.Count - 1);
					btnDel.Enabled = true;
				}
			}
			else
			{
				btnPrev.Enabled = false;
				btnProx.Enabled = false;
				btnDel.Enabled = false;
			}
		}
	}

	public static class Globales
	{
		public static string ObtenerNombreArchivo()
		{
			using (SaveFileDialog saveDialog = new SaveFileDialog())
			{
				saveDialog.Filter = "Hu Files|*.hu";
				saveDialog.Title = "Guardar momentos Hu";
				saveDialog.FileName = $"momentos_hu_{DateTime.Now:yyyyMMddHHmmss}.hu";
				saveDialog.OverwritePrompt = true;
				saveDialog.AddExtension = true;

				if (saveDialog.ShowDialog() == DialogResult.OK)
				{
					return saveDialog.FileName;
				}
				return null;
			}
		}

		public static void GuardarMomentosHu(List<List<ResultadoMomentosHu>> momentosHuPorImagen, string filePath)
		{
			try
			{
				using (StreamWriter writer = new StreamWriter(filePath))
				{
					foreach (var momentosHuImagen in momentosHuPorImagen)
					{
						if (momentosHuImagen.Count > 0)
						{
							var momento = momentosHuImagen[0];

							string linea = $"{momento.Moments[0]},{momento.Moments[1]}," +
										   $"{momento.Moments[2]},{momento.Moments[3]}," +
										   $"{momento.Moments[4]},{momento.Moments[5]}," +
										   $"{momento.Moments[6]}";
							writer.WriteLine(linea);
						}
						else
						{
							writer.WriteLine("0,0,0,0,0,0,0");
						}
					}
				}

				MessageBox.Show($"Momentos Hu guardados correctamente en:\n{filePath}",
							  "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error al guardar los momentos Hu: {ex.Message}",
							  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

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

		public static List<Bitmap> CargarMultiplesImagenes()
		{
			using (OpenFileDialog finder = new OpenFileDialog())
			{
				try
				{
					finder.Filter = "Imagenes|*.jpg;*.jpeg;*.png;*.bmp;*.jfif";
					finder.Title = "Selecciona una o más imágenes";
					finder.Multiselect = true;
					finder.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

					if (finder.ShowDialog() == DialogResult.OK)
					{
						List<Bitmap> imagenes = new List<Bitmap>();
						foreach (string fileName in finder.FileNames)
						{
							try
							{
								Bitmap imagen = (Bitmap)Image.FromFile(fileName);
								imagenes.Add(imagen);
							}
							catch (Exception ex)
							{
								MessageBox.Show($"Error al cargar la imagen {Path.GetFileName(fileName)}: {ex.Message}");
							}
						}
						return imagenes;
					}
					return null;
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error al cargar las imágenes: " + ex.Message);
					return null;
				}
			}
		}
	}

	public class ResultadoProcesamiento
	{
		public Bitmap ImagenProcesada { get; set; }
		public int TotalObjetos { get; set; }
		public List<ResultadoMomentosHu> MomentosHu { get; set; }
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
		private List<List<ResultadoMomentosHu>> momentosHuPorImagen = new List<List<ResultadoMomentosHu>>();

		public List<List<ResultadoMomentosHu>> GetMomentosHu()
		{
			return momentosHuPorImagen;
		}

		public void SetMomentosHu(List<ResultadoProcesamiento> resultados)
		{
			momentosHuPorImagen.Clear();
			objetos = 0;

			foreach (var resultado in resultados)
			{
				momentosHuPorImagen.Add(resultado.MomentosHu);
				objetos += resultado.TotalObjetos;
			}
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
					img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
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
								var momentosHuList = JsonConvert.DeserializeObject<List<ResultadoMomentosHu>>(
									jsonObj.MomentosHu.ToString());
								momentosHuPorImagen.Clear();
								momentosHuPorImagen.Add(momentosHuList);
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
							momentosHuPorImagen.Clear();
							return new Bitmap(responseStream);
						}
					}
					else
					{
						var errorContent = await response.Content.ReadAsStringAsync();
						throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}. Detalles: {errorContent}");
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error en Enviar: " + ex.Message);
				throw new Exception($"Error al enviar imagen: {ex.Message}", ex);
			}
		}

		public async Task<List<ResultadoProcesamiento>> EnviarMultiplesImagenes(int opcion, List<Bitmap> imagenes)
		{
			string endpoint = $"{url}/Procesamiento/Multiples{opciones[opcion]}";
			var resultados = new List<ResultadoProcesamiento>();

			try
			{
				using (var content = new MultipartFormDataContent())
				{
					for (int i = 0; i < imagenes.Count; i++)
					{
						using (MemoryStream ms = new MemoryStream())
						{
							imagenes[i].Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
							ms.Position = 0;
							var imageContent = new ByteArrayContent(ms.ToArray());
							imageContent.Headers.ContentType =
								new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
							content.Add(imageContent, "files", $"image{i}.png");
						}
					}

					var response = await client.PostAsync(endpoint, content);

					if (response.IsSuccessStatusCode)
					{
						var jsonResponse = await response.Content.ReadAsStringAsync();
						var jsonArray = JsonConvert.DeserializeObject<List<dynamic>>(jsonResponse);

						foreach (var jsonObj in jsonArray)
						{
							string imageBase64 = jsonObj.Imagen;
							byte[] imageBytes = Convert.FromBase64String(imageBase64);

							int totalObjetos = 0;
							try { totalObjetos = jsonObj.TotalObjetos; } catch { }

							List<ResultadoMomentosHu> momentosHu = null;
							if (jsonObj.MomentosHu != null && ((Newtonsoft.Json.Linq.JArray)jsonObj.MomentosHu).Count > 0)
							{
								try
								{
									momentosHu = JsonConvert.DeserializeObject<List<ResultadoMomentosHu>>(
										jsonObj.MomentosHu.ToString());
								}
								catch
								{
									momentosHu = new List<ResultadoMomentosHu>();
								}
							}
							else
							{
								momentosHu = new List<ResultadoMomentosHu>();
							}

							using (MemoryStream imageStream = new MemoryStream(imageBytes))
							{
								var resultado = new ResultadoProcesamiento
								{
									ImagenProcesada = new Bitmap(imageStream),
									TotalObjetos = totalObjetos,
									MomentosHu = momentosHu
								};
								resultados.Add(resultado);
							}
						}
					}
					else
					{
						var errorContent = await response.Content.ReadAsStringAsync();
						throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}. Detalles: {errorContent}");
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error en EnviarMultiplesImagenes: " + ex.Message);
				throw new Exception($"Error al procesar múltiples imágenes: {ex.Message}", ex);
			}

			return resultados;
		}
	}
}
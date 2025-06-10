using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace WebService.Scripts
{
	public static class MetodosProcesamiento
	{
		public static Bitmap Escala_Grises(Bitmap btm)
		{
			Bitmap imagenFormato = new Bitmap(btm.Width, btm.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			using (Graphics g = Graphics.FromImage(imagenFormato))
			{
				g.DrawImage(btm, new Rectangle(0, 0, btm.Width, btm.Height));
			}

			BitmapData bmpdata = imagenFormato.LockBits(
				new Rectangle(0, 0, imagenFormato.Width, imagenFormato.Height),
				ImageLockMode.ReadWrite,
				imagenFormato.PixelFormat
			);

			int numbytes = bmpdata.Stride * imagenFormato.Height;
			byte[] bytedata = new byte[numbytes];
			IntPtr arregloImagen = bmpdata.Scan0;

			Marshal.Copy(arregloImagen, bytedata, 0, numbytes);

			for (int y = 0; y < imagenFormato.Height; y++)
			{
				int currentLine = y * bmpdata.Stride;
				for (int x = 0; x < imagenFormato.Width; x++)
				{
					int currentPixel = currentLine + x * 4;

					byte B = bytedata[currentPixel];
					byte G = bytedata[currentPixel + 1];
					byte R = bytedata[currentPixel + 2];

					byte gris = (byte)(0.299 * R + 0.587 * G + 0.114 * B);

					bytedata[currentPixel] = bytedata[currentPixel + 1] = bytedata[currentPixel + 2] = gris;
				}
			}

			Marshal.Copy(bytedata, 0, arregloImagen, numbytes);
			imagenFormato.UnlockBits(bmpdata);

			return imagenFormato;
		}

		public static Bitmap Binarizar(Bitmap btm)
		{
			btm = Escala_Grises(btm);
			BitmapData bmpdata = btm.LockBits(new Rectangle(0, 0, btm.Width, btm.Height),
											ImageLockMode.ReadWrite, btm.PixelFormat);
			int numbytes = bmpdata.Stride * btm.Height;
			byte[] bytedata = new byte[numbytes];
			IntPtr arregloImagen = bmpdata.Scan0;

			Marshal.Copy(arregloImagen, bytedata, 0, numbytes);

			long sum = 0;
			int pixelCount = 0;

			for (int i = 0; i < numbytes; i += 4)
			{
				sum += bytedata[i];
				pixelCount++;
			}

			byte promedioBrillo = (byte)(sum / pixelCount);
			byte umbral = CalcularUmbralOtsu(bytedata, btm.Width, btm.Height, bmpdata.Stride);
			bool objetoEsOscuro = promedioBrillo < umbral;

			for (int i = 0; i < numbytes; i += 4)
			{
				byte bwValue;

				if (objetoEsOscuro)
					bwValue = bytedata[i] > umbral ? (byte)255 : (byte)0;
				else
					bwValue = bytedata[i] > umbral ? (byte)0 : (byte)255;

				bytedata[i] = bytedata[i + 1] = bytedata[i + 2] = bwValue;
			}

			Marshal.Copy(bytedata, 0, arregloImagen, numbytes);
			btm.UnlockBits(bmpdata);

			return btm;
		}

		public static Bitmap Detectar_Bordes(Bitmap btm)
		{
			btm = Escala_Grises(btm);
			BitmapData bmpdata = btm.LockBits(new Rectangle(0, 0, btm.Width, btm.Height),
												ImageLockMode.ReadWrite, btm.PixelFormat);
			int numbytes = bmpdata.Stride * btm.Height;
			int umbral = 45;
			byte[] bytedata = new byte[numbytes];
			IntPtr arregloImagen = bmpdata.Scan0;
			Marshal.Copy(arregloImagen, bytedata, 0, numbytes);
			byte[] copiaDatos = new byte[numbytes];
			Array.Copy(bytedata, copiaDatos, numbytes);
			int[,] sobelX = { { -1, 0, 1 }, { -1, 0, 1 }, { -1, 0, 1 } };
			int[,] sobelY = { { -1, -1, -1 }, { 0, 0, 0 }, { 1, 1, 1 } };
			int width = btm.Width;
			int height = btm.Height;
			int stride = bmpdata.Stride;
			Bitmap bordes = new Bitmap(width, height);
			using (Graphics g = Graphics.FromImage(bordes))
			{
				g.Clear(Color.Black);
				for (int y = 1; y < height - 1; y++)
				{
					for (int x = 1; x < width - 1; x++)
					{
						int gx = 0, gy = 0;
						int pos = y * stride + x * 4;
						for (int ky = -1; ky <= 1; ky++)
						{
							for (int kx = -1; kx <= 1; kx++)
							{
								int offset = (y + ky) * stride + (x + kx) * 4;
								byte pixelValue = copiaDatos[offset];

								gx += sobelX[ky + 1, kx + 1] * pixelValue;
								gy += sobelY[ky + 1, kx + 1] * pixelValue;
							}
						}
						int magnitude = (int)Math.Sqrt(gx * gx + gy * gy);
						if (magnitude > umbral)
						{
							using (Brush brush = new SolidBrush(Color.White))
							{
								g.FillRectangle(brush, x, y, 1, 1);
							}
						}
					}
				}
			}
			Marshal.Copy(bytedata, 0, arregloImagen, numbytes);
			btm.UnlockBits(bmpdata);

			return bordes;
		}

		public static Bitmap Etiquetado(Bitmap btm)
		{
			btm = Detectar_Bordes(btm);
			BitmapData bmpdata = btm.LockBits(new Rectangle(0, 0, btm.Width, btm.Height), ImageLockMode.ReadWrite, btm.PixelFormat);
			Bitmap etiquetado = new Bitmap(btm.Width, btm.Height);

			return etiquetado;
		}

		public static byte CalcularUmbralOtsu(byte[] datosImagen, int ancho, int alto, int stride)
		{
			int[] histograma = new int[256];

			for (int y = 0; y < alto; y++)
			{
				for (int x = 0; x < ancho; x++)
				{
					int index = y * stride + x * 4;
					byte intensidad = datosImagen[index];
					histograma[intensidad]++;
				}
			}

			int total = ancho * alto;
			float sum = 0;
			for (int t = 0; t < 256; t++) sum += t * histograma[t];

			float sumB = 0;
			int wB = 0;
			int wF = 0;
			float varMax = 0;
			byte threshold = 0;

			for (int t = 0; t < 256; t++)
			{
				wB += histograma[t];
				if (wB == 0) continue;

				wF = total - wB;
				if (wF == 0) break;

				sumB += (float)(t * histograma[t]);

				float mB = sumB / wB;
				float mF = (sum - sumB) / wF;

				float varBetween = (float)wB * (float)wF * (mB - mF) * (mB - mF);

				if (varBetween > varMax)
				{
					varMax = varBetween;
					threshold = (byte)t;
				}
			}

			return threshold;
		}
	}
}
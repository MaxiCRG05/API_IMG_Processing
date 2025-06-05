using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace WebService.Scripts
{
	public static class MetodosProcesamiento
	{

		public static Bitmap Escala_Grises(Bitmap btm)
		{
			BitmapData bmpdata = btm.LockBits(new Rectangle(0, 0, btm.Width, btm.Height), ImageLockMode.ReadWrite, btm.PixelFormat);
			int numbytes = bmpdata.Stride * btm.Height;
			byte[] bytedata = new byte[numbytes];
			IntPtr arregloImagen = bmpdata.Scan0;

			Marshal.Copy(arregloImagen, bytedata, 0, numbytes);

			for (int i = 0; i < numbytes; i += 4)
			{
				byte gris = (byte)(0.0 * bytedata[i + 2] + 0.587 * bytedata[i + 1] + 0.114 * bytedata[i]);
				bytedata[i] = bytedata[i + 1] = bytedata[i + 2] = gris;
			}

			Marshal.Copy(bytedata, 0, arregloImagen, numbytes);
			btm.UnlockBits(bmpdata);

			return btm;
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
			byte umbral = 130;
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
			return btm;
		}
	}
}
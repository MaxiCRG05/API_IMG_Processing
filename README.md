# 🖼️ Image Processing API & Client [ESP/ENG]

![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![ASP.NET](https://img.shields.io/badge/ASP.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)

Un sistema completo de procesamiento de imágenes con API web backend y cliente en Windows Forms que ofrece diversas operaciones de visión por computadora.

A complete image processing system with Web API backend and Windows Forms client providing various computer vision operations.

## ✨ Funcionalidades / Features

| Español | English |
|---------|---------|
| **Escala de grises** - Conversión de imágenes a tonos grises | **Grayscale Conversion** - Convert images to grayscale |
| **Binarización** - Umbralización adaptativa con método de Otsu | **Binarization** - Adaptive thresholding with Otsu's method |
| **Detección de bordes** - Implementación del operador Sobel | **Edge Detection** - Sobel operator implementation |
| **Etiquetado de componentes** - Análisis de componentes conectados con colores | **Component Labeling** - Connected components analysis with color coding |
| **Momentos de Hu** - Descriptores de forma para reconocimiento de objetos | **Hu Moments** - Shape descriptors for object recognition |
| **Métricas de rendimiento** - Medición del tiempo de procesamiento | **Performance Metrics** - Processing time measurement |

## 🛠️ Tecnologías / Technologies

- **Backend**: ASP.NET Web API
- **Cliente/Client**: Windows Forms (.NET)
- **Procesamiento de imágenes/Image Processing**: System.Drawing, algoritmos personalizados/custom algorithms
- **Comunicación/Communication**: HTTP, Multipart Form Data

## 📦 Instalación / Installation

### Configuración del API / API Setup

git clone https://github.com/tu-usuario/tu-repositorio.gitAbre el proyecto WebService en Visual Studio

Compila y ejecuta el proyecto (URL por defecto: http://localhost:3193)

Configuración del Cliente / Client Setup
Abre el proyecto TestAPI en Visual Studio

Actualiza la variable url en Form1.cs si es necesario

Compila y ejecuta la aplicación

### 🖥️ Cómo usar / Usage
Haz clic en "Subir Imagen" para cargar una imagen / Click "Subir Imagen" to load an image

Selecciona una operación del menú desplegable / Select an operation from the dropdown:

Escala de Grises (Grayscale)

Binarizar (Binarization)

Detectar Bordes (Edge Detection)

Etiquetado (Component Labeling)

Invariantes Hu (Hu Moments)

Haz clic en "Enviar" para procesar la imagen / Click "Enviar" to process the image

Doble clic en el resultado para guardarlo / Double-click the result to save it

### ⚙️ Endpoints del API / API Endpoints
Método	Endpoint	Descripción
POST	/api/Procesamiento/EscalaGrises	Convierte la imagen a escala de grises
POST	/api/Procesamiento/Binarizar	Aplica binarización a la imagen
POST	/api/Procesamiento/DetectarBordes	Detecta bordes usando operador Sobel
POST	/api/Procesamiento/Etiquetado	Etiqueta componentes conectados
POST	/api/Procesamiento/InvariantesHu	Calcula los momentos de Hu

### 🤝 Contribuciones / Contributing
¡Las contribuciones son bienvenidas! Por favor abre un issue o envía un pull request.

# Contributions are welcome! Please open an issue or submit a pull request.

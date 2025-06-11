#🖼️ API de Procesamiento de Imágenes con Cliente

Un sistema completo de procesamiento de imágenes con API web backend y cliente en Windows Forms que ofrece diversas operaciones de visión por computadora.

##✨ Funcionalidades
Escala de grises - Conversión de imágenes a tonos grises

Binarización - Umbralización adaptativa con método de Otsu

Detección de bordes - Implementación del operador Sobel

Etiquetado de componentes - Análisis de componentes conectados con colores

Momentos de Hu - Descriptores de forma para reconocimiento de objetos

Métricas de rendimiento - Medición del tiempo de procesamiento

🛠️ ##Tecnologías
Backend: ASP.NET Web API

Cliente: Windows Forms (.NET)

Procesamiento de imágenes: System.Drawing, algoritmos personalizados

Comunicación: HTTP, Multipart Form Data

📦 ##Instalación
Configuración del API
Clona el repositorio

Abre el proyecto WebService en Visual Studio

Compila y ejecuta el proyecto (URL por defecto: http://localhost:3193)

Configuración del Cliente
Abre el proyecto TestAPI en Visual Studio

Actualiza la variable url en Form1.cs si es necesario

Compila y ejecuta la aplicación

🖥️ ##Cómo usar
Haz clic en "Subir Imagen" para cargar una imagen

Selecciona una operación del menú desplegable:

Escala de Grises

Binarizar

Detectar Bordes

Etiquetado

Invariantes Hu

Haz clic en "Enviar" para procesar la imagen

Doble clic en el resultado para guardarlo

⚙️ ##Endpoints del API
POST /api/Procesamiento/EscalaGrises

POST /api/Procesamiento/Binarizar

POST /api/Procesamiento/DetectarBordes

POST /api/Procesamiento/Etiquetado

POST /api/Procesamiento/InvariantesHu

🤝 ##Contribuciones
¡Las contribuciones son bienvenidas! Por favor abre un issue o envía un pull request.

###Hecho con ❤️ y C#

🖼️ #Image Processing API & Client

A powerful image processing system with a Web API backend and Windows Forms client that provides various computer vision operations.

✨ ##Features
Grayscale Conversion - Convert images to grayscale

Binarization - Adaptive thresholding with Otsu's method

Edge Detection - Sobel operator implementation

Component Labeling - Connected components analysis with color coding

Hu Moments - Shape descriptors for object recognition

Performance Metrics - Processing time measurement

🛠️ ##Technologies
Backend: ASP.NET Web API

Client: Windows Forms (.NET)

Image Processing: System.Drawing, custom algorithms

Communication: HTTP, Multipart Form Data

📦 ##Installation
API Setup
Clone the repository

Open the WebService project in Visual Studio

Build and run the project (default URL: http://localhost:3193)

Client Setup
Open the TestAPI project in Visual Studio

Update the url variable in Form1.cs if needed

Build and run the application

🖥️ ##Usage
Click "Subir Imagen" to load an image

Select an operation from the dropdown:

Escala de Grises (Grayscale)

Binarizar (Binarization)

Detectar Bordes (Edge Detection)

Etiquetado (Component Labeling)

Invariantes Hu (Hu Moments)

Click "Enviar" to process the image

Double-click the result to save it

⚙️ ##API Endpoints
POST /api/Procesamiento/EscalaGrises

POST /api/Procesamiento/Binarizar

POST /api/Procesamiento/DetectarBordes

POST /api/Procesamiento/Etiquetado

POST /api/Procesamiento/InvariantesHu

🤝 ##Contributing
Contributions are welcome! Please open an issue or submit a pull request.
